using Ja2StracSaveEditorLib.Structures;
using NLog;
using System.Text;
using Ja2StracSaveEditorLib.Managers;
using Ja2StracSaveEditorLib.Utilities;

namespace Ja2StracSaveEditorLib;

public class SaveGame : IDisposable
{
    private readonly string _pathToItemsJson = Path.Combine("externalized", "items.json");
    private readonly string _pathToMagazinesJson = Path.Combine("externalized", "magazines.json");
    private readonly string _pathToWeaponsJson = Path.Combine("externalized", "weapons.json");

    private const uint Ja2StracSavedGameVersion = 102;

    private const uint StrategicEventsCntOffset = 815;
    private const uint StrategicEventsOffset = 819;
    private const uint LaptopInfoLength = 7440;
    private const uint MercProfilesCnt = 170;
    private const int NumKeys = 64;

    private readonly Logger _log;

    private readonly FileStream _fileStream;
    private readonly BinaryReader _fileReader;
    private readonly BinaryWriter _fileWriter;
    private SAVED_GAME_HEADER _header;
    private int _encryptionSet;
    private uint _mercProfilesOffset;

    private readonly ItemDataManager _itemDataManager;

    public List<MercProfile> MercProfiles { get; } = new List<MercProfile>();
    public List<Soldier> Soldiers { get; } = new List<Soldier>();

    public SaveGame(string filename, string pathToStracciatella, Logger log = null)
    {
        _log = log ?? LogManager.CreateNullLogger();

        _fileStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
        _fileReader = new BinaryReader(_fileStream);
        _fileWriter = new BinaryWriter(_fileStream);

        _itemDataManager = ItemDataManager.LoadFromFile(new Dictionary<string, int?>
        {
            { Path.Combine(pathToStracciatella, _pathToItemsJson), null},
            { Path.Combine(pathToStracciatella, _pathToMagazinesJson), Item.IC_AMMO },
            { Path.Combine(pathToStracciatella, _pathToWeaponsJson), Item.IC_GUN }
        });

        _log.Info($"Loading file {filename}");
    }

    private void ExtractSavedGameHeader()
    {
        _fileStream.Seek(0, SeekOrigin.Begin);
        _header = new SAVED_GAME_HEADER
        {
            uiSavedGameVersion = _fileReader.ReadUInt32()
        };

        _log.Debug($"uiSavedGameVersion = {_header.uiSavedGameVersion}");
        if (_header.uiSavedGameVersion != Ja2StracSavedGameVersion)
            throw new FormatException($"Supported only version {Ja2StracSavedGameVersion}");

        ParseSavedGameHeader();
    }

    public void Load()
    {
        ExtractSavedGameHeader();

        _encryptionSet = Encryption.CalcJa2EncryptionSet(_header);
        _log.Debug($"_encryptionSet = {_encryptionSet}");

        SetMercProfilesOffset();
        LoadMercProfiles();
        LoadSoldiers();
    }

    public void Save()
    {
        //SaveMercProfiles();
        SaveSoldiers();
        
        _fileWriter.Flush();
    }

    private void SaveSoldiers()
    {
        foreach (var soldier in Soldiers)
        {
            var soldierData = soldier.Serialize();
            var encryptedSoldierData =
                Encryption.NewJa2EncryptedFileWrite(_encryptionSet, soldierData, (uint) soldierData.Length);
            
            _fileWriter.BaseStream.Seek(soldier.SaveFileOffset.Start, SeekOrigin.Begin);
            _fileWriter.Write(encryptedSoldierData);
        }
    }

    private void LoadSoldiers()
    {
        Soldiers.Clear();

        for (var i = 0; i < Soldier.TOTAL_SOLDIERS; i++)
        {
            var active = _fileReader.ReadBoolean();
            if (!active) continue;

            var offset = new BinaryReaderOffsetItem
            {
                Start = _fileStream.Position,
                Type = typeof(Soldier)
            };

            var encryptedSoldierBytes = _fileReader.ReadBytes(Soldier.SOLDIER_SIZE);
            var soldierBytes = Encryption.NewJa2EncryptedFileRead(_encryptionSet, encryptedSoldierBytes, Soldier.SOLDIER_SIZE);

            offset.End = _fileStream.Position;
            var soldier = Soldier.Deserialize(soldierBytes, _itemDataManager, offset);

            uint uiNumOfNodes = _fileReader.ReadUInt32();
            var uiNumOfNodesSkipBytes = 20 * uiNumOfNodes;
            _fileReader.BaseStream.Seek(uiNumOfNodesSkipBytes, SeekOrigin.Current);

            byte hasKeyring = _fileReader.ReadByte();
            if (hasKeyring == 1)
                soldier.pKeyRing = _fileReader.ReadBytes(NumKeys * 2);

            Soldiers.Add(soldier);
        }
    }

    private void LoadMercProfiles()
    {
        MercProfiles.Clear();
        _fileStream.Seek(_mercProfilesOffset, SeekOrigin.Begin);

        for (var i = 0; i < MercProfilesCnt; i++)
        {
            var offset = new BinaryReaderOffsetItem
            {
                Start = _fileStream.Position,
                Type = typeof(MercProfile)
            };
            var encryptedMercProfileBytes = _fileReader.ReadBytes(MercProfile.MercProfileSize);
            var mercProfileBytes = Encryption.NewJa2EncryptedFileRead(_encryptionSet, encryptedMercProfileBytes, MercProfile.MercProfileSize);

            offset.End = _fileStream.Position;
            var mercProfile = MercProfile.Deserialize(mercProfileBytes, offset);
            _log.Debug($"Loaded merc profile {mercProfile.zNickname}");

            MercProfiles.Add(mercProfile);
        }
    }

    private void SetMercProfilesOffset()
    {
        _fileStream.Seek(StrategicEventsCntOffset, SeekOrigin.Begin);
        var strategicEventsCnt = _fileReader.ReadInt32();
        if (strategicEventsCnt is > 1000 or < 0)
        {
            throw new FormatException($"Internal Error: {nameof(strategicEventsCnt)} = {strategicEventsCnt}");
        }

        _mercProfilesOffset = StrategicEventsOffset + 
                              (uint)strategicEventsCnt * 28 +
                              LaptopInfoLength;

        _log.Debug($"{nameof(_mercProfilesOffset)} = {_mercProfilesOffset}");
    }

    private void ParseSavedGameHeader()
    {
        _header.sInitialGameOptions = new GAME_OPTIONS();

        _fileStream.Seek(0, SeekOrigin.Begin);

        _header.uiSavedGameVersion = _fileReader.ReadUInt32();

        var versionBytes = _fileReader.ReadBytes(SAVED_GAME_HEADER.GAME_VERSION_LENGTH);
        _header.zGameVersionNumber = Encoding.ASCII.GetString(versionBytes).TrimEnd('\0');
        _header.sSavedGameDesc = _fileReader.ReadUtf16Le(SAVED_GAME_HEADER.SIZE_OF_SAVE_GAME_DESC);

        _fileStream.Seek(4, SeekOrigin.Current);

        _header.uiDay = _fileReader.ReadUInt32();
        _header.ubHour = _fileReader.ReadByte();
        _header.ubMin = _fileReader.ReadByte();
        _header.sSector.x = _fileReader.ReadInt16();
        _header.sSector.y = _fileReader.ReadInt16();
        _header.sSector.z = _fileReader.ReadSByte();
        _header.ubNumOfMercsOnPlayersTeam = _fileReader.ReadByte();
        _header.iCurrentBalance = _fileReader.ReadInt32();
        _header.uiCurrentScreen = _fileReader.ReadUInt32();
        _header.fAlternateSector = _fileReader.ReadBoolean();
        _header.fWorldLoaded = _fileReader.ReadBoolean();
        _header.ubLoadScreenID = _fileReader.ReadByte();

        _header.sInitialGameOptions.fGunNut = _fileReader.ReadBoolean();
        _header.sInitialGameOptions.fSciFi = _fileReader.ReadBoolean();
        _header.sInitialGameOptions.ubDifficultyLevel = _fileReader.ReadByte();
        _header.sInitialGameOptions.fTurnTimeLimit = _fileReader.ReadBoolean();
        _header.sInitialGameOptions.ubGameSaveMode = _fileReader.ReadByte();

        _fileStream.Seek(8, SeekOrigin.Current);

        _header.uiRandom = _fileReader.ReadUInt32();
        _header.uiSaveStateSize = _fileReader.ReadUInt32();

        _fileStream.Seek(108, SeekOrigin.Current);

        _log.Debug($"iCurrentBalance = {_header.iCurrentBalance}");
        _log.Debug($"sSector.x sSector.y sSector.z = {_header.sSector.x} {_header.sSector.y} {_header.sSector.z}");
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
        _fileReader?.Dispose();
        _fileWriter?.Dispose();
    }
}
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable InconsistentNaming

namespace Ja2EditorLib.notWorking;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct SAVED_GAME_HEADER
{
    public const int GAME_VERSION_LENGTH = 16;
    public const int SIZE_OF_SAVE_GAME_DESC = 128;
    public const int ON_DISK_SIZE = 432;
    public const int ON_DISK_SIZE_STRAC_LIN = 688;

    public uint uiSavedGameVersion;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = GAME_VERSION_LENGTH)]
    public string zGameVersionNumber;

    // UTF-16 строка на 128 символов (256 байт) — в оригинале ST::string, но предполагаем сериализованную фикс. длину
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SIZE_OF_SAVE_GAME_DESC)]
    public string sSavedGameDesc;

    public uint uiDay;
    public byte ubHour;
    public byte ubMin;

    public SGPSector sSector;

    public byte ubNumOfMercsOnPlayersTeam;
    public int iCurrentBalance;
    public uint uiCurrentScreen;

    [MarshalAs(UnmanagedType.I1)] public bool fAlternateSector;

    [MarshalAs(UnmanagedType.I1)] public bool fWorldLoaded;

    public byte ubLoadScreenID;

    public GAME_OPTIONS sInitialGameOptions;

    public uint uiRandom;
    public uint uiSaveStateSize;

    // Заменяем ubFiller[110] → 110 байт padding
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 110)]
    public byte[] ubFiller;
}

public enum DifficultyLevel
{
    DIF_LEVEL_EASY = 1,
    DIF_LEVEL_MEDIUM = 2,
    DIF_LEVEL_HARD = 3,
    NUM_DIF_LEVELS = DIF_LEVEL_HARD
};

public static class SaveLoadGame
{
    public const int MERC_PROFILE_SIZE = 716;
    public const int MERC_PROFILE_SIZE_STRAC_LINUX = 796;
    public const int NUM_PROFILES = 170;

    public const int BASE_NUMBER_OF_ROTATION_ARRAYS = 19;

    public const int TACTICAL_STATUS_TYPE_SIZE = 316;
    public const int TACTICAL_STATUS_TYPE_SIZE_STRAC_LINUX = 360;

    public const int NAME_LENGTH = 30;
    public const int NICKNAME_LENGTH = 10;

    public static MERCPROFILESTRUCT[] gMercProfiles = new MERCPROFILESTRUCT[NUM_PROFILES];

    private static void ExtractGameOptions(DataReader d, ref GAME_OPTIONS g)
    {
        int start = d.GetConsumed();

        g.fGunNut = d.Read<bool>();
        g.fSciFi = d.Read<bool>();
        g.ubDifficultyLevel = d.ReadU8();
        g.fTurnTimeLimit = d.Read<bool>();
        g.ubGameSaveMode = d.ReadU8();

        d.Skip(7);

#if DEBUG
        Debug.Assert(d.GetConsumed() == start + 12);
#endif
    }

    public static DataReader ParseSavedGameHeader(byte[] data, ref SAVED_GAME_HEADER h, bool stracLinuxFormat)
    {
        var d = new DataReader(data);

        h.uiSavedGameVersion = d.ReadU32();

        var versionBytes = new byte[SAVED_GAME_HEADER.GAME_VERSION_LENGTH];
        LoadSaveMacros.EXTR_STR(d, versionBytes, SAVED_GAME_HEADER.GAME_VERSION_LENGTH);
        h.zGameVersionNumber = Encoding.ASCII.GetString(versionBytes).TrimEnd('\0');

        h.sSavedGameDesc = d.ReadString(SAVED_GAME_HEADER.SIZE_OF_SAVE_GAME_DESC, stracLinuxFormat);

        d.Skip(4);
        h.uiDay = d.ReadU32();
        h.ubHour = d.ReadU8();
        h.ubMin = d.ReadU8();
        h.sSector.x = d.Read<short>();
        h.sSector.y = d.Read<short>();
        h.sSector.z = d.Read<sbyte>();
        h.ubNumOfMercsOnPlayersTeam = d.ReadU8();
        h.iCurrentBalance = d.Read<int>();
        h.uiCurrentScreen = d.ReadU32();
        h.fAlternateSector = d.Read<bool>();
        h.fWorldLoaded = d.Read<bool>();
        h.ubLoadScreenID = d.ReadU8();

        ExtractGameOptions(d, ref h.sInitialGameOptions);

        d.Skip(1);
        h.uiRandom = d.ReadU32();

        if (h.uiSavedGameVersion >= 102)
        {
            h.uiSaveStateSize = d.ReadU32();
            d.Skip(108);
        }
        else
        {
            d.Skip(112);
        }

        return d;
    }

    public static bool IsValidSavedGameHeader(SAVED_GAME_HEADER h)
    {
        if (h.sSector.x == 0 && h.sSector.y == 0 && h.sSector.z == -1)
        {
            if (h.uiDay == 0 || h.iCurrentBalance < 0)
                return false;
        }
        else
        {
            if (h.uiDay == 0 || !h.sSector.IsValid() || h.iCurrentBalance < 0)
                return false;
        }

        return true;
    }

    public static byte[] ExtractSavedGameHeaderFromFile(string path, out SAVED_GAME_HEADER h, out bool stracLinuxFormat)
    {
        h = new SAVED_GAME_HEADER();

        byte[] data;

        // Try Stracciatella Linux format first
        try
        {
            data = File.ReadAllBytes(path);
            if (data.Length >= SAVED_GAME_HEADER.ON_DISK_SIZE_STRAC_LIN)
            {
                var linData = data.Take(SAVED_GAME_HEADER.ON_DISK_SIZE_STRAC_LIN).ToArray();
                ParseSavedGameHeader(linData, ref h, true);
                if (IsValidSavedGameHeader(h))
                {
                    stracLinuxFormat = true;
                    return data;
                }
            }
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch
        {
            ;
        }

        // Try Vanilla format
        stracLinuxFormat = false;
        data = File.ReadAllBytes(path);
        if (data.Length >= SAVED_GAME_HEADER.ON_DISK_SIZE)
        {
            var vanillaData = data.Take(SAVED_GAME_HEADER.ON_DISK_SIZE).ToArray();
            ParseSavedGameHeader(vanillaData, ref h, false);
        }

        return data;
    }

    public static uint SoldierProfileChecksum(MERCPROFILESTRUCT p)
    {
        uint sum = 1;

        sum += (uint)(1 + p.bLife);
        sum *= (uint)(1 + p.bLifeMax);
        sum += (uint)(1 + p.bAgility);
        sum *= (uint)(1 + p.bDexterity);
        sum += (uint)(1 + p.bStrength);
        sum *= (uint)(1 + p.bMarksmanship);
        sum += (uint)(1 + p.bMedical);
        sum *= (uint)(1 + p.bMechanical);
        sum += (uint)(1 + p.bExplosive);
        sum *= (uint)(1 + p.bExpLevel);

        if (p.inv != null)
        {
            foreach (ushort val in p.inv)
                sum += val;
        }

        if (p.bInvNumber != null)
        {
            foreach (byte val in p.bInvNumber)
                sum += val;
        }

        return sum;
    }

    public static void CalcJA2EncryptionSet(SAVED_GAME_HEADER h)
    {
        var set = (uint)h.iCurrentBalance;
        set *= (uint)(h.ubNumOfMercsOnPlayersTeam + 1);
        set += (uint)(h.sSector.z * 3);
        set += h.ubLoadScreenID;

        if (h.fAlternateSector)
        {
            set += 7;
        }

        uint r = h.uiRandom;
        if (r % 2 == 0)
        {
            set++;
            if (r % 7 == 0)
            {
                set++;
                if (r % 23 == 0) set++;
                if (r % 79 == 0) set += 2;
            }
        }

        if (IsGermanVersion())
        {
            set *= 11;
        }

        set %= BASE_NUMBER_OF_ROTATION_ARRAYS;
        set += h.uiDay / 10;
        set %= BASE_NUMBER_OF_ROTATION_ARRAYS;

        GAME_OPTIONS o = h.sInitialGameOptions;
        if (o.fGunNut) set += BASE_NUMBER_OF_ROTATION_ARRAYS * 6;
        if (o.fSciFi) set += BASE_NUMBER_OF_ROTATION_ARRAYS * 3;

        switch (o.ubDifficultyLevel)
        {
            case (byte)DifficultyLevel.DIF_LEVEL_EASY:
                set += 0;
                break;
            case (byte)DifficultyLevel.DIF_LEVEL_MEDIUM:
                set += BASE_NUMBER_OF_ROTATION_ARRAYS;
                break;
            case (byte)DifficultyLevel.DIF_LEVEL_HARD:
                set += BASE_NUMBER_OF_ROTATION_ARRAYS * 2;
                break;
        }

        Debug.Assert(set < BASE_NUMBER_OF_ROTATION_ARRAYS * 12);

        throw new Exception("No");
        //TacticalSave.GuiJa2EncryptionSet = set;
    }

    private static bool IsGermanVersion()
    {
        return false;
    }

    public static void ExtractMercProfile(
        byte[] src,
        ref MERCPROFILESTRUCT p,
        bool stracLinuxFormat,
        out uint checksum,
        bool isCorrectlyEncoded)
    {
        var S = new DataReader(src);


        if (isCorrectlyEncoded)
        {
            p.zName = S.ReadString(NAME_LENGTH, stracLinuxFormat);
            p.zNickname = S.ReadString(NICKNAME_LENGTH, stracLinuxFormat);
        }
        else
        {
            p.zName = S.ReadUTF16(NAME_LENGTH, false);
            p.zNickname = S.ReadUTF16(NICKNAME_LENGTH, false);
        }

        checksum = 1;
    }

    /*
    public static void LoadSavedMercProfiles(DataReader reader, uint savegameVersion, bool stracLinuxFormat)
    {
        // Выбираем функцию чтения: до версии 87 включительно — старая
        Action<DataReader, byte[], uint> fileReader = savegameVersion < 87
            ? TacticalSave.Ja2EncryptedFileRead
            : TacticalSave.NewJa2EncryptedFileRead;

        var dataSize = stracLinuxFormat
            ? MERC_PROFILE_SIZE_STRAC_LINUX
            : MERC_PROFILE_SIZE;

        for (var i = 0; i < NUM_PROFILES; i++)
        {
            var buffer = new byte[dataSize];

            fileReader(reader, buffer, (uint)dataSize);

            var profile = new MERCPROFILESTRUCT();
            ExtractMercProfile(buffer, ref profile, stracLinuxFormat, out uint checksum, true);

            if (checksum != SoldierProfileChecksum(profile))
            {
                throw new InvalidDataException($"Merc profile checksum mismatch at index {i}");
            }

            gMercProfiles[i] = profile;
        }
    }
    */
    public static void LoadSavedGame(string saveFilename)
    {
        var data = ExtractSavedGameHeaderFromFile(saveFilename, out var stracWinHeader, out var stracWinLinux);
        if (data == null) throw new Exception("DataReader is null");
        CalcJA2EncryptionSet(stracWinHeader);

        var reader = new DataReader(data);

        // skip header
        reader.Skip(stracWinLinux ? SAVED_GAME_HEADER.ON_DISK_SIZE_STRAC_LIN : SAVED_GAME_HEADER.ON_DISK_SIZE);

        // skip tactical status
        reader.Skip(stracWinLinux ? TACTICAL_STATUS_TYPE_SIZE_STRAC_LINUX : TACTICAL_STATUS_TYPE_SIZE);
        reader.Skip(5);

        // skip time
        reader.Skip(62);

        var strategiesCnt = (int)reader.ReadU32();
        var strategiesBytesCnt = strategiesCnt * 28;
        reader.Skip(strategiesBytesCnt);

        // skip laptop
        reader.Skip(7440);

        //LoadSavedMercProfiles(reader, stracWinHeader.uiSavedGameVersion, stracWinLinux);
        ;
    }
}
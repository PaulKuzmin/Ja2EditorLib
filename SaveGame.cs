using System.Collections;
using Ja2EditorLib.notWorking;

// ReSharper disable InconsistentNaming

namespace Ja2EditorLib;

public class SaveGame
{
    public const int SAVED_GAME_HEADER_Offset = 0;
    public const int SAVED_GAME_HEADER_Length_99 = 432;
    public const int SAVED_GAME_HEADER_Length_103 = 436;
    public const int uiSavedGameVersion_Offset = 0;
    public const int uiSavedGameVersion_Length = 4;
    public const int uiDay_Offset = 280;
    public const int ubHour_Offset = 284;
    public const int ubMin_Offset = 285;
    public const int iCurrentBalance_Offset = 292;
    public const int sInitialGameOptions_Offset = 303;
    public const int sInitialGameOptions_Length_99 = 12;
    public const int sInitialGameOptions_Length_103 = 14;
    public const int uiRandom_Offset = 320;
    public const int ubInventorySystem_Offset_103 = 324;
    public const int TacticalStatusType_Offset_99 = 432;
    public const int TacticalStatusType_Offset_103 = 436;
    public const int TacticalStatusType_Length = 316;
    public const int gWorldSectorXYZ_Offset_99 = 748;
    public const int gWorldSectorXYZ_Offset_103 = 752;
    public const int gWorldSectorXYZ_Length = 5;
    public const int GameClock_Offset_99 = 753;
    public const int GameClock_Offset_103 = 757;
    public const int GameClock_Length = 62;
    public const int uiNumGameEvents_Offset_99 = 815;
    public const int uiNumGameEvents_Offset_103 = 819;
    public const int STRATEGICEVENT_Offset_99 = 819;
    public const int STRATEGICEVENT_Offset_103 = 823;
    public const int STRATEGICEVENT_Length = 28;
    public const int MAXITEMS_99 = 351;
    public const int MAXITEMS_103 = 5001;
    public const int LaptopSaveInfoStruct_Length_99 = 7440;
    public const int LaptopSaveInfoStruct_Length_103 = 81840;
    public const int BobbyRayInventory_RelOffset = 1654;
    public const int usNumberOfBobbyRayOrderItems_RelOffset_99 = 7276;
    public const int usNumberOfBobbyRayOrderItems_RelOffset_103 = 81676;
    public const int BobbyRayOrderStruct_Length = 84;
    public const int ubNumberLifeInsurancePayouts_RelOffset_99 = 7284;
    public const int ubNumberLifeInsurancePayouts_RelOffset_103 = 81684;
    public const int MERCPROFILESTRUCT_Count = 170;
    public const int MERCPROFILESTRUCT_Length_99 = 716;
    public const int MERCPROFILESTRUCT_Length_103 = 632;
    public const int MERCPROFILESTRUCT_invsize_RelOffset_103 = 632;
    public const int TOTAL_SOLDIERS = 156;
    public const int SOLDIERTYPE_Length_99 = 2328;
    public const int SOLDIERTYPE_Length_103 = 1072;
    public const int SOLDIERTYPE_TrailingLength_103 = 525;
    public const int PathSt_Length = 20;
    public const int NUM_KEYS = 64;
    public const int KEY_ON_RING_Length = 2;
    public const int OBJECTTYPE_Length_103 = 5;
    public const int STACKEDOBJECTDATA_Length_103 = 16;
    public const int LBENODE_Length_103 = 16;

    public string filename;
    public FileStream fileStream;
    public BinaryReader fileReader;
    public BinaryWriter fileWriter;
    public int codeTableIdx;
    public int codeTableSubIdx;
    public int[] codeTable;
    public int uiSavedGameVersion;
    public int STRATEGICEVENT_Count;
    public int LaptopSaveInfoStruct_Offset;
    public int usNumberOfBobbyRayOrderItems;
    public int ubNumberLifeInsurancePayouts;
    public int actorCount;
    public int actorOffset;
    public Actor[] actors;
    public int mercCount;
    public int mercOffset;
    public Mercenary[] mercs;
    public int SAVED_GAME_HEADER_Length;
    public int sInitialGameOptionsLength;
    public int TacticalStatusType_Offset;
    public int gWorldSectorXYZ_Offset;
    public int GameClock_Offset;
    public int uiNumGameEvents_Offset;
    public int STRATEGICEVENT_Offset;
    public int MAXITEMS;
    public int LaptopSaveInfoStruct_Length;
    public int usNumberOfBobbyRayOrderItems_RelOffset;
    public int ubNumberLifeInsurancePayouts_RelOffset;
    public int MERCPROFILESTRUCT_Length;
    public int SOLDIERTYPE_Length;

    public SaveGame()
    {
        this.uiSavedGameVersion = 0;
        this.STRATEGICEVENT_Count = 0;
        this.LaptopSaveInfoStruct_Offset = 0;
        this.usNumberOfBobbyRayOrderItems = 0;
        this.ubNumberLifeInsurancePayouts = 0;
        this.actorCount = 170;
        this.actorOffset = 0;
        this.actors = new Actor[170];
        this.mercCount = 0;
        this.mercOffset = 0;
        this.mercs = new Mercenary[156];
    }

    public void Load(string filename)
    {
        this.filename = filename;
        this.fileStream = new FileStream(this.filename, FileMode.Open, FileAccess.ReadWrite);
        this.fileReader = new BinaryReader(fileStream);
        this.fileWriter = new BinaryWriter(fileStream);

        try
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            this.uiSavedGameVersion = this.ReadIntLE(this.fileReader);
            Console.WriteLine($"Read saved game {this.filename}, version={this.uiSavedGameVersion}");

            if (this.uiSavedGameVersion >= 95 && this.uiSavedGameVersion <= 99)
            {
                this.SAVED_GAME_HEADER_Length = 432;
                this.sInitialGameOptionsLength = 12;
                this.TacticalStatusType_Offset = 432;
                this.gWorldSectorXYZ_Offset = 748;
                this.GameClock_Offset = 753;
                this.uiNumGameEvents_Offset = 815;
                this.STRATEGICEVENT_Offset = 819;
                this.MAXITEMS = 351;
                this.LaptopSaveInfoStruct_Length = 7440;
                this.usNumberOfBobbyRayOrderItems_RelOffset = 7276;
                this.ubNumberLifeInsurancePayouts_RelOffset = 7284;
                this.MERCPROFILESTRUCT_Length = 716;
                this.SOLDIERTYPE_Length = 2328;
            }
            else if (this.uiSavedGameVersion == 102)
            {
                this.SAVED_GAME_HEADER_Length = 688;
                this.sInitialGameOptionsLength = 12;
                this.TacticalStatusType_Offset = 432;
                this.gWorldSectorXYZ_Offset = 748;
                this.GameClock_Offset = 753;
                this.uiNumGameEvents_Offset = 815;
                this.STRATEGICEVENT_Offset = 819;
                this.MAXITEMS = 351;

                this.LaptopSaveInfoStruct_Length = 7440;
                this.usNumberOfBobbyRayOrderItems_RelOffset = 7276;
                this.ubNumberLifeInsurancePayouts_RelOffset = 7284;
                this.MERCPROFILESTRUCT_Length = 716;//796
                this.SOLDIERTYPE_Length = 2328;
            }
            else
            {
                if (this.uiSavedGameVersion is < 103 or > 103)
                {
                    throw new FormatException(
                        $"Save game version {this.uiSavedGameVersion} not supported. Supported values are 95, 96, 97, 97, 99, 102 and 103");
                }

                this.SAVED_GAME_HEADER_Length = 436;
                this.sInitialGameOptionsLength = 14;
                this.TacticalStatusType_Offset = 436;
                this.gWorldSectorXYZ_Offset = 752;
                this.GameClock_Offset = 757;
                this.uiNumGameEvents_Offset = 819;
                this.STRATEGICEVENT_Offset = 823;
                this.MAXITEMS = 5001;
                this.LaptopSaveInfoStruct_Length = 81840;
                this.usNumberOfBobbyRayOrderItems_RelOffset = 81676;
                this.ubNumberLifeInsurancePayouts_RelOffset = 81684;
                this.MERCPROFILESTRUCT_Length = 632;
                this.SOLDIERTYPE_Length = 1072;
            }

            //
            var header = new SAVED_GAME_HEADER();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            var headerBytes = this.fileReader.ReadBytes(this.SAVED_GAME_HEADER_Length);
            SaveLoadGame.ParseSavedGameHeader(headerBytes, ref header, true);
            ;
            //

            this.fileStream.Seek(this.uiNumGameEvents_Offset, SeekOrigin.Begin);
            this.STRATEGICEVENT_Count = this.ReadIntLE(this.fileReader);
            if (this.STRATEGICEVENT_Count < 0 || this.STRATEGICEVENT_Count > 1000)
            {
                throw new FormatException($"Internal Error: STRATEGICEVENT_Count = {this.STRATEGICEVENT_Count}");
            }

            //
            Console.WriteLine($"STRATEGICEVENT_Count = {this.STRATEGICEVENT_Count}");
            Console.WriteLine($"STRATEGICEVENT_Count * 28 = {this.STRATEGICEVENT_Count * 28}");
            Console.WriteLine($"LaptopSaveInfoStruct_Length = {this.LaptopSaveInfoStruct_Length}");
            //

            this.actorOffset = this.STRATEGICEVENT_Offset + this.STRATEGICEVENT_Count * 28 +
                               this.LaptopSaveInfoStruct_Length;

            //
            Console.WriteLine($"actorOffset = {this.actorOffset}");
            //

            //this.fileStream.Seek(this.actorOffset, SeekOrigin.Begin);
            var setId = TacticalSave.CalcJA2EncryptionSet(header);
            /* test

            byte[] encrypted = this.fileReader.ReadBytes(this.MERCPROFILESTRUCT_Length);
            var setId = TacticalSave.CalcJA2EncryptionSet(header);

            if (File.Exists("test.txt")) File.Delete("test.txt");
            for (int i = 0; i < TacticalSave.GetRotationArrayLenth(); i++)
            {
                var r = TacticalSave.NewJa2EncryptedFileRead(i, encrypted, (uint)l);

                File.AppendAllText("test.txt", $"{i}");
                File.AppendAllText("test.txt", Encoding.Unicode.GetString(r));
                File.AppendAllText("test.txt", "\r\n");
            }

            var r = TacticalSave.NewJa2EncryptedFileRead(setId, encrypted, (uint)this.MERCPROFILESTRUCT_Length);
            File.WriteAllBytes("xxx.bin", r);
            ;
            //

            byte[] rawData = this.fileReader.ReadBytes(16);
            int[] indexes = this.FindActorCodeTable(rawData);
            if (indexes == null)
            {
                throw new FormatException("Unable to determine code table");
            }

            this.codeTableIdx = indexes[0];
            this.codeTableSubIdx = indexes[1];
            if (indexes[0] == -1)
            {
                this.codeTable = null;
            }
            else
            {
                this.codeTable = JapeConst.CodeTables[this.codeTableIdx][this.codeTableSubIdx];
            }
            */

            this.LoadActors(setId);
            //this.mercOffset = (int)this.fileStream.Position;
            //this.LoadMercs();
        }
        finally
        {
            this.fileReader?.Close();
            this.fileWriter?.Close();
            this.fileStream?.Close();
        }
    }

    public void LoadActors(int setId)
    {
        this.fileStream.Seek(this.actorOffset, SeekOrigin.Begin);
        for (int idx = 0; idx < 170; ++idx)
        {
            using (MemoryStream actorData = new MemoryStream())
            {
                byte[] mercProfileStructData = this.fileReader.ReadBytes(this.MERCPROFILESTRUCT_Length);
                actorData.Write(mercProfileStructData, 0, mercProfileStructData.Length);

                //File.WriteAllBytes("xxx.bin", actorData.ToArray());

                /*
                if (this.uiSavedGameVersion == 103)
                {
                    int inventorySize = this.ReadIntLE(this.fileReader);
                    this.WriteIntLE(actorData, inventorySize);
                    if (inventorySize < 0 || inventorySize > 1000)
                    {
                        throw new FormatException(
                            $"Invalid inventory size {inventorySize} for MERCPROFILESTRUCT {idx}");
                    }

                    for (int itemIdx = 0; itemIdx < inventorySize; ++itemIdx)
                    {
                        byte[] inventoryItem = this.fileReader.ReadBytes(12);
                        actorData.Write(inventoryItem, 0, inventoryItem.Length);
                    }
                }
                */

                var rawData =
                    TacticalSave.NewJa2EncryptedFileRead(setId, actorData.ToArray(), (uint)this.MERCPROFILESTRUCT_Length);

                Actor actor = new Actor(rawData, this.uiSavedGameVersion);
                Console.WriteLine($"Read MERCPROFILE '{actor.get("Nickname")}'");
                actor.validateChecksum();
                Console.WriteLine($"Read MERCPROFILE '{actor.get("Nickname")}'");
                this.actors[idx] = actor;
            }
        }
    }

    public void LoadMercs()
    {
        this.fileStream.Seek(this.mercOffset, SeekOrigin.Begin);
        int mercIdx;

        for (mercIdx = 0; mercIdx < 156; ++mercIdx, ++this.mercOffset)
        {
            byte initialActive = this.fileReader.ReadByte();
            if (initialActive != 0)
            {
                this.fileStream.Seek(-1, SeekOrigin.Current);
                break;
            }
        }

        while (mercIdx < 156)
        {
            int trailingOffset = 0;
            using (MemoryStream mercData = new MemoryStream())
            {
                byte ubActive = this.fileReader.ReadByte();
                mercData.WriteByte(ubActive);

                if (ubActive != 1)
                {
                    throw new FormatException($"ubActive = {ubActive}");
                }

                byte[] soldierData = this.fileReader.ReadBytes(this.SOLDIERTYPE_Length);
                mercData.Write(soldierData, 0, soldierData.Length);

                int inventorySum = 0;
                if (this.uiSavedGameVersion == 103)
                {
                    inventorySum = this.InventoryLoad_103(this.fileReader, mercData);
                    trailingOffset = (int)mercData.Position - 1;
                    byte[] trailingData = this.fileReader.ReadBytes(525);
                    mercData.Write(trailingData, 0, trailingData.Length);
                }

                int uiNumOfNodes = this.ReadIntLE(this.fileReader);
                this.WriteIntLE(mercData, uiNumOfNodes);
                if (uiNumOfNodes < 0 || uiNumOfNodes > 1000)
                {
                    throw new FormatException($"Internal error: uiNumOfNodes={uiNumOfNodes}");
                }

                byte[] pathStData = this.fileReader.ReadBytes(uiNumOfNodes * 20);
                mercData.Write(pathStData, 0, pathStData.Length);

                byte ubOne = this.fileReader.ReadByte();
                mercData.WriteByte(ubOne);
                if (ubOne != 1 && ubOne != 0)
                {
                    throw new FormatException($"ubOne = {ubOne}");
                }

                if (ubOne == 1)
                {
                    byte[] keyRingData = this.fileReader.ReadBytes(64 * 2);
                    mercData.Write(keyRingData, 0, keyRingData.Length);
                }

                while (mercIdx < 156)
                {
                    byte nextActive = this.fileReader.ReadByte();
                    if (nextActive != 0)
                    {
                        this.fileStream.Seek(-1, SeekOrigin.Current);
                        break;
                    }

                    ++mercIdx;
                    mercData.WriteByte(nextActive);
                }

                Mercenary merc = new Mercenary(mercData.ToArray(), this.uiSavedGameVersion, trailingOffset,
                    inventorySum, this.codeTable);
                Console.WriteLine($"Read SOLDIERTYPE '{merc.get("Nickname")}'");
                merc.validateChecksum();
                this.mercs[this.mercCount] = merc;
                ++this.mercCount;
                ++mercIdx;

                //

                foreach (DictionaryEntry field in merc.fields)
                {
                    Console.WriteLine($"{field.Key} = {merc.get((string)field.Key)}");
                }
            }
        }
    }

    public void Save()
    {
        this.fileStream = new FileStream(this.filename, FileMode.Open, FileAccess.ReadWrite);
        this.fileWriter = new BinaryWriter(fileStream);

        try
        {
            this.SaveActors();
            this.SaveMercs();
        }
        finally
        {
            this.fileWriter?.Close();
            this.fileStream?.Close();
        }
    }

    public void SaveActors()
    {
        this.fileStream.Seek(this.actorOffset, SeekOrigin.Begin);
        for (int idx = 0; idx < 170; ++idx)
        {
            Actor actor = this.actors[idx];
            byte[] actorData = actor.encode(this.codeTable);
            this.fileWriter.Write(actorData);
        }
    }

    public void SaveMercs()
    {
        this.fileStream.Seek(this.mercOffset, SeekOrigin.Begin);
        for (int idx = 0; idx < this.mercCount; ++idx)
        {
            Mercenary merc = this.mercs[idx];
            byte[] mercData = merc.encode(this.codeTable);
            this.fileWriter.Write(mercData);
        }
    }

    public int[] FindActorCodeTable(byte[] ciphertext)
    {
        for (int mainTable = 0; mainTable < 4; ++mainTable)
        {
            for (int subTable = 0; subTable < 57; ++subTable)
            {
                byte[] plaintext = JapeAlg.Decode(ciphertext, 8, JapeConst.CodeTables[mainTable][subTable]);
                if (plaintext[1] == 0 && plaintext[3] == 0 && plaintext[5] == 0 && plaintext[7] == 0)
                {
                    return new int[] { mainTable, subTable };
                }
            }
        }

        if (ciphertext[1] == 0 && ciphertext[3] == 0 && ciphertext[5] == 0 && ciphertext[7] == 0)
        {
            return new int[] { -1, -1 };
        }

        return null;
    }

    public Actor GetActor(int idx)
    {
        return this.actors[idx];
    }

    public Mercenary GetMerc(int idx)
    {
        return this.mercs[idx];
    }

    public Actor GetActorByNick(string nick)
    {
        for (int idx = 0; idx < 170; ++idx)
        {
            Actor actor = this.actors[idx];
            if (actor != null && actor.get("Nickname").Equals(nick))
            {
                return actor;
            }
        }

        return null;
    }

    public Mercenary GetMercByNick(string nick)
    {
        for (int idx = 0; idx < this.mercCount; ++idx)
        {
            Mercenary merc = this.mercs[idx];
            if (merc != null && merc.get("Nickname").Equals(nick))
            {
                return merc;
            }
        }

        return null;
    }

    /*
    public int ReadIntLE(BinaryReader d)
    {
        int i = d.ReadInt32();
        return (i & 0xFF) << 24 | (i & 0xFF00) << 8 | (i & 0xFF0000) >> 8 | (i & 0xFF000000) >> 24;
    }
    */

    public int ReadIntLE(BinaryReader d)
    {
        return (int)d.ReadUInt32();
    }

    public void WriteIntLE(Stream os, int i)
    {
        os.WriteByte((byte)(i & 0xFF));
        os.WriteByte((byte)((i & 0xFF00) >> 8));
        os.WriteByte((byte)((i & 0xFF0000) >> 16));
        os.WriteByte((byte)((i & 0xFF000000) >> 24));
    }

    public int InventoryLoad_103(BinaryReader file, Stream mercData)
    {
        int inventorySize = this.ReadIntLE(file);
        this.WriteIntLE(mercData, inventorySize);
        if (inventorySize < 0 || inventorySize > 55)
        {
            throw new FormatException($"Invalid inventory size {inventorySize}");
        }

        int sum = 0;
        for (int itemIdx = 0; itemIdx < inventorySize; ++itemIdx)
        {
            sum += this.ObjecttypeLoad_103(file, mercData);
            int bNewItemCount = this.ReadIntLE(file);
            this.WriteIntLE(mercData, bNewItemCount);
            int bNewItemCycleCount = this.ReadIntLE(file);
            this.WriteIntLE(mercData, bNewItemCycleCount);
        }

        return sum;
    }

    public int ObjecttypeLoad_103(BinaryReader file, Stream mercData)
    {
        byte[] obj = file.ReadBytes(5);
        mercData.Write(obj, 0, obj.Length);
        ShortField usItem = new ShortField(0);
        ByteField ubNumberOfObjects = new ByteField(2);
        int sum = usItem.getInt(obj) + ubNumberOfObjects.getInt(obj);

        int objStackSize = this.ReadIntLE(file);
        this.WriteIntLE(mercData, objStackSize);
        if (objStackSize < 0 || objStackSize > 55)
        {
            throw new FormatException($"Invalid object stack size {objStackSize}");
        }

        for (int idx = 0; idx < objStackSize; ++idx)
        {
            this.StackedObjectDataLoad_103(file, mercData);
        }

        return sum;
    }

    public void StackedObjectDataLoad_103(BinaryReader file, Stream mercData)
    {
        byte[] sob = file.ReadBytes(16);
        mercData.Write(sob, 0, sob.Length);

        int attachmentsSize = this.ReadIntLE(file);
        this.WriteIntLE(mercData, attachmentsSize);
        if (attachmentsSize < 0 || attachmentsSize > 55)
        {
            throw new FormatException($"Invalid attachment size {attachmentsSize}");
        }

        for (int idx = 0; idx < attachmentsSize; ++idx)
        {
            this.ObjecttypeLoad_103(file, mercData);
        }
    }

    public static void Main(string[] args)
    {
        DirectoryInfo saveDir = new DirectoryInfo("Saves");
        FileInfo[] filenames = saveDir.GetFiles();
        for (int idx = 0; idx < filenames.Length; ++idx)
        {
            FileInfo filename = filenames[idx];
            if (filename.Name.EndsWith(".sav"))
            {
                SaveGame saveGame = new SaveGame();
                try
                {
                    saveGame.Load(filename.FullName);
                }
                catch (IOException e)
                {
                    Console.Error.WriteLine(e);
                }
                catch (FormatException e2)
                {
                    Console.Error.WriteLine(e2);
                }
            }
        }
    }
}
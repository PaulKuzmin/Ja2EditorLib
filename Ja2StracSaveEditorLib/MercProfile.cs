using System.ComponentModel;
using Ja2StracSaveEditorLib.Structures;
using Ja2StracSaveEditorLib.Utilities;

// ReSharper disable InconsistentNaming

namespace Ja2StracSaveEditorLib;

public class MercProfile : INotifyPropertyChanged
{
    public const int MercProfileSize = 716;

    private const int NAME_LENGTH = 30;
    private const int NICKNAME_LENGTH = 10;
    private const int PaletteRepID_LENGTH = 30;

#pragma warning disable CS0067 // The event is never used
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

    public string zName { get; set; }
    public string zNickname { get; set; }
    public sbyte bSex { get; set; }
    public byte ubCivilianGroup { get; set; }
    public byte ubMiscFlags { get; set; }
    public byte ubMiscFlags2 { get; set; }
    public byte ubMiscFlags3 { get; set; }
    public byte ubBodyType { get; set; }

    public uint uiBodyTypeSubFlags { get; set; } // BODY TYPE SUBSITUTIONS

    /* Portrait */
    public byte ubFaceIndex { get; set; } // overwritten with the char's ID on profile load
    public ushort usEyesX { get; set; }
    public ushort usEyesY { get; set; }
    public ushort usMouthX { get; set; }
    public ushort usMouthY { get; set; }
    public uint uiBlinkFrequency { get; set; } // { 3000 };
    public uint uiExpressionFrequency { get; set; } // { 2000 };

    public string PANTS { get; set; }
    public string VEST { get; set; }
    public string SKIN { get; set; }

    public string HAIR { get; set; }

    /* stats */
    public sbyte bEvolution { get; set; }

    public sbyte bLifeMax { get; set; } // { 15 };
    public sbyte bLife { get; set; } // { 15 };
    public sbyte bAgility { get; set; } // { 1 }; // agility (speed) value
    public sbyte bDexterity { get; set; } // { 1 }; // dexterity (hand coord) value
    public sbyte bStrength { get; set; } // { 1 };
    public sbyte bLeadership { get; set; } // { 1 };
    public sbyte bWisdom { get; set; } // { 1 };
    public sbyte bExpLevel { get; set; } // { 1 }; // general experience level
    public sbyte bMarksmanship { get; set; } // { 0 };
    public sbyte bExplosive { get; set; } // { 0 };
    public sbyte bMechanical { get; set; } //  { 0 };
    public sbyte bMedical { get; set; } //  { 0 };
    public byte ubNeedForSleep { get; set; } //  { 7 };

    public short sLifeGain { get; set; }
    public sbyte bLifeDelta { get; set; }
    public short sAgilityGain { get; set; }
    public sbyte bAgilityDelta { get; set; }
    public short sDexterityGain { get; set; }
    public sbyte bDexterityDelta { get; set; }
    public short sStrengthGain { get; set; }
    public sbyte bStrengthDelta { get; set; }
    public short sLeadershipGain { get; set; }
    public sbyte bLeadershipDelta { get; set; }
    public short sWisdomGain { get; set; }
    public sbyte bWisdomDelta { get; set; }
    public short sExpLevelGain { get; set; }
    public sbyte bExpLevelDelta { get; set; }
    public short sMarksmanshipGain { get; set; }
    public sbyte bMarksmanshipDelta { get; set; }
    public short sExplosivesGain { get; set; }
    public sbyte bExplosivesDelta { get; set; }
    public short sMechanicGain { get; set; }
    public sbyte bMechanicDelta { get; set; }
    public short sMedicalGain { get; set; }
    public sbyte bMedicalDelta { get; set; }

    public ushort[] usStatChangeChances { get; set; } = new ushort[12]; // used strictly for balancing, never shown!
    public ushort[] usStatChangeSuccesses { get; set; } = new ushort[12]; // used strictly for balancing, never shown!

    public sbyte bPersonalityTrait { get; set; }
    public sbyte bSkillTrait { get; set; }
    public sbyte bSkillTrait2 { get; set; }
    public sbyte bAttitude { get; set; }
    public byte bSexist { get; set; }

    /* Contract */
    public sbyte
        bMercStatus
    {
        get;
        set;
    } //The status of the merc. If negative, see flags at the top of this file. Positive: The number of days the merc is away for. 0: Not hired but ready to be.

    public sbyte bReputationTolerance { get; set; }
    public sbyte bDeathRate { get; set; }
    public uint uiDayBecomesAvailable { get; set; } //day the merc will be available. used with the bMercStatus
    public short sSalary { get; set; }
    public uint uiWeeklySalary { get; set; }
    public uint uiBiWeeklySalary { get; set; }
    public sbyte bMedicalDeposit { get; set; } // Is medical deposit required?
    public ushort sMedicalDepositAmount { get; set; }

    public int
        iMercMercContractLength
    {
        get;
        set;
    } //Used for MERC mercs, specifies how many days the merc has gone since last page

    public ushort usOptionalGearCost { get; set; }
    public byte ubSuspiciousDeath { get; set; }

    public byte ubDaysOfMoraleHangover { get; set; } // used only when merc leaves team while having poor morale

    /* Locations */
    public SGPSector sSector { get; set; }
    public short sGridNo { get; set; } // The Gridno the NPC was in before leaving the sector
    public short sPreCombatGridNo { get; set; }
    public byte ubStrategicInsertionCode { get; set; }
    public ushort usStrategicInsertionData { get; set; }
    public bool fUseProfileInsertionInfo { get; set; } // Set to various flags, ( contained in TacticalSave.h )
    public sbyte bTown { get; set; }
    public sbyte bTownAttachment { get; set; }
    public byte[] ubRoomRangeStart { get; set; } = new byte[2];
    public byte[] ubRoomRangeEnd { get; set; } = new byte[2];

    public sbyte[] bBuddy { get; set; } =
        new sbyte[]
        {
            -1, -1, -1, -1, -1
        }; // Only indices 0, 1, 2 are used. Contain id's for friend1, friend2 and eventual friend respectively

    public sbyte[] bHated { get; set; } =
        new sbyte[]
        {
            -1, -1, -1, -1, -1
        }; // Only indices 0, 1, 2 are used. Contain id's for enemy1, enemy2 and eventual enemy respectively

    public sbyte[] bHatedCount { get; set; } =
        new sbyte[5]; // Only indices 0, 1, 2 are used. Contain remaining decrements till contract termination due to an enemy present on the team

    public sbyte[] bHatedTime { get; set; } =
        new sbyte[5]; // Only indices 0, 1, 2 are used. Contain decrements till contract termination due to an enemy present on the team

    public sbyte bLearnToLike { get; set; } // { -1 }; // eventual friend's id

    public sbyte
        bLearnToLikeCount { get; set; } // remaining decrements till the eventual friend becomes an actual friend

    public sbyte bLearnToLikeTime { get; set; } // how many decrements till the eventual friend becomes an actual friend
    public sbyte bLearnToHate { get; set; } // { -1 }; // eventual enemy's id
    public sbyte bLearnToHateCount { get; set; } // remaining decrements till the eventual enemy becomes an actual enemy

    public sbyte bLearnToHateTime { get; set; } // how many decrements till the eventual enemy becomes an actual enemy

    // Flags used for the precedent to repeating oneself in Contract negotiations. Used for quote 80 - ~107. Gets reset every day
    public byte ubTimeTillNextHatedComplaint { get; set; }

    public sbyte[] bMercOpinion { get; set; } = new sbyte[75];

    public ushort[] inv { get; set; } = new ushort[19];
    public byte[] bInvNumber { get; set; } = new byte[19];
    public byte[] bInvStatus { get; set; } = new byte[19];
    public byte ubInvUndroppable { get; set; }
    public uint uiMoney { get; set; }
    public sbyte bArmourAttractiveness { get; set; }
    public sbyte bMainGunAttractiveness { get; set; }

    public int iBalance { get; set; } // if negative the player owes money to this NPC (e.g. for Skyrider's services)

    public byte
        ubNumTimesDrugUseInLifetime { get; set; } // The # times a drug has been used in the player's lifetime...

    /* Specific quest or script related */
    public sbyte bNPCData { get; set; } // NPC specific

    public sbyte bNPCData2 { get; set; } // NPC specific

    /* Dialogue and script records */
    public byte ubQuoteRecord { get; set; }
    public byte ubLastQuoteSaid { get; set; }
    public uint uiPrecedentQuoteSaid { get; set; }
    public byte bLastQuoteSaidWasSpecial { get; set; }
    public byte ubLastDateSpokenTo { get; set; }
    public byte ubQuoteActionID { get; set; }
    public sbyte bFriendlyOrDirectDefaultResponseUsedRecently { get; set; }
    public sbyte bRecruitDefaultResponseUsedRecently { get; set; }
    public sbyte bThreatenDefaultResponseUsedRecently { get; set; }

    public sbyte bApproached { get; set; }
    public ushort[] usApproachFactor { get; set; } = new ushort[4];
    public byte[] ubApproachVal { get; set; } = new byte[4];

    //public byte[,] ubApproachMod { get; set; } = new byte[3,4];
    public byte[] ubApproachMod { get; set; } = new byte[12];

    /* Statistics */
    public ushort usKills { get; set; }
    public ushort usAssists { get; set; }
    public ushort usShotsFired { get; set; }
    public ushort usShotsHit { get; set; }
    public ushort usBattlesFought { get; set; }
    public ushort usTimesWounded { get; set; }
    public ushort usTotalDaysServed { get; set; }

    public uint
        uiTotalCostToDate { get; set; } // The total amount of money that has been paid to the merc for their salary

    public sbyte bSectorZ { get; set; } // unused
    public sbyte bRace { get; set; } // unused
    public sbyte bRacist { get; set; } // unused
    public sbyte bNationality { get; set; } // unused
    public sbyte bAppearance { get; set; } // unused
    public sbyte bAppearanceCareLevel { get; set; } // unused
    public sbyte bRefinement { get; set; } // unused
    public sbyte bRefinementCareLevel { get; set; } // unused
    public sbyte bHatedNationality { get; set; } // unused
    public sbyte bHatedNationalityCareLevel { get; set; } // unused

    //

    public uint CheckSum { get; set; }

    public BinaryReaderOffsetItem SaveFileOffset { get; private init; }

    public static MercProfile Deserialize(byte[] data, BinaryReaderOffsetItem offset)
    {
        using var memoryStream = new MemoryStream(data);
        using var reader = new BinaryReader(memoryStream);

        var p = new MercProfile
        {
            SaveFileOffset = offset,
            zName = reader.ReadUtf16Le(NAME_LENGTH),
            zNickname = reader.ReadUtf16Le(NICKNAME_LENGTH)
        };

        reader.Skip(28);
        p.ubFaceIndex = reader.ReadByte();

        p.PANTS = reader.ReadUtf8(PaletteRepID_LENGTH);
        p.VEST = reader.ReadUtf8(PaletteRepID_LENGTH);
        p.SKIN = reader.ReadUtf8(PaletteRepID_LENGTH);
        p.HAIR = reader.ReadUtf8(PaletteRepID_LENGTH);

        p.bSex = reader.ReadSByte();

        p.bArmourAttractiveness = reader.ReadSByte();
        p.ubMiscFlags2 = reader.ReadByte();
        p.bEvolution = reader.ReadSByte();
        p.ubMiscFlags = reader.ReadByte();
        p.bSexist = reader.ReadByte();
        p.bLearnToHate = reader.ReadSByte();

        reader.Skip(2);

        p.ubQuoteRecord = reader.ReadByte();
        p.bDeathRate = reader.ReadSByte();


        reader.Skip(2);

        p.sExpLevelGain = reader.ReadInt16();
        p.sLifeGain = reader.ReadInt16();
        p.sAgilityGain = reader.ReadInt16();
        p.sDexterityGain = reader.ReadInt16();
        p.sWisdomGain = reader.ReadInt16();
        p.sMarksmanshipGain = reader.ReadInt16();
        p.sMedicalGain = reader.ReadInt16();
        p.sMechanicGain = reader.ReadInt16();
        p.sExplosivesGain = reader.ReadInt16();

        p.ubBodyType = reader.ReadByte();
        p.bMedical = reader.ReadSByte();

        p.usEyesX = reader.ReadUInt16();
        p.usEyesY = reader.ReadUInt16();
        p.usMouthX = reader.ReadUInt16();
        p.usMouthY = reader.ReadUInt16();


        reader.Skip(10);

        p.uiBlinkFrequency = reader.ReadUInt32();
        p.uiExpressionFrequency = reader.ReadUInt32();

        var sectorX = reader.ReadUInt16();
        var sectorY = reader.ReadUInt16();

        p.uiDayBecomesAvailable = reader.ReadUInt32();

        p.bStrength = reader.ReadSByte();
        p.bLifeMax = reader.ReadSByte();
        p.bExpLevelDelta = reader.ReadSByte();
        p.bLifeDelta = reader.ReadSByte();
        p.bAgilityDelta = reader.ReadSByte();
        p.bDexterityDelta = reader.ReadSByte();
        p.bWisdomDelta = reader.ReadSByte();
        p.bMarksmanshipDelta = reader.ReadSByte();
        p.bMedicalDelta = reader.ReadSByte();
        p.bMechanicDelta = reader.ReadSByte();
        p.bExplosivesDelta = reader.ReadSByte();
        p.bStrengthDelta = reader.ReadSByte();
        p.bLeadershipDelta = reader.ReadSByte();


        reader.Skip(1);

        p.usKills = reader.ReadUInt16();
        p.usAssists = reader.ReadUInt16();
        p.usShotsFired = reader.ReadUInt16();
        p.usShotsHit = reader.ReadUInt16();
        p.usBattlesFought = reader.ReadUInt16();
        p.usTimesWounded = reader.ReadUInt16();
        p.usTotalDaysServed = reader.ReadUInt16();

        p.sLeadershipGain = reader.ReadInt16();
        p.sStrengthGain = reader.ReadInt16();

        p.uiBodyTypeSubFlags = reader.ReadUInt32();

        p.sSalary = reader.ReadInt16();

        p.bLife = reader.ReadSByte();
        p.bDexterity = reader.ReadSByte();

        p.bPersonalityTrait = reader.ReadSByte();
        p.bSkillTrait = reader.ReadSByte();
        p.bReputationTolerance = reader.ReadSByte();
        p.bExplosive = reader.ReadSByte();
        p.bSkillTrait2 = reader.ReadSByte();
        p.bLeadership = reader.ReadSByte();

        p.bBuddy = reader.ReadSbytes(5);
        p.bHated = reader.ReadSbytes(5);

        p.bExpLevel = reader.ReadSByte();
        p.bMarksmanship = reader.ReadSByte();

        reader.Skip(1);

        p.bWisdom = reader.ReadSByte();

        reader.Skip(2);

        p.bInvStatus = reader.ReadBytes(19);
        p.bInvNumber = reader.ReadBytes(19);
        p.usApproachFactor = reader.ReadUshorts(4);

        p.bMainGunAttractiveness = reader.ReadSByte();
        p.bAgility = reader.ReadSByte();
        p.fUseProfileInsertionInfo = reader.ReadBoolean();

        reader.Skip(1);

        p.sGridNo = reader.ReadInt16();
        p.ubQuoteActionID = reader.ReadByte();
        p.bMechanical = reader.ReadSByte();
        p.ubInvUndroppable = reader.ReadByte();

        p.ubRoomRangeStart = reader.ReadBytes(2);

        reader.Skip(1);

        p.inv = reader.ReadUshorts(19);

        reader.Skip(20);

        p.usStatChangeChances = reader.ReadUshorts(12);
        p.usStatChangeSuccesses = reader.ReadUshorts(12);

        p.ubStrategicInsertionCode = reader.ReadByte();

        p.ubRoomRangeEnd = reader.ReadBytes(2);

        reader.Skip(4);

        p.ubLastQuoteSaid = reader.ReadByte();
        p.bRace = reader.ReadSByte();
        p.bNationality = reader.ReadSByte();
        p.bAppearance = reader.ReadSByte();
        p.bAppearanceCareLevel = reader.ReadSByte();
        p.bRefinement = reader.ReadSByte();
        p.bRefinementCareLevel = reader.ReadSByte();
        p.bHatedNationality = reader.ReadSByte();
        p.bHatedNationalityCareLevel = reader.ReadSByte();
        p.bRacist = reader.ReadSByte();

        reader.Skip(1);

        p.uiWeeklySalary = reader.ReadUInt32();
        p.uiBiWeeklySalary = reader.ReadUInt32();
        p.bMedicalDeposit = reader.ReadSByte();
        p.bAttitude = reader.ReadSByte();


        reader.Skip(2);

        p.sMedicalDepositAmount = reader.ReadUInt16();
        p.bLearnToLike = reader.ReadSByte();


        p.ubApproachVal = reader.ReadBytes(4);

        p.ubApproachMod = reader.ReadBytes(12);
        //EXTR_U8A(S, *p.ubApproachMod, sizeof(p.ubApproachMod) / sizeof(**p.ubApproachMod))

        p.bTown = reader.ReadSByte();
        p.bTownAttachment = reader.ReadSByte();

        reader.Skip(1);

        p.usOptionalGearCost = reader.ReadUInt16();

        p.bMercOpinion = reader.ReadSbytes(75);


        p.bApproached = reader.ReadSByte();
        p.bMercStatus = reader.ReadSByte();


        p.bHatedTime = reader.ReadSbytes(5);

        p.bLearnToLikeTime = reader.ReadSByte();
        p.bLearnToHateTime = reader.ReadSByte();


        p.bHatedCount = reader.ReadSbytes(5);

        p.bLearnToLikeCount = reader.ReadSByte();
        p.bLearnToHateCount = reader.ReadSByte();
        p.ubLastDateSpokenTo = reader.ReadByte();
        p.bLastQuoteSaidWasSpecial = reader.ReadByte();


        var sectorZ = reader.ReadSByte();

        p.usStrategicInsertionData = reader.ReadUInt16();
        p.bFriendlyOrDirectDefaultResponseUsedRecently = reader.ReadSByte();
        p.bRecruitDefaultResponseUsedRecently = reader.ReadSByte();
        p.bThreatenDefaultResponseUsedRecently = reader.ReadSByte();
        p.bNPCData = reader.ReadSByte();
        p.iBalance = reader.ReadInt32();


        reader.Skip(2);

        p.ubCivilianGroup = reader.ReadByte();
        p.ubNeedForSleep = reader.ReadByte();
        p.uiMoney = reader.ReadUInt32();
        p.bNPCData2 = reader.ReadSByte();
        p.ubMiscFlags3 = reader.ReadByte();
        p.ubDaysOfMoraleHangover = reader.ReadByte();
        p.ubNumTimesDrugUseInLifetime = reader.ReadByte();
        p.uiPrecedentQuoteSaid = reader.ReadUInt32();

        p.CheckSum = reader.ReadUInt32();

        p.sPreCombatGridNo = reader.ReadInt16();
        p.ubTimeTillNextHatedComplaint = reader.ReadByte();
        p.ubSuspiciousDeath = reader.ReadByte();
        p.iMercMercContractLength = reader.ReadInt32();
        p.uiTotalCostToDate = reader.ReadUInt32();


        reader.Skip(4);

        p.sSector = new SGPSector((short)sectorX, (short)sectorY, sectorZ);

        if (MercProfileSize != reader.BaseStream.Position)
            throw new Exception($"MercProfile must be {MercProfileSize} bytes, but read {reader.BaseStream.Position}");

        var checksum = Encryption.SoldierProfileChecksum(p);
        if (p.CheckSum != checksum)
            throw new Exception($"Wrong checksum. Must be {p.CheckSum}, calculated {checksum}");

        return p;
    }
}
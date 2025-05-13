using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Ja2EditorLib.notWorking;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MERCPROFILESTRUCT
{
    public string zName;

    public string zNickname;

    public sbyte bSex;
    public byte ubCivilianGroup;

    public byte ubMiscFlags;
    public byte ubMiscFlags2;
    public byte ubMiscFlags3;

    public byte ubBodyType;
    public uint uiBodyTypeSubFlags;

    public byte ubFaceIndex;
    public ushort usEyesX;
    public ushort usEyesY;
    public ushort usMouthX;
    public ushort usMouthY;
    public uint uiBlinkFrequency;
    public uint uiExpressionFrequency;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] PANTS;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] VEST;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] SKIN;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] HAIR;

    public sbyte bEvolution;

    public sbyte bLifeMax;
    public sbyte bLife;
    public sbyte bAgility;
    public sbyte bDexterity;
    public sbyte bStrength;
    public sbyte bLeadership;
    public sbyte bWisdom;
    public sbyte bExpLevel;
    public sbyte bMarksmanship;
    public sbyte bExplosive;
    public sbyte bMechanical;
    public sbyte bMedical;
    public byte ubNeedForSleep;

    public short sLifeGain;
    public sbyte bLifeDelta;
    public short sAgilityGain;
    public sbyte bAgilityDelta;
    public short sDexterityGain;
    public sbyte bDexterityDelta;
    public short sStrengthGain;
    public sbyte bStrengthDelta;
    public short sLeadershipGain;
    public sbyte bLeadershipDelta;
    public short sWisdomGain;
    public sbyte bWisdomDelta;
    public short sExpLevelGain;
    public sbyte bExpLevelDelta;
    public short sMarksmanshipGain;
    public sbyte bMarksmanshipDelta;
    public short sExplosivesGain;
    public sbyte bExplosivesDelta;
    public short sMechanicGain;
    public sbyte bMechanicDelta;
    public short sMedicalGain;
    public sbyte bMedicalDelta;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public ushort[] usStatChangeChances;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public ushort[] usStatChangeSuccesses;

    public sbyte bPersonalityTrait;
    public sbyte bSkillTrait;
    public sbyte bSkillTrait2;
    public sbyte bAttitude;
    public byte bSexist;

    public sbyte bMercStatus;
    public sbyte bReputationTolerance;
    public sbyte bDeathRate;
    public uint uiDayBecomesAvailable;
    public short sSalary;
    public uint uiWeeklySalary;
    public uint uiBiWeeklySalary;
    public sbyte bMedicalDeposit;
    public ushort sMedicalDepositAmount;
    public int iMercMercContractLength;
    public ushort usOptionalGearCost;
    public byte ubSuspiciousDeath;
    public byte ubDaysOfMoraleHangover;

    public SGPSector sSector;
    public short sGridNo;
    public short sPreCombatGridNo;
    public byte ubStrategicInsertionCode;
    public ushort usStrategicInsertionData;
    [MarshalAs(UnmanagedType.I1)] public bool fUseProfileInsertionInfo;
    public sbyte bTown;
    public sbyte bTownAttachment;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public byte[] ubRoomRangeStart;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public byte[] ubRoomRangeEnd;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public sbyte[] bBuddy;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public sbyte[] bHated;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public sbyte[] bHatedCount;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public sbyte[] bHatedTime;

    public sbyte bLearnToLike;
    public sbyte bLearnToLikeCount;
    public sbyte bLearnToLikeTime;
    public sbyte bLearnToHate;
    public sbyte bLearnToHateCount;
    public sbyte bLearnToHateTime;
    public byte ubTimeTillNextHatedComplaint;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 75)]
    public sbyte[] bMercOpinion;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
    public ushort[] inv;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
    public byte[] bInvNumber;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
    public byte[] bInvStatus;

    public byte ubInvUndroppable;
    public uint uiMoney;
    public sbyte bArmourAttractiveness;
    public sbyte bMainGunAttractiveness;

    public int iBalance;
    public byte ubNumTimesDrugUseInLifetime;

    public sbyte bNPCData;
    public sbyte bNPCData2;

    public byte ubQuoteRecord;
    public byte ubLastQuoteSaid;
    public uint uiPrecedentQuoteSaid;
    public byte bLastQuoteSaidWasSpecial;
    public byte ubLastDateSpokenTo;
    public byte ubQuoteActionID;
    public sbyte bFriendlyOrDirectDefaultResponseUsedRecently;
    public sbyte bRecruitDefaultResponseUsedRecently;
    public sbyte bThreatenDefaultResponseUsedRecently;

    public sbyte bApproached;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public ushort[] usApproachFactor;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] ubApproachVal;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public byte[] ubApproachMod;

    public ushort usKills;
    public ushort usAssists;
    public ushort usShotsFired;
    public ushort usShotsHit;
    public ushort usBattlesFought;
    public ushort usTimesWounded;
    public ushort usTotalDaysServed;
    public uint uiTotalCostToDate;

    public sbyte bSectorZ;
    public sbyte bRace;
    public sbyte bRacist;
    public sbyte bNationality;
    public sbyte bAppearance;
    public sbyte bAppearanceCareLevel;
    public sbyte bRefinement;
    public sbyte bRefinementCareLevel;
    public sbyte bHatedNationality;
    public sbyte bHatedNationalityCareLevel;
}

public static class SoldierProfileType
{
    public const int NUM_PROFILES = 170;
    public const int NUM_RECRUITABLE = 75;

    public const int NAME_LENGTH = 30;
    public const int NICKNAME_LENGTH = 10;

    public const byte PROFILE_MISC_FLAG_RECRUITED = 0x01;
    public const byte PROFILE_MISC_FLAG_HAVESEENCREATURE = 0x02;
    public const byte PROFILE_MISC_FLAG_FORCENPCQUOTE = 0x04;
    public const byte PROFILE_MISC_FLAG_WOUNDEDBYPLAYER = 0x08;
    public const byte PROFILE_MISC_FLAG_TEMP_NPC_QUOTE_DATA_EXISTS = 0x10;
    public const byte PROFILE_MISC_FLAG_SAID_HOSTILE_QUOTE = 0x20;
    public const byte PROFILE_MISC_FLAG_EPCACTIVE = 0x40;
    public const byte PROFILE_MISC_FLAG_ALREADY_USED_ITEMS = 0x80;

    public const byte PROFILE_MISC_FLAG2_DONT_ADD_TO_SECTOR = 0x01;
    public const byte PROFILE_MISC_FLAG2_LEFT_COUNTRY = 0x02;
    public const byte PROFILE_MISC_FLAG2_BANDAGED_TODAY = 0x04;
    public const byte PROFILE_MISC_FLAG2_SAID_FIRSTSEEN_QUOTE = 0x08;
    public const byte PROFILE_MISC_FLAG2_NEEDS_TO_SAY_HOSTILE_QUOTE = 0x10;
    public const byte PROFILE_MISC_FLAG2_MARRIED_TO_HICKS = 0x20;
    public const byte PROFILE_MISC_FLAG2_ASKED_BY_HICKS = 0x40;

    public const byte PROFILE_MISC_FLAG3_PLAYER_LEFT_MSG_FOR_MERC_AT_AIM = 0x01;
    public const byte PROFILE_MISC_FLAG3_PERMANENT_INSERTION_CODE = 0x02;
    public const byte PROFILE_MISC_FLAG3_PLAYER_HAD_CHANCE_TO_HIRE = 0x04;
    public const byte PROFILE_MISC_FLAG3_HANDLE_DONE_TRAVERSAL = 0x08;
    public const byte PROFILE_MISC_FLAG3_NPC_PISSED_OFF = 0x10;
    public const byte PROFILE_MISC_FLAG3_MERC_MERC_IS_DEAD_AND_QUOTE_SAID = 0x20;
    public const byte PROFILE_MISC_FLAG3_TOWN_DOESNT_CARE_ABOUT_DEATH = 0x40;
    public const byte PROFILE_MISC_FLAG3_GOODGUY = 0x80;

    public const int MERC_OK = 0;
    public const int MERC_HAS_NO_TEXT_FILE = -1;
    public const int MERC_ANNOYED_BUT_CAN_STILL_CONTACT = -2;
    public const int MERC_ANNOYED_WONT_CONTACT = -3;
    public const int MERC_HIRED_BUT_NOT_ARRIVED_YET = -4;
    public const int MERC_IS_DEAD = -5;
    public const int MERC_RETURNING_HOME = -6;
    public const int MERC_WORKING_ELSEWHERE = -7;
    public const int MERC_FIRED_AS_A_POW = -8;

    public const int SUPER_STAT_VALUE = 80;
    public const int NEEDS_TRAINING_STAT_VALUE = 50;
    public const int NO_CHANCE_IN_HELL_STAT_VALUE = 40;

    public const int SUPER_SKILL_VALUE = 80;
    public const int NEEDS_TRAINING_SKILL_VALUE = 50;
    public const int NO_CHANCE_IN_HELL_SKILL_VALUE = 0;

    public const int NERVOUS_RADIUS = 10;

    public const int BUDDY_OPINION = +25;
    public const int HATED_OPINION = -25;

    public static readonly sbyte[] gbSkillTraitBonus = new sbyte[]
    {
        0,  // NO_SKILLTRAIT
        25, // LOCKPICKING
        15, // HANDTOHAND
        15, // ELECTRONICS
        15, // NIGHTOPS
        12, // THROWING
        15, // TEACHING
        15, // HEAVY_WEAPS
        0,  // AUTO_WEAPS
        15, // STEALTHY
        0,  // AMBIDEXT
        0,  // THIEF
        30, // MARTIALARTS
        30, // KNIFING
        15, // ONROOF
        0   // CAMOUFLAGED
    };
}

public enum SkillTrait
{
    NO_SKILLTRAIT = 0,
    LOCKPICKING,
    HANDTOHAND,
    ELECTRONICS,
    NIGHTOPS,
    THROWING,
    TEACHING,
    HEAVY_WEAPS,
    AUTO_WEAPS,
    STEALTHY,
    AMBIDEXT,
    THIEF,
    MARTIALARTS,
    KNIFING,
    ONROOF,
    CAMOUFLAGED,
    NUM_SKILLTRAITS
}

public enum PersonalityTrait
{
    NO_PERSONALITYTRAIT = 0,
    HEAT_INTOLERANT,
    NERVOUS,
    CLAUSTROPHOBIC,
    NONSWIMMER,
    FEAR_OF_INSECTS,
    FORGETFUL,
    PSYCHO
}

public enum Attitudes
{
    ATT_NORMAL = 0,
    ATT_FRIENDLY,
    ATT_LONER,
    ATT_OPTIMIST,
    ATT_PESSIMIST,
    ATT_AGGRESSIVE,
    ATT_ARROGANT,
    ATT_BIG_SHOT,
    ATT_ASSHOLE,
    ATT_COWARD,
    NUM_ATTITUDES
}

public enum Sexes
{
    MALE = 0,
    FEMALE
}

public enum SexistLevels
{
    NOT_SEXIST = 0,
    SOMEWHAT_SEXIST,
    VERY_SEXIST,
    GENTLEMAN
}

public enum CharacterEvolution
{
    NORMAL_EVOLUTION = 0,
    NO_EVOLUTION,
    DEVOLVE
}

public enum BuddySlot
{
    BUDDY_NOT_FOUND = -1,
    BUDDY_SLOT1,
    BUDDY_SLOT2,
    LEARNED_TO_LIKE_SLOT,
    NUM_BUDDY_SLOTS
}

public enum HatedSlot
{
    HATED_NOT_FOUND = -1,
    HATED_SLOT1,
    HATED_SLOT2,
    LEARNED_TO_HATE_SLOT,
    NUM_HATED_SLOTS
}

public static class ProfileHelper
{
    public static bool IsBuddy(this MERCPROFILESTRUCT prof, int bud) =>
        prof.bBuddy[0] == bud || prof.bBuddy[1] == bud || prof.bBuddy[2] == bud;

    public static bool IsHated(this MERCPROFILESTRUCT prof, int hat) =>
        prof.bHated[0] == hat || prof.bHated[1] == hat || prof.bHated[2] == hat;
}
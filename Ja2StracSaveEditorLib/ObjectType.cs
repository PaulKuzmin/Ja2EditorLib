﻿using Ja2StracSaveEditorLib.Utilities;
using System.ComponentModel;
using Ja2StracSaveEditorLib.Managers;

// ReSharper disable InconsistentNaming

namespace Ja2StracSaveEditorLib;

public enum ITEMDEFINE
{
    NONE = 0,
    NOTHING = NONE,

    // weapons
    GLOCK_17,
    GLOCK_18,
    __ITEM_3, // BERETTA_92F,
    __ITEM_4, // BERETTA_93R,
    SW38,
    __ITEM_6, // BARRACUDA,
    DESERTEAGLE,
    __ITEM_8, // M1911,
    __ITEM_9, // MP5K,
    __ITEM_10, // MAC10,

    __ITEM_11, // THOMPSON,
    __ITEM_12, // COMMANDO,
    __ITEM_13, // MP53,
    __ITEM_14, // AKSU74,
    __ITEM_15, // P90,
    __ITEM_16, // TYPE85,
    __ITEM_17, // SKS,
    __ITEM_18, // DRAGUNOV,
    __ITEM_19, // M24,
    __ITEM_20, // AUG,

    G41,
    __ITEM_22, // MINI14,
    __ITEM_23, // C7,
    __ITEM_24, // FAMAS,
    __ITEM_25, // AK74,
    __ITEM_26, // AKM,
    __ITEM_27, // M14,
    __ITEM_28, // FNFAL,
    __ITEM_29, // G3A3,
    G11,

    __ITEM_31, // M870,
    __ITEM_32, // SPAS15,
    __ITEM_33, // CAWS,
    MINIMI,
    __ITEM_35, // RPK74,
    __ITEM_36, // HK21E,
    COMBAT_KNIFE,
    THROWING_KNIFE,
    ROCK,
    GLAUNCHER,

    MORTAR,
    ROCK2,
    CREATURE_YOUNG_MALE_CLAWS,
    CREATURE_OLD_MALE_CLAWS,
    CREATURE_YOUNG_FEMALE_CLAWS,
    CREATURE_OLD_FEMALE_CLAWS,
    CREATURE_QUEEN_TENTACLES,
    CREATURE_QUEEN_SPIT,
    BRASS_KNUCKLES,
    UNDER_GLAUNCHER,

    ROCKET_LAUNCHER,
    BLOODCAT_CLAW_ATTACK,
    BLOODCAT_BITE,
    __ITEM_54, // MACHETE,
    ROCKET_RIFLE,
    AUTOMAG_III,
    CREATURE_INFANT_SPIT,
    CREATURE_YOUNG_MALE_SPIT,
    CREATURE_OLD_MALE_SPIT,
    TANK_CANNON,

    DART_GUN,
    BLOODY_THROWING_KNIFE,
    FLAMETHROWER,
    CROWBAR,
    AUTO_ROCKET_RIFLE,
    __ITEM_66, // NOTHING,
    __ITEM_67, // NOTHING,
    __ITEM_68, // NOTHING,
    __ITEM_69, // NOTHING,
    __ITEM_70, // NOTHING,

    CLIP9_15,
    CLIP9_30,
    __ITEM_73, // CLIP9_15_AP,
    __ITEM_74, // CLIP9_30_AP,
    __ITEM_75, // CLIP9_15_HP,
    __ITEM_76, // CLIP9_30_HP,
    CLIP38_6,
    __ITEM_78, // CLIP38_6_AP,
    __ITEM_79, // CLIP38_6_HP,
    CLIP45_7,

    CLIP45_30,
    __ITEM_82, // CLIP45_7_AP,
    __ITEM_83, // CLIP45_30_AP,
    __ITEM_84, // CLIP45_7_HP,
    __ITEM_85, // CLIP45_30_HP,
    CLIP357_6,
    CLIP357_9,
    __ITEM_88, // CLIP357_6_AP,
    __ITEM_89, // CLIP357_9_AP,
    __ITEM_90, // CLIP357_6_HP,

    __ITEM_91, // CLIP357_9_HP,
    __ITEM_92, // CLIP545_30_AP,
    CLIP545_30_HP,
    __ITEM_94, // CLIP556_30_AP,
    CLIP556_30_HP,
    __ITEM_96, // CLIP762W_10_AP,
    __ITEM_97, // CLIP762W_30_AP,
    CLIP762W_10_HP,
    CLIP762W_30_HP,
    CLIP762N_5_AP,

    __ITEM_101, // CLIP762N_20_AP,
    CLIP762N_5_HP,
    CLIP762N_20_HP,
    __ITEM_104, // CLIP47_50_SAP,
    __ITEM_105, // CLIP57_50_AP,
    __ITEM_106, // CLIP57_50_HP,
    CLIP12G_7,
    CLIP12G_7_BUCKSHOT,
    __ITEM_109, // CLIPCAWS_10_SAP,
    __ITEM_110, // CLIPCAWS_10_FLECH,

    __ITEM_111, // CLIPROCKET_AP,
    __ITEM_112, // CLIPROCKET_HE,
    __ITEM_113, // CLIPROCKET_HEAT,
    __ITEM_114, // CLIPDART_SLEEP,

    __ITEM_115, // CLIPFLAME,
    __ITEM_116, // NOTHING
    __ITEM_117, // NOTHING
    __ITEM_118, // NOTHING
    __ITEM_119, // NOTHING
    __ITEM_120, // NOTHING
    __ITEM_121, // NOTHING
    __ITEM_122, // NOTHING
    __ITEM_123, // NOTHING
    __ITEM_124, // NOTHING
    __ITEM_125, // NOTHING
    __ITEM_126, // NOTHING
    __ITEM_127, // NOTHING
    __ITEM_128, // NOTHING
    __ITEM_129, // NOTHING
    __ITEM_130, // NOTHING

    // explosives
    STUN_GRENADE,
    TEARGAS_GRENADE,
    MUSTARD_GRENADE,
    MINI_GRENADE,
    HAND_GRENADE,
    RDX,
    TNT,
    HMX,
    C1,
    MORTAR_SHELL,

    MINE,
    C4,
    TRIP_FLARE,
    TRIP_KLAXON,
    SHAPED_CHARGE,
    BREAK_LIGHT,
    GL_HE_GRENADE,
    GL_TEARGAS_GRENADE,
    GL_STUN_GRENADE,
    GL_SMOKE_GRENADE,

    SMOKE_GRENADE,
    TANK_SHELL,
    STRUCTURE_IGNITE,
    __ITEM_154, // CREATURE_COCKTAIL,
    STRUCTURE_EXPLOSION,
    GREAT_BIG_EXPLOSION,
    BIG_TEAR_GAS,
    SMALL_CREATURE_GAS,
    LARGE_CREATURE_GAS,
    VERY_SMALL_CREATURE_GAS,

    // armor
    FLAK_JACKET,
    FLAK_JACKET_18,
    FLAK_JACKET_Y,
    KEVLAR_VEST,
    KEVLAR_VEST_18,
    KEVLAR_VEST_Y,
    SPECTRA_VEST,
    SPECTRA_VEST_18,
    SPECTRA_VEST_Y,
    KEVLAR_LEGGINGS,

    KEVLAR_LEGGINGS_18,
    KEVLAR_LEGGINGS_Y,
    SPECTRA_LEGGINGS,
    SPECTRA_LEGGINGS_18,
    SPECTRA_LEGGINGS_Y,
    STEEL_HELMET,
    KEVLAR_HELMET,
    KEVLAR_HELMET_18,
    KEVLAR_HELMET_Y,
    SPECTRA_HELMET,

    SPECTRA_HELMET_18,
    SPECTRA_HELMET_Y,
    CERAMIC_PLATES,
    CREATURE_INFANT_HIDE,
    CREATURE_YOUNG_MALE_HIDE,
    CREATURE_OLD_MALE_HIDE,
    CREATURE_QUEEN_HIDE,
    LEATHER_JACKET,
    LEATHER_JACKET_W_KEVLAR,
    LEATHER_JACKET_W_KEVLAR_18,

    LEATHER_JACKET_W_KEVLAR_Y,
    CREATURE_YOUNG_FEMALE_HIDE,
    CREATURE_OLD_FEMALE_HIDE,
    TSHIRT,
    TSHIRT_DEIDRANNA,
    KEVLAR2_VEST,
    KEVLAR2_VEST_18,
    KEVLAR2_VEST_Y,
    __ITEM_199,
    __ITEM_200,

    // kits
    FIRSTAIDKIT,
    MEDICKIT,
    TOOLKIT,
    LOCKSMITHKIT,
    CAMOUFLAGEKIT,
    __ITEM_206, // BOOBYTRAPKIT,
    SILENCER,
    SNIPERSCOPE,
    BIPOD,
    EXTENDEDEAR,

    NIGHTGOGGLES,
    SUNGOGGLES,
    GASMASK,
    CANTEEN,
    METALDETECTOR,
    COMPOUND18,
    JAR_QUEEN_CREATURE_BLOOD,
    JAR_ELIXIR,
    MONEY,
    JAR,

    JAR_CREATURE_BLOOD,
    ADRENALINE_BOOSTER,
    DETONATOR,
    REMDETONATOR,
    __ITEM_225, // VIDEOTAPE,
    DEED,
    LETTER,
    __ITEM_228, // TERRORIST_INFO,
    CHALICE,
    BLOODCAT_CLAWS,

    BLOODCAT_TEETH,
    BLOODCAT_PELT,
    SWITCH,
    ACTION_ITEM,
    REGEN_BOOSTER,
    SYRINGE_3,
    SYRINGE_4,
    SYRINGE_5,
    JAR_HUMAN_BLOOD,
    OWNERSHIP,

    // additional items
    LASERSCOPE,
    REMOTEBOMBTRIGGER,
    WIRECUTTERS,
    DUCKBILL,
    ALCOHOL,
    UVGOGGLES,
    DISCARDED_LAW,
    HEAD_1,
    HEAD_2,
    HEAD_3,
    HEAD_4,
    HEAD_5,
    HEAD_6,
    HEAD_7,
    WINE,
    BEER,
    __ITEM_257, // PORNOS,
    VIDEO_CAMERA,
    ROBOT_REMOTE_CONTROL,
    CREATURE_PART_CLAWS,
    CREATURE_PART_FLESH,
    CREATURE_PART_ORGAN,
    REMOTETRIGGER,
    GOLDWATCH,
    __ITEM_265, // GOLFCLUBS,
    WALKMAN,
    __ITEM_267, // PORTABLETV,
    MONEY_FOR_PLAYERS_ACCOUNT,
    CIGARS,
    __ITEM_270, // NOTHING

    KEY_1,
    __ITEM_272, // KEY_2,
    __ITEM_273, // KEY_3,
    __ITEM_274, // KEY_4,
    __ITEM_275, // KEY_5,
    __ITEM_276, // KEY_6,
    __ITEM_277, // KEY_7,
    __ITEM_278, // KEY_8,
    __ITEM_279, // KEY_9,
    __ITEM_280, // KEY_10,

    __ITEM_281, // KEY_11,
    __ITEM_282, // KEY_12,
    __ITEM_283, // KEY_13,
    __ITEM_284, // KEY_14,
    __ITEM_285, // KEY_15,
    __ITEM_286, // KEY_16,
    __ITEM_287, // KEY_17,
    __ITEM_288, // KEY_18,
    __ITEM_289, // KEY_19,
    __ITEM_290, // KEY_20,

    __ITEM_291, // KEY_21,
    __ITEM_292, // KEY_22,
    __ITEM_293, // KEY_23,
    __ITEM_294, // KEY_24,
    __ITEM_295, // KEY_25,
    __ITEM_296, // KEY_26,
    __ITEM_297, // KEY_27,
    __ITEM_298, // KEY_28,
    __ITEM_299, // KEY_29,
    __ITEM_300, // KEY_30,

    __ITEM_301, // KEY_31,
    KEY_32,
    __ITEM_302, // SILVER_PLATTER,
    DUCT_TAPE,
    ALUMINUM_ROD,
    SPRING,
    SPRING_AND_BOLT_UPGRADE,
    STEEL_ROD,
    QUICK_GLUE,
    GUN_BARREL_EXTENDER,

    STRING,
    TIN_CAN,
    STRING_TIED_TO_TIN_CAN,
    MARBLES,
    LAME_BOY,
    COPPER_WIRE,
    DISPLAY_UNIT,
    FUMBLE_PAK,
    XRAY_BULB,
    CHEWING_GUM,

    FLASH_DEVICE,
    BATTERIES,
    __ITEM_323, // ELASTIC,
    XRAY_DEVICE,
    SILVER,
    GOLD,
    GAS_CAN,
    __ITEM_328, // UNUSED_26,
    __ITEM_329, // UNUSED_27,
    __ITEM_330, // UNUSED_28,

    __ITEM_331, // UNUSED_29,
    __ITEM_332, // UNUSED_30,
    __ITEM_333, // UNUSED_31,
    __ITEM_334, // UNUSED_32,
    __ITEM_335, // UNUSED_33,
    __ITEM_336, // UNUSED_34,
    __ITEM_337, // UNUSED_35,
    __ITEM_338, // UNUSED_36,
    __ITEM_339, // UNUSED_37,
    __ITEM_340, // UNUSED_38,

    __ITEM_341, // UNUSED_39,
    __ITEM_342, // UNUSED_40,
    __ITEM_343, // UNUSED_41,
    __ITEM_344, // UNUSED_42,
    __ITEM_345, // UNUSED_43,
    __ITEM_346, // UNUSED_44,
    __ITEM_347, // UNUSED_45,
    __ITEM_348, // UNUSED_46,
    __ITEM_349, // UNUSED_47,
    __ITEM_350, // UNUSED_48,

    MAXITEMS
};

public struct KeyOnRing
{
    public const byte INVALID_KEY_NUMBER = 255;

    public byte ubKeyID;
    public byte ubNumber;

    public KeyOnRing(byte keyID = INVALID_KEY_NUMBER, byte number = 0)
    {
        ubKeyID = keyID;
        ubNumber = number;
    }

    public bool IsValid()
    {
        return ubKeyID != INVALID_KEY_NUMBER && ubNumber != 0;
    }
}

public class ObjectType : INotifyPropertyChanged
{
    public const int MAX_OBJECTS_PER_SLOT = 8;
    public const int MAX_ATTACHMENTS = 4;
    public const int MAX_MONEY_PER_SLOT = 20000;

    public ushort usItem { get; set; }
    public ushort usItemCheckSum { get; set; }
    public byte ubNumberOfObjects { get; set; }

    // Gun-related
    public sbyte bGunStatus { get; set; }
    public byte ubGunAmmoType { get; set; }
    public byte ubGunShotsLeft { get; set; }
    public ushort usGunAmmoItem { get; set; }
    public sbyte bGunAmmoStatus { get; set; }

    // Alternative representations (mutually exclusive in union)
    public byte[] ubShotsLeft { get; set; } = new byte[MAX_OBJECTS_PER_SLOT];
    public sbyte[] bStatus { get; set; } = new sbyte[MAX_OBJECTS_PER_SLOT];

    // Money-related
    public sbyte bMoneyStatus { get; set; }
    public uint uiMoneyAmount { get; set; }
    public sbyte[] padding { get; set; } = new sbyte[4];

    // Bomb/action item
    public sbyte bBombStatus { get; set; }
    public sbyte bDetonatorType { get; set; }
    public ushort usBombItem { get; set; }
    public sbyte bDelay { get; set; } // or bFrequency (same memory)
    public byte ubBombOwner { get; set; }
    public byte bActionValue { get; set; }
    public byte ubTolerance { get; set; }

    // Key-related
    public sbyte[] bKeyStatus { get; set; } = new sbyte[6];
    public byte ubKeyID { get; set; }

    // Ownership
    public byte ubOwnerProfile { get; set; }
    public byte ubOwnerCivGroup { get; set; }

    // Attachments
    public ushort[] usAttachItem { get; set; } = new ushort[MAX_ATTACHMENTS];
    public sbyte[] bAttachStatus { get; set; } = new sbyte[MAX_ATTACHMENTS];

    public sbyte fFlags { get; set; }
    public byte ubMission { get; set; }
    public sbyte bTrap { get; set; }
    public byte ubImprintID { get; set; }

    public byte ubWeight { get; set; }
    public byte fUsed { get; set; }

    //

    public BinaryReaderOffsetItem Offset { get; set; }

    public Item Item { get; set; }

#pragma warning disable CS0067 // The event is never used
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
}
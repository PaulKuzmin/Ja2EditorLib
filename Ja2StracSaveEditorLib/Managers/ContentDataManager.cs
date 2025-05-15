using Newtonsoft.Json;
using System.ComponentModel;

// ReSharper disable InconsistentNaming

namespace Ja2StracSaveEditorLib.Managers;

public class TileGraphic
{
    public string type { get; set; }
    public int subIndex { get; set; }
}

public class Graphic
{
    public string path { get; set; }
    public int? subImageIndex { get; set; }
}

public class InventoryGraphics
{
    public Graphic small { get; set; }
    public Graphic big { get; set; }
}

public class Item : INotifyPropertyChanged
{
#pragma warning disable CS0067 // The event is never used
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

    // SUBTYPES
    public const int IC_NONE = 0x00000001;
    public const int IC_GUN = 0x00000002;
    public const int IC_BLADE = 0x00000004;
    public const int IC_THROWING_KNIFE = 0x00000008;

    public const int IC_LAUNCHER = 0x00000010;
    public const int IC_TENTACLES = 0x00000020;

    public const int IC_THROWN = 0x00000040;
    public const int IC_PUNCH = 0x00000080;

    public const int IC_GRENADE = 0x00000100;
    public const int IC_BOMB = 0x00000200;
    public const int IC_AMMO = 0x00000400;
    public const int IC_ARMOUR = 0x00000800;

    public const int IC_MEDKIT = 0x00001000;
    public const int IC_KIT = 0x00002000;
    public const int IC_FACE = 0x00008000;

    public const int IC_KEY = 0x00010000;

    public const int IC_MISC = 0x10000000;
    public const int IC_MONEY = 0x20000000;

    public int itemIndex { get; set; }
    public int usItemClass { get; set; }
    public string internalName { get; set; }
    public string internalType { get; set; } // Только в weapons.json
    public string calibre { get; set; } // Только в magazines.json и weapons.json
    public int? capacity { get; set; } // Только в magazines.json
    public string ammoType { get; set; } // Только в magazines.json
    public int? rateOfFire { get; set; } // Только в weapons.json
    public int? usRange { get; set; } // Только в weapons.json
    public int? ubMagSize { get; set; } // Только в weapons.json
    public int? ubImpact { get; set; }
    public int? ubDeadliness { get; set; }
    public int? ubAttackVolume { get; set; }
    public int? ubHitVolume { get; set; }
    public int? ubReadyTime { get; set; }
    public int? ubShotsPer4Turns { get; set; }
    public int? ubShotsPerBurst { get; set; }
    public int? ubBurstPenalty { get; set; }
    public int? ubBulletSpeed { get; set; }

    public int? ubClassIndex { get; set; }
    public int? ubCursor { get; set; }
    public int ubWeight { get; set; }
    public int ubPerPocket { get; set; }
    public int usPrice { get; set; }
    public int ubCoolness { get; set; }
    public int? bReliability { get; set; }
    public int? bRepairEase { get; set; }

    public bool? bDamageable { get; set; }
    public bool? bRepairable { get; set; }
    public bool? bMetal { get; set; }
    public bool? bSinks { get; set; }
    public bool? bWaterDamages { get; set; }
    public bool? bShowStatus { get; set; }
    public bool? bBigGunList { get; set; }
    public bool? bNotBuyable { get; set; }
    public bool? bNotEditor { get; set; }
    public bool? bTwoHanded { get; set; }
    public bool? bAttachment { get; set; }
    public bool? bElectronic { get; set; }

    public bool? attachment_Silencer { get; set; }
    public bool? attachment_LaserScope { get; set; }
    public bool? attachment_SpringAndBoltUpgrade { get; set; }
    public bool? attachment_GunBarrelExtender { get; set; }
    public bool? attachment_SniperScope { get; set; }
    public bool? attachment_UnderGLauncher { get; set; }

    public string standardReplacement { get; set; }

    public InventoryGraphics inventoryGraphics { get; set; }
    public TileGraphic tileGraphic { get; set; }
}

public class ContentDataManager
{
    public List<Item> Items { get; set; }

    public static ContentDataManager LoadFromFile(Dictionary<string, int?> filePaths)
    {
        var items = new List<Item>();
        foreach (var filePathClass in filePaths)
        {
            var json = File.ReadAllText(filePathClass.Key);
            var fileItems = JsonConvert.DeserializeObject<List<Item>>(json);
            if (fileItems == null) continue;
            if (filePathClass.Value != null)
                fileItems = fileItems.Select(s =>
                {
                    s.usItemClass = filePathClass.Value.Value;
                    if (!string.IsNullOrWhiteSpace(s.internalType))
                    {
                        var itemClass = GetItemClass(s.internalType);
                        if (itemClass != null) s.usItemClass = itemClass.Value;
                    }

                    return s;
                }).ToList();

            items.AddRange(fileItems);
        }

        return new ContentDataManager { Items = items };
    }

    public Item Get(ushort itemIndex)
    {
        return Items.FirstOrDefault(f => f.itemIndex == itemIndex);
    }

    public ushort ReplaceInvalidItem(ushort itemIndex)
    {
        var item = Get(itemIndex);

        ITEMDEFINE? defValue = null;
        if (Enum.IsDefined(typeof(ITEMDEFINE), (int)itemIndex))
            // ReSharper disable once RedundantCast
            defValue = (ITEMDEFINE)(int)itemIndex;

        if (item == null && defValue == null)
        {
            return (ushort)ITEMDEFINE.NONE;
        }

        return itemIndex;
    }

    public static int? GetItemClass(string internalType)
    {
        return internalType switch
        {
            "NOWEAPON" => Item.IC_NONE,
            "PUNCH" => Item.IC_PUNCH,
            "THROWN" => Item.IC_THROWN,
            "PISTOL" => Item.IC_GUN,
            "M_PISTOL" => Item.IC_GUN,
            "SMG" => Item.IC_GUN,
            "SN_RIFLE" => Item.IC_GUN,
            "RIFLE" => Item.IC_GUN,
            "ASRIFLE" => Item.IC_GUN,
            "SHOTGUN" => Item.IC_GUN,
            "LMG" => Item.IC_GUN,
            "BLADE" => Item.IC_BLADE,
            "THROWINGBLADE" => Item.IC_THROWING_KNIFE,
            "PUNCHWEAPON" => Item.IC_PUNCH,
            "LAUNCHER" => Item.IC_LAUNCHER,
            "LAW" => Item.IC_GUN,
            "CANNON" => Item.IC_GUN,
            "MONSTSPIT" => Item.IC_GUN,
            _ => null
        };
    }
}
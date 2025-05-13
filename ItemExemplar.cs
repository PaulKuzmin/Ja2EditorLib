using System.Collections;
// ReSharper disable InconsistentNaming

namespace Ja2EditorLib;

public class ItemExemplar
{
    public const int NONE_CATEGORY = 0;
    public const int WEAPON_CATEGORY = 1;
    public const int MELEE_WEAPON_CATEGORY = 2;
    public const int WEAPON_ATTACHMENT_CATEGORY = 3;
    public const int AMMO_CATEGORY = 4;
    public const int GRENADE_CATEGORY = 5;
    public const int EXPLOSIVE_CATEGORY = 6;
    public const int DETONATOR_CATEGORY = 7;
    public const int HELMET_CATEGORY = 8;
    public const int BODY_ARMOR_CATEGORY = 9;
    public const int ARMOR_ATTACHMENT_CATEGORY = 10;
    public const int LEG_ARMOR_CATEGORY = 11;
    public const int HEAD_GEAR_CATEGORY = 12;
    public const int MEDICAL_CATEGORY = 13;
    public const int TOOL_CATEGORY = 14;
    public const int USABLE_ITEM_CATEGORY = 15;
    public const int MISC_CATEGORY = 16;
    public const int KEY_CATEGORY = 17;
    public const int MONEY_CATEGORY = 18;

    public static Hashtable nameTable = new Hashtable();
    public static Hashtable exemplarTable = new Hashtable();
    public static ArrayList nameList = new ArrayList();
    public static ArrayList ammoNameList = new ArrayList();
    public static ArrayList attachmentNameList = new ArrayList();

    public int id;
    public string name;
    public int category;
    public int ammo1;
    public int ammo2;
    public int ammo3;
    public int ammo4;
    public int ammoVariety;
    public int ammoCapacity;

    public ItemExemplar(int id, string name, int category)
        : this(id, name, category, 0, 0, 0, 0)
    {
    }

    public ItemExemplar(int id, string name, int category, int arg1)
        : this(id, name, category, arg1, 0, 0, 0)
    {
    }

    public ItemExemplar(int id, string name, int category, int arg1, int arg2)
        : this(id, name, category, arg1, arg2, 0, 0)
    {
    }

    public ItemExemplar(int id, string name, int category, int arg1, int arg2, int arg3)
        : this(id, name, category, arg1, arg2, arg3, 0)
    {
    }

    public ItemExemplar(int id, string name, int category, int arg1, int arg2, int arg3, int arg4)
    {
        this.id = id;
        this.name = name;
        this.category = category;

        switch (category)
        {
            case WEAPON_CATEGORY:
                this.ammo1 = arg1;
                this.ammo2 = arg2;
                this.ammo3 = arg3;
                this.ammo4 = arg4;
                break;

            case AMMO_CATEGORY:
                this.ammoVariety = arg1;
                this.ammoCapacity = arg2;
                ammoNameList.Add(name);
                break;

            case WEAPON_ATTACHMENT_CATEGORY:
            case ARMOR_ATTACHMENT_CATEGORY:
            case GRENADE_CATEGORY:
            case DETONATOR_CATEGORY:
                attachmentNameList.Add(name);
                break;
        }

        nameTable[name] = id;
        nameTable[id] = name;
        exemplarTable[name] = this;
        exemplarTable[id] = this;
        nameList.Add(name);
    }

    // Для полной функциональности нужно вызвать инициализацию экземпляров где-то отдельно (например, статическим методом),
    // если вы хотите отделить объявления инициализации от самого класса
}
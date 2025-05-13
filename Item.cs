using System.Collections;

namespace Ja2EditorLib;

public class Item : BasicStructure
{
    public const int ITEM_LENGTH = 36;
    public const int ID_OFFSET = 0;
    public const int QUANTITY_OFFSET = 2;
    public const int ITEM_1_PERCENT_OFFSET = 4;
    public const int ITEM_2_PERCENT_OFFSET = 5;
    public const int ITEM_3_PERCENT_OFFSET = 6;
    public const int ITEM_4_PERCENT_OFFSET = 7;
    public const int ITEM_5_PERCENT_OFFSET = 8;
    public const int ITEM_6_PERCENT_OFFSET = 9;
    public const int MONEY_VALUE_OFFSET = 8;
    public const int AMMO_VARIETY_OFFSET = 5;
    public const int AMMO_QUANTITY_OFFSET = 6;
    public const int AMMO_ID_OFFSET = 8;
    public const int AMMO_PERCENT_OFFSET = 10;
    public const int ATTACHMENT_1_ID_OFFSET = 16;
    public const int ATTACHMENT_2_ID_OFFSET = 18;
    public const int ATTACHMENT_3_ID_OFFSET = 20;
    public const int ATTACHMENT_4_ID_OFFSET = 22;
    public const int ATTACHMENT_1_PERCENT_OFFSET = 24;
    public const int ATTACHMENT_2_PERCENT_OFFSET = 25;
    public const int ATTACHMENT_3_PERCENT_OFFSET = 26;
    public const int ATTACHMENT_4_PERCENT_OFFSET = 27;
    public const int UNKNOWN_1_OFFSET = 28;
    public const int UNKNOWN_2_OFFSET = 29;
    public const int UNKNOWN_3_OFFSET = 30;
    public const int UNKNOWN_4_OFFSET = 31;
    public const int WEIGHT_OFFSET = 32;

    public static Hashtable classFields;

    static Item()
    {
        classFields = new Hashtable();
        classFields["Item ID"] = new ChoiceField(new ShortField(0), ItemExemplar.nameTable);
        classFields["Quantity"] = new ShortField(2);
        classFields["Item 1 %"] = new ByteField(4);
        classFields["Item 2 %"] = new ByteField(5);
        classFields["Item 3 %"] = new ByteField(6);
        classFields["Item 4 %"] = new ByteField(7);
        classFields["Item 5 %"] = new ByteField(8);
        classFields["Item 6 %"] = new ByteField(9);
        classFields["Money Value"] = new ShortField(8);
        classFields["Ammo Variety"] = new ByteField(5);
        classFields["Ammo Quantity"] = new ShortField(6);
        classFields["Ammo ID"] = new ChoiceField(new ShortField(8), ItemExemplar.nameTable);
        classFields["Ammo %"] = new ByteField(10);
        classFields["Attachment 1 ID"] = new ChoiceField(new ShortField(16), ItemExemplar.nameTable);
        classFields["Attachment 2 ID"] = new ChoiceField(new ShortField(18), ItemExemplar.nameTable);
        classFields["Attachment 3 ID"] = new ChoiceField(new ShortField(20), ItemExemplar.nameTable);
        classFields["Attachment 4 ID"] = new ChoiceField(new ShortField(22), ItemExemplar.nameTable);
        classFields["Attachment 1 %"] = new ByteField(24);
        classFields["Attachment 2 %"] = new ByteField(25);
        classFields["Attachment 3 %"] = new ByteField(26);
        classFields["Attachment 4 %"] = new ByteField(27);
        classFields["Unknown 1"] = new ByteField(28);
        classFields["Unknown 2"] = new ByteField(29);
        classFields["Unknown 3"] = new ByteField(30);
        classFields["Unknown 4"] = new ByteField(31);
        classFields["Weight"] = new ShortField(32);
    }

    public Item(byte[] data) : base(Item.classFields)
    {
        this.data = data;
    }

    public byte[] encode()
    {
        return this.data;
    }

    public ItemExemplar getExemplar()
    {
        int fieldValue = this.getInt("Item ID");
        return (ItemExemplar)ItemExemplar.exemplarTable[fieldValue];
    }

    public new void set(string name, string value)
    {
        base.set(name, value);
    }

    public new void setInt(string name, int value)
    {
        base.setInt(name, value);
    }
}
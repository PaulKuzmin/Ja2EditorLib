using System.Collections;

namespace Ja2EditorLib;

public class Mercenary : BasicStructure
{
    public const int CIPHERTEXT_OFFSET = 1;
    public const int CIPHERTEXT_LENGTH = 2328;
    public const int FIRST_ITEM_OFFSET = 12;
    public const int ITEM_LENGTH = 36;
    public const int ITEM_COUNT = 19;
    public const int HELMET_INDEX = 0;
    public const int BODY_ARMOR_INDEX = 1;
    public const int LEG_ARMOR_INDEX = 2;
    public const int HEADGEAR_1_INDEX = 3;
    public const int HEADGEAR_2_INDEX = 4;
    public const int RIGHT_HAND_INDEX = 5;
    public const int LEFT_HAND_INDEX = 6;
    public const int BACKPACK_1_3_INDEX = 7;
    public const int BACKPACK_2_3_INDEX = 8;
    public const int BACKPACK_3_3_INDEX = 9;
    public const int BACKPACK_4_3_INDEX = 10;
    public const int BACKPACK_1_1_INDEX = 11;
    public const int BACKPACK_2_1_INDEX = 12;
    public const int BACKPACK_3_1_INDEX = 13;
    public const int BACKPACK_4_1_INDEX = 14;
    public const int BACKPACK_1_2_INDEX = 15;
    public const int BACKPACK_2_2_INDEX = 16;
    public const int BACKPACK_3_2_INDEX = 17;
    public const int BACKPACK_4_2_INDEX = 18;

    public byte[] allData;
    public Item[] items;
    public int savedGameVersion;
    public int trailingOffset;
    public int inventorySum;

    public int ENERGY_OFFSET;
    public int MAX_ENERGY_OFFSET;
    public int NICK_OFFSET;
    public int CHECKSUM_OFFSET;
    public int MORALE_OFFSET;
    public int SKILL1_OFFSET;
    public int SKILL2_OFFSET;
    public int DEX_OFFSET;
    public int WIS_OFFSET;
    public int LVL_OFFSET;
    public int HEALTH_OFFSET;
    public int AGI_OFFSET;
    public int STR_OFFSET;
    public int LDR_OFFSET;
    public int MEC_OFFSET;
    public int MAX_HEALTH_OFFSET;
    public int MED_OFFSET;
    public int MRK_OFFSET;
    public int EXP_OFFSET;

    public Mercenary(byte[] rawData, int savedGameVersion, int trailingOffset, int inventorySum, int[] table)
        : base(new Hashtable())
    {
        this.items = new Item[19];
        this.savedGameVersion = savedGameVersion;
        this.trailingOffset = trailingOffset;
        this.inventorySum = inventorySum;
        this.decode(rawData, table);

        if (savedGameVersion <= 99)
        {
            ENERGY_OFFSET = 711;
            MAX_ENERGY_OFFSET = 712;
            NICK_OFFSET = 730;
            CHECKSUM_OFFSET = 2208;
            MORALE_OFFSET = 1736;
            SKILL1_OFFSET = 832;
            SKILL2_OFFSET = 833;
            DEX_OFFSET = 840;
            WIS_OFFSET = 841;
            LVL_OFFSET = 849;
            HEALTH_OFFSET = 868;
            AGI_OFFSET = 880;
            STR_OFFSET = 886;
            LDR_OFFSET = 895;
            MEC_OFFSET = 916;
            MAX_HEALTH_OFFSET = 917;
            MED_OFFSET = 1372;
            MRK_OFFSET = 1377;
            EXP_OFFSET = 1378;
        }
        else
        {
            ENERGY_OFFSET = 45;
            MAX_ENERGY_OFFSET = 46;
            NICK_OFFSET = 2;
            CHECKSUM_OFFSET = 972;
            MORALE_OFFSET = trailingOffset + 204;
            SKILL1_OFFSET = trailingOffset + 436;
            SKILL2_OFFSET = trailingOffset + 437;
            DEX_OFFSET = trailingOffset + 438;
            WIS_OFFSET = trailingOffset + 439;
            LVL_OFFSET = trailingOffset + 430;
            HEALTH_OFFSET = trailingOffset + 428;
            AGI_OFFSET = trailingOffset + 431;
            STR_OFFSET = trailingOffset + 432;
            LDR_OFFSET = trailingOffset + 442;
            MEC_OFFSET = trailingOffset + 433;
            MAX_HEALTH_OFFSET = trailingOffset + 429;
            MED_OFFSET = trailingOffset + 440;
            MRK_OFFSET = trailingOffset + 434;
            EXP_OFFSET = trailingOffset + 435;
        }

        fields["Energy"] = new ByteField(ENERGY_OFFSET);
        fields["Max Energy"] = new ByteField(MAX_ENERGY_OFFSET);
        fields["Nickname"] = new StringField(NICK_OFFSET, 20);
        fields["Skill1"] = new ChoiceField(new ByteField(SKILL1_OFFSET), Skill.Table);
        fields["Skill2"] = new ChoiceField(new ByteField(SKILL2_OFFSET), Skill.Table);
        fields["Dexterity"] = new ByteField(DEX_OFFSET);
        fields["Wisdom"] = new ByteField(WIS_OFFSET);
        fields["Level"] = new ByteField(LVL_OFFSET);
        fields["Health"] = new ByteField(HEALTH_OFFSET);
        fields["Agility"] = new ByteField(AGI_OFFSET);
        fields["Strength"] = new ByteField(STR_OFFSET);
        fields["Leadership"] = new ByteField(LDR_OFFSET);
        fields["Mechanical"] = new ByteField(MEC_OFFSET);
        fields["Max Health"] = new ByteField(MAX_HEALTH_OFFSET);
        fields["Medical"] = new ByteField(MED_OFFSET);
        fields["Marksmanship"] = new ByteField(MRK_OFFSET);
        fields["Explosives"] = new ByteField(EXP_OFFSET);
        fields["Morale"] = new ByteField(MORALE_OFFSET);
        fields["ubProfile"] = new ByteField(717);
        fields["Checksum"] = new IntField(CHECKSUM_OFFSET);
    }

    public void decode(byte[] rawData, int[] table)
    {
        allData = new byte[rawData.Length];
        Array.Copy(rawData, 0, allData, 0, allData.Length);

        if (savedGameVersion <= 99)
        {
            byte[] ciphertext = new byte[2328];
            Array.Copy(rawData, 1, ciphertext, 0, 2328);
            byte[] plaintext = JapeAlg.Decode(ciphertext, ciphertext.Length, table);
            data = plaintext;

            for (int idx = 0; idx < 19; ++idx)
            {
                byte[] itemData = new byte[36];
                int itemOffset = 12 + idx * 36;
                Array.Copy(data, itemOffset, itemData, 0, 36);
                Item item = new Item(itemData);
                items[idx] = item;
            }
        }
        else
        {
            data = new byte[rawData.Length - 1];
            Array.Copy(rawData, 1, data, 0, data.Length);
        }
    }

    public byte[] encode(int[] table)
    {
        if (savedGameVersion <= 99)
        {
            for (int idx = 0; idx < 19; ++idx)
            {
                Item item = items[idx];
                byte[] itemData = item.encode();
                int itemOffset = 12 + idx * 36;
                Array.Copy(itemData, 0, data, itemOffset, 36);
            }
        }

        recomputeChecksum();
        byte[] rawData = new byte[allData.Length];
        Array.Copy(allData, 0, rawData, 0, allData.Length);

        if (savedGameVersion <= 99)
        {
            byte[] plaintext = new byte[2328];
            Array.Copy(data, 0, plaintext, 0, 2328);
            byte[] ciphertext = JapeAlg.Encode(plaintext, plaintext.Length, table);
            Array.Copy(ciphertext, 0, rawData, 1, 2328);
        }
        else
        {
            Array.Copy(data, 0, rawData, 1, data.Length);
        }

        return rawData;
    }

    public int computeChecksum()
    {
        if (savedGameVersion <= 99)
        {
            return JapeAlg.MercChecksum(data);
        }

        int checksum = 1;
        checksum += getInt("Health") + 1;
        checksum *= getInt("Max Health") + 1;
        checksum += getInt("Agility") + 1;
        checksum *= getInt("Dexterity") + 1;
        checksum += getInt("Strength") + 1;
        checksum *= getInt("Marksmanship") + 1;
        checksum += getInt("Medical") + 1;
        checksum *= getInt("Mechanical") + 1;
        checksum += getInt("Explosives") + 1;
        checksum *= getInt("Level") + 1;
        checksum += getInt("ubProfile") + 1;
        checksum += inventorySum;
        return checksum;
    }

    public void validateChecksum()
    {
        int checksum = computeChecksum();
        int savedChecksum = getInt("Checksum");
        if (checksum != savedChecksum)
        {
            throw new Exception(
                $"Invalid checksum (0x{checksum:X} vs 0x{savedChecksum:X}) in SOLDIERTYPE for '{get("Nickname")}'");
        }
    }

    public void recomputeChecksum()
    {
        int checksum = computeChecksum();
        setInt("Checksum", checksum);
    }
}
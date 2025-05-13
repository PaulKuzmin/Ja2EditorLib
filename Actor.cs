using System.Collections;
// ReSharper disable InconsistentNaming

namespace Ja2EditorLib;

public class Actor : BasicStructure
{
    public const int NAME_OFFSET = 0;
    public const int NAME_LENGTH = 60;
    public const int NICK_OFFSET = 60;
    public const int NICK_LENGTH = 20;
    public const int MED_OFFSET = 261;
    public const int STR_OFFSET = 296;
    public const int MAX_HEALTH_OFFSET = 297;
    public const int LVL_INC_OFFSET = 298;
    public const int HEALTH_INC_OFFSET = 299;
    public const int AGI_INC_OFFSET = 300;
    public const int DEX_INC_OFFSET = 301;
    public const int WIS_INC_OFFSET = 302;
    public const int MRK_INC_OFFSET = 303;
    public const int MED_INC_OFFSET = 304;
    public const int MEC_INC_OFFSET = 305;
    public const int EXP_INC_OFFSET = 306;
    public const int STR_INC_OFFSET = 307;
    public const int LDR_INC_OFFSET = 308;
    public const int KILLS_OFFSET = 310;
    public const int KILLS_LENGTH = 2;
    public const int ASSISTS_OFFSET = 312;
    public const int ASSISTS_LENGTH = 2;
    public const int SHOTS_FIRED_OFFSET = 314;
    public const int SHOTS_FIRED_LENGTH = 2;
    public const int SHOTS_HIT_OFFSET = 316;
    public const int SHOTS_HIT_LENGTH = 2;
    public const int BATTLES_OFFSET = 318;
    public const int BATTLES_LENGTH = 2;
    public const int WOUNDS_OFFSET = 320;
    public const int WOUNDS_LENGTH = 2;
    public const int HEALTH_OFFSET = 334;
    public const int DEX_OFFSET = 335;
    public const int PERSONALITY_OFFSET = 336;
    public const int SKILL1_OFFSET = 337;
    public const int EXP_OFFSET = 339;
    public const int SKILL2_OFFSET = 340;
    public const int LDR_OFFSET = 341;
    public const int LVL_OFFSET = 352;
    public const int MRK_OFFSET = 353;
    public const int WIS_OFFSET = 355;
    public const int AGI_OFFSET_99 = 405;
    public const int AGI_OFFSET_103 = 367;
    public const int MEC_OFFSET_99 = 411;
    public const int MEC_OFFSET_103 = 373;
    public const int CHECKSUM_OFFSET_99 = 696;
    public const int CHECKSUM_OFFSET_103 = 616;
    public const int CHECKSUM_LENGTH = 4;

    public int savedGameVersion;
    public int AGI_OFFSET;
    public int MEC_OFFSET;
    public int CHECKSUM_OFFSET;

    public Actor(byte[] rawData, int savedGameVersion) : base(new Hashtable())
    {
        data = rawData;
        this.savedGameVersion = savedGameVersion;
        if (savedGameVersion <= 99)
        {
            AGI_OFFSET = AGI_OFFSET_99;
            MEC_OFFSET = MEC_OFFSET_99;
            CHECKSUM_OFFSET = CHECKSUM_OFFSET_99;
        }
        else if (savedGameVersion == 102)
        {
            AGI_OFFSET = AGI_OFFSET_99;
            MEC_OFFSET = MEC_OFFSET_99;
            CHECKSUM_OFFSET = CHECKSUM_OFFSET_99;
        }
        else
        {
            AGI_OFFSET = AGI_OFFSET_103;
            MEC_OFFSET = MEC_OFFSET_103;
            CHECKSUM_OFFSET = CHECKSUM_OFFSET_103;
        }

        fields["Name"] = new StringField(0, 60);
        fields["Nickname"] = new StringField(60, 20);
        fields["Medical"] = new ByteField(261);
        fields["Strength"] = new ByteField(296);
        fields["Max Health"] = new ByteField(297);
        fields["Level Inc"] = new ByteField(298);
        fields["Health Inc"] = new ByteField(299);
        fields["Agility Inc"] = new ByteField(300);
        fields["Dexterity Inc"] = new ByteField(301);
        fields["Wisdom Inc"] = new ByteField(302);
        fields["Marksmanship Inc"] = new ByteField(303);
        fields["Medical Inc"] = new ByteField(304);
        fields["Mechanical Inc"] = new ByteField(305);
        fields["Explosives Inc"] = new ByteField(306);
        fields["Strength Inc"] = new ByteField(307);
        fields["Leadership Inc"] = new ByteField(308);
        fields["Kills"] = new ShortField(310);
        fields["Assists"] = new ShortField(312);
        fields["Shots Fired"] = new ShortField(314);
        fields["Shots Hit"] = new ShortField(316);
        fields["Battles"] = new ShortField(318);
        fields["Wounds"] = new ShortField(320);
        fields["Health"] = new ByteField(334);
        fields["Dexterity"] = new ByteField(335);
        fields["Personality"] = new ByteField(336);
        fields["Skill1"] = new ChoiceField(new ByteField(337), Skill.Table);
        fields["Explosives"] = new ByteField(339);
        fields["Skill2"] = new ChoiceField(new ByteField(340), Skill.Table);
        fields["Leadership"] = new ByteField(341);
        fields["Level"] = new ByteField(352);
        fields["Marksmanship"] = new ByteField(353);
        fields["Wisdom"] = new ByteField(355);
        fields["Agility"] = new ByteField(AGI_OFFSET);
        fields["Mechanical"] = new ByteField(MEC_OFFSET);
        fields["Checksum"] = new IntField(CHECKSUM_OFFSET);
    }

    public Actor(byte[] rawData, int savedGameVersion, int[] table) : base(new Hashtable())
    {
        this.savedGameVersion = savedGameVersion;
        this.decode(rawData, table);
        if (savedGameVersion <= 99)
        {
            AGI_OFFSET = AGI_OFFSET_99;
            MEC_OFFSET = MEC_OFFSET_99;
            CHECKSUM_OFFSET = CHECKSUM_OFFSET_99;
        }
        else if (savedGameVersion == 102)
        {
            AGI_OFFSET = AGI_OFFSET_99;
            MEC_OFFSET = MEC_OFFSET_99;
            CHECKSUM_OFFSET = CHECKSUM_OFFSET_99;
        }
        else
        {
            AGI_OFFSET = AGI_OFFSET_103;
            MEC_OFFSET = MEC_OFFSET_103;
            CHECKSUM_OFFSET = CHECKSUM_OFFSET_103;
        }

        fields["Name"] = new StringField(0, 60);
        fields["Nickname"] = new StringField(60, 20);
        fields["Medical"] = new ByteField(261);
        fields["Strength"] = new ByteField(296);
        fields["Max Health"] = new ByteField(297);
        fields["Level Inc"] = new ByteField(298);
        fields["Health Inc"] = new ByteField(299);
        fields["Agility Inc"] = new ByteField(300);
        fields["Dexterity Inc"] = new ByteField(301);
        fields["Wisdom Inc"] = new ByteField(302);
        fields["Marksmanship Inc"] = new ByteField(303);
        fields["Medical Inc"] = new ByteField(304);
        fields["Mechanical Inc"] = new ByteField(305);
        fields["Explosives Inc"] = new ByteField(306);
        fields["Strength Inc"] = new ByteField(307);
        fields["Leadership Inc"] = new ByteField(308);
        fields["Kills"] = new ShortField(310);
        fields["Assists"] = new ShortField(312);
        fields["Shots Fired"] = new ShortField(314);
        fields["Shots Hit"] = new ShortField(316);
        fields["Battles"] = new ShortField(318);
        fields["Wounds"] = new ShortField(320);
        fields["Health"] = new ByteField(334);
        fields["Dexterity"] = new ByteField(335);
        fields["Personality"] = new ByteField(336);
        fields["Skill1"] = new ChoiceField(new ByteField(337), Skill.Table);
        fields["Explosives"] = new ByteField(339);
        fields["Skill2"] = new ChoiceField(new ByteField(340), Skill.Table);
        fields["Leadership"] = new ByteField(341);
        fields["Level"] = new ByteField(352);
        fields["Marksmanship"] = new ByteField(353);
        fields["Wisdom"] = new ByteField(355);
        fields["Agility"] = new ByteField(AGI_OFFSET);
        fields["Mechanical"] = new ByteField(MEC_OFFSET);
        fields["Checksum"] = new IntField(CHECKSUM_OFFSET);
    }

    public void decode(byte[] ciphertext, int[] table)
    {
        if (savedGameVersion <= 99)
        {
            data = JapeAlg.Decode(ciphertext, ciphertext.Length, table);
        }
        else
        {
            data = new byte[ciphertext.Length];
            Array.Copy(ciphertext, data, ciphertext.Length);
        }
    }

    public byte[] encode(int[] table)
    {
        recomputeChecksum();
        if (savedGameVersion <= 99)
        {
            return JapeAlg.Encode(data, data.Length, table);
        }
        else
        {
            byte[] ciphertext = new byte[data.Length];
            Array.Copy(data, ciphertext, data.Length);
            return ciphertext;
        }
    }

    public int computeChecksum()
    {
        if (savedGameVersion <= 99)
        {
            return JapeAlg.ActorChecksum(data);
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

        int invBaseOffset = 632;
        IntField invsize = new IntField(invBaseOffset);
        int inventorySize = invsize.getInt(data);
        for (int itemIdx = 0; itemIdx < inventorySize; ++itemIdx)
        {
            int offset = invBaseOffset + 4 + itemIdx * 12;
            IntField inv = new IntField(offset);
            checksum += inv.getInt(data);
            IntField bInvNumber = new IntField(offset + 8);
            checksum += bInvNumber.getInt(data);
        }

        return checksum;
    }

    public void validateChecksum()
    {
        int checksum = computeChecksum();
        int savedChecksum = getInt("Checksum");
        if (checksum != savedChecksum)
        {
            throw new Exception(
                $"Invalid checksum (0x{checksum:X} vs 0x{savedChecksum:X}) in MERCPROFILESTRUCT for '{get("Name")}'");
        }
    }

    public void recomputeChecksum()
    {
        int checksum = computeChecksum();
        setInt("Checksum", checksum);
    }
}
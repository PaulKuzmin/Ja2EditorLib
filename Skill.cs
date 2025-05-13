using System.Collections;
// ReSharper disable InconsistentNaming

namespace Ja2EditorLib;

public class Skill
{
    public const int SKILL_NONE = 0;
    public const int SKILL_LOCKPICKING = 1;
    public const int SKILL_HAND_TO_HAND = 2;
    public const int SKILL_ELECTRONICS = 3;
    public const int SKILL_NIGHT_OPS = 4;
    public const int SKILL_THROWING = 5;
    public const int SKILL_TEACHING = 6;
    public const int SKILL_HEAVY_WEAPONS = 7;
    public const int SKILL_AUTO_WEAPONS = 8;
    public const int SKILL_STEALTHY = 9;
    public const int SKILL_AMIDEXTROUS = 10;
    public const int SKILL_THIEF = 11;
    public const int SKILL_MARTIAL_ARTS = 12;
    public const int SKILL_KNIFING = 13;
    public const int SKILL_ROOF_BONUS = 14;
    public const int SKILL_CAMOUFLAGED = 15;

    public static readonly ArrayList List;
    public static readonly Hashtable Table;

    static Skill()
    {
        List = new ArrayList();
        string[] temp =
        {
            "None", "Lockpicking", "Hand to Hand", "Electronics", "Night Ops",
            "Throwing", "Teaching", "Heavy Weapons", "Auto Weapons", "Stealthy",
            "Ambidextrous", "Thief", "Martial Arts", "Knifing", "Roof Bonus", "Camouflaged"
        };

        foreach (string skill in temp)
        {
            List.Add(skill);
        }

        Table = new Hashtable();
        for (int i = 0; i < List.Count; i++)
        {
            Table[List[i]] = i;
            Table[i] = List[i];
        }
    }
}
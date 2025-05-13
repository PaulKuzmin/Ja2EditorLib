// ReSharper disable InconsistentNaming
namespace Ja2StracSaveEditorLib.Structures;

//Enums for the difficulty levels
public enum DifficultyLevel
{
    DIF_LEVEL_EASY = 1,
    DIF_LEVEL_MEDIUM = 2,
    DIF_LEVEL_HARD = 3,
    NUM_DIF_LEVELS = DIF_LEVEL_HARD
}

public struct GAME_OPTIONS
{
    public bool fGunNut;

    public bool fSciFi;

    public byte ubDifficultyLevel;

    public bool fTurnTimeLimit;

    public byte ubGameSaveMode;
}
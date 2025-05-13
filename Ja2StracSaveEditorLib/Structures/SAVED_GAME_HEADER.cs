// ReSharper disable InconsistentNaming

namespace Ja2StracSaveEditorLib.Structures;

public struct SAVED_GAME_HEADER
{
    public const byte GAME_VERSION_LENGTH = 16;
    public const byte SIZE_OF_SAVE_GAME_DESC = 128;

    public uint uiSavedGameVersion;
    public string zGameVersionNumber;
    public string sSavedGameDesc;
    public uint uiDay;
    public byte ubHour;
    public byte ubMin;
    public SGPSector sSector;
    public byte ubNumOfMercsOnPlayersTeam;
    public int iCurrentBalance;
    public uint uiCurrentScreen;
    public bool fAlternateSector;
    public bool fWorldLoaded;
    public byte ubLoadScreenID;
    public GAME_OPTIONS sInitialGameOptions;
    public uint uiRandom;
    public uint uiSaveStateSize;

    public SAVED_GAME_HEADER()
    {
        uiSavedGameVersion = 0;
        zGameVersionNumber = null;
        sSavedGameDesc = null;
        uiDay = 0;
        ubHour = 0;
        ubMin = 0;
        sSector = default;
        ubNumOfMercsOnPlayersTeam = 0;
        iCurrentBalance = 0;
        uiCurrentScreen = 0;
        fAlternateSector = false;
        fWorldLoaded = false;
        ubLoadScreenID = 0;
        sInitialGameOptions = default;
        uiRandom = 0;
        uiSaveStateSize = 0;
    }
}
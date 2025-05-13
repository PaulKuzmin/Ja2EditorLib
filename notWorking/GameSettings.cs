using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace Ja2EditorLib.notWorking
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GAME_OPTIONS
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool fGunNut;

        [MarshalAs(UnmanagedType.I1)]
        public bool fSciFi;

        public byte ubDifficultyLevel;

        [MarshalAs(UnmanagedType.I1)]
        public bool fTurnTimeLimit;

        public byte ubGameSaveMode;
    }

    public class GameSettings
    {

    }
}

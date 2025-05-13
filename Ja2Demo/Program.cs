namespace Ja2Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string stracWin =
                @"F:\ja2\saves\2025-05-06t23-48-32z-1.sav";

            var z = new Ja2StracSaveEditorLib.SaveGame(stracWin, @"F:\ja2\JA2Stracciatella");
            z.Load();

            var t = z.Soldiers.FirstOrDefault();
            ;
            //t.bExpLevel = sbyte.MaxValue;
            //t.bLife = sbyte.MaxValue;
            //t.bLifeMax = sbyte.MaxValue;

            //z.Save();
        }
    }
}

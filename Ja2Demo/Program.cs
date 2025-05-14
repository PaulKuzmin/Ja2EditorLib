using Ja2StracSaveEditorLib;

namespace Ja2Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var save = new SaveGame(@"F:\ja2\saves\2025-05-06t23-48-32z-1.sav", @"F:\ja2\JA2Stracciatella");
            save.Load();

            /*
            var slfFilenames = Directory.GetFiles(@"F:\ja2\Ja2Game\Data\", "*.slf", SearchOption.AllDirectories);
            foreach (var slfFilename in slfFilenames)
            {
                try
                {
                    var slf = Ja2Slf.FromFile(slfFilename);
                    foreach (var slfFile in slf.Files)
                    {
                        slf.M_Io.Seek(slfFile.Offset);
                        var bytes = slf.M_Io.ReadBytes(slfFile.Length);
                        var slfFilenameWe = Path.GetFileNameWithoutExtension(slfFilename);

                        if (!Directory.Exists(slfFilenameWe))
                            Directory.CreateDirectory(slfFilenameWe);
                        
                        var subDir = Path.GetDirectoryName(slfFile.FilePath);
                        if (!string.IsNullOrWhiteSpace(subDir))
                            Directory.CreateDirectory(Path.Combine(slfFilenameWe, subDir));

                        var newFilename = Path.Combine(slfFilenameWe, slfFile.FilePath);
                        File.WriteAllBytes(newFilename, bytes);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(slfFilename);
                    Console.WriteLine(e);
                    Console.WriteLine();
                }
            }
            */
            /*
            Console.WriteLine("Convert");

            var filenames = Directory.GetFiles(@"F:\ja2\Ja2EditorLib\Ja2Demo\bin\Debug\net8.0-windows\", "*.sti",
                SearchOption.AllDirectories);

            //var filename = @"F:\ja2\Ja2EditorLib\Ja2Demo\bin\Debug\net8.0-windows\DATA\INT1.STI";
            foreach (var filename in filenames) {
                try
                {
                    var sti = new Ja2Sti(filename);
                    sti.ExtractImages("");
                }
                catch (Exception e)
                {
                    Console.WriteLine(filename);
                    Console.WriteLine(e);
                    Console.WriteLine();
                }
            }
            */
        }
    }
}

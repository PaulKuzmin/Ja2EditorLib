using Ja2SlfLib;
using Ja2StracSaveEditorLib.Managers;

namespace Ja2StracSaveEditor;

public static class Exts
{
    public static string GetItemImageFilename(Item item)
    {
        if (item is { internalName: { } } && item.internalName.Equals("nothing", StringComparison.InvariantCultureIgnoreCase) ||
            item.internalName.Equals("none", StringComparison.InvariantCultureIgnoreCase))
            return "";

        if (item.inventoryGraphics?.big == null ||
            string.IsNullOrWhiteSpace(item.inventoryGraphics?.big?.path)) return Program.ImageNotFound;
        var imageIndex = item.inventoryGraphics.big.subImageIndex ?? 0;

        var cacheFilename = Path.Combine(Program.ImageCacheDir, $"{item.itemIndex}_{imageIndex}.png");
        if (File.Exists(cacheFilename)) return cacheFilename;

        var slfFilename = Path.Combine(Program.Settings.Ja2DataPath,
            $"{Path.GetDirectoryName(item.inventoryGraphics.big.path)}.slf");
        if (!File.Exists(slfFilename)) return Program.ImageNotFound;

        var fileToFound = Path.GetFileName(item.inventoryGraphics.big.path);
        if (string.IsNullOrWhiteSpace(fileToFound)) return Program.ImageNotFound;
        if (!fileToFound.EndsWith("sti", StringComparison.InvariantCultureIgnoreCase))
            return Program.ImageNotFound;

        var slf = Ja2Slf.FromFile(slfFilename);
        foreach (var slfFile in slf.Files)
        {
            var innerFilename = Path.GetFileName(slfFile.FilePath);
            if (string.IsNullOrWhiteSpace(innerFilename)) continue;
            if (!innerFilename.Equals(fileToFound, StringComparison.InvariantCultureIgnoreCase)) continue;

            slf.M_Io.Seek(slfFile.Offset);
            var bytes = slf.M_Io.ReadBytes(slfFile.Length);

            var sti = new Ja2Sti(bytes);
            var filenames = sti.ExtractImages(Program.ImageCacheDir, $"{item.itemIndex}");
            if (filenames.Any(a => a.Equals(cacheFilename, StringComparison.InvariantCultureIgnoreCase)))
                return cacheFilename;
        }

        return Program.ImageNotFound;
    }
}

public enum AttachmentSlot
{
    Slot1 = 0,
    Slot2 = 1,
    Slot3 = 2,
    Slot4 = 3,
}
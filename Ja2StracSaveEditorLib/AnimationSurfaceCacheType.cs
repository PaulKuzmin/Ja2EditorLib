// ReSharper disable InconsistentNaming
namespace Ja2StracSaveEditorLib;

public class AnimationSurfaceCacheType
{
    public const int ANIM_CACHE_SIZE = 3;
    public const ushort EMPTY_CACHE_ENTRY = 65000;

    // Ensure cache state is valid even if init is never called
    public ushort[] usCachedSurfaces = new ushort[ANIM_CACHE_SIZE]
    {
        EMPTY_CACHE_ENTRY, EMPTY_CACHE_ENTRY, EMPTY_CACHE_ENTRY
    };

    public short[] sCacheHits = new short[ANIM_CACHE_SIZE] { 0, 0, 0 };

    private byte mPid = 0;

    /// <summary>
    /// Load an animation surface if it is not already cached.
    /// </summary>
    public void Cache(ushort usSurfaceIndex, ushort usCurrentAnimation)
    {
        // TODO: implement cache logic
    }

    /// <summary>
    /// Init the animation cache for the specified soldier.
    /// </summary>
    public void Init(byte usSoldierID)
    {
        mPid = usSoldierID;
        for (int i = 0; i < ANIM_CACHE_SIZE; i++)
        {
            usCachedSurfaces[i] = EMPTY_CACHE_ENTRY;
            sCacheHits[i] = 0;
        }
    }

    /// <summary>
    /// Unload all cached animation surfaces.
    /// </summary>
    public void Free()
    {
        for (int i = 0; i < ANIM_CACHE_SIZE; i++)
        {
            usCachedSurfaces[i] = EMPTY_CACHE_ENTRY;
            sCacheHits[i] = 0;
        }
    }
}

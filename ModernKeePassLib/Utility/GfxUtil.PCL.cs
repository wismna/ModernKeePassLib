
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ModernKeePassLib.Utility
{
    public class GfxUtil
    {
        public static Image<Rgba32> LoadImage(byte[] pb)
        {
            return Image.Load<Rgba32>(pb);
        }

        public static Image<Rgba32> ScaleImage(Image<Rgba32> m_imgOrg, int? w, int? h, ScaleTransformFlags flags)
        {
            m_imgOrg.Mutate(i => i.Resize(w.GetValueOrDefault(), h.GetValueOrDefault()));
            return m_imgOrg;
        }
    }
}

using System.Drawing;

namespace HeaderImgr.ImageProcessing.System.Drawing
{
    public interface IBitmapOperation
    {
        Bitmap Apply(Bitmap bitmap);
    }
}

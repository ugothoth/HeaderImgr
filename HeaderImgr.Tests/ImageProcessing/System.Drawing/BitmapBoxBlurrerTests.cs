using HeaderImgr.ImageProcessing.System.Drawing;

namespace HeaderImgr.Tests.ImageProcessing.System.Drawing
{
    public class NaiveBitmapBoxBlurrerTests : BitmapBoxBlurrerBaseTests
    {
        protected override IBitmapOperation CreateBlurrerWith(int radius)
            => new NaiveBitmapBoxBlurrer(radius);
    }

    public class NaiveCompositeBitmapBoxBlurrerTests : BitmapBoxBlurrerBaseTests
    {
        protected override IBitmapOperation CreateBlurrerWith(int radius)
            => new CompositeBitmapBoxBlurrer(radius);
    }

    public class NaiveHorizontalVerticalCompositeBoxBlurrerTests : BitmapBoxBlurrerBaseTests
    {
        protected override IBitmapOperation CreateBlurrerWith(int radius)
            => new CompositeBitmapOperation(
                new HorizontalBitmapBlurrer(radius),
                new VerticalBitmapBlurrer(radius)
            );
    }
    
    public class NaiveVerticalHorizontalCompositeBoxBlurrerTests : BitmapBoxBlurrerBaseTests
    {
        protected override IBitmapOperation CreateBlurrerWith(int radius)
            => new CompositeBitmapOperation(
                new VerticalBitmapBlurrer(radius),
                new HorizontalBitmapBlurrer(radius)
            );
    }
    
    public class HorizontalVerticalCompositeBoxBlurrerTests : BitmapBoxBlurrerBaseTests
    {
        protected override IBitmapOperation CreateBlurrerWith(int radius)
            => new CompositeBitmapOperation(
                new InheritedHorizontalBitmapBlurrer(radius),
                new InheritedVerticalBitmapBlurrer(radius)
            );
    }
    
    public class VerticalHorizontalCompositeBoxBlurrerTests : BitmapBoxBlurrerBaseTests
    {
        protected override IBitmapOperation CreateBlurrerWith(int radius)
            => new CompositeBitmapOperation(
                new InheritedVerticalBitmapBlurrer(radius),
                new InheritedHorizontalBitmapBlurrer(radius)
            );
    }
}

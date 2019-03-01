using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeaderImgr.ImageProcessing.System.Drawing
{
    public class InheritedHorizontalBitmapBlurrer : BitmapBlurrerBase
    {
        private readonly int radius;

        public InheritedHorizontalBitmapBlurrer(int radius)
        {
            if (radius < 0)
                throw new ArgumentException("Radius cannot be negative");

            this.radius = radius;
        }

        protected override IEnumerable<(int X, int Y)> GetPixelsToSample(int x, int y, Bitmap bitmap)
            => GetRange(x, radius, bitmap.Width).Select(sampleX => (sampleX, y));
    }
}

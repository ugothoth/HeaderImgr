using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeaderImgr.ImageProcessing.System.Drawing
{
    public class InheritedVerticalBitmapBlurrer : BitmapBlurrerBase
    {
        private readonly int radius;

        public InheritedVerticalBitmapBlurrer(int radius)
        {
            if (radius < 0)
                throw new ArgumentException("Radius cannot be negative");

            this.radius = radius;
        }

        protected override IEnumerable<(int X, int Y)> GetPixelsToSample(int x, int y, Bitmap bitmap)
            => GetRange(y, radius, bitmap.Height).Select(sampleY => (x, sampleY));
    }
}

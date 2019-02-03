using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeaderImgr.ImageProcessing.System.Drawing
{
    public class NaiveBitmapBoxBlurrer : IBitmapOperation
    {
        private readonly int radius;

        public NaiveBitmapBoxBlurrer(int radius)
        {
            if (radius < 0)
                throw new ArgumentException("Radius cannot be negative");

            this.radius = radius;
        }

        public Bitmap Apply(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            foreach (var x in Enumerable.Range(0, bitmap.Width))
            {
                foreach (var y in Enumerable.Range(0, bitmap.Height))
                {
                    applyToPixel(bitmap, newBitmap, x, y);
                }
            }

            return newBitmap;
        }

        private void applyToPixel(Bitmap bitmap, Bitmap newBitmap, int x, int y)
        {
            var argb = getBlurredColour(bitmap, x, y);
            newBitmap.SetPixel(x, y, argb);
        }

        private Color getBlurredColour(Bitmap bitmap, int x, int y)
        {
            var xRange = getRange(x, bitmap.Width);
            var yRange = getRange(y, bitmap.Height);

            var sampleCount = 0;
            var (red, green, blue, alpha) = (0L, 0L, 0L, 0L);

            foreach (var sampleX in xRange)
            {
                foreach (var sampleY in yRange)
                {
                    var sample = bitmap.GetPixel(sampleX, sampleY);

                    red += sample.R;
                    green += sample.G;
                    blue += sample.B;
                    alpha += sample.A;
                    sampleCount++;
                }
            }

            var argb = Color.FromArgb(
                (int) (alpha / sampleCount),
                (int) (red / sampleCount),
                (int) (green / sampleCount),
                (int) (blue / sampleCount)
            );

            return argb;
        }

        private IEnumerable<int> getRange(int center, int maximum)
        {
            var minInclusive = Math.Max(0, center - radius);
            var maxExclusive = Math.Min(maximum, center + radius + 1);

            return Enumerable.Range(minInclusive, maxExclusive - minInclusive);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeaderImgr.ImageProcessing.System.Drawing
{
    public class CompositeBitmapBoxBlurrer : IBitmapOperation
    {
        private readonly int radius;

        public CompositeBitmapBoxBlurrer(int radius)
        {
            if (radius < 0)
                throw new ArgumentException("Radius cannot be negative");

            this.radius = radius;
        }

        public Bitmap Apply(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            var intermediateBitmap = blur(bitmap, true);
            var finalBitmap = blur(intermediateBitmap, false);
            intermediateBitmap.Dispose();

            return finalBitmap;
        }

        private Bitmap blur(Bitmap bitmap, bool vertical)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            foreach (var x in Enumerable.Range(0, bitmap.Width))
            {
                foreach (var y in Enumerable.Range(0, bitmap.Height))
                {
                    applyToPixel(bitmap, newBitmap, x, y, vertical);
                }
            }

            return newBitmap;
        }

        private void applyToPixel(Bitmap bitmap, Bitmap newBitmap, int x, int y, bool vertical)
        {
            var argb = getBlurredColour(bitmap, x, y, vertical);
            newBitmap.SetPixel(x, y, argb);
        }

        private Color getBlurredColour(Bitmap bitmap, int x, int y, bool vertical)
        {
            var (xRange, yRange) = vertical
                ? (Enumerable.Repeat(x, 1), getRange(y, bitmap.Height))
                : (getRange(x, bitmap.Width), Enumerable.Repeat(y, 1));

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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeaderImgr.ImageProcessing.System.Drawing
{
    public abstract class BitmapBlurrerBase : IBitmapOperation
    {
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
            var sampleCount = 0;
            var (red, green, blue, alpha) = (0L, 0L, 0L, 0L);

            foreach (var (sampleX, sampleY) in GetPixelsToSample(x, y, bitmap))
            {
                var sample = bitmap.GetPixel(sampleX, sampleY);

                red += sample.R;
                green += sample.G;
                blue += sample.B;
                alpha += sample.A;
                sampleCount++;
            }

            var argb = Color.FromArgb(
                (int) (alpha / sampleCount),
                (int) (red / sampleCount),
                (int) (green / sampleCount),
                (int) (blue / sampleCount)
            );

            return argb;
        }

        protected abstract IEnumerable<(int X, int Y)> GetPixelsToSample(int x, int y, Bitmap bitmap);

        protected IEnumerable<int> GetRange(int center, int radius, int maximum)
        {
            var minInclusive = Math.Max(0, center - radius);
            var maxExclusive = Math.Min(maximum, center + radius + 1);

            return Enumerable.Range(minInclusive, maxExclusive - minInclusive);
        }
    }
}

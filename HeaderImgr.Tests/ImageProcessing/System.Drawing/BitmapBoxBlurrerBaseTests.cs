using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using HeaderImgr.ImageProcessing.System.Drawing;
using Xunit;

namespace HeaderImgr.Tests.ImageProcessing.System.Drawing
{
    public abstract class BitmapBoxBlurrerBaseTests
    {
        protected abstract IBitmapOperation CreateBlurrerWith(int radius);
        
        [Property]
        public void TheConstructorThrowsWithNegativeRadius(NegativeInt negativeInt)
        {
            Action constructorWithNegativeRadius =
                () => CreateBlurrerWith(negativeInt.Get);

            constructorWithNegativeRadius
                .Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void ThrowsForNullArgument()
        {
            var blurrer = CreateBlurrerWith(0);

            Action callingWithNull = () => blurrer.Apply(null);

            callingWithNull.Should().Throw<ArgumentNullException>();
        }

        [Property(MaxTest = 10)]
        public void ReturnsBitmapOfTheSameSizeAsInput(byte width, byte height)
        {
            var bitmap = new Bitmap(width + 1, height + 1);
            var blurrer = CreateBlurrerWith(0);

            var returnedBitmap = blurrer.Apply(bitmap);

            returnedBitmap.Width.Should().Be(bitmap.Width);
            returnedBitmap.Height.Should().Be(bitmap.Height);
        }

        [Property(MaxTest = 10)]
        public void ReturnsEquivalentImageIfRadiusIsZero(byte width, byte height, int seed)
        {
            var bitmap = randomBitmap(width + 1, height + 1, seed);
            var blurrer = CreateBlurrerWith(0);

            var returnedBitmap = blurrer.Apply(bitmap);

            bitmapsShouldBeEquivalent(bitmap, returnedBitmap);
        }

        [Property(MaxTest = 10)]
        public void ReturnsEquivalentImageForAnyRadiusIfImageIsSinglePixel(NonNegativeInt radius, int seed)
        {
            var bitmap = randomBitmap(1, 1, seed);
            var blurrer = CreateBlurrerWith(radius.Get);

            var returnedBitmap = blurrer.Apply(bitmap);

            bitmapsShouldBeEquivalent(bitmap, returnedBitmap);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(2, 2, 4)]
        [InlineData(5, 1, 5)]
        [InlineData(8, 4, 7)]
        public void ReturnsConstantColorImageIfRadiusIsLargeEnough(int width, int height, int radius)
        {
            var bitmap = randomBitmap(width, height, 0);
            var blurrer = CreateBlurrerWith(radius);

            var returnedBitmap = blurrer.Apply(bitmap);

            bitmapShouldBeConstantColor(returnedBitmap);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void ReturnsImageWithCenterPixelSpreadRadiusFar(int radius)
        {
            var bitmap = new Bitmap(100, 100);
            var blurrer = CreateBlurrerWith(radius);
            bitmap.SetPixel(50, 50, Color.Red);
            var spreadWidth = radius * 2 + 1;
            var spreadArea = spreadWidth * spreadWidth;
            var expectedColorCenter = Color
                .FromArgb(255 / spreadArea, 255 / spreadArea, 0, 0);
            var expectedColorOther = Color.FromArgb(0, 0, 0, 0);

            var returnedBitmap = blurrer.Apply(bitmap);

            foreach (var x in Enumerable.Range(0, bitmap.Width))
            {
                foreach (var y in Enumerable.Range(0, bitmap.Height))
                {
                    var isCenterX = Math.Abs(x - 50) <= radius;
                    var isCenterY = Math.Abs(y - 50) <= radius;
                    var isCenterArea = isCenterX && isCenterY;
                    var expectedColor = isCenterArea ? expectedColorCenter : expectedColorOther;

                    var actualColor = returnedBitmap.GetPixel(x, y);

                    actualColor.Should().Be(expectedColor);
                }
            }
        }

        private void bitmapShouldBeConstantColor(Bitmap bitmap)
        {
            var expectedColor = bitmap.GetPixel(0, 0);

            foreach (var x in Enumerable.Range(0, bitmap.Width))
            {
                foreach (var y in Enumerable.Range(0, bitmap.Height))
                {
                    var actualColor = bitmap.GetPixel(x, y);

                    actualColor.Should().Be(expectedColor);
                }
            }
        }

        private void bitmapsShouldBeEquivalent(Bitmap bitmap1, Bitmap bitmap2)
        {
            bitmap1.Width.Should().Be(bitmap2.Width);
            bitmap1.Height.Should().Be(bitmap2.Height);

            foreach (var x in Enumerable.Range(0, bitmap1.Width))
            {
                foreach (var y in Enumerable.Range(0, bitmap1.Height))
                {
                    var pixel1 = bitmap1.GetPixel(x, y);
                    var pixel2 = bitmap2.GetPixel(x, y);

                    pixel1.Should().Be(pixel2);
                }
            }
        }

        private Bitmap randomBitmap(int width, int height, int seed)
        {
            var bitmap = new Bitmap(width, height);
            var random = new Random(seed);

            foreach (var x in Enumerable.Range(0, width))
            {
                foreach (var y in Enumerable.Range(0, height))
                {
                    var argb = randomColor(random);
                    bitmap.SetPixel(x, y, argb);
                }
            }

            return bitmap;
        }

        private Color randomColor(Random random)
        {
            return Color.FromArgb(
                random.Next(255),
                random.Next(255),
                random.Next(255),
                random.Next(255)
            );
        }
    }
}

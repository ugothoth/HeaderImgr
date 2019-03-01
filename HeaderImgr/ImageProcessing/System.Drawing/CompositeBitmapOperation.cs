using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeaderImgr.ImageProcessing.System.Drawing
{
    public class CompositeBitmapOperation : IBitmapOperation
    {
        private readonly List<IBitmapOperation> operations;

        public CompositeBitmapOperation(params IBitmapOperation[] operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));
            if (operations.Any(o => o == null))
                throw new ArgumentException("Operations cannot contain null operation.", nameof(operations));

            this.operations = operations.ToList();
        }

        public Bitmap Apply(Bitmap bitmap)
        {
            foreach (var operation in operations)
            {
                bitmap = operation.Apply(bitmap);
            }

            return bitmap;
        }
    }
}

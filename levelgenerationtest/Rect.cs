using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levelgenerationtest
{
    public class Rect
    {
        private static Random random = new Random();

        public int left;
        public int top;
        public int right;
        public int bottom;

        public Rect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public Rect(Rect source)
        {
            this.left = source.left;
            this.top = source.top;
            this.right = source.right;
            this.bottom = source.bottom;
        }

        public int width()
        {
            return right - left;
        }

        public int height()
        {
            return bottom - top;
        }

        public int square()
        {
            return (right - left) * (bottom - top);
        }

        public Point center()
        {
            return new Point(
                (left + right) / 2 + (((right - left) & 1) == 1 ? random.Next(2) : 0),
                (top + bottom) / 2 + (((bottom - top) & 1) == 1 ? random.Next(2) : 0));
        }
    }
}

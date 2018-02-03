using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressXPEG.Compression
{
    class ByteBlock
    {

        public ByteBlock(int width, int height)
        {
            this.width = width;
            this.height = height;
            block = new byte[height, width];
        }

        public void SetByte(int x, int y, byte value)
        {
            block[y, x] = value;
        }

        public byte GetByte(int x, int y)
        {
            return block[y, x];
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        private int width;
        private int height;
        private byte[,] block;
    }
}

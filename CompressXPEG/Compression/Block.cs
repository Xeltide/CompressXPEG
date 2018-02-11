using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressXPEG.Compression
{
    class Block<T>
    {

        public Block(int width, int height)
        {
            this.width = width;
            this.height = height;
            block = new T[height, width];
        }
        
        // Culls the channel by keeping every 1 in scaleDownFactor row-cols
        // Culls the block to a factor of newBlockSize
        public Block<T> CullBlockToBlockFactor(int scaleDownFactor, int blockDivisibleBy)
        {
            // Culled dimensions
            int resizeW = (int)Math.Round((float)this.width / scaleDownFactor);
            int resizeH = (int)Math.Round((float)this.height / scaleDownFactor);
            // Dimension padding
            int xPad = resizeW % blockDivisibleBy;
            int yPad = resizeH % blockDivisibleBy;
            // Added cull padding for 0's
            resizeW += xPad;
            resizeH += yPad;
            // Auto pads 0's
            Block<T> output = new Block<T>(resizeW, resizeH);

            for (int y = 0; y * scaleDownFactor < this.height; y++)
            {
                for (int x = 0; x * scaleDownFactor < this.width; x++)
                {
                    output.SetCell(x, y, this.GetCell(x * scaleDownFactor, y * scaleDownFactor));
                }
            }

            return output;
        }

        public Block<T> PadBlock (int scaleFactor)
        {
            Block<T> output = new Block<T>(width * 2, height * 2);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int yScale = 0; yScale < scaleFactor; yScale++)
                    {
                        for (int xScale = 0; xScale < scaleFactor; xScale++)
                        {
                            output.SetCell((x * scaleFactor) + xScale, (y * scaleFactor) + yScale, GetCell(x, y));
                        }
                    }
                }
            }

            return output;
        }

        public void SetCell(int x, int y, T value)
        {
            block[y, x] = value;
        }

        public T GetCell(int x, int y)
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
        private T[,] block;
    }
}

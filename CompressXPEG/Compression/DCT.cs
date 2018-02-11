using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressXPEG.Compression
{
    static class DCT
    {
        // Constant returned for DCT
        private static float C(int i)
        {
            return i == 0 ? (float)(1 / Math.Sqrt(2)) : 1;
        }

        // DCT's and returns an 8x8 block
        // TESTED, temp public
        private static Block<short> DCTBlock(Block<short> input, int xStart, int yStart, int blockSize)
        {
            Block<short> block = new Block<short>(blockSize, blockSize);

            for (int j = 0; j < blockSize; j++)
            {
                for (int i = 0; i < blockSize; i++)
                {
                    float dct = 0;
                    for (int y = 0; y < blockSize; y++)
                    {
                        for (int x = 0; x < blockSize; x++)
                        {
                            float denom = (float)(2 * blockSize);

                            float xNum = (float)(((2 * x) + 1) * i * Math.PI);
                            float xCos = (float)Math.Cos(xNum / denom);

                            float yNum = (float)(((2 * y) + 1) * j * Math.PI);
                            float yCos = (float)Math.Cos(yNum / denom);
                            dct += input.GetCell(x + xStart, y + yStart) * xCos * yCos;
                        }
                    }
                    short pixel = (short)Math.Round(0.25 * C(i) * C(j) * dct, MidpointRounding.AwayFromZero);
                    block.SetCell(i, j, pixel);
                }
            }

            return block;
        }

        // Breaks channels into 8x8 blocks and runs DCT
        public static List<Block<short>> DCTChannel(Block<short> channel, int blockSize)
        {
            List<Block<short>> output = new List<Block<short>>();
            int i = 0;
            for (int y = 0; y + blockSize <= channel.GetHeight(); y += blockSize)
            {
                for (int x = 0; x + blockSize <= channel.GetWidth(); x += blockSize)
                {
                    i++;
                    output.Add(DCTBlock(channel, x, y, blockSize));
                }
            }

            return output;
        }

        private static void IDCTBlock(Block<byte> channelOut, int xStart, int yStart, Block<short> input)
        {
            for (int j = 0; j < input.GetHeight(); j++)
            {
                for (int i = 0; i < input.GetWidth(); i++)
                {
                    float colour = 0;
                    for (int y = 0; y < input.GetHeight(); y++)
                    {
                        for (int x = 0; x < input.GetWidth(); x++)
                        {
                            float denom = (float)(2 * input.GetWidth());

                            float xNum = (float)(((2 * i) + 1) * x * Math.PI);
                            float xCos = (float)Math.Cos(xNum / denom);

                            float yNum = (float)(((2 * j) + 1) * y * Math.PI);
                            float yCos = (float)Math.Cos(yNum / denom);
                            colour += (float)(0.25 * C(x) * C(y) * input.GetCell(x, y) * xCos * yCos);
                        }
                    }
                    if (colour > 255)
                    {
                        colour = 255;
                    }
                    else if (colour < 0)
                    {
                        colour = 0;
                    }
                    channelOut.SetCell(i + xStart, j + yStart, (byte)Math.Round(colour, MidpointRounding.AwayFromZero));
                }
            }
        }

        public static Block<byte> IDCTChannel(List<Block<short>> dctBlocks, int width, int height)
        {
            Block<byte> block = new Block<byte>(width, height);

            int blockHeight = dctBlocks[0].GetHeight();
            int blockWidth = dctBlocks[0].GetWidth();
            int blocksPerRow = width / blockWidth;
            for (int y = 0; y < height; y += blockHeight)
            {
                for (int x = 0; x < width; x += blockWidth)
                {
                    int blockY = y / dctBlocks[0].GetHeight();
                    int blockX = x / dctBlocks[0].GetWidth();
                    IDCTBlock(block, x, y, dctBlocks[(blockY * blocksPerRow) + blockX]);
                }
            }

            return block;
        }
    }
}

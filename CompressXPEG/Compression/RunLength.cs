using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressXPEG.Compression
{
    static class RunLength
    {
        public static List<List<Block<short>>> RLD(byte[] compressedData, int blockSize, int width, int height, byte[,] lumiQT, byte[,] chromaQT)
        {
            List<List<Block<short>>> lumiChromLists = new List<List<Block<short>>>();
            List<Block<short>> dctBlocks = new List<Block<short>>();
            byte[,] qt = lumiQT;

            // Calculate channel indexes
            int yBlockW = (int)Math.Ceiling(width / 8.0);
            int yBlockH = (int)Math.Ceiling(height / 8.0);
            int cbStartIndex = yBlockW * yBlockH;

            int rawDataIndex = 0;
            int blockNumber = 0;
            while (rawDataIndex < compressedData.Length)
            {
                Block<short> block = new Block<short>(blockSize, blockSize);
                byte run = 0;
                // TOP LEFT TRIANGLE
                int maxFinal = 7;
                int maxCurrent = 0;
                while (maxCurrent <= maxFinal)
                {
                    int x = 0;
                    int y = maxCurrent;
                    while (x <= maxCurrent)
                    {
                        if (rawDataIndex >= compressedData.Length)
                        {
                            break;
                        }

                        short current = (short)((sbyte)compressedData[rawDataIndex] * qt[y, x]);

                        if (current != 0 && run == 0)
                        {
                            block.SetCell(x, y, current);
                            rawDataIndex++;
                        }
                        else if (current == 0 && run == 0)
                        {
                            rawDataIndex++;
                            run = compressedData[rawDataIndex];
                            run--;
                            rawDataIndex++;
                        }
                        else
                        {
                            run--;
                        }
                        x++;
                        y--;
                    }
                    maxCurrent++;
                }

                // BOTTOM RIGHT TRIANGLE
                int minFinal = 7;
                int minCurrent = 1;
                while (minCurrent <= minFinal)
                {
                    int x = minCurrent;
                    int y = minFinal;
                    while (y >= minCurrent)
                    {
                        if (rawDataIndex >= compressedData.Length)
                        {
                            break;
                        }

                        short current = (short)((sbyte)compressedData[rawDataIndex] * qt[y, x]);

                        if (current != 0 && run == 0)
                        {
                            block.SetCell(x, y, current);
                            rawDataIndex++;
                        }
                        else if (current == 0 && run == 0)
                        {
                            rawDataIndex++;
                            run = compressedData[rawDataIndex];
                            run--;
                            rawDataIndex++;
                        }
                        else
                        {
                            run--;
                        }
                        x++;
                        y--;
                    }
                    minCurrent++;
                }
                blockNumber++;
                dctBlocks.Add(block);
                
                if (blockNumber == cbStartIndex)
                {
                    lumiChromLists.Add(dctBlocks);
                    dctBlocks = new List<Block<short>>();
                    qt = chromaQT;
                }
            }

            lumiChromLists.Add(dctBlocks.GetRange(0, dctBlocks.Count / 2));
            lumiChromLists.Add(dctBlocks.GetRange(dctBlocks.Count / 2, dctBlocks.Count / 2));

            return lumiChromLists;
        }

        public static List<byte> RLE(List<Block<short>> channel, byte[,] qt)
        {
            List<byte> output = new List<byte>();

            foreach (Block<short> block in channel)
            {
                byte run = 0;
                // TOP LEFT TRIANGLE
                int maxFinal = 7;
                int maxCurrent = 0;
                while (maxCurrent <= maxFinal)
                {
                    int x = 0;
                    int y = maxCurrent;
                    while (x <= maxCurrent)
                    {
                        short current = (short)(block.GetCell(x, y) / qt[y, x]);
                        if (current > 127)
                        {
                            current = 127;
                        } else if (current < -128)
                        {
                            current = -128;
                        }

                        sbyte quantized = (sbyte)current;

                        if (quantized == 0 && run == 0)
                        {
                            output.Add(0);
                            run++;
                        }
                        else if (quantized == 0 && run > 0)
                        {
                            run++;
                        }
                        else if (run > 0)
                        {
                            output.Add(run);
                            run = 0;
                            output.Add((byte)quantized);
                        }
                        else
                        {
                            output.Add((byte)quantized);
                        }
                        x++;
                        y--;
                    }
                    maxCurrent++;
                }

                // BOTTOM RIGHT TRIANGLE
                int minFinal = 7;
                int minCurrent = 1;
                while (minCurrent <= minFinal)
                {
                    int x = minCurrent;
                    int y = minFinal;
                    while (y >= minCurrent)
                    {
                        short current = (short)(block.GetCell(x, y) / qt[y, x]);
                        if (current > 127)
                        {
                            current = 127;
                        }
                        else if (current < -128)
                        {
                            current = -128;
                        }

                        sbyte quantized = (sbyte)current;

                        if (quantized == 0 && run == 0)
                        {
                            output.Add(0);
                            run++;
                        }
                        else if (quantized == 0 && run > 0)
                        {
                            run++;
                        }
                        else if (run > 0)
                        {
                            output.Add(run);
                            run = 0;
                            output.Add((byte)quantized);
                        }
                        else
                        {
                            output.Add((byte)quantized);
                        }
                        x++;
                        y--;
                    }
                    minCurrent++;
                }

                if (run > 0)
                {
                    output.Add((byte)run);
                }
            }

            return output;
        }
    }
}

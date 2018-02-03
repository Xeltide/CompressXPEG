using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

// COMPRESSION:
// Convert RGB channels to YCrCb
// Cut out every 2nd row-col for CrCb
// For each channel, run DCT
// Quantize the channels
// Write 4 bytes for w&h, plus RLE

// DECOMPRESSION:
// Create bitmap of w&h
// Load YCrCb with reversed RLE
// Reverse quantization
// Inverse DCT
// Pad row-col CrCb with duplicate
// Convert YCrCb to RGB
namespace CompressXPEG.Compression
{
    class CompressJPEG
    {
        // TODO: Remove!!! Only for testing
        public CompressJPEG()
        {

        }

        public CompressJPEG(Bitmap b)
        {
            this.bitmap = b;
        }

        // Public facing compression function
        public void Compress()
        {
            BitmapRGBToYCC(this.bitmap);

            ByteBlock cbCulled = CullPadChannel(cbChannel);
            ByteBlock crCulled = CullPadChannel(crChannel);

            List<ByteBlock> yBlocks = DCTChannel(yChannel);
            List<ByteBlock> cbBlocks = DCTChannel(cbChannel);
            List<ByteBlock> crBlocks = DCTChannel(crChannel);
        }

        // Constant returned for DCT
        private float C(int i)
        {
            return i == 0 ? (float)(1 / Math.Sqrt(2)) : 1;
        }

        // DCT's and returns an 8x8 block
        // TESTED, temp public
        public ByteBlock DCTBlock(ByteBlock input, int xStart, int yStart)
        {
            ByteBlock block = new ByteBlock(8, 8);

            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    float dct = 0;
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            float denom = (float)(2 * 8);

                            float xNum = (float)(((2 * x) + 1) * i * Math.PI);
                            float xCos = (float)Math.Cos(xNum / denom);

                            float yNum = (float)(((2 * y) + 1) * j * Math.PI);
                            float yCos = (float)Math.Cos(yNum / denom);
                            dct += input.GetByte(x + xStart, y + yStart) * xCos * yCos;
                        }
                    }
                    byte pixel = (byte)Math.Round(0.25 * C(i) * C(j) * dct);
                    block.SetByte(i, j, pixel);
                }
            }
            return block;
        }

        // Breaks channels into 8x8 blocks and runs DCT
        private List<ByteBlock> DCTChannel(ByteBlock channel)
        {
            List<ByteBlock> output = new List<ByteBlock>();

            for (int y = 0; y < channel.GetHeight(); y += 8)
            {
                for (int x = 0; x < channel.GetWidth(); x += 8)
                {
                    output.Add(DCTBlock(channel, x, y));
                }
            }

            return output;
        }

        // Culls every 2nd row/col, and pads to fit 8x8 blocks
        // TESTED, temp public
        public ByteBlock CullPadChannel(ByteBlock channel)
        {
            // Culled dimensions
            int resizeW = (int)Math.Round(channel.GetWidth() / 2.0);
            int resizeH = (int)Math.Round(channel.GetHeight() / 2.0);
            // Dimension padding
            int xPad = resizeW % 8;
            int yPad = resizeH % 8;
            // Added cull padding for 0's
            resizeW += xPad;
            resizeH += yPad;
            // Auto pads 0's
            ByteBlock output = new ByteBlock(resizeW, resizeH);

            for (int y = 0; y * 2 < channel.GetHeight(); y++)
            {
                for (int x = 0; x * 2 < channel.GetWidth(); x++)
                {
                    output.SetByte(x, y, channel.GetByte(x * 2, y * 2));
                }
            }

            return output;
        }

        // Converts RGB colors to YCC
        // Pads Y channel to fit 8x8 blocks
        private void BitmapRGBToYCC(Bitmap b)
        {
            int xPad = b.Width % 8;
            int yPad = b.Height % 8;
            // Padded since no culling
            yChannel = new ByteBlock(b.Width + xPad, b.Height + yPad);
            // Pad after, in culling function
            cbChannel = new ByteBlock(b.Width, b.Height);
            crChannel = new ByteBlock(b.Width, b.Height);
            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    // Load YCC channels
                    Color ycc = RGBToYCC(b.GetPixel(x, y));
                    yChannel.SetByte(x, y, ycc.R);
                    cbChannel.SetByte(x, y, ycc.G);
                    crChannel.SetByte(x, y, ycc.B);
                }
            }
        }

        // Returns Color:
        // [ R, G, B ] |-> [ Y, Cr, Cb ]
        private Color RGBToYCC(Color c)
        {
            byte y = (byte)(16 + (((65.738 * c.R) / 256) + ((128.533 * c.G) / 256) + ((24.966 * c.B) / 256)));
            byte cb = (byte)(128 - (((37.954 * c.R) / 256) - ((74.494 * c.G) / 256) + ((112.439 * c.B) / 256)));
            byte cr = (byte)(128 + (((112.439 * c.R) / 256) - ((94.154 * c.G) / 256) - ((18.285 * c.B) / 256)));
            Color output = Color.FromArgb(y, cb, cr);
            return output;
        }

        private Bitmap bitmap;
        // Color-split, non-culled
        private ByteBlock yChannel;
        private ByteBlock cbChannel;
        private ByteBlock crChannel;
    }
}

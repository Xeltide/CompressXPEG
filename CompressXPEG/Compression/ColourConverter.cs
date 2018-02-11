using System.Collections.Generic;
using System.Drawing;

namespace CompressXPEG.Compression
{
    static class ColourConverter
    {
        // Converts RGB colors to YCC
        // Pads Y channel to fit 8x8 blocks
        public static List<Block<short>> BitmapRGBToYCC(Bitmap b, int blockWidth)
        {
            List<Block<short>> colourChannels = new List<Block<short>>();

            int xPad = b.Width % blockWidth;
            int yPad = b.Height % blockWidth;
            // Padded since no culling
            Block<short> yChannel = new Block<short>(b.Width + xPad, b.Height + yPad);
            // Pad after, in culling function
            Block<short> cbChannel = new Block<short>(b.Width, b.Height);
            Block<short> crChannel = new Block<short>(b.Width, b.Height);
            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    // Load YCC channels
                    Color ycc = RGBToYCC(b.GetPixel(x, y));
                    yChannel.SetCell(x, y, ycc.R);
                    cbChannel.SetCell(x, y, ycc.G);
                    crChannel.SetCell(x, y, ycc.B);
                }
            }
            colourChannels.Add(yChannel);
            colourChannels.Add(cbChannel);
            colourChannels.Add(crChannel);

            return colourChannels;
        }

        // Returns Color:
        // [ R, G, B ] |-> [ Y, Cr, Cb ]
        public static Color RGBToYCC(Color c)
        {
            int y = (int)((0.299 * c.R) + (0.587 * c.G) + (0.114 * c.B));
            int cb = (int)(128 - (0.168736 * c.R) - (0.331264 * c.G) + (0.5 * c.B));
            int cr = (int)(128 + (0.5 * c.R) - (0.418688 * c.G) - (0.081312 * c.B));

            return Color.FromArgb(CheckThreshold(y), CheckThreshold(cb), CheckThreshold(cr));
        }

        public static Color YCCToRGB(byte y, byte cb, byte cr)
        {
            int r = (int)(y + 1.402 * (cr - 128));
            int g = (int)(y - 0.344136 * (cb - 128) - 0.714136 * (cr - 128));
            int b = (int)(y + 1.772 * (cb - 128));
            
            return Color.FromArgb(CheckThreshold(r), CheckThreshold(g), CheckThreshold(b));
        }

        private static byte CheckThreshold(int pixelChannel)
        {
            if (pixelChannel > 255)
            {
                return 255;
            }
            else if (pixelChannel < 0)
            {
                return 0;
            }
            return (byte)pixelChannel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CompressXPEG.Compression
{
    class JAPGDecompressor
    {
        // Byte order is Y, Cb, Cr per channel (not interleaving)
        public JAPGDecompressor(byte[] rawData)
        {
            width = BitConverter.ToInt16(rawData, 0);
            height = BitConverter.ToInt16(rawData, 2);
            compressedData = new byte[rawData.Length - 4];
            Array.Copy(rawData, 4, compressedData, 0, rawData.Length - 4);
        }

        public Bitmap Decompress()
        {
            // Load blocks
            Quantizer quantizer = new Quantizer();
            List<List<Block<short>>> channelBlocks = RunLength.RLD(compressedData, 8, width, height, quantizer.GetLumiQT(), quantizer.GetChromaQT());

            Block<byte> yChannel = DCT.IDCTChannel(channelBlocks[0], width, height);
            Block<byte> cbChannel = DCT.IDCTChannel(channelBlocks[1], width / 2, height / 2);
            Block<byte> crChannel = DCT.IDCTChannel(channelBlocks[2], width / 2, height / 2);

            Block<byte> cbPadded = cbChannel.PadBlock(2);
            Block<byte> crPadded = crChannel.PadBlock(2);

            List<byte> uncompressed = LoadImageData(yChannel, cbPadded, crPadded);

            return CreateBitmapFromBytes(width, height, uncompressed);
        }

        public List<byte> LoadImageData(Block<byte> yChannel, Block<byte> cbChannel, Block<byte> crChannel)
        {
            List<byte> rawImageData = new List<byte>();

            for (int j = 0; j < yChannel.GetHeight(); j++)
            {
                for (int i = 0; i < yChannel.GetWidth(); i++)
                {
                    Color rgb = ColourConverter.YCCToRGB(yChannel.GetCell(i, j), cbChannel.GetCell(i, j), crChannel.GetCell(i, j));
                    rawImageData.Add(rgb.B);
                    rawImageData.Add(rgb.G);
                    rawImageData.Add(rgb.R);
                }
            }

            return rawImageData;
        }

        // Points the bitmap data to the input byte stream
        public Bitmap CreateBitmapFromBytes(int width, int height, List<byte> input)
        {
            PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
            Bitmap b = new Bitmap(width, height, pixelFormat);
            BitmapData bmpData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
            Marshal.Copy(input.ToArray(), 0, bmpData.Scan0, input.Count);
            b.UnlockBits(bmpData);

            return b;
        }

        private int width;
        private int height;
        private byte[] compressedData;
    }
}

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
    class JAPGCompressor
    {
        public JAPGCompressor(Bitmap b)
        {
            this.bitmap = b;
        }
        
        public List<byte> Compress()
        {
            List<Block<short>> yccChannels = ColourConverter.BitmapRGBToYCC(this.bitmap, 8);

            Block<short> cbCulled = yccChannels[1].CullBlockToBlockFactor(2, 8);
            Block<short> crCulled = yccChannels[2].CullBlockToBlockFactor(2, 8);

            List<Block<short>> yBlocks = DCT.DCTChannel(yccChannels[0], 8);
            List<Block<short>> cbBlocks = DCT.DCTChannel(cbCulled, 8);
            List<Block<short>> crBlocks = DCT.DCTChannel(crCulled, 8);

            Quantizer quantizer = new Quantizer();
            List<byte> yStream = RunLength.RLE(yBlocks, quantizer.GetLumiQT());
            List<byte> cbStream = RunLength.RLE(cbBlocks, quantizer.GetChromaQT());
            List<byte> crStream = RunLength.RLE(crBlocks, quantizer.GetChromaQT());

            yStream.AddRange(cbStream);
            yStream.AddRange(crStream);

            return yStream;
        }

        private Bitmap bitmap;
    }
}

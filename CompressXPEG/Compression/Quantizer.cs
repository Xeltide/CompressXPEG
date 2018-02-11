using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressXPEG.Compression
{
    class Quantizer
    {

        public Quantizer()
        {
            this.chromQT = new byte[,]{
                { 17, 18, 24, 47, 99, 99, 99, 99 },
                { 18, 21, 26, 66, 99, 99, 99, 99 },
                { 24, 26, 56, 99, 99, 99, 99, 99 },
                { 47, 66, 99, 99, 99, 99, 99, 99 },
                { 99, 99, 99, 99, 99, 99, 99, 99 },
                { 99, 99, 99, 99, 99, 99, 99, 99 },
                { 99, 99, 99, 99, 99, 99, 99, 99 },
                { 99, 99, 99, 99, 99, 99, 99, 99 }
            };
            this.lumQT = new byte[,]{
                { 16, 11, 10, 16, 24, 40, 51, 61 },
                { 12, 12, 14, 19, 26, 58, 60, 55 },
                { 14, 13, 16, 24, 40, 57, 69, 56 },
                { 14, 17, 22, 29, 51, 87, 80, 62 },
                { 18, 22, 37, 56, 68, 109, 103, 77 },
                { 24, 35, 55, 64, 81, 104, 113, 92 },
                { 49, 64, 78, 87, 103, 121, 120, 101 },
                { 72, 92, 95, 98, 112, 100, 103, 99 }
            };
        }

        public Quantizer(string filePath)
            : this(filePath, filePath)
        { /**/ }

        public Quantizer(string lumiFilePath, string chromaFilePath)
        {
            // TODO: file loader to set QT
        }

        public byte GetQFactorLumi(int x, int y)
        {
            return lumQT[y, x];
        }

        public byte GetQFactorChroma(int x, int y)
        {
            return chromQT[y, x];
        }

        public byte[,] GetLumiQT()
        {
            return lumQT;
        }

        public byte[,] GetChromaQT()
        {
            return chromQT;
        }

        private byte[,] lumQT;
        private byte[,] chromQT;
        private float quantizerCoefficient = 1;
    }
}

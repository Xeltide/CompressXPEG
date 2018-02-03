using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CompressXPEG
{
    class ActionBar : MenuStrip
    {

        public ActionBar(AppStore store)
        {
            this.store = store;
            BackColor = Color.FromArgb(0, 0, 0);

            // File menu items
            ToolStripMenuItem file = new ToolStripMenuItem("File");
            file.ForeColor = Color.FromArgb(255, 255, 255);
            ToolStripMenuItem newItem = new ToolStripMenuItem("New");
            newItem.Click += new EventHandler(OnNew);
            file.DropDownItems.Add(newItem);
            ToolStripMenuItem openItem = new ToolStripMenuItem("Open");
            openItem.Click += new EventHandler(OnOpen);
            file.DropDownItems.Add(openItem);

            this.Items.Add(file);

            // Compress menu items
            ToolStripMenuItem compress = new ToolStripMenuItem("Compress");
            compress.ForeColor = Color.FromArgb(255, 255, 255);
            ToolStripMenuItem jpeg = new ToolStripMenuItem("JPEG");
            jpeg.Click += new EventHandler(OnJPEG);
            compress.DropDownItems.Add(jpeg);

            this.Items.Add(compress);
        }

        private void OnNew(object sender, EventArgs e)
        {
            store.ImagePath = null;
        }

        private void OnOpen(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                store.ImagePath = ofd.FileName;
            }
        }

        private void OnJPEG(object sender, EventArgs e)
        {
            if (store.Images.Count > 0)
            {
                Compression.CompressJPEG compressor = new Compression.CompressJPEG(store.Images[0]);
                List<byte> output = compressor.Compress();
                Console.WriteLine("Raw byte output: " + output.Count);
            }
            /*Compression.CompressJPEG cmpress = new Compression.CompressJPEG();
            Compression.ByteBlock block = new Compression.ByteBlock(8, 8);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    block.SetByte(x, y, (byte)x);
                    Console.Write(block.GetByte(x, y) + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Compression.ByteBlock cull = cmpress.CullPadChannel(block);
            Compression.ByteBlock dctRes = cmpress.DCTBlock(cull, 0, 0);

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Console.Write((sbyte)cull.GetByte(x, y) + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();*/
            /*Compression.CompressJPEG cmpress = new Compression.CompressJPEG();
            Compression.ByteBlock test = new Compression.ByteBlock(8, 8);
            test.SetByte(0, 0, 168);
            test.SetByte(1, 0, 161);
            test.SetByte(2, 0, 161);
            test.SetByte(3, 0, 150);
            test.SetByte(4, 0, 154);
            test.SetByte(5, 0, 168);
            test.SetByte(6, 0, 164);
            test.SetByte(7, 0, 154);

            test.SetByte(0, 1, 171);
            test.SetByte(1, 1, 154);
            test.SetByte(2, 1, 161);
            test.SetByte(3, 1, 150);
            test.SetByte(4, 1, 157);
            test.SetByte(5, 1, 171);
            test.SetByte(6, 1, 150);
            test.SetByte(7, 1, 164);

            test.SetByte(0, 2, 171);
            test.SetByte(1, 2, 168);
            test.SetByte(2, 2, 147);
            test.SetByte(3, 2, 164);
            test.SetByte(4, 2, 164);
            test.SetByte(5, 2, 161);
            test.SetByte(6, 2, 143);
            test.SetByte(7, 2, 154);

            test.SetByte(0, 3, 164);
            test.SetByte(1, 3, 171);
            test.SetByte(2, 3, 154);
            test.SetByte(3, 3, 161);
            test.SetByte(4, 3, 157);
            test.SetByte(5, 3, 157);
            test.SetByte(6, 3, 147);
            test.SetByte(7, 3, 132);

            test.SetByte(0, 4, 161);
            test.SetByte(1, 4, 161);
            test.SetByte(2, 4, 157);
            test.SetByte(3, 4, 154);
            test.SetByte(4, 4, 143);
            test.SetByte(5, 4, 161);
            test.SetByte(6, 4, 154);
            test.SetByte(7, 4, 132);

            test.SetByte(0, 5, 164);
            test.SetByte(1, 5, 161);
            test.SetByte(2, 5, 161);
            test.SetByte(3, 5, 154);
            test.SetByte(4, 5, 150);
            test.SetByte(5, 5, 157);
            test.SetByte(6, 5, 154);
            test.SetByte(7, 5, 140);

            test.SetByte(0, 6, 161);
            test.SetByte(1, 6, 168);
            test.SetByte(2, 6, 157);
            test.SetByte(3, 6, 154);
            test.SetByte(4, 6, 161);
            test.SetByte(5, 6, 140);
            test.SetByte(6, 6, 140);
            test.SetByte(7, 6, 132);

            test.SetByte(0, 7, 154);
            test.SetByte(1, 7, 161);
            test.SetByte(2, 7, 157);
            test.SetByte(3, 7, 150);
            test.SetByte(4, 7, 140);
            test.SetByte(5, 7, 132);
            test.SetByte(6, 7, 136);
            test.SetByte(7, 7, 128);
            Compression.ByteBlock dct = cmpress.DCTBlock(test, 0, 0);
            // TODO: Confirm that 0, 0 should return 0-255 (unsigned byte)
            //       and rest return signed byte
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Console.Write(dct.GetByte(x, y) + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            List<Compression.ByteBlock> wrapper = new List<Compression.ByteBlock>();
            wrapper.Add(dct);
            byte[,] lumQT = new byte[,]{
                { 16, 11, 10, 16, 24, 40, 51, 61 },
                { 12, 12, 14, 19, 26, 58, 60, 55 },
                { 14, 13, 16, 24, 40, 57, 69, 56 },
                { 14, 17, 22, 29, 51, 87, 80, 62 },
                { 18, 22, 37, 56, 68, 109, 103, 77 },
                { 24, 35, 55, 64, 81, 104, 113, 92 },
                { 49, 64, 78, 87, 103, 121, 120, 101 },
                { 72, 92, 95, 98, 112, 100, 103, 99 }
            };
            List<byte> quantized = cmpress.QuantizeRLE(wrapper, lumQT);
            foreach (byte b in quantized)
            {
                Console.WriteLine((sbyte)b);
            }*/
        }

        private AppStore store;
    }
}

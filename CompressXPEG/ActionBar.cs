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
                Console.WriteLine("OK Clicked: " + ofd.FileName);
            }
        }

        private void OnJPEG(object sender, EventArgs e)
        {
            Compression.CompressJPEG cmpress = new Compression.CompressJPEG();
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
        }

        private AppStore store;
    }
}

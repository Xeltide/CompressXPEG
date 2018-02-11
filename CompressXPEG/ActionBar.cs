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
            ToolStripMenuItem openItem = new ToolStripMenuItem("Import");
            openItem.Click += new EventHandler(OnOpen);
            file.DropDownItems.Add(openItem);
            ToolStripMenuItem importJAPG = new ToolStripMenuItem("Import JAPG");
            importJAPG.Click += new EventHandler(OnImportJAPG);
            file.DropDownItems.Add(importJAPG);
            ToolStripMenuItem importJAMPG = new ToolStripMenuItem("Import JAMPG");
            importJAMPG.Click += new EventHandler(OnImportJAMPG);
            file.DropDownItems.Add(importJAMPG);

            this.Items.Add(file);

            // Compress menu items
            ToolStripMenuItem compress = new ToolStripMenuItem("Compress");
            compress.ForeColor = Color.FromArgb(255, 255, 255);
            ToolStripMenuItem jpeg = new ToolStripMenuItem("JAPG");
            jpeg.Click += new EventHandler(OnJPEG);
            compress.DropDownItems.Add(jpeg);
            ToolStripMenuItem jampg = new ToolStripMenuItem("JAMPG");
            jampg.Click += new EventHandler(OnJAMPG);
            compress.DropDownItems.Add(jampg);

            this.Items.Add(compress);
        }

        private void OnNew(object sender, EventArgs e)
        {
            store.Images = new List<ImageBundle>();
        }

        private void OnOpen(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP; *.JPG; *.PNG)| *.BMP; *.JPG; *.PNG; | All files(*.*) | *.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImageBundle ib = new ImageBundle(ofd.FileName);
                ib.FileName = ofd.SafeFileName;
                store.AddImage(ib);
            }
        }

        private void OnImportJAPG(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image File(*.JAPG)| *.JAPG; | All files(*.*) | *.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] rawData = System.IO.File.ReadAllBytes(ofd.FileName);
                Compression.JAPGDecompressor decompress = new Compression.JAPGDecompressor(rawData);
                ImageBundle ib = new ImageBundle(decompress.Decompress());
                ib.FilePath = ofd.FileName;
                ib.FileName = ofd.SafeFileName;
                store.AddImage(ib);
            }
        }

        private void OnImportJAMPG(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image File(*.JAMPG)| *.JAMPG; | All files(*.*) | *.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // TODO: MPEG decompression
            }
        }

        private void OnJPEG(object sender, EventArgs e)
        {
            if (store.CurrentImage != null)
            {
                Compression.JAPGCompressor compress = new Compression.JAPGCompressor(store.CurrentImage.Image);
                List<byte> compressed = compress.Compress();
                byte[] wByte = BitConverter.GetBytes(store.CurrentImage.Image.Width);
                byte[] hByte = BitConverter.GetBytes(store.CurrentImage.Image.Height);
                compressed.Insert(0, hByte[1]);
                compressed.Insert(0, hByte[0]);
                compressed.Insert(0, wByte[1]);
                compressed.Insert(0, wByte[0]);
                Compression.JAPGStream.ByteToFile(System.IO.Path.ChangeExtension(store.CurrentImage.FilePath, "japg"), compressed.ToArray());
            }
        }

        private void OnJAMPG(object sender, EventArgs e)
        {
            if (store.Images.Count > 0)
            {
                // TODO: MPEG compression
            }
        }

        private AppStore store;
    }
}

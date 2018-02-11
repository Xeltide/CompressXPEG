using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CompressXPEG
{
    class ImageBundle
    {
        public ImageBundle(Bitmap b)
        {
            this.Image = b;
        }

        public ImageBundle(string filePath)
        {
            this.FilePath = filePath;
            try
            {
                this.Image = new Bitmap(filePath);
                this.FailedLoad = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                this.FailedLoad = true;
            }
        }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Bitmap Image { get; set; }
        public bool FailedLoad { get; set; }
    }
}

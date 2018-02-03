﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CompressXPEG
{
    class DrawPanel : Panel
    {
        public DrawPanel(AppStore store)
        {
            this.store = store;
            store.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PropertyChangedEvent);

            this.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
            this.BackColor = Color.FromArgb(125, 125, 125);
            this.Dock = DockStyle.Fill;

            this.Resize += new EventHandler(OnResize);
            this.Paint += new PaintEventHandler(OnPaint);
        }

        private void PropertyChangedEvent(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ImagePath")
            {
                if (store.Images.Count == 0)
                {
                    Bitmap b = new Bitmap(store.ImagePath);
                    store.AddImage(b);
                }
                Invalidate();
            }
        }

        private Size GetFillDimensions(Bitmap img)
        {
            Size output;

            float hPercent = (this.Height - 24) / (float)img.Height;
            float wPercent = this.Width / (float)img.Width;
            float nPercent;
            if (hPercent < wPercent)
            {
                nPercent = hPercent;
            }
            else
            {
                nPercent = wPercent;
            }

            output = new Size((int)(img.Width * nPercent), (int)(img.Height * nPercent));
            return output;
        }

        private void OnResize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (store.ImagePath != null)
            {
                Size imgSize = GetFillDimensions(store.Images[0]);

                g.DrawImage(store.Images[0], 0, 24, imgSize.Width, imgSize.Height);
            }
        }

        private AppStore store;
    }
}

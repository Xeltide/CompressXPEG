using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompressXPEG
{
    public partial class WindowForm : Form
    {
        public WindowForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            store = new AppStore();
            
            ActionBar bar = new ActionBar(store);
            this.Controls.Add(bar);

            FrameList list = new FrameList(store);
            list.Size = new Size(200, Height - 62);
            list.Location = new Point(0, 24);
            this.Controls.Add(list);

            DrawPanel panel = new DrawPanel(store);
            panel.Size = new Size(Width - 200, Height - 24);
            panel.Location = new Point(200, 0);
            this.Controls.Add(panel);
        }

        private AppStore store;
    }
}

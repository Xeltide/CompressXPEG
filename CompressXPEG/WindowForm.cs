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

            DrawPanel panel = new DrawPanel(store);
            this.Controls.Add(panel);
        }

        private AppStore store;
    }
}

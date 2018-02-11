using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CompressXPEG
{
    class FrameList : ListView
    {

        public FrameList(AppStore store)
        {
            this.store = store;
            store.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PropertyChangedEvent);

            this.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left;
            this.BackColor = Color.FromArgb(200, 200, 200);
            this.View = View.Details;
            this.GridLines = true;
            this.FullRowSelect = true;
            this.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            ColumnHeader column = new ColumnHeader();
            column.Text = "Frames";
            column.Width = 200 - 4;
            this.Columns.Add(column);

            this.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(ItemClicked);
        }

        private void ItemClicked(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            store.CurrentImage = store.Images[e.ItemIndex];
        }

        private void PropertyChangedEvent(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ImageAdded")
            {
                ListViewItem item = new ListViewItem(store.CurrentImage.FileName);
                item.BackColor = Color.FromArgb(200, 200, 200);
                DeselectItems();
                item.Selected = true;
                this.Items.Add(item);
            }
        }

        private void DeselectItems()
        {
            foreach (ListViewItem item in this.Items)
            {
                item.Selected = false;
            }
        }

        private AppStore store;
    }
}

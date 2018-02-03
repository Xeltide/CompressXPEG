using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;

namespace CompressXPEG
{
    class AppStore : INotifyPropertyChanged
    {

        public AppStore()
        {
            images = new List<Bitmap>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string imagePath;
        public string ImagePath {
            get {
                return imagePath;
            }
            set {
                if (value != imagePath)
                {
                    imagePath = value;
                    OnPropertyChanged("ImagePath");
                }
            }
        }

        private List<Bitmap> images;
        public List<Bitmap> Images {
            get {
                return images;
            }
            set {
                if (value != images)
                {
                    images = value;
                    OnPropertyChanged("Images");
                }
            }
        }

        public void AddImage(Bitmap b)
        {
            images.Add(b);
            OnPropertyChanged("Images");
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

    }
}

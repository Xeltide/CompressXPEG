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
            images = new List<ImageBundle>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<ImageBundle> images;
        public List<ImageBundle> Images {
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

        private ImageBundle currentImage;
        public ImageBundle CurrentImage {
            get {
                return currentImage;
            }
            set {
                if (value != currentImage)
                {
                    currentImage = value;
                    OnPropertyChanged("CurrentImage");
                }
            }
        }

        public void AddImage(ImageBundle b)
        {
            images.Add(b);
            CurrentImage = b;
            OnPropertyChanged("ImageAdded");
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

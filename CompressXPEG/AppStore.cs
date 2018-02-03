using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CompressXPEG
{
    class AppStore : INotifyPropertyChanged
    {

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

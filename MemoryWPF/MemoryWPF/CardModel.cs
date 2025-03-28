using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MemoryWPF
{
    public class CardModel : INotifyPropertyChanged
    {
        private bool _isFlipped;
        private bool _isMatched;
        private string _imagePath;
        public string ImagePath
        {
            get
            {
                if (!IsFlipped)
                {
                    string imageFolder = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "Images");
                    return Path.Combine(imageFolder, "back.jpg");
                }
                return _imagePath;
            }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                if (_isFlipped != value)
                {
                    _isFlipped = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ImagePath)); 
                }
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

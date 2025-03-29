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
    public class CardModel : BaseClass
    {
        private bool _isFlipped;
        private bool _isMatched;
        private string _realImage;
        private string _imagePath;
        public CardModel(bool isFlipped, bool isMatched, string imagePath)
        {
            string imageFolder = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Images");
            _imagePath = Path.Combine(imageFolder, "green_question.png");
            _isFlipped = isFlipped;
            _isMatched = isMatched;
            _realImage = imagePath;
        }
        public string ImagePath
        {
            get
            {
                if (!IsFlipped)
                {
                    return _imagePath;
                }
                return _realImage;
            }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }
        public string RealImage
        {
            get { return _realImage; }
            set
            {
                _realImage = value;
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
    }
}

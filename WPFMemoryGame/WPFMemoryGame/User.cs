using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFMemoryGame
{
    public class User: INotifyPropertyChanged
    {
        public User(string name, int imageIndex)
        {
            _name= name;
            ImageIndex = imageIndex;
        }
        public string Name { 
            get { return _name; } 
            set { 
                _name = value;
                OnPropertyChanged();
            }
        }
        public int ImageIndex { get; set; }

        private String _name;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

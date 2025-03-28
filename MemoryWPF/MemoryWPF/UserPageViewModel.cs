using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MemoryWPF
{
    public class UserPageViewModel:INotifyPropertyChanged
    {
        private UserModel _user;
        public UserPageViewModel(UserModel user) {
            _user = user;
        }
        public UserModel User
        {
            get { return _user; }
            set { _user = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

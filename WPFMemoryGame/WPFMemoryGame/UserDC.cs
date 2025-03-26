using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMemoryGame
{
    public class UserDC
    {
        public User CurrentUser { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public UserDC()
        {
        }
    }
}

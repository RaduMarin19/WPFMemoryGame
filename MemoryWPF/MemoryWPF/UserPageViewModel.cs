using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MemoryWPF
{
    public class UserPageViewModel:BaseClass
    {
        private int _rows;
        private int _columns;
        private UserModel _user;
        private GameView _gameView;
        public ICommand NewGame { get; }
        public UserPageViewModel(UserModel user) {
            _user = user;
            NewGame = new RelayCommand(OnNewGameClick,CanStartGame);
            _gameView = new GameView();
        }
        public int Rows { 
            get
            { 
                return _rows;
            } 
            set
            { 
                _rows = value; 
                OnPropertyChanged(); 
                _gameView.Data.Rows = _rows;
            } 
        }
        public int Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                OnPropertyChanged();
                _gameView.Data.Columns = _columns;
            }
        }
        public UserModel User
        {
            get {
                return _user; 
            }
            set { 
                _user = value;
                OnPropertyChanged();
            }
        }
        private bool CanStartGame(object obj)
        {
            return (_columns * _rows)%2 == 0;
        }
        private void OnNewGameClick(object obj)
        {
            _gameView.Show();
        }

    }
}

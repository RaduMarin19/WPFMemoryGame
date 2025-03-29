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
        public ICommand Statistics { get; }
        public ICommand Exit {  get; }
        public event Action ExitAction;
        public UserPageViewModel(UserModel user) {
            _user = user;
            NewGame = new RelayCommand(OnNewGameClick,CanStartGame);
            Statistics = new RelayCommand(OnStatisticsClick);
            Exit = new RelayCommand(OnExitPressed,CanPressExit);
            _rows = 4;
            _columns = 4;
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
                ((RelayCommand)NewGame).RaiseCanExecuteChanged();
            } 
        }
        public int Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                OnPropertyChanged();
                ((RelayCommand)NewGame).RaiseCanExecuteChanged();
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
        public void OnExitPressed(object obj)
        {
            ExitAction?.Invoke();
        }
        public bool CanPressExit(object obj)
        {
            return true;
        }
        private void OnStatisticsClick(object obj) 
        {
            (new StatisticsView()).Show();
        }
        private bool CanStartGame(object obj)
        {
            return _columns < 7 && _rows < 7 && (_columns * _rows) % 2 == 0;
        }
        private void OnNewGameClick(object obj)
        {
            _gameView=new GameView(Rows, Columns,_user);
            _gameView.Show();
        }

    }
}

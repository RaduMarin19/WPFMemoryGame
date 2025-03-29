using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemoryWPF
{
    /// <summary>
    /// Interaction logic for UserPageView.xaml
    /// </summary>
    public partial class UserPageView : UserControl
    {
        private GameView _gameView;
        public UserPageView(UserModel user)
        {
            UserPageViewModel userPage = new UserPageViewModel(user);
            userPage.GameStarted += OnGameStarted;
            userPage.SaveGameAction += OnSaveGame;
            userPage.LoadGameAction += OnLoadGame;
            this.DataContext = userPage;
            InitializeComponent();
        }
        private void OnLoadGame(UserModel user)
        {
            _gameView = new GameView(user);
            ((GameViewModel)_gameView.DataContext).LoadSave();
            _gameView.Show();
        }
        private void OnSaveGame()
        {
            if (_gameView == null)
                return;
            ((GameViewModel)_gameView.DataContext).Save();
        }
        private void OnGameStarted(int rows, int columns,int time, UserModel user) 
        {
            _gameView = new GameView(rows, columns,time, user);
            _gameView.Show();
        }
        
    }
}

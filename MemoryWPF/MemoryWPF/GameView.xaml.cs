using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : Window
    {
        public GameView(int rows, int columns,UserModel user)
        {
            InitializeComponent();
            this.Data = new GameViewModel(rows,columns,user);
            Data.GameWon += OnGameWon;
            this.DataContext = Data;
        }
        public GameViewModel Data { get; set; }
        private void OnGameWon()
        {
            MessageBox.Show("You WON!");
            this.Close();
        }
    }
}

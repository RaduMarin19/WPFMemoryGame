using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFMemoryGame
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl, INotifyPropertyChanged
    {
        public UserDC UserDataContext { get; set; }
        private List<String> _imagePaths;
        private int _currentImageIndex;
        public ICommand LeftButtonClick { get; }
        public ICommand RightButtonClick { get; }
        public ICommand AddUser { get; }
        public ICommand RemoveUser { get; }
        public ICommand Play { get; }
        public ICommand Cancel { get; }
        public LoginView()
        {
            InitializeComponent();
            LoadImages();
            DataContext = this;
            LeftButtonClick = new RelayCommand(OnLeftClick);
            RightButtonClick = new RelayCommand(OnRightClick);
            AddUser = new RelayCommand(OnAddUserClick);

            UserDataContext = new UserDC();
        }
        public ImageSource CurrentImage
        {
            get
            {
                if (_imagePaths.Count == 0) return null;

                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(_imagePaths[_currentImageIndex], UriKind.Absolute);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

        private void LoadImages()
        {
            string imageFolder = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Images\\breakingbad");
            Console.Write(imageFolder);

            if (Directory.Exists(imageFolder))
            {
                _imagePaths = Directory.GetFiles(imageFolder, "*.png").ToList();
                _currentImageIndex = 0;
                OnPropertyChanged(nameof(CurrentImage));
            }
            else
            {
                _imagePaths = new List<string>();
            }
        }
        private void OnAddUserClick(object obj)
        {

        }
        private void OnLeftClick(object obj)
        {
            _currentImageIndex--;
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = _imagePaths.Count - 1;
            }
            OnPropertyChanged(nameof(CurrentImage));
        }
        private void OnRightClick(object obj)
        {
            _currentImageIndex++;
            if(_currentImageIndex > _imagePaths.Count - 1)
            {
                _currentImageIndex = 0;
            }
            OnPropertyChanged(nameof(CurrentImage));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

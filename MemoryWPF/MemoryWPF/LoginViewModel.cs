using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MemoryWPF
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private object _usersOrAddUser;
        private List<String> _imagePaths;
        private int _currentImageIndex;
        private string _username;
        public ICommand LeftButtonClick { get; }
        public ICommand RightButtonClick { get; }
        public ICommand AddUser { get; }
        public ICommand RemoveUser { get; }
        public ICommand Play { get; }
        public ICommand Cancel { get; }
        public ICommand Ok { get; }
        public LoginViewModel()
        {
            LoadImages();
            LeftButtonClick = new RelayCommand(OnLeftClick);
            RightButtonClick = new RelayCommand(OnRightClick);
            AddUser = new RelayCommand(OnAddUserClick);
            Ok = new RelayCommand(OnOkClick,CanClickOk);
            Cancel = new RelayCommand(OnCancelClick);
            RemoveUser = new RelayCommand(OnRemoveUserClick,CanClickRemoveUser);

            Username = "";

            Users = new ObservableCollection<UserModel>();
            UsersOrAddUser = new UserListView();
        }
        public UserModel CurrentUser { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public object UsersOrAddUser
        {
            get { return _usersOrAddUser; }
            set
            {
                _usersOrAddUser = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged();
                ((RelayCommand)Ok).RaiseCanExecuteChanged();
            }
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
        private void OnRemoveUserClick(object obj)
        {
            Users.Remove(CurrentUser);
        }
        private bool CanClickRemoveUser(object obj)
        {
            return CurrentUser != null;
        }
        private void OnCancelClick(object obj)
        {
            UsersOrAddUser = new UserListView();
            Username = "";
        }
        private bool CanClickOk(object obj)
        {
            return !string.IsNullOrWhiteSpace(_username);
        }
        private void OnOkClick(object obj)
        {
            UserModel temp = new UserModel(_username, _currentImageIndex);
            Users.Add(temp);
            UsersOrAddUser = new UserListView();
            Username = "";
        }
        private void OnAddUserClick(object obj)
        {
            UsersOrAddUser = new AddUser();
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
            if (_currentImageIndex > _imagePaths.Count - 1)
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MemoryWPF
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private List<string> _imagePaths;
        private CardModel _firstFlippedCard;
        public ObservableCollection<CardModel> Cards { get; set; }
        public ICommand CardClickCommand { get; }
        public GameViewModel()
        {
            Cards = new ObservableCollection<CardModel>();
            LoadImages();
            CardClickCommand = new RelayCommand(OnCardClicked);
        }

        private void LoadImages()
        {
            string imageFolder = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Images\\game_breaking_bad");
            if (Directory.Exists(imageFolder))
            {
                _imagePaths = Directory.GetFiles(imageFolder, "*.jpeg").ToList();
            }
            else
            {
                _imagePaths = new List<string>();
            }
            // Duplicate the list to create pairs
            _imagePaths.AddRange(_imagePaths);
            ShuffleImages();

            foreach (var path in _imagePaths)
            {
                Cards.Add(new CardModel
                {
                    ImagePath = path,
                    IsFlipped = false,
                    IsMatched = false
                });
            }
        }
        
        private void OnCardClicked(object parameter)
        {
            CardModel clickedCard = (CardModel)parameter;
            clickedCard.IsFlipped = true;
        }

        private void ShuffleImages()
        {
            var rnd = new Random();
            _imagePaths = _imagePaths.OrderBy(x => rnd.Next()).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

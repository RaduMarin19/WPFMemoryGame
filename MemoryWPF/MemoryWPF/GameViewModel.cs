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
    public class GameViewModel : BaseClass
    {
        private List<string> _imagePaths;
        private CardModel _firstFlippedCard;
        private bool _isBusy = false;
        private int _rows;
        private int _columns;
        public ObservableCollection<CardModel> Cards { get; set; }
        public ICommand CardClickCommand { get; }
        public GameViewModel()
        {
            Cards = new ObservableCollection<CardModel>();
            Rows = 4;
            Columns = 4;
            CardClickCommand = new RelayCommand(OnCardClicked);
            LoadImages();
        }
        public int Rows
        {
            get { return _rows;}
            set
            {
                _rows = value;
                OnPropertyChanged();
            }
        }
        public int Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                OnPropertyChanged();
            }
        }
        private void LoadImages()
        {
            string imageFolder = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Images\\game_breaking_bad");
            if (Directory.Exists(imageFolder))
            {
                var allImages = Directory.GetFiles(imageFolder, "*.jpeg").ToList();
                int numberOfPairs = (Rows * Columns) / 2;
                _imagePaths = allImages.Take(numberOfPairs).ToList();
            }
            else
            {
                _imagePaths = new List<string>();
            }

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
        private async void OnCardClicked(object parameter)
        {
            if (_isBusy) 
                return;

            CardModel clickedCard = (CardModel)parameter;
            if (clickedCard.IsFlipped || clickedCard.IsMatched) //sanity check
                return;

            clickedCard.IsFlipped = true;

            if (_firstFlippedCard == null)
            {
                _firstFlippedCard = clickedCard;
            }
            else
            {
                _isBusy = true; 

                if (clickedCard.ImagePath == _firstFlippedCard.ImagePath)
                {
                    clickedCard.IsMatched = true;
                    _firstFlippedCard.IsMatched = true;
                    _firstFlippedCard = null;
                    _isBusy = false;
                }
                else
                {
                    await Task.Delay(1000);
                    clickedCard.IsFlipped = false;
                    _firstFlippedCard.IsFlipped = false;
                    _firstFlippedCard = null;
                    _isBusy = false;
                    if (CheckWin())
                    {

                    }
                }
            }
        }
        private bool CheckWin()
        {
            foreach(var card in Cards)
            {
                if(card.IsMatched==false)
                    return false;
            }
            return true;
        }

        private void ShuffleImages()
        {
            var rnd = new Random();
            _imagePaths = _imagePaths.OrderBy(x => rnd.Next()).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;

namespace MemoryWPF
{
    public class SaveGameData
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<CardSaveModel> Cards { get; set; }
        public int RemainingTime { get; set; }
    }

    public class CardSaveModel
    {
        public string RealImage { get; set; }
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }
    }
    public class GameViewModel : BaseClass
    {
        private List<string> _imagePaths;
        private CardModel _firstFlippedCard;
        private UserModel _user;
        private bool _isBusy = false;
        private int _rows;
        private int _columns; 
        private DispatcherTimer _gameTimer;
        private int _remainingTime;
        private int _startingTime;
        public event Action TimeExpired;
        public ObservableCollection<CardModel> Cards { get; set; }
        public ICommand CardClickCommand { get; }
        public event Action GameEnded;
        public GameViewModel(UserModel user)
        {
            Cards = new ObservableCollection<CardModel>();
            CardClickCommand = new RelayCommand(OnCardClicked);
            _user = user;
            StartTimer();
        }
        public GameViewModel(int rows, int columns, int time, UserModel user)
        {
            Cards = new ObservableCollection<CardModel>();
            Rows = rows;
            Columns = columns;
            _startingTime = time;
            _user = user;
            CardClickCommand = new RelayCommand(OnCardClicked);
            LoadImages();
            StartTimer();
        }
        public string TimerDisplay => TimeSpan.FromSeconds(_remainingTime).ToString(@"mm\:ss");

        public int StartingTime
        {
            get { return _startingTime; }
            set
            {
                _startingTime = value;
                OnPropertyChanged();
            }
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
        private void StartTimer()
        {
            _remainingTime = StartingTime;
            OnPropertyChanged(nameof(TimerDisplay));

            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += OnTimerTick;
            _gameTimer.Start();
        }

        private void StopTimer()
        {
            if (_gameTimer != null)
            {
                _gameTimer.Stop();
                _gameTimer.Tick -= OnTimerTick;
                _gameTimer = null;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _remainingTime--;
            OnPropertyChanged(nameof(TimerDisplay));

            if (_remainingTime <= 0)
            {
                StopTimer();
                MessageBox.Show("Time is up! You lost!");
                GameEnded?.Invoke();
            }
        }
        public void LoadSave()
        {
            string fileName = _user.Name + "_Game.json";
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                var saveData = JsonSerializer.Deserialize<SaveGameData>(json);

                if (saveData != null)
                {
                    Rows = saveData.Rows;
                    Columns = saveData.Columns;
                    _remainingTime = saveData.RemainingTime;
                    Cards.Clear();

                    foreach (var cardData in saveData.Cards)
                    {
                        Cards.Add(new CardModel(
                            isFlipped: cardData.IsFlipped,
                            isMatched: cardData.IsMatched,
                            imagePath: cardData.RealImage));
                    }
                }
            }
        }
        public void Save()
        {
            var saveData = new SaveGameData
            {
                Rows = this.Rows,
                Columns = this.Columns,
                RemainingTime = this._remainingTime,
                Cards = Cards.Select(c => new CardSaveModel
                {
                    RealImage = c.RealImage,
                    IsFlipped = c.IsFlipped,
                    IsMatched = c.IsMatched
                }).ToList()
            };

            string fileName = _user.Name + "_Game.json";
            var json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
            _gameTimer.Stop();
            GameEnded?.Invoke();
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
                Cards.Add(new CardModel(false, false, path));
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

                if (clickedCard.RealImage == _firstFlippedCard.RealImage)
                {
                    clickedCard.IsMatched = true;
                    _firstFlippedCard.IsMatched = true;
                    _firstFlippedCard = null;
                    _isBusy = false;
                    if (CheckWin())
                    {
                        _user.GamesWon++;
                        _user.GamesPlayed++;
                        _user.Save();
                        GameEnded?.Invoke();
                    }
                }
                else
                {
                    await Task.Delay(1000);
                    clickedCard.IsFlipped = false;
                    _firstFlippedCard.IsFlipped = false;
                    _firstFlippedCard = null;
                    _isBusy = false;
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

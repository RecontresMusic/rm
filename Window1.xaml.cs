using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;


namespace rm
{

    #region ViewModel
    public class ViewModel : INotifyPropertyChanged
    {
        #region Init
        private bool _isPlaying;

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged();
                }
            }
        }
        private Dictionary<string, Track> _tracksDictionary;
        private const string TracksFilePath = "C:/Users/User/source/repos/rm/track.json"; // Путь к файлу для сохранения треков

        public Dictionary<string, Track> TracksDictionary
        {
            get => _tracksDictionary;
            set
            {
                _tracksDictionary = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region WorkWithTracks
        public ObservableCollection<Track> Items { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ViewModel()
        {
            _tracksDictionary = new Dictionary<string, Track>();
            Items = new ObservableCollection<Track>();
            LoadTracks();
            OpenFileCommand = new RelayCommand(OpenFileCommandExecute);
        }

        private void LoadTracks()
        {
            if (File.Exists(TracksFilePath))
            {
                try
                {
                    string json = File.ReadAllText(TracksFilePath);
                    var tracksDictionary = JsonSerializer.Deserialize<Dictionary<string, Track>>(json);
                    if (tracksDictionary != null)
                    {
                        Items.Clear();
                        foreach (var track in tracksDictionary.Values)
                        {
                            Items.Add(track);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading tracks: " + ex.Message);
                    // Handle exceptions as needed
                }
            }
        }

        private void OpenFileCommandExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav|All files (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                if (path != null)
                {
                    AddTrack(path);
                }
            }
        }

        public void AddTrack(string path)
        {
            string key = Path.GetFileNameWithoutExtension(path);

            if (!_tracksDictionary.ContainsKey(key))
            {
                var newTrack = new Track
                {
                    Author = "Unknown",
                    TrackName = key,
                    Path = path
                };

                _tracksDictionary.Add(key, newTrack);
                Items.Add(newTrack); // Add to the ObservableCollection as well
                SaveTracks();
                OnPropertyChanged(nameof(Items)); // Notify the change for Items
            }
            else
            {
                MessageBox.Show("Track already exists.");
            }
        }

        public void SaveTracks()
        {
            try
            {
                string json = JsonSerializer.Serialize(_tracksDictionary);
                File.WriteAllText(TracksFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error saving tracks: " + ex.Message);
                // Здесь можете добавить более подробную обработку ошибок
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        ~ViewModel()
        {
            SaveTracks();
        }
    }
    #endregion

    #region etc
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
        #endregion
    }
    #endregion
    #region WorkWithWPF
    public partial class Window1 : Window
    {      
        public Window1()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
            this.DataContext = new ViewModel();            
        }
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (DataContext is ViewModel vm)
            {
                vm.SaveTracks();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateClip();
            this.SizeChanged += (s, t) => UpdateClip();  // Обновление Clip при изменении размера окна
        }

        private void AnimatedButton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = animatedButton.FindResource("ShrinkAndGrowAnimation") as Storyboard;
            if (storyboard != null)
            {
                storyboard.Begin(animatedButton);
            }
        }

        private void AnimatedButton_Next(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = AnimateNextButtton.FindResource("GoRightNext") as Storyboard;
            if (storyboard != null)
            {
                storyboard.Begin(AnimateNextButtton);
            }
        }

        private void UpdateClip()
        {
            mainGrid.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, mainGrid.ActualWidth, mainGrid.ActualHeight),
                RadiusX = 20,
                RadiusY = 20
            };
        }

        // Работа с верхней панелью

        private void CloseWindow(object sender, RoutedEventArgs e)
        {            this.Close();
        }

        private void GoFullScreenButton(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
                (sender as Button).Content = "▣";
            } else
            {
                this.WindowState = WindowState.Normal;
                (sender as Button).Content = "▢";
            }
        }

        private void GoFullScreenRectangle()
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void RollDownWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void DragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            if (e.ClickCount == 2)
            {
                GoFullScreenRectangle();
            }
        }

        //  Бар меню


        private void PlayClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel vm)
            {
                vm.IsPlaying = !vm.IsPlaying;
            }
        }
    }
}
#endregion
    #region Classes
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Track
{
    public string Author { get; set; }
    public string TrackName { get; set; }
    public string Path { get; set; }
    // Здесь можете добавить свои свойства, например, Image обложки
}
#endregion
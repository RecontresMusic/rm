using System;
using System.Windows;
using System.Windows.Controls;


namespace rm
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>

    using System.ComponentModel;
    using System.Windows.Input;
    using System.Runtime.CompilerServices;
    using System.IO;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Windows.Media;
    using System.Collections.ObjectModel;

    public class ViewModel : INotifyPropertyChanged
    {
        private Visibility _elementsVisibility = Visibility.Visible;
        private Brush _background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF656565"));
        private Brush _foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDEDED"));
        private Brush _borderbrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF656565")); 
        private string _buttonContent = "↩️";
        public ObservableCollection<string> Items { get; set; }

        public ICommand AddItemCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        private string _selectedItem;

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private void AddItem()
        {
            // Добавление нового элемента в коллекцию
            Items.Add("Playlist #" + (Items.Count + 1));
        }

        public Visibility ElementsVisibility
        {
            get => _elementsVisibility;
            set
            {
                if (_elementsVisibility != value)
                {
                    _elementsVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public Brush Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged();
                }
            }
        }

        public Brush Foreground
        {
            get => _foreground;
            set
            {
                if (_foreground != value)
                {
                    _foreground = value;
                    OnPropertyChanged();
                }
            }
        }

        public Brush BorderBrush
        {
            get => _borderbrush;
            set
            {
                if (_borderbrush != value)
                {
                    _borderbrush = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ButtonContent
        {
            get => _buttonContent;
            set
            {
                if (_buttonContent != value)
                {
                    _buttonContent = value;
                    OnPropertyChanged();
                }
            }
        }



        public ICommand ToggleVisibilityCommand { get; private set; }

        private void ToggleVisibility()
        {
            ElementsVisibility = ElementsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; 

            var currentColor = ((SolidColorBrush)_background).Color.ToString();
            var newColorHex = currentColor == "#FF656565" ? "#FFEDEDED" : "#FF656565";
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(newColorHex));

            var currentColor2 = ((SolidColorBrush)_foreground).Color.ToString();
            var newColorHex2 = currentColor2 == "#FFEDEDED" ? "#FF656565" : "#FFEDEDED";
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(newColorHex2));

            var currentColor3 = ((SolidColorBrush)_borderbrush).Color.ToString();
            var newColorHex3 = currentColor3 == "#FF656565" ? "#FFEDEDED" : "#FF656565";
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(newColorHex3));

            ButtonContent = ButtonContent == "↩️" ? "↪️" : "↩️";
        }

        private bool _isPlaying;
        private Thickness _iconMargin = new Thickness(20);
        private double _iconWidth = 20;
        private double _iconHeight = 30;
        private string _playPauseIcon;


        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IconData)); 
                    IconMargin = IsPlaying ? new Thickness(18, 0, 0, 0) : new Thickness(20, 0, 0, 0); // Изменение отступов
                    OnPropertyChanged(nameof(IconMargin));
                    IconWidth = IsPlaying ? 20 : 20;
                    IconHeight = IsPlaying ? 20 : 25;
                    OnPropertyChanged(nameof(IconMargin));
                    OnPropertyChanged(nameof(IconWidth));
                    OnPropertyChanged(nameof(IconHeight));
                }
            }
        }

        public string IconData => IsPlaying ? "m 10 13 L 10 4 H 3 L 3 13 Z V 4 L 14 4 H 14 H 14 V 13 Z" : "M0,0 L0,40 30,20 Z";
        public Thickness IconMargin
        {
            get => _iconMargin;
            set
            {
                if (_iconMargin != value)
                {
                    _iconMargin = value;
                    OnPropertyChanged(nameof(IconMargin));
                }
            }
        }

        public double IconWidth
        {
            get => _iconWidth;
            set
            {
                if (_iconWidth != value)
                {
                    _iconWidth = value;
                    OnPropertyChanged(nameof(IconWidth));
                }
            }
        }

        public double IconHeight
        {
            get => _iconHeight;
            set
            {
                if (_iconHeight != value)
                {
                    _iconHeight = value;
                    OnPropertyChanged(nameof(IconHeight));
                }
            }
        }

        public ICommand TogglePlayCommand { get; }
        //System.Windows.Data Error: 40 : BindingExpression path error: 'RemoveItemCommand' property not found on 'object' ''String' (HashCode=-830691222)'. BindingExpression:Path=RemoveItemCommand; DataItem='String' (HashCode=-830691222); target element is 'MenuItem' (Name=''); target property is 'Command' (type 'ICommand')
        public ViewModel()
        {
            TogglePlayCommand = new RelayCommand(ExecuteTogglePlayCommand);
            _iconMargin = new Thickness(20, 0, 0, 0);
            _iconWidth = 20;
            _iconHeight = 25;
            _playPauseIcon = "M 0 0 L 15 0 L 15 30 L 0 30 Z M 15 0 L 30 0 L 30 30 L 15 30 Z";
            ToggleVisibilityCommand = new RelayCommand(ToggleVisibility);
            // Инициализация коллекции элементов
            Items = new ObservableCollection<string>{
                "I",
            "i",
            "i",
            "i",
            "i",
                "I",
            "i",
            "i",
            "i",
            "i",
                "I",
            "i",
            "i",
            "i",
            "i",
            };
            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand(RemoveItem, () => SelectedItem != null);
            Console.WriteLine("1");
        }

        private void RemoveItem()
        {
            Console.WriteLine("1");
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
            }
        }

        private void ExecuteTogglePlayCommand()
        {
            IsPlaying = !IsPlaying;
            OnPropertyChanged("TriggerAnimation");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

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
    }

    public partial class Window1 : Window
    {

        //private void HackInit()
        //{
        //    string Host = System.Net.Dns.GetHostName();
        //    string IP = System.Net.Dns.GetHostByName(Host).AddressList[0].ToString();
        //    string PCName = Environment.MachineName;
        //    string ConnString = "server=188.187.190.68;user=ftp;password=qweqwe123;database=recmusic;";
        //    string mbInfo = String.Empty;
        //    ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
        //    scope.Connect();
        //    ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

        //    foreach (PropertyData propData in wmiClass.Properties)
        //    {
        //        if (propData.Name == "SerialNumber")
        //        {
        //            mbInfo = Convert.ToString(propData.Value);
        //            break;
        //        }
        //    }
        //    using (var connection = new MySqlConnection(ConnString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sql = "INSERT INTO recmusic (PCName, IP, HWID) VALUES (@pcName, @ip, @hwid)";
        //            using (var cmd = new MySqlCommand(sql, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@pcName", PCName);
        //                cmd.Parameters.AddWithValue("@ip", IP);
        //                cmd.Parameters.AddWithValue("@hwid", mbInfo);
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error: " + ex);
        //        }
        //    }
        //    //Process.Start("chrome.exe", "https://vk.com/music/album/-2000317188_20317188_18ad97b4c0c6cb394c?act=album");
        //}


        public Window1()
        {
            InitializeComponent();
            //HackInit();
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
        {
            this.Close();
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

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
}
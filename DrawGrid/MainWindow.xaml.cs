using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DrawGrid.Annotations;

namespace DrawGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            InitData();
            SizeChanged += MainWindow_Resize;

            var brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Image/20141008063846.jpg"))
            };
            MyCircle.Background = brush;
        }
        
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            MyGrid.Children.Clear();
            GridTool.Draw(MyGrid);
            DrawPath(MyGrid);
            MyGrid2.Children.Clear();
            GridTool.Paint(MyGrid2);
        }
        
        private readonly Polyline _polyline = new Polyline();
        private readonly PointCollection _collection = new PointCollection();
        private readonly Random _random = new Random();
        private double _lastX = 0;
        public double LastX
        {
            get => _lastX;
            set
            {
                _lastX = value;
                OnPropertyChanged(nameof(LastX));
            }
        }
        private double _lastY = 25 / 5D;

        public double LastY
        {
            get => _lastY;
            set
            {
                _lastY = value;
                OnPropertyChanged(nameof(LastY));
            }
        }

        private void ButtonPath_OnClick(object sender, RoutedEventArgs e)
        {
            _lastY += _random.Next(1, 5);
            _lastX = _random.Next(-2, 2);
            _collection.Add(new Point(_lastX, _lastY));
            MyGrid.Children.Clear();
            GridTool.Draw(MyGrid);
            DrawPath(MyGrid);
            MyGrid2.Children.Clear();
            GridTool.Paint(MyGrid2);
        }

        private void DrawPath(Panel panel)
        {
            _polyline.Points = _collection;
            _polyline.Stroke = new SolidColorBrush(Colors.Black);
            _polyline.StrokeThickness = 1;
            panel.Children.Add(_polyline);

            var tipText = new TextBlock {FontSize = 10};
            var textBinding = new Binding {Source = LastY, StringFormat = "{0}m"};
            tipText.SetBinding(TextBlock.TextProperty, textBinding);
            panel.Children.Add(tipText);
            var rotateTransform = new RotateTransform(180);
            tipText.LayoutTransform = rotateTransform;
            Canvas.SetLeft(tipText, LastX);
            Canvas.SetTop(tipText, LastY);
        }

        private void ButtonDrawCircle_OnClick(object sender, RoutedEventArgs e)
        {
            MyCircle.Children.Clear();
            GridTool.DrawCircle(MyCircle);
        }

        private void InitData()
        {
            _collection.Add(new Point(0,0));
            _collection.Add(new Point(20 / 5D,20 / 5D));
            _collection.Add(new Point(40 / 5D,25 / 5D));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
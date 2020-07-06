using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DrawGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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
        
        private readonly Polyline _line = new Polyline();
        private readonly PointCollection _collection = new PointCollection();
        private readonly Random _random = new Random();
        private void ButtonPath_OnClick(object sender, RoutedEventArgs e)
        {
            _collection.Add(new Point(_random.Next(1, (int)MyGrid.Width) - MyGrid.Width/2,_random.Next(1, (int)MyGrid.Height) - 30));
            MyGrid.Children.Clear();
            GridTool.Draw(MyGrid);
            DrawPath(MyGrid);
            MyGrid2.Children.Clear();
            GridTool.Paint(MyGrid2);
        }

        private void DrawPath(Panel panel)
        {
            _line.Points = _collection;
            _line.Stroke = new SolidColorBrush(Colors.Black);
            _line.StrokeThickness = 1;
            panel.Children.Add(_line);
        }

        private void ButtonDrawCircle_OnClick(object sender, RoutedEventArgs e)
        {
            MyCircle.Children.Clear();
            GridTool.DrawCircle(MyCircle);
        }

        private void InitData()
        {
            _collection.Add(new Point(0,0));
            _collection.Add(new Point(20,20));
            _collection.Add(new Point(40,25));
        }
    }
}
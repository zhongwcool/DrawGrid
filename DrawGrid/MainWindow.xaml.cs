using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        }
        
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            MyGrid.Children.Clear();
            GridTool.Draw(MyGrid);
            DrawPath(MyGrid);
        }
        
        private readonly Polyline _line = new Polyline();
        private readonly PointCollection _collection = new PointCollection();
        private readonly Random _random = new Random();
        private void ButtonPath_OnClick(object sender, RoutedEventArgs e)
        {
            _collection.Add(new Point(_random.Next(1, (int)ActualWidth),_random.Next(1, (int)ActualHeight)));
            MyGrid.Children.Clear();
            GridTool.Draw(MyGrid);
            DrawPath(MyGrid);
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
            MyGrid.Children.Clear();
            GridTool.DrawCircle(MyGrid);
        }

        private void InitData()
        {
            _collection.Add(new Point(20,20));
            _collection.Add(new Point(40,25));
            _collection.Add(new Point(60,40));
            _collection.Add(new Point(80,120));
            _collection.Add(new Point(120,140));
            _collection.Add(new Point(200,180));
        }
    }
}
using System;
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
        private Random rd = new Random();
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _collection.Add(new Point(20,20));
            _collection.Add(new Point(40,25));
            _collection.Add(new Point(60,40));
            _collection.Add(new Point(80,120));
            _collection.Add(new Point(120,140));
            _collection.Add(new Point(200,180));
        }

        private void DrawPath(Panel panel)
        {
            _line.Points = _collection;
            _line.Stroke = new SolidColorBrush(Colors.Black);
            _line.StrokeThickness = 1;
            panel.Children.Add(_line);
        }
    }
}
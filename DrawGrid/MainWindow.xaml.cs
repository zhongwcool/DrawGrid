using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DrawGrid.Converter;
using DrawGrid.Data;
using DrawGrid.Model;
using DrawGrid.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace DrawGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Polyline _polyline = new Polyline();

        public MainWindow()
        {
            InitializeComponent();
            SizeChanged += MainWindow_Resize;

            var brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Image/20141008063846.jpg"))
            };
            MyCircle.Background = brush;

            Messenger.Default.Register<Message>(this, MessageToken.MainPoster, OnMessageHandle);
        }

        private void OnMessageHandle(Message msg)
        {
            switch (msg.Key)
            {
                case Message.Main.DrawCircle:
                {
                    OnDrawCircle(msg.Msg);
                }
                    break;
                case Message.Main.GeneratePoint:
                {
                    OnDrawPath(msg.Msg);
                }
                    break;
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            MyGrid.Children.Clear();
            GridTool.Draw(MyGrid);
            DrawPath(MyGrid);
            MyCircle.Children.Clear();
            GridTool.Paint(MyCircle);
        }

        private void DrawPath(Panel panel)
        {
            _polyline.Points = ViewModelLocator.Instance.Main.Collection;
            _polyline.Stroke = new SolidColorBrush(Colors.Black);
            _polyline.StrokeThickness = 1;
            panel.Children.Add(_polyline);

            var tipText = new TextBlock {FontSize = 10};
            var textBinding = new Binding
            {
                Source = ViewModelLocator.Instance.Main.LastY, StringFormat = "{0}m",
                Converter = new Analog2RealConverter()
            };
            tipText.SetBinding(TextBlock.TextProperty, textBinding);
            panel.Children.Add(tipText);
            var rotateTransform = new RotateTransform(180);
            tipText.LayoutTransform = rotateTransform;
            Canvas.SetLeft(tipText, ViewModelLocator.Instance.Main.LastX);
            Canvas.SetTop(tipText, ViewModelLocator.Instance.Main.LastY);
        }

        private void OnDrawCircle(string msg, object extra = null)
        {
            MyCircle.Children.Clear();
            GridTool.DrawCircle(MyCircle);
        }

        private void OnDrawPath(string msg, object extra = null)
        {
            MyGrid.Children.Clear();
            GridTool.Draw(MyGrid);
            DrawPath(MyGrid);
            MyCircle.Children.Clear();
            GridTool.Paint(MyCircle);
        }

        private void ButtonFullScreen_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
        }

        private void MainWindows_Keydown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                {
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.ResizeMode = ResizeMode.CanResize;
                    this.Topmost = false;
                }
                    break;
            }
        }
    }
}
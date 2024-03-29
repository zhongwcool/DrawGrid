﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using DrawGrid.Converter;
using DrawGrid.Keyboard;
using DrawGrid.Model;
using DrawGrid.Tool;
using DrawGrid.ViewModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using WpfAnimatedGif;

namespace DrawGrid.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Polyline _polyline = new Polyline();
        private readonly KeyboardHook _keyboardHook;
        private readonly MainViewModel _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new MainViewModel();
            DataContext = _context;
            SizeChanged += MainWindow_Resize;

            _keyboardHook = new KeyboardHook();
            _keyboardHook.SetOnKeyDownEvent(Win32_Keydown);
            _keyboardHook.SetHook();

            var brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Image/20141008063846.jpg"))
            };
            MyCircle.Background = brush;

            var gif = new BitmapImage(new Uri("pack://application:,,,/Image/IMG_7189.gif"));
            ImageBehavior.SetAnimatedSource(MyImage, gif);
            ImageBehavior.SetRepeatBehavior(MyImage, RepeatBehavior.Forever);

            WeakReferenceMessenger.Default.Register<Message>(this, OnReceive);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _keyboardHook.UnHook();
        }

        private void OnReceive(object recipient, Message msg)
        {
            switch (msg.Key)
            {
                case Message.DrawCircle:
                {
                    OnDrawCircle(msg.Key);
                }
                    break;
                case Message.GeneratePoint:
                {
                    OnDrawPath(msg.Key);
                }
                    break;
                case Message.InstantAdd:
                {
                    OnInstantAdd(msg.Key);
                }
                    break;
                case Message.InstantRemove:
                {
                    OnInstantRemove(msg.Key);
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
            _polyline.Points = _context.Collection;
            _polyline.Stroke = new SolidColorBrush(Colors.Black);
            _polyline.StrokeThickness = 1;
            panel.Children.Add(_polyline);

            var tipText = new TextBlock { FontSize = 10 };
            var textBinding = new Binding
            {
                Source = _context.LastY, StringFormat = "{0}m",
                Converter = new Analog2RealConverter()
            };
            tipText.SetBinding(TextBlock.TextProperty, textBinding);
            panel.Children.Add(tipText);
            var rotateTransform = new RotateTransform(180);
            tipText.LayoutTransform = rotateTransform;
            Canvas.SetLeft(tipText, _context.LastX);
            Canvas.SetTop(tipText, _context.LastY);
        }

        private void OnDrawCircle(string msg, object extra = null)
        {
            MyCircle.Children.Clear();
            GridTool.DrawCircle(MyCircle);
        }

        private PointText _point;
        private bool _hasAdded;
        private DispatcherTimer _tipsTimer;

        private void OnInstantAdd(string msg, object extra = null)
        {
            if (null == _point)
            {
                _point = new PointText
                {
                    HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
                };
            }

            if (!_hasAdded)
            {
                RootGrid.Children.Add(_point);
                _hasAdded = true;

                if (null == _tipsTimer)
                {
                    _tipsTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 5) };
                    _tipsTimer.Tick += (sender, args) =>
                    {
                        if (null != _point && _hasAdded)
                        {
                            RootGrid.Children.Remove(_point);
                            _hasAdded = false;
                        }

                        _tipsTimer.Stop();
                    };
                }

                _tipsTimer.Start();
            }
            else
            {
                _tipsTimer.Stop();
                _tipsTimer.Start();
            }
        }

        private void OnInstantRemove(string msg, object extra = null)
        {
            if (null != _point && _hasAdded)
            {
                RootGrid.Children.Remove(_point);
                _hasAdded = false;
            }
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
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;
        }

        private void Win32_Keydown(Key key)
        {
            switch (key)
            {
                case Key.Escape:
                {
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    ResizeMode = ResizeMode.CanResize;
                    Topmost = false;
                }
                    break;
            }
        }
    }
}
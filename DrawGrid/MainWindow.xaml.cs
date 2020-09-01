using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
using DrawGrid.Data;
using DrawGrid.Model;
using DrawGrid.View;
using DrawGrid.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using WpfAnimatedGif;

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

            SetHook();

            var brush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Image/20141008063846.jpg"))
            };
            MyCircle.Background = brush;

            var gif = new BitmapImage(new Uri("pack://application:,,,/Image/IMG_7189.gif"));
            ImageBehavior.SetAnimatedSource(MyImage, gif);
            ImageBehavior.SetRepeatBehavior(MyImage, RepeatBehavior.Forever);

            Messenger.Default.Register<Message>(this, MessageToken.MainPoster, OnMessageHandle);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            UnHook();
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
                case Message.Main.InstantAdd:
                {
                    OnInstantAdd(msg.Msg);
                }
                    break;
                case Message.Main.InstantRemove:
                {
                    OnInstantRemove(msg.Msg);
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
                    _tipsTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 5)};
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

        #region 键盘

        /// <summary>
        ///     声明回调函数委托
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        public const int WM_KEYDOWN = 0x100;

        public const int WM_KEYUP = 0x101;

        public const int WM_SYSKEYDOWN = 0x104;

        public const int WM_SYSKEYUP = 0x105;

        public const int WH_KEYBOARD_LL = 13;

        //安装钩子的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //卸下钩子的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //下一个钩挂的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        //声明键盘钩子的封送结构类型   
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode; //表示一个在1到254间的虚似键盘码   
            public int scanCode; //表示硬件扫描码   
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        /// <summary>
        ///     键盘钩子句柄
        /// </summary>
        private int _hKeyboardHook;

        private HookProc _keyboardHookDelegate;

        /// <summary>
        ///     安装键盘钩子
        /// </summary>
        private void SetHook()
        {
            _keyboardHookDelegate = KeyboardHookProc;
            var cModule = Process.GetCurrentProcess().MainModule;
            if (null == cModule) return;
            var moduleHandle = GetModuleHandle(cModule.ModuleName);
            _hKeyboardHook =
                SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardHookDelegate, moduleHandle, 0);
        }

        /// <summary>
        ///     卸载键盘钩子
        /// </summary>
        private void UnHook()
        {
            var retKeyboard = true;

            if (_hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(_hKeyboardHook);
                _hKeyboardHook = 0;
            }

            //如果卸下钩子失败   
            if (!retKeyboard) throw new Exception("卸下钩子失败！");
        }

        /// <summary>
        ///     获取键盘消息
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            // 如果该消息被丢弃（nCode<0
            if (nCode < 0) return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);

            KeyboardHookStruct keyboardHookStruct =
                (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
            Key key = KeyInterop.KeyFromVirtualKey(keyboardHookStruct.vkCode);

            switch (wParam)
            {
                case WM_KEYDOWN:
                case WM_SYSKEYDOWN:
                    //WM_KEYDOWN和WM_SYSKEYDOWN消息，将会引发OnKeyDownEvent事件
                    // 此处触发键盘按下事件
                    Win32_Keydown(key);
                    break;
                case WM_KEYUP:
                case WM_SYSKEYUP:
                    //WM_KEYUP和WM_SYSKEYUP消息，将引发OnKeyUpEvent事件
                    // 此处触发键盘抬起事件
                    //_onKeyUp?.Invoke(key);
                    break;
            }

            return CallNextHookEx(_hKeyboardHook, nCode, wParam, lParam);
        }

        #endregion
    }
}
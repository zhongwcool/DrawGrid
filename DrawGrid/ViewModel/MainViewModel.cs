using System;
using System.Windows;
using System.Windows.Media;
using DrawGrid.Data;
using DrawGrid.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace DrawGrid.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public readonly PointCollection Collection = new PointCollection();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                InitData();
            }
            else
            {
                Collection.Add(new Point(0, 0));
            }
        }

        private void InitData()
        {
            Collection.Add(new Point(0, 0));
            Collection.Add(new Point(20 / 5D, 20 / 5D));
            Collection.Add(new Point(40 / 5D, 25 / 5D));
        }

        private double _lastX = 0;

        public double LastX
        {
            get => _lastX;
            set
            {
                _lastX = value;
                RaisePropertyChanged(nameof(LastX));
            }
        }

        private double _lastY = 25 / 5D;

        public double LastY
        {
            get => _lastY;
            set
            {
                _lastY = value;
                RaisePropertyChanged(nameof(LastY));
            }
        }

        public RelayCommand CommandDrawPath => new Lazy<RelayCommand>(() =>
            new RelayCommand(HandleGeneratePoint)
        ).Value;

        private readonly Random _random = new Random();

        private void HandleGeneratePoint()
        {
            _lastY += _random.Next(1, 5);
            _lastX = _random.Next(-2, 2);
            Collection.Add(new Point(_lastX, _lastY));
            Messenger.Default.Send(new Message(Message.Main.GeneratePoint), MessageToken.MainPoster);
        }

        public RelayCommand CommandDrawCircle => new Lazy<RelayCommand>(() =>
            new RelayCommand(HandleDrawCircle)
        ).Value;

        private void HandleDrawCircle()
        {
            Messenger.Default.Send(new Message(Message.Main.DrawCircle), MessageToken.MainPoster);
        }
    }
}
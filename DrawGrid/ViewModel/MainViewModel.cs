using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using DrawGrid.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace DrawGrid.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public readonly PointCollection Collection = new PointCollection();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                DummyData();
            }
            else
            {
                Collection.Add(new Point(0, 0));
            }
        }

        private void DummyData()
        {
            Collection.Add(new Point(0, 0));
            Collection.Add(new Point(20 / 5D, 20 / 5D));
            Collection.Add(new Point(40 / 5D, 25 / 5D));
        }

        private double _lastX = 0;

        public double LastX
        {
            get => _lastX;
            set => SetProperty(ref _lastX, value);
        }

        private double _lastY = 25 / 5D;

        public double LastY
        {
            get => _lastY;
            set => SetProperty(ref _lastY, value);
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
            WeakReferenceMessenger.Default.Send(new Message(Message.GeneratePoint));
        }

        public RelayCommand CommandDrawCircle => new Lazy<RelayCommand>(() =>
            new RelayCommand(HandleDrawCircle)
        ).Value;

        private void HandleDrawCircle()
        {
            WeakReferenceMessenger.Default.Send(new Message(Message.DrawCircle));
        }

        public RelayCommand CommandInstantAdd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => { WeakReferenceMessenger.Default.Send(new Message(Message.InstantAdd)); })
        ).Value;

        public RelayCommand CommandInstantRemove => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => { WeakReferenceMessenger.Default.Send(new Message(Message.InstantRemove)); })
        ).Value;
    }
}
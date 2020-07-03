using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawGrid
{
    public static class GridTool
    {
        public static void Draw(Canvas canvas)
        {
            var gridBrush = new SolidColorBrush {Color = Colors.Red};

            double scaleX = 30;
            double currentPosY = 0;
            currentPosY += scaleX;
            while (currentPosY < canvas.ActualHeight)
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = currentPosY,
                    X2 = canvas.ActualWidth,
                    Y2 = currentPosY,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(line);
                
                currentPosY += scaleX;
            }
            
            double scaleY = 30;
            double currentPosX = 0;
            currentPosX += scaleY;
            while (currentPosX < canvas.ActualWidth)
            {
                Line line = new Line
                {
                    X1 = currentPosX,
                    Y1 = 0,
                    X2 = currentPosX,
                    Y2 = canvas.ActualHeight,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(line);
                
                currentPosX += scaleY;
            }
        }

        public static void DrawCircle(Canvas canvas)
        { 
            var random = new Random();
            var centerX = random.Next(1, (int)canvas.ActualWidth)/2;
            var centerY = random.Next(1, (int)canvas.ActualHeight)/2;
            double step = 20;
            double radius = random.Next(20, 100);
            double cursor = 0;
            var gridBrush = new SolidColorBrush {Color = Colors.Red};

            // 画一个圆边
            var myEllipse = new Ellipse {Stroke = Brushes.Gray, Width = radius * 2, Height = radius * 2};
            Canvas.SetLeft(myEllipse, centerX - radius);
            Canvas.SetTop(myEllipse, centerY - radius);
            canvas.Children.Add(myEllipse);

            while (cursor <= radius)
            {
                var contour = Math.Sqrt(radius * radius - Math.Pow(cursor, 2));
                Line aa = new Line
                {
                    X1 = centerX - contour,
                    Y1 = centerY + cursor,
                    X2 = centerX + contour,
                    Y2 = centerY + cursor,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                canvas.Children.Add(aa);

                Line bb = new Line
                {
                    X1 = centerX - contour,
                    Y1 = centerY - cursor,
                    X2 = centerX + contour,
                    Y2 = centerY - cursor,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                canvas.Children.Add(bb);

                Line cc = new Line
                {
                    X1 = centerX - cursor,
                    Y1 = centerY - contour,
                    X2 = centerX - cursor,
                    Y2 = centerY + contour,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                canvas.Children.Add(cc);

                Line dd = new Line
                {
                    X1 = centerX + cursor,
                    Y1 = centerY - contour,
                    X2 = centerX + cursor,
                    Y2 = centerY + contour,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                canvas.Children.Add(dd);

                cursor += step;
            }
        }
    }
}
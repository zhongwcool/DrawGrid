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
            var diffX = canvas.ActualWidth / 2;
            var offsetX = canvas.ActualWidth - diffX;
            var diffY = 50;
            var offsetY = canvas.ActualHeight - diffY;
            canvas.RenderTransform = SetAngleXy(180, offsetX, offsetY);

            //绘制Y轴
            var lineAxisY = new Line
            {
                X1 = 0,
                Y1 = 0 - diffY,
                X2 = 0,
                Y2 = canvas.ActualHeight - diffY,
                Stroke = new SolidColorBrush {Color = Colors.Red},
                StrokeThickness = 1
            };
            canvas.Children.Add(lineAxisY);
            //绘制X轴
            var lineAxisX = new Line
            {
                X1 = 0 - diffX,
                Y1 = 0,
                X2 = canvas.ActualWidth - diffX,
                Y2 = 0,
                Stroke = new SolidColorBrush {Color = Colors.Red},
                StrokeThickness = 1
            };
            canvas.Children.Add(lineAxisX);
            //绘制X轴、Y轴的标签
            var labelX = new TextBlock {Text = "x", Foreground = Brushes.White};
            var labelY = new TextBlock {Text = "y", Foreground = Brushes.White};
            var rotateTransform = new RotateTransform(180);
            labelX.LayoutTransform = rotateTransform;
            labelY.LayoutTransform = rotateTransform;
            canvas.Children.Add(labelX);
            canvas.Children.Add(labelY);
            Canvas.SetLeft(labelX, 0 - diffX);
            Canvas.SetTop(labelX, 0);
            Canvas.SetLeft(labelY, 0);
            Canvas.SetTop(labelY, canvas.ActualHeight - diffY - 10);

            DrawGrid(canvas, diffX, diffY);
        }

        public static void Paint(Canvas canvas)
        {
            DrawGrid(canvas, 0, 0);
        }

        /// <summary>
        /// 设置旋转角度和位置
        /// </summary>
        /// <param name="angle">偏转角度</param>
        /// <param name="offsetX">X轴偏移位置</param>
        /// <param name="offsetY">X轴偏移位置</param>
        /// <returns></returns>
        private static TransformGroup SetAngleXy(double angle, double offsetX, double offsetY)
        {
            var tfGroup = new TransformGroup();
            var rt = new RotateTransform {Angle = angle, CenterX = offsetX / 2, CenterY = offsetY / 2};
            tfGroup.Children.Add(rt);
            return tfGroup;
        }

        private static void DrawGrid(Canvas canvas, double diffX, double diffY)
        {
            var gridBrush = new SolidColorBrush {Color = Colors.Red};

            double scaleX = 10;
            //画 y>0 的部分
            double currentPosY = 0;
            currentPosY += scaleX;
            while (currentPosY + diffY < canvas.ActualHeight)
            {
                var lineRow = new Line
                {
                    X1 = 0 - diffX,
                    Y1 = currentPosY,
                    X2 = canvas.ActualWidth - diffX,
                    Y2 = currentPosY,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineRow);

                currentPosY += scaleX;
            }

            //画 y<0 的部分
            currentPosY = 0;
            currentPosY -= scaleX;
            while (currentPosY > 0 - diffY)
            {
                var lineRow = new Line
                {
                    X1 = 0 - diffX,
                    Y1 = currentPosY,
                    X2 = canvas.ActualWidth - diffX,
                    Y2 = currentPosY,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineRow);

                currentPosY -= scaleX;
            }

            double scaleY = 10;
            double currentPosX = 0;
            currentPosX += scaleY;
            while (currentPosX + diffX < canvas.ActualWidth)
            {
                var lineCol = new Line
                {
                    X1 = currentPosX,
                    Y1 = 0 - diffY,
                    X2 = currentPosX,
                    Y2 = canvas.ActualHeight - diffY,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineCol);

                currentPosX += scaleY;
            }

            currentPosX = 0;
            currentPosX -= scaleY;
            while (currentPosX > 0 - diffX)
            {
                var lineCol = new Line
                {
                    X1 = currentPosX,
                    Y1 = 0 - diffY,
                    X2 = currentPosX,
                    Y2 = canvas.ActualHeight - diffY,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineCol);

                currentPosX -= scaleY;
            }
        }

        public static void DrawCircle(Canvas canvas)
        {
            var random = new Random();
            var centerX = random.Next(1, (int) canvas.ActualWidth) / 2;
            var centerY = random.Next(1, (int) canvas.ActualHeight) / 2;
            double step = 10;
            double radius = random.Next(20, 100);
            double cursor = 0;
            var gridBrush = new SolidColorBrush {Color = Colors.Red};

            // 画一个圆边
            var myEllipse = new Ellipse {Stroke = Brushes.Black, Width = radius * 2, Height = radius * 2};
            Canvas.SetLeft(myEllipse, centerX - radius);
            Canvas.SetTop(myEllipse, centerY - radius);
            canvas.Children.Add(myEllipse);

            while (cursor <= radius)
            {
                var contour = Math.Sqrt(radius * radius - Math.Pow(cursor, 2));
                Line lineRowDown = new Line
                {
                    X1 = centerX - contour,
                    Y1 = centerY + cursor,
                    X2 = centerX + contour,
                    Y2 = centerY + cursor,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineRowDown);

                Line lineRowUp = new Line
                {
                    X1 = centerX - contour,
                    Y1 = centerY - cursor,
                    X2 = centerX + contour,
                    Y2 = centerY - cursor,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineRowUp);

                Line lineColLeft = new Line
                {
                    X1 = centerX - cursor,
                    Y1 = centerY - contour,
                    X2 = centerX - cursor,
                    Y2 = centerY + contour,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineColLeft);

                Line lineColRight = new Line
                {
                    X1 = centerX + cursor,
                    Y1 = centerY - contour,
                    X2 = centerX + cursor,
                    Y2 = centerY + contour,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                canvas.Children.Add(lineColRight);

                cursor += step;
            }
        }
    }
}
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawGrid
{
    public class GridTool
    {
        public static void Draw(Panel panel)
        {
            SolidColorBrush gridBrush = new SolidColorBrush {Color = Colors.Red};

            double scaleX = 30;
            double currentPosY = 0;
            currentPosY += scaleX;
            while (currentPosY < panel.ActualHeight)
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = currentPosY,
                    X2 = panel.ActualWidth,
                    Y2 = currentPosY,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                panel.Children.Add(line);
                
                currentPosY += scaleX;
            }
            
            double scaleY = 30;
            double currentPosX = 0;
            currentPosX += scaleY;
            while (currentPosX < panel.ActualWidth)
            {
                Line line = new Line
                {
                    X1 = currentPosX,
                    Y1 = 0,
                    X2 = currentPosX,
                    Y2 = panel.ActualHeight,
                    Stroke = gridBrush,
                    StrokeThickness = 0.1
                };
                panel.Children.Add(line);
                
                currentPosX += scaleY;
            }
        }

        public static void DrawCircle(Panel panel)
        {
            var centerX = panel.ActualWidth/2;
            var centerY = panel.ActualHeight/2;
            double step = 20;
            double radius = 100;
            double cursor = 0;
            var gridBrush = new SolidColorBrush {Color = Colors.Red};

            while (cursor <= radius)
            {
                var contour = System.Math.Sqrt(radius * radius - System.Math.Pow(cursor, 2));
                Line aa = new Line
                {
                    X1 = centerX - contour,
                    Y1 = centerY + cursor,
                    X2 = centerX + contour,
                    Y2 = centerY + cursor,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                panel.Children.Add(aa);

                Line bb = new Line
                {
                    X1 = centerX - contour,
                    Y1 = centerY - cursor,
                    X2 = centerX + contour,
                    Y2 = centerY - cursor,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                panel.Children.Add(bb);

                Line cc = new Line
                {
                    X1 = centerX - cursor,
                    Y1 = centerY - contour,
                    X2 = centerX - cursor,
                    Y2 = centerY + contour,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                panel.Children.Add(cc);

                Line dd = new Line
                {
                    X1 = centerX + cursor,
                    Y1 = centerY - contour,
                    X2 = centerX + cursor,
                    Y2 = centerY + contour,
                    Stroke = gridBrush,
                    StrokeThickness = 1
                };
                panel.Children.Add(dd);

                cursor += step;
            }
        }
    }
}
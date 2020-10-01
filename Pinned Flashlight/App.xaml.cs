using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Pinned_Flashlight
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Color defaultColor;
        private Color brighterColor;

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = sender as Border;
            InitializeEffect(border);
        }

        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = sender as Border;
            double x = e.GetPosition(border).X / border.Width;
            double y = e.GetPosition(border).Y / border.Height;
            AnimateEffect(border, x, y);
        }

        private void Border_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = sender as Border;
            double x = e.GetPosition(border).X / border.Width;
            double y = e.GetPosition(border).Y / border.Height;
            AnimateEffect(border, x, y);
        }

        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = sender as Border;
            FreeseEffect(border);
        }

        private void InitializeEffect(Border border)
        {
            defaultColor = Colors.Goldenrod;
            brighterColor = Colors.LightGoldenrodYellow;

            GradientStopCollection backgroundGradientStopCollection = new GradientStopCollection();
            backgroundGradientStopCollection.Add(new GradientStop(brighterColor, 0));
            backgroundGradientStopCollection.Add(new GradientStop(defaultColor, 2.5));

            RadialGradientBrush backgroundRadialGradientBrush = new RadialGradientBrush(backgroundGradientStopCollection);
            backgroundRadialGradientBrush.GradientOrigin = new Point(0.5, 0.5);
            backgroundRadialGradientBrush.Center = new Point(0.5, 0.5);

            border.Background = backgroundRadialGradientBrush;

            GradientStopCollection borderGradientStopCollection = new GradientStopCollection();
            borderGradientStopCollection.Add(new GradientStop(Colors.White, 0.38));
            borderGradientStopCollection.Add(new GradientStop(defaultColor, 0.62));

            RadialGradientBrush borderRadialGradientBrush = new RadialGradientBrush(borderGradientStopCollection);
            borderRadialGradientBrush.GradientOrigin = new Point(0.5, 0.5);
            borderRadialGradientBrush.Center = new Point(0.5, 0.5);

            border.BorderBrush = borderRadialGradientBrush;

            border.CornerRadius = new CornerRadius(border.Width / 200 * 38);

            DropShadowEffect borderDropShadowEffect = new DropShadowEffect();
            borderDropShadowEffect.Color = Colors.Black;
            borderDropShadowEffect.Direction = 0;
            borderDropShadowEffect.ShadowDepth = 0;
            borderDropShadowEffect.Opacity = 0.62;
            borderDropShadowEffect.BlurRadius = border.Width / 200 * 38;
            border.Effect = borderDropShadowEffect;

            ContentPresenter contentPresenter = border.Child as ContentPresenter;
            contentPresenter.Width = border.Width / 100 * 62;
            contentPresenter.Height = border.Height / 100 * 62;

            DropShadowEffect contentDropShadowEffect = new DropShadowEffect();
            contentDropShadowEffect.Color = Colors.Black;
            contentDropShadowEffect.Direction = 0;
            contentDropShadowEffect.ShadowDepth = 0;
            contentDropShadowEffect.Opacity = 0.62;
            contentDropShadowEffect.BlurRadius = contentPresenter.Width / 200 * 38;
            contentPresenter.Effect = contentDropShadowEffect;
        }

        private void AnimateEffect(Border border, double x, double y)
        {
            int period = 0;
            if (x < 0.5 && y < 0.5)
                period = 1;
            else if (x > 0.5 && y < 0.5)
                period = 2;
            else if (x > 0.5 && y > 0.5)
                period = 3;
            else if (x < 0.5 && y > 0.5)
                period = 4;
            
            double distanceFromCenter = Math.Sqrt(Math.Pow((x - 0.5), 2) + Math.Pow((y - 0.5), 2)) * border.Width;

            GradientStopCollection backgroundGradientStopCollection = new GradientStopCollection();
            backgroundGradientStopCollection.Add(new GradientStop(brighterColor, 0));
            backgroundGradientStopCollection.Add(new GradientStop(defaultColor, 2.5));

            RadialGradientBrush backgroundRadialGradientBrush = new RadialGradientBrush(backgroundGradientStopCollection);
            backgroundRadialGradientBrush.GradientOrigin = new Point(x, y);
            backgroundRadialGradientBrush.Center = new Point(x, y);

            border.Background = backgroundRadialGradientBrush;

            GradientStopCollection borderGradientStopCollection = new GradientStopCollection();
            borderGradientStopCollection.Add(new GradientStop(Colors.White, 0.38));
            borderGradientStopCollection.Add(new GradientStop(defaultColor, 0.62));

            RadialGradientBrush borderRadialGradientBrush = new RadialGradientBrush(borderGradientStopCollection);
            borderRadialGradientBrush.GradientOrigin = new Point(x, y);
            borderRadialGradientBrush.Center = new Point(x, y);

            border.BorderBrush = borderRadialGradientBrush;

            double[] distancesFromCorners = new double[4];
            distancesFromCorners[0] = Math.Sqrt(Math.Pow((x - 0), 2) + Math.Pow((y - 0), 2)) * border.Width;
            distancesFromCorners[1] = Math.Sqrt(Math.Pow((x - 1), 2) + Math.Pow((y - 0), 2)) * border.Width;
            distancesFromCorners[2] = Math.Sqrt(Math.Pow((x - 1), 2) + Math.Pow((y - 1), 2)) * border.Width;
            distancesFromCorners[3] = Math.Sqrt(Math.Pow((x - 0), 2) + Math.Pow((y - 1), 2)) * border.Width;

            double[] cornerRadiuses = new double[4];
            for (int i = 0; i < 4; i++)
            {
                cornerRadiuses[i] = border.Width * Math.Sqrt(2) - distancesFromCorners[i];
                cornerRadiuses[i] = 38 * cornerRadiuses[i] / 100;
                if (cornerRadiuses[i] < border.Width / 200 * 38)
                    cornerRadiuses[i] = border.Width / 200 * 38;
            }

            CornerRadius cornerRadius = new CornerRadius(cornerRadiuses[0], cornerRadiuses[1], cornerRadiuses[2], cornerRadiuses[3]);
            border.CornerRadius = cornerRadius;
            
            Point vector1Point = new Point(0.5, 0);
            Point vector2Point = new Point(x - 0.5, y - 0.5);
            double scalar = vector1Point.X * vector2Point.X + vector1Point.Y * vector2Point.Y;
            double vector1Absolute = Math.Sqrt(Math.Pow(vector1Point.X, 2) + Math.Pow(vector1Point.Y, 2));
            double vector2Absolute = Math.Sqrt(Math.Pow(vector2Point.X, 2) + Math.Pow(vector2Point.Y, 2));
            double angle = Math.Acos(scalar / (vector1Absolute * vector2Absolute)) / Math.PI * 180;
            if (period > 2)
                angle = 360 - angle;

            DropShadowEffect borderDropShadowEffect = new DropShadowEffect();
            borderDropShadowEffect.Color = Colors.Black;
            borderDropShadowEffect.Direction = angle;
            borderDropShadowEffect.ShadowDepth = distanceFromCenter / 200 * 38;
            borderDropShadowEffect.Opacity = 0.62;
            borderDropShadowEffect.BlurRadius = border.Width / 200 * 38;
            border.Effect = borderDropShadowEffect;
            
            DropShadowEffect contentDropShadowEffect = new DropShadowEffect();
            contentDropShadowEffect.Color = Colors.Black;
            contentDropShadowEffect.Direction = angle;
            contentDropShadowEffect.ShadowDepth = distanceFromCenter / 100 * 62 / 200 * 38;
            contentDropShadowEffect.Opacity = 0.62;
            contentDropShadowEffect.BlurRadius = border.Width / 200 * 62 / 200 * 38;
            (border.Child as ContentPresenter).Effect = contentDropShadowEffect;
        }

        private void FreeseEffect(Border border)
        {
            GradientStopCollection backgroundGradientStopCollection = new GradientStopCollection();
            backgroundGradientStopCollection.Add(new GradientStop(brighterColor, 0));
            backgroundGradientStopCollection.Add(new GradientStop(defaultColor, 2.5));

            RadialGradientBrush backgroundRadialGradientBrush = new RadialGradientBrush(backgroundGradientStopCollection);
            backgroundRadialGradientBrush.GradientOrigin = new Point(0.5, 0.5);
            backgroundRadialGradientBrush.Center = new Point(0.5, 0.5);

            border.Background = backgroundRadialGradientBrush;
            
            GradientStopCollection borderGradientStopCollection = new GradientStopCollection();
            borderGradientStopCollection.Add(new GradientStop(Colors.White, 0.38));
            borderGradientStopCollection.Add(new GradientStop(defaultColor, 0.62));

            RadialGradientBrush borderRadialGradientBrush = new RadialGradientBrush(borderGradientStopCollection);
            borderRadialGradientBrush.GradientOrigin = new Point(0.5, 0.5);
            borderRadialGradientBrush.Center = new Point(0.5, 0.5);

            border.BorderBrush = borderRadialGradientBrush;

            border.CornerRadius = new CornerRadius(border.Width / 200 * 38);

            DropShadowEffect borderDropShadowEffect = new DropShadowEffect();
            borderDropShadowEffect.Color = Colors.Black;
            borderDropShadowEffect.Direction = 0;
            borderDropShadowEffect.ShadowDepth = 0;
            borderDropShadowEffect.Opacity = 0.62;
            borderDropShadowEffect.BlurRadius = border.Width / 200 * 38;
            border.Effect = borderDropShadowEffect;

            DropShadowEffect contentDropShadowEffect = new DropShadowEffect();
            contentDropShadowEffect.Color = Colors.Black;
            contentDropShadowEffect.Direction = 0;
            contentDropShadowEffect.ShadowDepth = 0;
            contentDropShadowEffect.Opacity = 0.62;
            contentDropShadowEffect.BlurRadius = border.Width / 200 * 62 / 200 * 38;
            (border.Child as ContentPresenter).Effect = contentDropShadowEffect;
        }
    }
}

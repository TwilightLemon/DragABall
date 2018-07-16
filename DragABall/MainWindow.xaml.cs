using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragABall
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<Con> cData = new List<Con>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i != 500; i++)
            {
                var r = new Random(Con.GetRandomSeed());
                var el = new Ellipse() {Opacity=0.5, Height = 20, Width = 20, Fill = new SolidColorBrush
                (Color.FromRgb(Convert.ToByte(r.Next(0, 255)),
                Convert.ToByte(r.Next(0, 255)),
                Convert.ToByte(r.Next(0, 255))))
                };
                el.MouseEnter += delegate { MessageBox.Show("你被吃了！"); };
                layout.Children.Add(el);
                var cn = new Con(el, layout);
                cn.Loads();
                cData.Add(cn);
            }      
        }
        
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Con dt in cData)
                dt.Moves(e);
        }
    }

    public class Con {
        public Con(Ellipse ex,Grid lay) {
            m = ex;
            layout = lay;
        }
        #region 变量
        private Ellipse m;
        private Grid layout;
        private Vector v1 = new Vector(0, 0);
        private TimeSpan prevTime = TimeSpan.Zero;
        private Point point = new Point(0, 0);
        private Stopwatch stopwatch = new Stopwatch();
        private Point pMouse = new Point(0, 0);
        #endregion
        #region 方法
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);

        }
        public void Loads()
        {
            m.RenderTransform = new TranslateTransform();
            stopwatch.Start();
            CompositionTarget.Rendering += delegate { Render(); };
        }
        public void Render()
        {
            TimeSpan currentTime = stopwatch.Elapsed;
            double t = (currentTime - prevTime).TotalSeconds;
            prevTime = currentTime;
            Vector F = new Vector(point.X, point.Y);
            var r = new Random(GetRandomSeed());
            var eta = r.Next(10, 100);
            double d = r.Next(10, 100);
            double rho = r.Next(1, 5);
            double mx = (Math.PI * d * d * d * rho) / 6;
            double coefficient = d * d * d;
            F = coefficient * F;
            Vector vDiff = ((F - 3 * Math.PI * eta * v1 * d) / mx) * t;
            Vector s = v1 * t + (vDiff * t) / 2;

            TranslateTransform translate = (TranslateTransform)m.RenderTransform;
            translate.X += s.X;
            translate.Y += s.Y;
            point.X -= s.X;
            point.Y -= s.Y;
            Vector v2 = vDiff + v1;
            v1 = v2;
        }
        public void Moves( MouseEventArgs e)
        {
            point = e.GetPosition(m);
            point.Offset(-m.ActualWidth / 2, -m.ActualHeight / 2);
            pMouse = e.GetPosition(layout);
        }
        #endregion
    }
}

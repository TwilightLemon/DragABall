using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
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
        Con cnx;
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tx = new System.Windows.Forms.Timer();
        Ellipse elx;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            t.Interval = 1000;
            tx.Interval = 3000;
            t.Tick += T_Tick;
            tx.Tick += Tx_Tick;
            for (int i = 0; i != 400; i++)
            {
                var r = new Random(Con.GetRandomSeed());
                var el = new Ellipse()
                {
                    Opacity = 0.5,
                    Height = 50,
                    Width = 50,
                    Fill = new SolidColorBrush
                (Color.FromRgb(Convert.ToByte(r.Next(0, 255)),
                Convert.ToByte(r.Next(0, 255)),
                Convert.ToByte(r.Next(0, 255)))),
                    Margin = new Thickness(800, 300, 0, 203)
                };
                el.MouseEnter += delegate { MessageBox.Show("你被吃了！"); };
                layout.Children.Add(el);
                var cn = new Con(el, layout);
                cn.Loads();
                cData.Add(cn);
            }
            for (int i = 0; i != 200; i++)
            {
                var r = new Random(Con.GetRandomSeed());
                var el = new Ellipse()
                {
                    Opacity = 0.5,
                    Height = 50,
                    Width = 50,
                    Fill = new SolidColorBrush(Color.FromArgb(255, 251, 251, 251)),
                    Margin = new Thickness(100, 200, 200, 403)
                };
                el.MouseEnter += delegate { MessageBox.Show("你被隐身怪吃了！"); };
                layout.Children.Add(el);
                var cn = new Con(el, layout);
                cn.Loads();
                cData.Add(cn);
            }
            elx = new Ellipse()
            {
                Opacity = 0.5,
                Height = 50,
                Width = 50,
                Fill = new SolidColorBrush(Color.FromArgb(255, 251, 251, 251)),
                Uid="st"
            };
            elx.MouseEnter += Elx_MouseEnter;
            elx.MouseDown += Elx_MouseDown;
            layout.Children.Add(elx);
             cnx = new Con(elx, layout);
            cnx.Loads();
            cData.Add(cnx);
            t.Start();
            tx.Start();
        }
        int indexs = 0;
        private void Elx_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (indexs==0)
            {
                cData.Remove(cnx);
                tx.Stop();
                var ani = new DoubleAnimation(600, TimeSpan.FromSeconds(0.3));
                elx.BeginAnimation(HeightProperty, ani);
                elx.BeginAnimation(WidthProperty, ani);
            }
            var an = new DoubleAnimation(600-indexs*20, TimeSpan.FromSeconds(0.3));
            elx.BeginAnimation(HeightProperty, an);
            elx.BeginAnimation(WidthProperty, an);
            if (indexs == 12)
            {
                var ani = new DoubleAnimationUsingKeyFrames();
                ani.KeyFrames.Add(new LinearDoubleKeyFrame(600, TimeSpan.FromSeconds(0.3)));
                ani.KeyFrames.Add(new LinearDoubleKeyFrame(0, TimeSpan.FromSeconds(0.6)));
                elx.BeginAnimation(HeightProperty, ani);
                elx.BeginAnimation(WidthProperty, ani);
                ani.Completed += delegate { layout.Children.Remove(elx);if(layout.Children.Count==0) MessageBox.Show("你赢了"); };
            }
            else indexs++;
        }

        private void Elx_MouseEnter(object sender, MouseEventArgs e)
        {
            if(e.LeftButton!=MouseButtonState.Pressed)
                MessageBox.Show("你被变形怪吃了！");
        }

        private void Tx_Tick(object sender, EventArgs e)
        {
            var ys = elx.Height;
            var r = new Random(Con.GetRandomSeed());
            var s = 100 * r.Next(1, 9);
            var ani = new DoubleAnimationUsingKeyFrames();
            ani.KeyFrames.Add(new LinearDoubleKeyFrame(s, TimeSpan.FromSeconds(0.3)));
            ani.KeyFrames.Add(new LinearDoubleKeyFrame(s, TimeSpan.FromSeconds(2.3)));
            ani.KeyFrames.Add(new LinearDoubleKeyFrame(ys, TimeSpan.FromSeconds(2.6)));
            elx.BeginAnimation(HeightProperty, ani);
            elx.BeginAnimation(WidthProperty, ani);
        }

        private void T_Tick(object sender, EventArgs e)
        {
            try
            {
                var dt = cData[0];
                if (dt.point.X < 0 && dt.point.Y < 0)
                    for (int i = 0; i != 50; i++)
                        if (cData.Count > i)
                            if (cData[i].m.Uid != "st"){
                                layout.Children.Remove(cData[i].m);
                                cData.Remove(cData[i]);
                            }

                if (cData.Count < 25) {
                    if (!hasblack)
                    {
                        for (int i = 0; i != 25; i++)
                        {
                            var r = new Random(Con.GetRandomSeed());
                            var s = 100 * r.Next(1, 5);
                            var el = new Ellipse()
                            {
                                Opacity = 0.5,
                                Height = s,
                                Width = s,
                                Fill = new SolidColorBrush(Color.FromArgb(255, 253, 253, 253))
                            };
                            el.MouseEnter += delegate { MessageBox.Show("你被巨型怪吃了！"); };
                            layout.Children.Add(el);
                            var cn = new Con(el, layout);
                            cn.Loads();
                            cData.Add(cn);
                        }
                        for (int i = 0; i != 200; i++)
                        {
                            var r = new Random(Con.GetRandomSeed());
                            var el = new Ellipse()
                            {
                                Opacity = 0.5,
                                Height = 50,
                                Width = 50,
                                Fill = new SolidColorBrush(Color.FromArgb(255, 253, 253, 253))
                            };
                            el.MouseEnter += delegate { MessageBox.Show("你被隐身怪吃了！"); };
                            layout.Children.Add(el);
                            var cn = new Con(el, layout);
                            cn.Loads();
                            cData.Add(cn);
                        }
                        hasblack = true;
                    }
                }
            }
            catch { }
        }
        bool hasblack = false;
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
        public Ellipse m;
        private Grid layout;
        public Vector v1 = new Vector(0, 0);
        private TimeSpan prevTime = TimeSpan.Zero;
        public Point point = new Point(0, 0);
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

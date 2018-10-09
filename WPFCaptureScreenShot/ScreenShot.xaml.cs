using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.Windows.Threading;

namespace WPFCaptureScreenShot
{
    /// <summary>
    /// ScreenShot.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenShot : Window
    {
        private double x;  // 横坐标
        private double y;  // 纵坐标
        private bool isMouseDown = false; // 判断鼠标是否按下
        public ScreenShot()
        {
            InitializeComponent();
            this.Loaded -= Window_Loaded;
        }

        public ScreenShot(string str)
        {
            InitializeComponent();
            this.MouseDown -= Window_MouseDown;
            this.MouseMove -= Window_MouseMove;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true; // 鼠标按下事件发生时将此字段设为true
            x = e.GetPosition(null).X;  // 获取鼠标当前位置的横坐标
            y = e.GetPosition(null).Y;  // 获取鼠标当前位置的纵坐标
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)  // 如果鼠标已经被按下
            {
                // 1. 通过一个矩形来表示目前截图区域
                System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                double dx = e.GetPosition(null).X;
                double dy = e.GetPosition(null).Y;
                double width = Math.Abs(dx - x);
                double height = Math.Abs(dy - y);
                SolidColorBrush brush = new SolidColorBrush(Colors.White);
                rect.Width = width;
                rect.Height = height;
                rect.Fill = brush;
                rect.Stroke = brush;
                rect.StrokeThickness = 1;
                // 通过鼠标按下时与鼠标当前的位置关系来设定矩形与画布左边和顶部的距离
                if (dx < x)
                {
                    Canvas.SetLeft(rect, dx);
                    if (dy < y)
                    {
                        Canvas.SetTop(rect, dy);
                    }
                    else
                    {
                        Canvas.SetTop(rect, y);
                    }
                }
                else
                {
                    Canvas.SetLeft(rect, x);
                    if (dy < y)
                    {
                        Canvas.SetTop(rect, dy);
                    }
                    else
                    {
                        Canvas.SetTop(rect, y);
                    }
                }
                CaptureCanvas.Children.Clear();
                CaptureCanvas.Children.Add(rect);

                if (e.LeftButton == MouseButtonState.Released)
                {
                    //CaptureCanvas.Children.Clear();
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    rect.StrokeThickness =2;

                    // 2. 获取当前截图区域
                    if (e.GetPosition(null).X > x)
                    {
                        if (e.GetPosition(null).Y > y)
                        {
                            //CaptureScreen(x, y, width, height);
                        }
                        else
                        {
                            //CaptureScreen(x, e.GetPosition(null).Y, width, height);
                            y = e.GetPosition(null).Y;
                        }
                    }
                    else
                    {
                        if (e.GetPosition(null).Y > y)
                        {
                            //CaptureScreen(e.GetPosition(null).X, y, width, height);
                            x = e.GetPosition(null).X;
                        }
                        else
                        {
                            //CaptureScreen(e.GetPosition(null).X, e.GetPosition(null).Y, width, height);
                            x = e.GetPosition(null).X;
                            y = e.GetPosition(null).Y;
                        }
                    }
                    try
                    {
                        // 由于选择矩形区域截图时实际截取的图像向右下角偏移了大约7个像素，故作此调整
                        CaptureScreen(x-7, y-7, width, height);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("所选区域应为一个矩形，长或宽不能为零！");
                        return;
                    }
                    finally
                    {
                        isMouseDown = false;
                        x = 0.0;
                        y = 0.0;
                        this.Hide();
                        Application.Current.MainWindow.Show();
                        this.Close();
                    }
                }
            }
        }

        private void CaptureScreen(double x, double y, double width, double height)
        {
            int ix = Convert.ToInt32(x);
            int iy = Convert.ToInt32(y);
            int iw = Convert.ToInt32(width);
            int ih = Convert.ToInt32(height);

            System.Drawing.Bitmap bitmap = new Bitmap(iw, ih);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iw, ih));

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Png Files|*.png";
                if (dialog.ShowDialog() == true)
                {
                    bitmap.Save(dialog.FileName, ImageFormat.Png);
                    
                    MessageBox.Show("图片已保存至"+dialog.FileName);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CaptureScreen(0, 0, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            this.Hide();
            Application.Current.MainWindow.Show();
            this.Close();
        }
    }
}

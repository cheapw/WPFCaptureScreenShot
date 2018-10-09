using System;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;


namespace WPFCaptureScreenShot
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
        private int currentImageIndex = 0;
        private int currentColorIndex = 1;
        private bool isImageIndex_InitialState = true;
        private bool isColorIndex_InitialState = true;
        #region 主窗口移动的UI
        private double x;  // 横坐标
        private double y;  // 纵坐标
        private bool isMouseDown = false; // 判断鼠标是否按下
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) // 鼠标按下的事件处理程序
        {
            this.Cursor = Cursors.SizeAll;
            isMouseDown = true; // 鼠标按下事件发生时将此字段设为true
            x = e.GetPosition(null).X;  // 获取鼠标当前位置的横坐标
            y = e.GetPosition(null).Y;  // 获取鼠标当前位置的纵坐标
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)  // 鼠标移动的事件处理程序
        {
            if (isMouseDown)
            {
                if (e.GetPosition(null).X > x)
                {
                    this.Left += e.GetPosition(null).X - x;
                }
                else
                {
                    this.Left  -= x-e.GetPosition(null).X;
                }
                if (e.GetPosition(null).Y > y)
                {
                    this.Top += e.GetPosition(null).Y - y;
                }
                else
                {
                    this.Top -= y - e.GetPosition(null).Y;
                }
            }
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            this.Cursor = null;
        }


        #endregion

        private void BtnFullScreenShot_Click(object sender, RoutedEventArgs e)
        {
            ScreenShot screenShot = new ScreenShot("");
            this.Hide();
            screenShot.Show();
        }

        private void BtnRectShot_Click_1(object sender, RoutedEventArgs e)
        {
            ScreenShot screenShot = new ScreenShot();
            this.Visibility = Visibility.Collapsed;
            screenShot.Show();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri($"pack://application:,,,/{button.Tag.ToString()}" + ".jpg", UriKind.Absolute);
            bi.EndInit();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = bi;
            imageBrush.Stretch = Stretch.UniformToFill;
            this.Background = imageBrush;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (FindResource("DefaultBackground") != null)
            {
                this.Background = (ImageBrush)FindResource("DefaultBackground");
                isColorIndex_InitialState = true;
                isImageIndex_InitialState = true;
            }
        }

        private void BtnColorChange_Click(object sender, RoutedEventArgs e)
        {
            if ((string)((sender as Button).Tag) == "bottom")
            {
                if (isColorIndex_InitialState)
                {
                    this.Background = (LinearGradientBrush)FindResource("1");
                    isColorIndex_InitialState = false;
                }
                else if (currentColorIndex==10)
                {
                    currentColorIndex = 1;
                    this.Background = (LinearGradientBrush)FindResource(currentColorIndex.ToString());
                }
                else
                {
                    currentColorIndex++;
                    this.Background = (LinearGradientBrush)FindResource(currentColorIndex.ToString());
                }
            }
            else
            {
                if (isColorIndex_InitialState)
                {
                    this.Background = (LinearGradientBrush)FindResource("1");
                    isColorIndex_InitialState = false;
                }
                else if (currentColorIndex==1)
                {
                    currentColorIndex = 10;
                    this.Background = (LinearGradientBrush)FindResource(currentColorIndex.ToString());
                }
                else
                {
                    currentColorIndex--;
                    this.Background = (LinearGradientBrush)FindResource(currentColorIndex.ToString());
                }
            }
        }
        private void BtnImageChange_Click(object sender, RoutedEventArgs e)
        {
            if ((string)((sender as Button).Tag) == "right")
            {
                if (isImageIndex_InitialState)
                {

                    ChangeBackground(0);
                    isImageIndex_InitialState = false;
                }
                else if (currentImageIndex == 22)
                {
                    currentImageIndex = 0;
                    ChangeBackground(0);
                }
                else
                {
                    currentImageIndex++;
                    ChangeBackground(currentImageIndex);
                }
            }
            else
            {
                if (isImageIndex_InitialState)
                {
                    ChangeBackground(0);
                    isImageIndex_InitialState = false;
                }
                else if (currentImageIndex == 0)
                {
                    currentImageIndex = 22;
                    ChangeBackground(currentImageIndex);
                }
                else
                {
                    currentImageIndex--;
                    ChangeBackground(currentImageIndex);
                }
            }
        }
        private void ChangeBackground(int imageIndex)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri($"pack://application:,,,/Images/BackImages/{imageIndex}" + ".jpg", UriKind.Absolute);
            bi.EndInit();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = bi;
            imageBrush.Stretch = Stretch.UniformToFill;
            this.Background = imageBrush;
        }

        
    }
}

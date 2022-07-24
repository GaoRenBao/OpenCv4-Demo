using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;
using System;
using System.Windows.Forms;
 
namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        string WINDOW_NAME = "【程序窗口】";
        Mat srcImage = new Mat(600, 800, MatType.CV_8UC3, Scalar.All(0));
        Rect g_rectangle;
        bool g_bDrawingBox = false;//是否进行绘制
        Random g_rng = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //【1】准备参数
            g_rectangle = new Rect(-1, -1, 0, 0);
            Mat tempImage = new Mat();

            //【2】设置鼠标操作回调函数
            Cv2.NamedWindow(WINDOW_NAME);
            Cv2.SetMouseCallback(WINDOW_NAME, new MouseCallback(on_MouseHandle));

            //【3】程序主循环，当进行绘制的标识符为真时，进行绘制
            while (true)
            {
                srcImage.CopyTo(tempImage);//拷贝源图到临时变量
                if (g_bDrawingBox)
                {
                    DrawRectangle(ref tempImage, g_rectangle);//当进行绘制的标识符为真，则进行绘制
                }
                Cv2.ImShow(WINDOW_NAME, tempImage);
                if (Cv2.WaitKey(10) == 27) break;//按下ESC键，程序退出
            }
        }
        public void on_MouseHandle(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            //鼠标移动消息
            if (@event == MouseEventTypes.MouseMove)
            {
                if (g_bDrawingBox)//如果是否进行绘制的标识符为真，则记录下长和宽到RECT型变量中
                {
                    g_rectangle.Width = x - g_rectangle.X;
                    g_rectangle.Height = y - g_rectangle.Y;
                }
            }
            //左键按下消息
            if (@event == MouseEventTypes.LButtonDown)
            {
                g_bDrawingBox = true;
                g_rectangle = new Rect(x, y, 0, 0);//记录起始点
            }

            //左键抬起消息
            if (@event == MouseEventTypes.LButtonUp)
            {
                g_bDrawingBox = false;//置标识符为false
                //对宽和高小于0的处理
                if (g_rectangle.Width < 0)
                {
                    g_rectangle.X += g_rectangle.Width;
                    g_rectangle.Width *= -1;
                }
                if (g_rectangle.Height < 0)
                {
                    g_rectangle.Y += g_rectangle.Height;
                    g_rectangle.Height *= -1;
                }
                //调用函数进行绘制
                DrawRectangle(ref srcImage, g_rectangle);
            }
        }
        void DrawRectangle(ref Mat img, Rect box)
        {
            if((box.BottomRight.X > box.TopLeft.X) && (box.BottomRight.Y > box.TopLeft.Y))
                Cv2.Rectangle(img, box.TopLeft, box.BottomRight, new Scalar(g_rng.Next(255), g_rng.Next(255), g_rng.Next(255)), 2);//随机颜色
        }
    }
}
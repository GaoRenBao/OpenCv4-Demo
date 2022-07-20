using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        //定义原始图、目标图、灰度图、掩模图
        Mat g_srcImage = new Mat();
        Mat g_dstImage = new Mat();
        Mat g_grayImage = new Mat();
        Mat g_maskImage = new Mat();

        //漫水填充的模式
        int g_nFillMode = 1;
        //负差最大值、正差最大值
        int g_nLowDifference = 20, g_nUpDifference = 20;
        //表示floodFill函数标识符低八位的连通值
        int g_nConnectivity = 4;
        //是否为彩色图的标识符布尔值
        bool g_bIsColor = true;
        //是否显示掩膜窗口的布尔值
        bool g_bUseMask = false;
        //新的重新绘制的像素值
        int g_nNewMaskVal = 255;

        Random g_rng = new Random();

        /// <summary>
        /// 视频操作
        /// </summary>
        public VideoCapture Cap = new VideoCapture();

        public Form1()
        {
            InitializeComponent();
        }

        private void Log(string str)
        {
            textBox1.Text += str + "\r\n";
        }

        private void ShowImage(int x = 0, int y = 0)
        {
            //-------------------【<1>调用floodFill函数之前的参数准备部分】---------------
            Point seed = new Point(x, y);
            //空范围的漫水填充，此值设为0，否则设为全局的g_nLowDifference
            int LowDifference = g_nFillMode == 0 ? 0 : g_nLowDifference;

            //空范围的漫水填充，此值设为0，否则设为全局的g_nUpDifference
            int UpDifference = g_nFillMode == 0 ? 0 : g_nUpDifference;

            //标识符的0~7位为g_nConnectivity，8~15位为g_nNewMaskVal左移8位的值，16~23位为CV_FLOODFILL_FIXED_RANGE或者0。
            int mflags = (int)(g_nConnectivity + (g_nNewMaskVal << 8) + (g_nFillMode == 1 ? FloodFillFlags.FixedRange : 0));

            //随机生成bgr值
            int b = g_rng.Next(255);//随机返回一个0~255之间的值
            int g = g_rng.Next(255);//随机返回一个0~255之间的值
            int r = g_rng.Next(255);//随机返回一个0~255之间的值

            //定义重绘区域的最小边界矩形区域
            Rect ccomp = new Rect();

            // 在重绘区域像素的新值，若是彩色图模式，取Scalar(b, g, r)；
            // 若是灰度图模式，取Scalar(r*0.299 + g*0.587 + b*0.114)
            Scalar newVal = g_bIsColor ? new Scalar(b, g, r) : new Scalar(r * 0.299 + g * 0.587 + b * 0.114);
            //目标图的赋值
            Mat dst = g_bIsColor ? g_dstImage : g_grayImage;
            int area;

            //--------------------【<2>正式调用floodFill函数】-----------------------------
            if (g_bUseMask)
            {
                Cv2.Threshold(g_maskImage, g_maskImage, 1, 128, ThresholdTypes.Binary);
                area = Cv2.FloodFill(dst, g_maskImage, seed, newVal, out ccomp,
                    new Scalar(LowDifference, LowDifference, LowDifference),
                    new Scalar(UpDifference, UpDifference, UpDifference), mflags);
                Cv2.ImShow("mask", g_maskImage);
            }
            else
            {
                area = Cv2.FloodFill(dst, seed, newVal, out ccomp,
                    new Scalar(LowDifference, LowDifference, LowDifference),
                    new Scalar(UpDifference, UpDifference, UpDifference), mflags);
            }

            Cv2.ImShow("效果图", dst);
            Log($"{area} 个像素被重绘");
            Cv2.WaitKey(10);
        }

        public void onMouse(MouseEvent @event, int x, int y, MouseEvent flags, IntPtr userdata)
        {
            // 若鼠标左键没有按下，便返回
            if (@event != MouseEvent.LButtonDown)
                return;
            ShowImage(x, y);
        }

        private void button_开始测试_Click(object sender, EventArgs e)
        {
            // 打开ID为0的摄像头
            if (!Cap.IsOpened())
                Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                MessageBox.Show("摄像头打开失败.");
                return;
            }
            // 设置采集的图像尺寸为：640*480
            Cap.Set(CaptureProperty.FrameWidth, 640);
            Cap.Set(CaptureProperty.FrameHeight, 480);
        
            if (!Cap.Read(g_srcImage))
            {
                return;
            }

            // 载入原图
            //g_srcImage = Cv2.ImRead("2.jpg");


            //拷贝源图到目标图
            g_srcImage.CopyTo(g_dstImage);

            //转换三通道的image0到灰度图
            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);

            //利用image0的尺寸来初始化掩膜mask
            g_maskImage.Create(g_srcImage.Rows + 2, g_srcImage.Cols + 2, MatType.CV_8UC1);

            Cv2.NamedWindow("效果图");
            CvMouseCallback GetRGBCvMouseCallback = new CvMouseCallback(onMouse);
            Cv2.SetMouseCallback("效果图", GetRGBCvMouseCallback);

            //Cv2.ImShow("效果图", g_dstImage);
            ShowImage();
            Cv2.WaitKey(0);
        }

        // 效果图在在灰度图，彩色图之间互换
        private void button1_Click(object sender, EventArgs e)
        {
            if (g_bIsColor)//若原来为彩色，转为灰度图，并且将掩膜mask所有元素设置为0
            {
                Log("切换彩色/灰度模式，当前操作为将【彩色模式】切换为【灰度模式】");
                Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);
                g_maskImage = g_maskImage.SetTo(new Scalar(0,0,0));//将mask所有元素设置为0
                g_bIsColor = false; //将标识符置为false，表示当前图像不为彩色，而是灰度
            }
            else//若原来为灰度图，便将原来的彩图image0再次拷贝给image，并且将掩膜mask所有元素设置为0
            {
                Log("切换彩色/灰度模式，当前操作为将【彩色模式】切换为【灰度模式】");
                g_srcImage.CopyTo(g_dstImage);
                g_maskImage = g_maskImage.SetTo(new Scalar(0,0,0));//将mask所有元素设置为0
                g_bIsColor = true;//将标识符置为true，表示当前图像模式为彩色
            }
            ShowImage();
        }

        // 显示/隐藏掩膜窗口
        private void button2_Click(object sender, EventArgs e)
        {
            if (g_bUseMask)
            {
                Cv2.DestroyWindow("mask");
                g_bUseMask = false;
            }
            else
            {
                Cv2.NamedWindow("mask", 0);
                g_maskImage = g_maskImage.SetTo(new Scalar(0,0,0));//将mask所有元素设置为0
                Cv2.ImShow("mask", g_maskImage);
                g_bUseMask = true;
            }
        }

        // 恢复原始图像
        private void button3_Click(object sender, EventArgs e)
        {
            g_srcImage.CopyTo(g_dstImage);
            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);
            g_maskImage = g_maskImage.SetTo(new Scalar(0, 0, 0));
            ShowImage();
        }

        // 使用空范围的漫水填充
        private void button4_Click(object sender, EventArgs e)
        {
            g_nFillMode = 0;
        }

        // 使用渐变、固定范围的漫水填充
        private void button5_Click(object sender, EventArgs e)
        {
            g_nFillMode = 1;
        }

        // 使用渐变、浮动范围的漫水填充
        private void button6_Click(object sender, EventArgs e)
        {
            g_nFillMode = 2;
        }

        // 操作标志符的低八位使用4位的连接模式
        private void button7_Click(object sender, EventArgs e)
        {
            g_nConnectivity = 4;
        }

        // 操作标志符的低八位使用8位的连接模式
        private void button8_Click(object sender, EventArgs e)
        {
            g_nConnectivity = 8;
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            g_nLowDifference = hScrollBar1.Value;
            label4.Text = g_nLowDifference.ToString();
        }

        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            g_nUpDifference = hScrollBar2.Value;
            label3.Text = g_nUpDifference.ToString();
        }
    }
}


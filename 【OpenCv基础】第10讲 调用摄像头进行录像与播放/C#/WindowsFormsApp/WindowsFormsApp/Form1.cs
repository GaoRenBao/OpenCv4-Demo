using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 视频操作
        /// </summary>
        public VideoCapture Cap = new VideoCapture();

        /// <summary>
        /// 录像
        /// </summary>
        public VideoWriter myavi;

        /// <summary>
        /// 录像开关
        /// </summary>
        public bool video = false; 

        public Form1()
        {
            InitializeComponent();
        }

        // 打开摄像头
        private void button1_Click(object sender, EventArgs e)
        {
            // 打开ID为0的摄像头
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

            //// 亮度
            //Cap.Set(CaptureProperty.Brightness, 1);
            //// 对比度
            //Cap.Set(CaptureProperty.Contrast, 0);
            //// 饱和度
            //Cap.Set(CaptureProperty.Saturation, 100);
            //// 色调
            //Cap.Set(CaptureProperty.Hue,150);
            //// 曝光
            //Cap.Set(CaptureProperty.Exposure, -1);

            Mat Img = new Mat();
            var window = new Window("capture");
            // 录像控制
            int end = 0;
            // 视频显示
            while (end < 2)
            {
                if (Cap.Read(Img))
                {
                    if(video)
                    {
                        // 用户点击了开始录像
                        end = 1;
                        // 录像，本地存储
                        myavi.Write(Img);
                    }
                    else if(end == 1)
                    {
                        // 用户点击了结束录像
                        end = 2; 
                        // 释放资源
                        myavi.Release();
                    }

                    // 在Window窗口中播放视频(方法1)
                    window.ShowImage(Img);
                    // 在Window窗口中播放视频(方法2)
                    //Cv2.ImShow("avi", Img);

                    // 在pictureBox1中显示效果图
                    pictureBox1.Image = BitmapConverter.ToBitmap(Img);
                    Cv2.WaitKey(10);
                }
            }
        }

        // 开始录像
        private void button2_Click(object sender, EventArgs e)
        {
            // 定义录像的数据类, true 表示保存的是彩色图像
            myavi = new VideoWriter("a.avi", FourCC.MJPG, 25, new OpenCvSharp.Size() { Width = 640, Height = 480 }, true);
            video = true;
        }

        // 停止录像
        private void button3_Click(object sender, EventArgs e)
        {
            video = false;
        }

        // 播放录像
        private void button4_Click(object sender, EventArgs e)
        {
            var capture = new VideoCapture(@"a.avi");
            // 计算帧率
            int sleepTime = (int)Math.Round(1000 / capture.Fps);

            using (var window = new Window("capture"))
            {
                // 声明实例 Mat类
                Mat image = new Mat();
                // 进入读取视频每镇的循环
                while (true)
                {
                    capture.Read(image);
                    //判断是否还有没有视频图像 
                    if (image.Empty())
                        break;

                    // 在Window窗口中播放视频(方法1)
                    window.ShowImage(image);
                    // 在Window窗口中播放视频(方法2)
                    //Cv2.ImShow("avi", image);

                    // 在pictureBox1中显示效果图
                    pictureBox1.Image = BitmapConverter.ToBitmap(image);
                    Cv2.WaitKey(sleepTime);
                }
            }
        }
    }
}

using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var capture = new VideoCapture("1.avi");
            if (!capture.IsOpened())
            {
                MessageBox.Show("Open video failed!");
                return;
            }

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
                    //pictureBox1.Image = BitmapConverter.ToBitmap(image);
                    Cv2.WaitKey(sleepTime);
                }
            }
        }
    }
}

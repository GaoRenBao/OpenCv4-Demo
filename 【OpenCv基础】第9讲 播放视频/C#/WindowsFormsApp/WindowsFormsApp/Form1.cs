using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

// https://shimat.github.io/opencvsharp_docs/html/d69c29a1-7fb1-4f78-82e9-79be971c3d03.htm

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
            var capture = new VideoCapture(@"1.avi");
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

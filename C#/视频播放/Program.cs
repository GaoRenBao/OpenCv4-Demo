/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：视频播放
博客：http://www.bilibili996.com/Course?id=4380386000009
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var capture = new VideoCapture("../../../images/lol.avi");
            if (!capture.IsOpened())
            {
                Console.WriteLine("Open video failed!");
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

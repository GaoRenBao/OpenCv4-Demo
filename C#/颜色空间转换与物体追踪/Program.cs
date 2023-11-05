/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VideoCapture Cap = new VideoCapture();
            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                Console.Read();
                return;
            }

            // 摄像头图像
            Mat frame = new Mat();

            // 设置蓝色的阈值
            Scalar lower = new Scalar(90, 50, 50);
            Scalar upper = new Scalar(130, 255, 255);

            while (true)
            {
                if (Cap.Read(frame))
                {
                    Mat hsv = new Mat();
                    Mat res = new Mat();
                    Mat mask = new Mat();

                    // 换到 HSV
                    Cv2.CvtColor(frame, hsv, ColorConversionCodes.BGR2HSV);
                    // 根据阈值构建掩模
                    Cv2.InRange(hsv, lower, upper, mask);
                    // 对原图像和掩模位运算
                    Cv2.BitwiseAnd(frame, frame, res, mask);
                    // 显示图像
                    Cv2.ImShow("frame", frame);
                    Cv2.MoveWindow("frame", 0, 0); // 原地
                    Cv2.ImShow("mask", mask);
                    Cv2.MoveWindow("mask", frame.Width, 0);// 右边
                    Cv2.ImShow("res", res);
                    Cv2.MoveWindow("res", 0, frame.Height);// 下边
                    Cv2.WaitKey(10);
                }
            }
        }
    }
}

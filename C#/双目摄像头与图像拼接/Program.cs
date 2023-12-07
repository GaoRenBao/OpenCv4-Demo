/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：双目摄像头与图像拼接
博客：http://www.bilibili996.com/Course?id=1048651000244
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;
using static OpenCvSharp.Stitcher;
using System.Collections.Generic;

namespace demo
{
    internal class Program
    {
        /// <summary>
        /// 视频操作
        /// </summary>
        public static VideoCapture Cap = new VideoCapture();

        static void Main(string[] args)
        {
            //demo1();
            demo2();
            Cv2.WaitKey();
        }

        /// <summary>
        /// 调用双目摄像头
        /// </summary>
        private static void demo1()
        {
            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                Console.Read();
                return;
            }

            Cap.Set(VideoCaptureProperties.FrameWidth, 3040);
            Cap.Set(VideoCaptureProperties.FrameHeight, 1080);

            Mat Image = new Mat();
            List<Mat> images;
            Mat panoMat = new Mat();
            Stitcher stitcher;

            while (true)
            {
                if (Cap.Read(Image))
                {
                    Cv2.Resize(Image, Image, new Size(Image.Cols * 0.5, Image.Rows * 0.5), 0, 0, InterpolationFlags.Area);

                    // 将双目摄像头读取到的图像裁剪成左右两张图
                    Rect faces1 = new Rect()
                    {
                        X = 0,
                        Y = 0,
                        Width = (int)(Image.Width * 0.5),
                        Height = Image.Height
                    };
                    Rect faces2 = new Rect()
                    {
                        X = (int)(Image.Width * 0.5),
                        Y = 0,
                        Width = (int)(Image.Width * 0.5),
                        Height = Image.Height
                    };
                    Mat img_1 = new Mat(Image, faces1);
                    Mat img_2 = new Mat(Image, faces2);
                    Cv2.ImShow("img_1", img_1);
                    Cv2.ImShow("img_2", img_2);

                    // 对两张图像进行图像拼接
                    images = new List<Mat>() { img_2, img_1 };
                    stitcher = Stitcher.Create(Mode.Scans);
                    if (stitcher.Stitch(images, panoMat) != Stitcher.Status.OK)
                    {
                        Cv2.WaitKey(1);
                        continue;
                    }

                    Cv2.ImWrite("1.jpg", img_1);
                    Cv2.ImWrite("2.jpg", img_2);
                    //Cv2.ImWrite("拼接结果.jpg", panoMat);
                    Cv2.ImShow("拼接结果", panoMat);
                    return;
                }
            }
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        private static void demo2()
        {
            Mat img_1 = new Mat("../../../images/orb (1).jpg");
            Mat img_2 = new Mat("../../../images/orb (2).jpg");
            Cv2.ImShow("img_1", img_1);
            Cv2.ImShow("img_2", img_2);

            Mat panoMat = new Mat();
            List<Mat> images = new List<Mat>() { img_2, img_1 };
            Stitcher stitcher = Stitcher.Create(Mode.Scans);
            if (stitcher.Stitch(images, panoMat) != Stitcher.Status.OK)
            {
                Console.WriteLine("拼接失败.");
                Console.Read();
                return;
            }
            Cv2.ImShow("拼接结果", panoMat);
        }
    }
}

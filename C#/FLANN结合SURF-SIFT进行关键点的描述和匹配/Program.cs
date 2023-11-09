/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：FLANN结合SURF/SIFT进行关键点的描述和匹配
博客：http://www.bilibili996.com/Course?id=3106495000236
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;
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
        /// SURF
        /// </summary>
        static void demo1()
        {
            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                return;
            }

            //Cap.Set(VideoCaptureProperties.FrameWidth, 640);  // 设置采集的图像宽度：640
            //Cap.Set(VideoCaptureProperties.FrameHeight, 480); // 设置采集的图像高度：480
            //Cap.Set(VideoCaptureProperties.Exposure, 0);     // 曝光

            // 载入源图片
            Mat trainImage = new Mat();
            for (int i = 0; i < 10; i++)
                Cap.Read(trainImage);

            Mat trainImage_gray = new Mat();
            Cv2.CvtColor(trainImage, trainImage_gray, ColorConversionCodes.BGR2GRAY);

            // 定义一个特征检测类对象
            var MySurf = OpenCvSharp.XFeatures2D.SURF.Create(80);
            Mat trainDescriptor = new Mat();
            KeyPoint[] train_keyPoint;

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            // 方法1：计算描述符（特征向量），将Detect和Compute操作分开
            train_keyPoint = MySurf.Detect(trainImage_gray);
            MySurf.Compute(trainImage_gray, ref train_keyPoint, trainDescriptor);

            // 方法2：计算描述符（特征向量），将Detect和Compute操作合并
            //MySurf.DetectAndCompute(trainImage_gray, null, out train_keyPoint, trainDescriptor);

            // 创建基于FLANN的描述符匹配对象
            List<Mat> descriptors = new List<Mat>() { trainDescriptor };
            //BFMatcher matcher = new BFMatcher();
            FlannBasedMatcher matcher = new FlannBasedMatcher();
            matcher.Add(descriptors);
            matcher.Train();

            Mat testImage = new Mat();
            Mat testImage_gray = new Mat();

            while (true)
            {
                if (Cap.Read(testImage))
                {
                    // 转化图像到灰度
                    Cv2.CvtColor(testImage, testImage_gray, ColorConversionCodes.BGR2GRAY);

                    // 检测S关键点、提取测试图像描述符
                    Mat testDescriptor = new Mat();
                    KeyPoint[] test_keyPoint;

                    // 方法1：计算描述符（特征向量）
                    test_keyPoint = MySurf.Detect(testImage_gray);
                    MySurf.Compute(testImage_gray, ref test_keyPoint, testDescriptor);

                    // 方法2：计算描述符（特征向量）
                    //MySurf.DetectAndCompute(testImage_gray, null, out test_keyPoint, testDescriptor);

                    // 匹配训练和测试描述符
                    DMatch[][] matches = matcher.KnnMatch(testDescriptor, 2);

                    // 根据劳氏算法（Lowe's algorithm），得到优秀的匹配点
                    List<DMatch> goodMatches = new List<DMatch>();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        if (matches[i][0].Distance < 0.6 * matches[i][1].Distance)
                            goodMatches.Add(matches[i][0]);
                    }

                    //绘制匹配点并显示窗口
                    Mat dstImage = new Mat();
                    Cv2.DrawMatches(testImage, test_keyPoint, trainImage, train_keyPoint, goodMatches, dstImage);

                    // 显示效果图
                    Cv2.ImShow("匹配窗口", dstImage);
                    // 按ESC退出
                    if (Cv2.WaitKey(10) == 27)
                        break;

                }
            }
        }

        /// <summary>
        /// SIFT
        /// </summary>
        static void demo2()
        {
            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                return;
            }

            //Cap.Set(VideoCaptureProperties.FrameWidth, 640);  // 设置采集的图像宽度：640
            //Cap.Set(VideoCaptureProperties.FrameHeight, 480); // 设置采集的图像高度：480
            //Cap.Set(VideoCaptureProperties.Exposure, 0);     // 曝光

            // 载入源图片
            Mat trainImage = new Mat();
            for(int i = 0;i<10;i++)
                Cap.Read(trainImage);

            Mat trainImage_gray = new Mat();
            Cv2.CvtColor(trainImage, trainImage_gray, ColorConversionCodes.BGR2GRAY);

            // 定义一个特征检测类对象
            var MySift = OpenCvSharp.Features2D.SIFT.Create(80);
            Mat trainDescriptor = new Mat();
            KeyPoint[] train_keyPoint;

            // 模板类是能够存放任意类型的动态数组，能够增加和压缩数据
            // 方法1：计算描述符（特征向量），将Detect和Compute操作分开
            train_keyPoint = MySift.Detect(trainImage_gray);
            MySift.Compute(trainImage_gray, ref train_keyPoint, trainDescriptor);

            // 方法2：计算描述符（特征向量），将Detect和Compute操作合并
            // MySift.DetectAndCompute(trainImage_gray, null, out train_keyPoint, trainDescriptor);

            // 创建基于FLANN的描述符匹配对象
            List<Mat> descriptors = new List<Mat>() { trainDescriptor };
            FlannBasedMatcher matcher = new FlannBasedMatcher();
            matcher.Add(descriptors);
            matcher.Train();

            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                return;
            }

            Mat testImage = new Mat();
            Mat testImage_gray = new Mat();

            while (true)
            {
                if (Cap.Read(testImage))
                {
                    // 转化图像到灰度
                    Cv2.CvtColor(testImage, testImage_gray, ColorConversionCodes.BGR2GRAY);

                    // 检测S关键点、提取测试图像描述符
                    Mat testDescriptor = new Mat();
                    KeyPoint[] test_keyPoint;

                    // 方法1：计算描述符（特征向量）
                    test_keyPoint = MySift.Detect(testImage_gray);
                    MySift.Compute(testImage_gray, ref test_keyPoint, testDescriptor);

                    // 方法2：计算描述符（特征向量）
                    // MySift.DetectAndCompute(testImage_gray, null, out test_keyPoint, testDescriptor);

                    // 匹配训练和测试描述符
                    DMatch[][] matches = matcher.KnnMatch(testDescriptor, 2);

                    // 根据劳氏算法（Lowe's algorithm），得到优秀的匹配点
                    List<DMatch> goodMatches = new List<DMatch>();
                    for (int i = 0; i < matches.Length; i++)
                    {
                        if (matches[i][0].Distance < 0.6 * matches[i][1].Distance)
                            goodMatches.Add(matches[i][0]);
                    }

                    //绘制匹配点并显示窗口
                    Mat dstImage = new Mat();
                    Cv2.DrawMatches(testImage, test_keyPoint, trainImage, train_keyPoint, goodMatches, dstImage);

                    // 显示效果图
                    Cv2.ImShow("匹配窗口", dstImage);
                    // 按ESC退出
                    if (Cv2.WaitKey(10) == 27)
                        break;
                }
            }
        }
    }
}

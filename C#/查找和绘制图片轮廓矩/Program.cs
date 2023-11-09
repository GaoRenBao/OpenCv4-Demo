/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：查找和绘制图片轮廓矩
博客：http://www.bilibili996.com/Course?id=4739103000210
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
        static Mat g_cannyMat_output = new Mat();
        static Mat g_grayImage = new Mat();
        static int g_nThresh = 100;
        static RNG rng = new RNG(12345);

        static void Main(string[] args)
        {
            //【1】载入3通道的原图像
            Mat src = Cv2.ImRead("../../../images/lake2.jpg");
            if (src.Empty())
            {
                Console.WriteLine("Could not open or find the image!");
                return;
            }
            //【2】得到原图的灰度图像并进行平滑
            Cv2.CvtColor(src, g_grayImage, ColorConversionCodes.BGR2GRAY);
            Cv2.Blur(g_grayImage, g_grayImage, new Size(3, 3));

            //【3】创建原始图窗口并显示
            Cv2.NamedWindow("Source", WindowFlags.AutoSize);
            Cv2.ImShow("Source", src);

            //【4】设置滚动条并调用一次回调函数
            Cv2.CreateTrackbar("阈值:", "Source", ref g_nThresh, 255, thresh_callback);
            thresh_callback(0, IntPtr.Zero);
            Cv2.WaitKey();
        }

        private static void thresh_callback(int pos, IntPtr userData)
        {
            // 使用Canndy检测边缘
            Cv2.Canny(g_grayImage, g_cannyMat_output, g_nThresh, g_nThresh * 2, 3);

            // 找出轮廓
            Point[][] g_vContours;
            HierarchyIndex[] g_vHierarchy;
            Cv2.FindContours(g_cannyMat_output, out g_vContours, out g_vHierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            // 计算矩
            List<Moments> mu = new List<Moments>();
            for (int i = 0; i < g_vContours.Length; i++)
            {
                mu.Add(Cv2.Moments(g_vContours[i], false));
            }

            // 计算中心矩
            List<Point> mc = new List<Point>();
            for (int i = 0; i < g_vContours.Length; i++)
            {
                mc.Add(new Point((float)(mu[i].M10 / mu[i].M00), (float)(mu[i].M01 / mu[i].M00)));
            }

            // 绘制轮廓
            Mat drawing = Mat.Zeros(g_cannyMat_output.Size(), MatType.CV_8UC3);
            for (int i = 0; i < g_vContours.Length; i++)
            {
                // 随机设置颜色
                Scalar color = new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255));
                // 绘制外层和内层轮廓
                Cv2.DrawContours(drawing, g_vContours, (int)i, color, 2, LineTypes.Link8, g_vHierarchy, 0, new Point());
                //绘制圆
                Cv2.Circle(drawing, mc[i], 4, color, -1, LineTypes.Link8, 0);
            }

            // 通过m00计算轮廓面积并且和OpenCV函数比较
            System.Diagnostics.Debug.WriteLine("\t输出内容: 面积和轮廓长度\n");
            for (int i = 0; i < g_vContours.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine($">通过m00计算出轮廓[{i}]的面积: (M_00) = {mu[i].M00}");
                System.Diagnostics.Debug.WriteLine($"OpenCV函数计算出的面积={Cv2.ContourArea(g_vContours[i])}");
                System.Diagnostics.Debug.WriteLine($"长度: {Cv2.ArcLength(g_vContours[i], true)}");
                Scalar color = new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255));
                Cv2.DrawContours(drawing, g_vContours, i, color, 2, LineTypes.Link8, g_vHierarchy, 0, new Point());
                Cv2.Circle(drawing, mc[i], 4, color, -1, LineTypes.Link8, 0);
            }

            Cv2.ImShow("Contours", drawing);
        }
    }
}

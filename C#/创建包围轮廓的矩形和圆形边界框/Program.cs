/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        static Mat g_grayImage = new Mat();
        static int thresh = 100;
        static RNG rng = new RNG(12345);

        static void Main(string[] args)
        {
            // 【1】读取图像
            Mat src = Cv2.ImRead("../../../images/home2.jpg");
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
            Cv2.CreateTrackbar("阈值:", "Source", ref thresh, 255, thresh_callback);
            thresh_callback(0, IntPtr.Zero);
            Cv2.WaitKey();
        }

        private static void thresh_callback(int pos, IntPtr userData)
        {
            Mat threshold_output = new Mat();
            // 使用Threshold检测图像边缘
            Cv2.Threshold(g_grayImage, threshold_output, thresh, 255, ThresholdTypes.Binary);

            // 找出轮廓
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(threshold_output, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            // 多边形逼近轮廓 + 获取矩形和圆形边界框
            Point[][] contours_poly = new Point[contours.Length][];
            Rect[] boundRect = new Rect[contours.Length];
            Point2f[] center = new Point2f[contours.Length];
            float[] radius = new float[contours.Length];

            // 一个循环，遍历所有部分，进行本程序最核心的操作
            for (int i = 0; i < contours.Length; i++)
            {
                // 用指定精度逼近多边形曲线 
                contours_poly[i] = Cv2.ApproxPolyDP(contours[i], 3, true);
                // 计算点集的最外面（up-right）矩形边界
                boundRect[i] = Cv2.BoundingRect(contours_poly[i]);
                // 对给定的 2D点集，寻找最小面积的包围圆形 
                Cv2.MinEnclosingCircle(contours_poly[i], out center[i], out radius[i]);
            }

            // 绘制多边形轮廓 + 包围的矩形框 + 圆形框
            Mat drawing = Mat.Zeros(threshold_output.Size(), MatType.CV_8UC3);
            for (int i = 0; i < contours.Length; i++)
            {
                //随机设置颜色
                Scalar color = new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255));
                //绘制轮廓
                Cv2.DrawContours(drawing, contours_poly, (int)i, color);
                //绘制矩形
                Cv2.Rectangle(drawing, boundRect[i].TopLeft, boundRect[i].BottomRight, color, 1);
                //绘制圆
                Cv2.Circle(drawing, new Point(center[i].X, center[i].Y), (int)radius[i], color, 1);
            }

            Cv2.ImShow("Contours", drawing);
        }
    }
}

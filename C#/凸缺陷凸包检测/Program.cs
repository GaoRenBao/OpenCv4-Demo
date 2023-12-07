/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：凸缺陷/凸包检测
博客：http://www.bilibili996.com/Course?id=3128323000263
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
            demo1();
            demo2();
            Cv2.WaitKey(0);
        }

        static void demo1()
        {
            Mat img = Cv2.ImRead("../../../images/star3.jpg");
            Mat img_gray = new Mat();
            Cv2.CvtColor(img, img_gray, ColorConversionCodes.BGR2GRAY);

            Mat thresh = new Mat();
            Cv2.Threshold(img_gray, thresh, 127, 255, ThresholdTypes.Binary);

            Mat[] contours = new Mat[] { };
            Cv2.FindContours(thresh, out contours, new Mat(), RetrievalModes.CComp, ContourApproximationModes.ApproxNone, null);
            Mat cnt = contours[0];

            // 函数 cv2.isContourConvex() 可以可以用来检测一个曲线是不是凸的。它只能返回 True 或 False。
            bool k = Cv2.IsContourConvex(cnt);
            Console.WriteLine($"K={k}");

            // 函数 cv2.convexHull() 可以用来检测一个曲线的凸包
            Mat hull = new Mat();
            Cv2.ConvexHull(cnt, hull, false);

            // 创建结果图像并绘制轮廓和凸包  
            Cv2.DrawContours(img, contours, 0, new Scalar(0, 0, 255), 2); // 绘制原始轮廓  
            Cv2.DrawContours(img, new Mat[] { hull }, 0, new Scalar(0, 255, 0), 2); // 绘制凸包  

            Cv2.ImShow("img1", img);
        }

        static void demo2()
        {
            Mat img = Cv2.ImRead("../../../images/star3.jpg");
            Mat img_gray = new Mat();
            Cv2.CvtColor(img, img_gray, ColorConversionCodes.BGR2GRAY);

            Mat thresh = new Mat();
            Cv2.Threshold(img_gray, thresh, 127, 255, ThresholdTypes.Binary);

            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(thresh, out contours, out hierarcy, RetrievalModes.CComp, ContourApproximationModes.ApproxNone, null);
            Point[] cnt = contours[0];

            // 函数 cv2.isContourConvex() 可以可以用来检测一个曲线是不是凸的。它只能返回 True 或 False。
            bool k = Cv2.IsContourConvex(cnt);
            Console.WriteLine($"K={k}");

            // 函数 cv2.convexHull() 可以用来检测一个曲线的凸包
            int[] hull = Cv2.ConvexHullIndices(cnt, false);

            // 将所有凸包提取出来
            //前三个值得含义分别为：凸缺陷的起始点，凸缺陷的终点，凸缺陷的最深点（即边缘点到凸包距离最大点）。
            var defects = Cv2.ConvexityDefects(cnt, hull);
            for (int j = 0; j < defects.Length; j++)
            {
                Point start = cnt[defects[j].Item0];
                Point end = cnt[defects[j].Item1];
                Point far = cnt[defects[j].Item2];
                //Point fart = cnt[defects[j].Item3];
                Cv2.Line(img, start, end, new Scalar(0, 255, 0), 2);
                Cv2.Circle(img, far, 5, new Scalar(0, 0, 255), -1);
            }

            Cv2.ImShow("img2", img);
        }
    }
}

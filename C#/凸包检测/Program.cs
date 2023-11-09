/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：凸包检测
博客：http://www.bilibili996.com/Course?id=3741165000202
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
        static void Main(string[] args)
        {
            demo1();
            //demo2();
            //demo3();
            Cv2.WaitKey(0);
        }

        #region 演示1
        static void demo1()
        {
            Mat srcImage = Cv2.ImRead("../../../images/kele.jpg");

            //图像灰度图转化并平滑滤波
            Mat grayImage = new Mat();
            Cv2.CvtColor(srcImage, grayImage, ColorConversionCodes.BGR2GRAY);
            // 均值滤波
            Cv2.Blur(grayImage, grayImage, new OpenCvSharp.Size() { Width = 3, Height = 3 });
            Cv2.ImShow("原图像", grayImage);

            Mat threshold_output = new Mat();
            // 使用Threshold检测图像边缘
            Cv2.Threshold(grayImage, threshold_output, 200, 255, ThresholdTypes.Binary);

            // 寻找图像轮廓
            Random rd = new Random();
            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(threshold_output, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

            //寻找图像凸包
            Point[][] hull = new Point[contours.Length][];
            for (int i = 0; i < contours.Length; i++)
            {
                hull[i] = Cv2.ConvexHull(contours[i], false);
            }

            //绘制轮廓和凸包
            RNG rng = new RNG(12345);
            Mat drawing = Mat.Zeros(threshold_output.Size(), MatType.CV_8UC3);
            for (int i = 0; i < hull.Length; i++)
            {
                Scalar color = new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255));
                // 绘制轮廓
                Cv2.DrawContours(drawing, contours, (int)i, color, 1, LineTypes.Link8);
                // 绘制凸包
                Cv2.DrawContours(drawing, hull, (int)i, color, 5, LineTypes.Link8);
            }
            Cv2.ImShow("凸包", drawing);
        }
        #endregion

        #region 演示2
        private static Mat grayImage = new Mat();
        private static Mat drawing = new Mat();
        private static Mat threshold_output = new Mat();
        private static RNG rng = new RNG(12345);
        private static int temp = 100;

        static void demo2()
        {
            Mat srcImage = Cv2.ImRead("../../../images/kele.jpg");

            //图像灰度图转化并平滑滤波
            Cv2.CvtColor(srcImage, grayImage, ColorConversionCodes.BGR2GRAY);
            // 均值滤波
            Cv2.Blur(grayImage, grayImage, new Size() { Width = 3, Height = 3 });

            Cv2.NamedWindow("凸包", WindowFlags.AutoSize);
            Cv2.CreateTrackbar("阈值", "凸包", 255, null);
            Cv2.SetMouseCallback("凸包", new MouseCallback(on_Mouse));
            while (true)
            {
                if (Cv2.WaitKey(10) == 'q')
                {
                    break;
                }
            }
        }

        public static void on_Mouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            int thresh = Cv2.GetTrackbarPos("阈值", "凸包");

            // 滑动条滑动一次，回调会执行N次，为避免不必要的运算，这里加个过滤
            if (temp == thresh) return;
            temp = thresh;

            Console.WriteLine(thresh);
            // 使用Threshold检测图像边缘
            Cv2.Threshold(grayImage, threshold_output, thresh, 255, ThresholdTypes.Binary);

            // 寻找图像轮廓
            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(threshold_output, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

            //寻找图像凸包
            Point[][] hull = new Point[contours.Length][];
            for (int i = 0; i < contours.Length; i++)
            {
                hull[i] = Cv2.ConvexHull(contours[i], false);
            }

            //绘制轮廓和凸包
            drawing = Mat.Zeros(threshold_output.Size(), MatType.CV_8UC3);
            for (int i = 0; i < hull.Length; i++) //contours.Length
            {
                Scalar color = new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255));
                // 绘制轮廓
                Cv2.DrawContours(drawing, contours, (int)i, color, 1, LineTypes.Link8);
                // 绘制凸包
                Cv2.DrawContours(drawing, hull, (int)i, color, 5, LineTypes.Link8);
            }
            Cv2.ImShow("凸包", drawing);
            Cv2.WaitKey(1);
        }

        #endregion

        #region 演示3
        static void demo3()
        {
            RNG rng = new RNG(12345);
            Mat image = Mat.Zeros(600, 600, MatType.CV_8UC3);

            while (true)
            {
                List<Point> points = new List<Point>(); //点值

                //参数初始化
                int count = rng.Uniform(20, 50);//随机生成点的数量
                points.Clear();

                //随机生成点坐标
                for (int i = 0; i < count; i++)
                {
                    Point point;
                    point.X = rng.Uniform(image.Cols / 4, image.Cols * 3 / 4);
                    point.Y = rng.Uniform(image.Rows / 4, image.Rows * 3 / 4);
                    points.Add(point);
                }

                //寻找图像凸包
                Point[] hull = Cv2.ConvexHull(points, false);

                //绘制出随机颜色的点
                image = Mat.Zeros(600, 600, MatType.CV_8UC3);
                for (int i = 0; i < count; i++)
                    Cv2.Circle(image, points[i], 3, new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255)), -1, LineTypes.AntiAlias);

                //准备参数
                int hullcount = (int)hull.Length;//凸包的边数
                Point point0 = hull[hullcount - 1];//连接凸包边的坐标点

                //绘制凸包的边
                for (int i = 0; i < hullcount; i++)
                {
                    Point point = hull[i];
                    Cv2.Line(image, point0, point, new Scalar(255, 255, 255), 2, LineTypes.AntiAlias);
                    point0 = point;
                }

                //显示效果图
                Cv2.ImShow("凸包检测示例", image);
                Cv2.WaitKey(0);
            }
        }
        #endregion
    }
}

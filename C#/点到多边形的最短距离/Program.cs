/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：点到多边形的最短距离
博客：http://www.bilibili996.com/Course?id=3657332000262
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
            Mat srcImage = Cv2.ImRead("../../../images/lightning.png");
            Mat img = new Mat();
            Cv2.CvtColor(srcImage, img, ColorConversionCodes.BGR2GRAY);

            Mat threshold = new Mat();
            Cv2.Threshold(img, threshold, 127, 255, ThresholdTypes.Binary);

            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(threshold, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);
            Console.WriteLine($"contours len:{contours.Length}");
            Point[] cnt = contours[0];

            Point point = new Point(50, 50);
            Cv2.Circle(srcImage, point, 5, new Scalar(0, 0, 255), -1);
            Cv2.ImShow("srcImage", srcImage);

            double dist = Cv2.PointPolygonTest(cnt, point, true);
            Console.WriteLine($"距离:{dist}");
            Cv2.WaitKey(0);
        }
    }
}

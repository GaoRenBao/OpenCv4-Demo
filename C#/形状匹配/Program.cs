/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：形状匹配
博客：http://www.bilibili996.com/Course?id=0930272000264
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
            Mat img1 = Cv2.ImRead("../../../images/star.jpg", 0);
            //Mat img2 = Cv2.ImRead("../../../images/star_b.jpg",0);
            Mat img2 = Cv2.ImRead("../../../images/star_c.jpg", 0);

            Cv2.ImShow("img1", img1);
            Cv2.ImShow("img2", img2);

            Mat thresh1 = new Mat();
            Mat thresh2 = new Mat();
            Cv2.Threshold(img1, thresh1, 127, 255, ThresholdTypes.Binary);
            Cv2.Threshold(img2, thresh2, 127, 255, ThresholdTypes.Binary);

            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(thresh1, out contours, out hierarcy, RetrievalModes.CComp, ContourApproximationModes.ApproxNone, null);
            Point[] cnt1 = contours[0];
            Console.WriteLine($"contours len1：{contours.Length}");

            Cv2.FindContours(thresh2, out contours, out hierarcy, RetrievalModes.CComp, ContourApproximationModes.ApproxNone, null);
            Point[] cnt2 = contours[0];
            Console.WriteLine($"contours len2：{contours.Length}");

            double ret = Cv2.MatchShapes(cnt1, cnt2, ShapeMatchModes.I1, 0.0);
            Console.WriteLine($"ret：{ret}");
            Cv2.WaitKey(0);
        }
    }
}

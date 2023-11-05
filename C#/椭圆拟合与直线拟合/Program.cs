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
        static void Main(string[] args)
        {
            Mat rImg = Cv2.ImRead("../../../images/lightning.png");
            Mat img = new Mat();
            Cv2.CvtColor(rImg, img, ColorConversionCodes.BGR2GRAY);

            Mat thresh = new Mat();
            Cv2.Threshold(img, thresh, 127, 255, ThresholdTypes.Binary);

            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(thresh, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);
            Console.WriteLine($"contours len:{contours.Length}");
            Point[] cnt = contours[0];

            // 椭圆拟合
            // 使用的函数为 cv2.ellipse()  返回值其实就是旋转边界矩形的内切圆
            RotatedRect ellipse = Cv2.FitEllipse(cnt);
            float angle = ellipse.Angle;
            Cv2.Ellipse(rImg, ellipse, new Scalar(0, 255, 0), 2);

            // 直线拟合
            // 我们可以根据一组点拟合出一条直线 同样我们也可以为图像中的白色点 拟合出一条直线。
            Line2D line = Cv2.FitLine(cnt, DistanceTypes.L2, 0, 0.01, 0.01);
            int lefty = (int)((-line.X1 * line.Vy / line.Vx) + line.Y1);
            int righty = (int)(((img.Cols - line.X1) * line.Vy / line.Vx) + line.Y1);
            Cv2.Line(rImg, new Point(img.Cols - 1, righty), new Point(0, lefty), new Scalar(0, 0, 255), 2);

            Cv2.ImShow("rImg", rImg);
            Cv2.WaitKey(0);
        }
    }
}

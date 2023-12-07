/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：轮廓的性质
博客：http://www.bilibili996.com/Course?id=4292381000261
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;
using System.Linq;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat srcImage = Cv2.ImRead("../../../images/star3.jpg");

            Mat img = new Mat();
            Cv2.CvtColor(srcImage, img, ColorConversionCodes.BGR2GRAY);

            Mat thresh = new Mat();
            Cv2.Threshold(img, thresh, 127, 255, ThresholdTypes.Binary);
            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(thresh, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);
            Console.WriteLine($"contours len:{contours.Length}");
            Point[] cnt = contours[0];  // contours 有三个，挑个最大的，方便演示

            test1(cnt);
            test2(cnt);
            test3(cnt);
            test4(cnt);
            test5(cnt);
            test6(img, cnt);
            test7(srcImage, cnt);
        }

        static void test1(Point[] cnt)
        {
            // 边界矩形的宽高比
            Rect rect = Cv2.BoundingRect(cnt);
            float aspect_ratio = (float)rect.Width / (float)rect.Height;
            Console.WriteLine($"边界矩形的宽高比:{aspect_ratio}");
        }
        static void test2(Point[] cnt)
        {
            // Extent轮廓面积与边界矩形面积的比
            double area = Cv2.ContourArea(cnt);
            Rect rect = Cv2.BoundingRect(cnt);
            double rect_area = rect.Width * rect.Height;
            double extent = area / rect_area;
            Console.WriteLine($"Extent轮廓面积与边界矩形面积的比:{extent}");
        }
        static void test3(Point[] cnt)
        {
            // Solidity轮廓面积与凸包面积的比。
            double area = Cv2.ContourArea(cnt);
            Point[] hull = Cv2.ConvexHull(cnt);
            double hull_area = Cv2.ContourArea(hull);
            double solidity = area / hull_area;
            Console.WriteLine($"Solidity轮廓面积与凸包面积的比:{solidity}");
        }
        static void test4(Point[] cnt)
        {
            // Equivalent Diameter与轮廓面积相等的圆形的直径
            double area = Cv2.ContourArea(cnt);
            double equi_diameter = Math.Sqrt(4 * area / Math.PI);
            Console.WriteLine($"Equivalent Diameter与轮廓面积相等的圆形的直径:{equi_diameter}");
        }
        static void test5(Point[] cnt)
        {
            // Orientation对象的方向下的方法会返回长轴和短轴的长度
            RotatedRect rect = Cv2.FitEllipse(cnt);
            Console.WriteLine($"Orientation对象的方向下的方法会返回长轴和短轴的长度:{rect.Center},{rect.Size},{rect.Angle}");
        }
        static void test6(Mat imgray, Point[] cnt)
        {
            // Mask and Pixel Points掩模和像素点
            Mat mask = new Mat(imgray.Size(), MatType.CV_8U, new Scalar(0, 0, 0));

            Point[][] cnts = new Point[][] { cnt };
            Cv2.DrawContours(mask, cnts, 0, 255, -1);

            // 最大值和最小值及它们的位置
            Cv2.MinMaxLoc(imgray, out double minVal, out double maxVal, out Point minLoc, out Point maxLoc, mask);
            Console.WriteLine($"最大值和最小值及它们的位置:{minVal},{maxVal},{minLoc},{maxLoc}");

            // 我们也可以使用相同的掩模求一个对象的平均颜色或平均灰度
            Scalar mean_val = Cv2.Mean(imgray, mask);
            Console.WriteLine($"平均色:{mean_val}");
        }
        static void test7(Mat img, Point[] cnt)
        {
            Point topmost = cnt.FirstOrDefault(x => x.Y == cnt.Min(p => p.Y));    // 最上面
            Point bottommost = cnt.FirstOrDefault(x => x.Y == cnt.Max(p => p.Y)); // 最下面
            Point leftmost = cnt.FirstOrDefault(x => x.X == cnt.Min(p => p.X));   // 最左面
            Point rightmost = cnt.FirstOrDefault(x => x.X == cnt.Max(p => p.X));  // 最右面

            Console.WriteLine($"最上面:{topmost}");
            Console.WriteLine($"最下面:{bottommost}");
            Console.WriteLine($"最左:{leftmost}");
            Console.WriteLine($"最右:{rightmost}");

            Cv2.Circle(img, topmost, 5, new Scalar(0, 0, 255), -1);
            Cv2.Circle(img, bottommost, 5, new Scalar(0, 0, 255), -1);
            Cv2.Circle(img, leftmost, 5, new Scalar(0, 0, 255), -1);
            Cv2.Circle(img, rightmost, 5, new Scalar(0, 0, 255), -1);
            Cv2.ImShow("img", img);
            Cv2.WaitKey(0);
        }
    }
}

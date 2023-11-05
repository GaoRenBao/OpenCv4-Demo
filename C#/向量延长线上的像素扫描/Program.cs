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
        // 计算顺时针角度：0~360
        private static double VectorLine(Vec4i A, Vec4i B)
        {
            Point P11 = new Point(A[0], A[1]);
            Point P12 = new Point(A[2], A[3]);
            Point P21 = new Point(B[0], B[1]);
            Point P22 = new Point(B[2], B[3]);
            double ax = P11.X - P12.X;
            double ay = P11.Y - P12.Y;
            double bx = P21.X - P22.X;
            double by = P21.Y - P22.Y;
            double x = Math.Sqrt(ax * ax + ay * ay);
            double y = Math.Sqrt(bx * bx + by * by);
            double ang = 180.0 * Math.Acos((ax * bx + ay * by) / (x * y)) / Math.PI;
            if (B[2] < B[0]) ang = 360 - ang;
            return 360 - ang;
        }

        /// <summary>
        /// 绘制沿途坐标点
        /// </summary>
        /// <param name="image"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private static void LongLine(Mat image, Point start, Point end)
        {
            // 起点和终点
            Cv2.Circle(image, start, 4, new Scalar(0, 255, 0), 2);
            Cv2.Circle(image, end, 4, new Scalar(0, 0, 255), 2);

            // 垂直基线
            Vec4i BaseLine = new Vec4i(image.Width / 2, image.Height / 2, image.Width / 2, image.Height / 2 + 300);
            // 需要求角度的线
            Vec4i EndLine = new Vec4i(start.X, start.Y, end.X, end.Y);
            // 计算夹角
            double Ang = VectorLine(BaseLine, EndLine);
            int difX = end.X - start.X;

            // 沿X轴扫描
            if (((45.0 <= Ang && Ang <= 135.0) || (225.0 <= Ang && Ang <= 315.0)) && difX != 0)
            {
                decimal K = (decimal)(end.Y - start.Y) / (decimal)difX;
                int x = start.X;
                while (x > 0 && x < image.Width)
                {
                    x = start.X > end.X ? x - 1 : x + 1;
                    int y = difX == 0 ? start.Y : (int)(K * (x - start.X) + start.Y);
                    if (y <= 0 || y >= image.Height || x <= 0 || x >= image.Width)
                        break;
                    // 像素点间隔绘制，提高绘制速度
                    if (x % 2 == 0)
                        continue;
                    Cv2.Circle(image, new Point(x, y), 1, new Scalar(255, 255, 255), -1);
                }
            }
            // 沿Y轴扫描
            else
            {
                int y = start.Y;
                decimal K = difX == 0 ? 0 : (decimal)(end.Y - start.Y) / (decimal)difX;
                while (y > 0 && y < image.Height)
                {
                    y = start.Y > end.Y ? y - 1 : y + 1;
                    int x = K == 0 ? start.X : (int)((y - start.Y) / K + start.X);
                    if (y <= 0 || y >= image.Height || x <= 0 || x >= image.Width)
                        break;

                    // 像素点间隔绘制，提高绘制速度
                    if (y % 2 == 0)
                        continue;
                    Cv2.Circle(image, new Point(x, y), 1, new Scalar(0, 255, 255), -1);
                }
            }
            Mat endImg = new Mat();
            Cv2.Resize(image, endImg, new Size(image.Cols * 0.5, image.Rows * 0.5), 0, 0, InterpolationFlags.Area);
            Cv2.ImShow("image", endImg);
            Cv2.WaitKey(1);
        }

        static void Main(string[] args)
        {
            Mat image = new Mat(new Size(800, 800), MatType.CV_8UC3, new Scalar(0, 0, 0));
            Point start = new Point(image.Width / 2, image.Height / 2);

            // 循环目标坐标
            for (int i = 200; i < 600; i++)
            {
                Point end1 = new Point(i, 200);
                LongLine(image, start, end1);

                Point end2 = new Point(i, 600);
                LongLine(image, start, end2);

                Point end3 = new Point(200, i);
                LongLine(image, start, end3);

                Point end4 = new Point(600, i);
                LongLine(image, start, end4);
            }
        }
    }
}

/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat org = Cv2.ImRead("../../../images/cards.png");

            Mat imgray = new Mat();
            Cv2.CvtColor(org, imgray, ColorConversionCodes.BGR2GRAY);
            Cv2.ImShow("imgray", imgray);

            // 白色背景
            Mat threshold = new Mat();
            Cv2.Threshold(imgray, threshold, 244, 255, ThresholdTypes.BinaryInv); // 把黑白颜色反转
            Cv2.ImShow("after threshold", threshold);

            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(threshold, out contours, out hierarcy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

            List<(int, double)> areas = new List<(int, double)>();
            for (int i = 0; i < contours.Length; i++)
            {
                areas.Add((i, Cv2.ContourArea(contours[i]))); // 面积大小
            }
            // 按面积大小，从大到小排序
            var a2 = areas.OrderByDescending(x => x.Item2).ToList();
            foreach (var area in a2)
            {
                if (area.Item2 < 150)
                    continue;

                Mat img22 = new Mat();
                org.CopyTo(img22); //逐个contour 显示
                Cv2.DrawContours(img22, contours, area.Item1, new Scalar(0, 0, 255), 3);
                Console.WriteLine(area);
                Cv2.ImShow("drawContours", img22);
                img22.Dispose(); // C# 中这里一定要释放内存，Mat库默认不会释放

                if (Cv2.WaitKey(200) == 'q')
                    break;
            }

            // 获取最大或某个contour，剪切
            int idx = a2[1].Item1;
            // Create mask where white is what we want, black otherwise
            Mat mask = new Mat(org.Size(), org.Type(), new Scalar(0, 0, 0));
            // Draw filled contour in mask
            Cv2.DrawContours(mask, contours, idx, new Scalar(0, 255, 0), -1);

            // Extract out the object and place into output image
            Mat matout = new Mat(org.Size(), org.Type(), new Scalar(0, 0, 0));
            org.CopyTo(matout, mask);
            Cv2.ImShow("out_contour.jpg", matout);

            //roi方法
            idx = a2[4].Item1;
            Rect rect = Cv2.BoundingRect(contours[idx]);
            Mat matroi = new Mat(org, rect);
            Cv2.ImShow("out_contour-roi4.jpg", matroi);
            Cv2.WaitKey(0);
        }
    }
}

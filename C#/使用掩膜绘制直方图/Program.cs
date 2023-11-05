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
        /// <summary>
        /// 绘制直方图
        /// </summary>
        /// <param name="histImage">直方图绘制结果</param>
        /// <param name="histSize">直方图数组大小</param>
        /// <param name="color">线的颜色</param>
        static void DrawHist(Mat histImage, Mat hist, Scalar color)
        {
            var binW = Math.Round((double)histImage.Width / hist.Height);
            //归一化
            Cv2.Normalize(hist, hist, 0, histImage.Rows, NormTypes.MinMax, -1);
            for (int i = 1; i < hist.Height; i++)
            {
                var pt1 = new Point2d(binW * (i - 1), histImage.Height - Math.Round(hist.At<float>(i - 1)));
                var pt2 = new Point2d(binW * (i), histImage.Height - Math.Round(hist.At<float>(i)));
                Cv2.Line(histImage, (Point)pt1, (Point)pt2, color, 1, LineTypes.AntiAlias);
            }
        }

        static void Main(string[] args)
        {
            Mat img = Cv2.ImRead("../../../images/home3.jpg", 0);
            Mat mask = new Mat(img.Size(), MatType.CV_8UC1, new Scalar(0, 0, 0));
            mask.SubMat(100, 300, 100, 400).SetTo(new Scalar(255, 255, 255));

            Mat masked_img = new Mat();
            Cv2.BitwiseAnd(img, img, masked_img, mask);

            // Calculate histogram with mask and without mask
            // Check third argument for mask
            int[] channels = { 0 }; // 通道索引
            int[] histSize = { 256 }; // 直方图中的条目数
            Rangef[] ranges = { new Rangef(0, 256) }; // 像素值范围

            Mat hist_full = new Mat();
            Mat hist_mask = new Mat();
            Cv2.CalcHist(new Mat[] { img }, channels, null, hist_full, 1, histSize, ranges);
            Cv2.CalcHist(new Mat[] { img }, channels, mask, hist_mask, 1, histSize, ranges);

            Mat hist = new Mat(img.Size(), MatType.CV_8UC3, new Scalar(255, 255, 255));
            DrawHist(hist, hist_full, Scalar.Red);
            DrawHist(hist, hist_mask, Scalar.Blue);

            Cv2.ImShow("img", img);
            Cv2.ImShow("mask", mask);
            Cv2.ImShow("masked_img", masked_img);
            Cv2.ImShow("hist", hist);
            Cv2.WaitKey(0);
        }
    }
}

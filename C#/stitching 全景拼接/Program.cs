/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：stitching 全景拼接
博客：http://www.bilibili996.com/Course?id=3291377000366
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System.Collections.Generic;
using System;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //stitch1();
            stitch2();
            Cv2.WaitKey(0);
        }

        /// <summary>
        /// 全景拼接，去掉黑边
        /// </summary>
        private static void stitch1()
        {
            List<Mat> imgs = new List<Mat>();
            Mat img1 = Cv2.ImRead("../../../images/stitching1.jpg");
            Mat img2 = Cv2.ImRead("../../../images/stitching2.jpg");
            Mat img3 = Cv2.ImRead("../../../images/stitching3.jpg");
            imgs.Add(img1);
            imgs.Add(img2);
            imgs.Add(img3);
            Mat pano = new Mat();
            Stitcher stitcher = Stitcher.Create();
            Stitcher.Status status = stitcher.Stitch(imgs, pano);
            if (status != Stitcher.Status.OK)
            {
                Console.WriteLine($"Can't stitch images, error code = {status}");
                Console.Read();
                return;
            }

            // 全景图轮廓提取
            Mat stitched = new Mat();
            Mat gray = new Mat();
            Mat thresh = new Mat();
            Cv2.CopyMakeBorder(pano, stitched, 10, 10, 10, 10, BorderTypes.Constant, new Scalar(0, 0, 0));
            Cv2.CvtColor(stitched, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, thresh, 0, 255, ThresholdTypes.Binary);

            //定义轮廓和层次结构
            Point[][] cnts = new Point[][] { };
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(thresh, out cnts, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple, null);

            // 创建单通道黑色图像, 轮廓最小正矩形
            Mat mask = new Mat(thresh.Size(), MatType.CV_8U, new Scalar(0, 0, 0));

            // 取出list中的轮廓二值图
            Rect boundRect = Cv2.BoundingRect(cnts[0]);

            //绘制矩形
            Cv2.Rectangle(mask, boundRect.TopLeft, boundRect.BottomRight, new Scalar(255, 255, 255), -1);

            // 腐蚀处理，直到minRect的像素值都为0
            Mat minRect = new Mat();
            Mat sub = new Mat();

            mask.CopyTo(minRect);
            mask.CopyTo(sub);
            while (Cv2.CountNonZero(sub) > 0)
            {
                // 行腐蚀操作 
                Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
                Cv2.Erode(minRect, minRect, element);
                Cv2.Subtract(minRect, thresh, sub);
            }

            // 提取minRect轮廓并裁剪
            Cv2.FindContours(minRect, out cnts, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple, new Point(0, 0));
            boundRect = Cv2.BoundingRect(cnts[0]);
            stitched = new Mat(stitched, boundRect);

            Cv2.ImShow("result1", stitched);
        }

        /// <summary>
        /// 全景拼接，没去黑边
        /// </summary>
        private static void stitch2()
        {
            List<Mat> imgs = new List<Mat>();
            Mat img1 = Cv2.ImRead("../../../images/stitching1.jpg");
            Mat img2 = Cv2.ImRead("../../../images/stitching2.jpg");
            Mat img3 = Cv2.ImRead("../../../images/stitching3.jpg");
            imgs.Add(img1);
            imgs.Add(img2);
            imgs.Add(img3);
            Mat pano = new Mat();
            Stitcher stitcher = Stitcher.Create();
            Stitcher.Status status = stitcher.Stitch(imgs, pano);
            if (status != Stitcher.Status.OK)
            {
                Console.WriteLine($"Can't stitch images, error code = {status}");
                Console.Read();
                return;
            }
            Cv2.ImShow("result2", pano);
        }
    }
}

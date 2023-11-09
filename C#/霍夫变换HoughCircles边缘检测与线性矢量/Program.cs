﻿/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：霍夫变换HoughCircles边缘检测与线性矢量
博客：http://www.bilibili996.com/Course?id=4885001000177
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //【1】载入原始图和Mat变量定义   
            Mat srcImage = Cv2.ImRead("../../../images/HoughCircles.jpg");

            //临时变量和目标图的定义
            Mat midImage = new Mat();

            //【2】显示原始图  
            Cv2.ImShow("【原始图】", srcImage);

            //【3】转为灰度图并进行图像平滑
            // 转化边缘检测后的图为灰度图
            Cv2.CvtColor(srcImage, midImage, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(midImage, midImage, new Size(9, 9), 2, 2);

            //【4】进行霍夫圆变换
            // opencv3：HoughMethods.Gradient
            // opencv4：HoughModes.Gradient
            CircleSegment[] circles = Cv2.HoughCircles(midImage, HoughModes.Gradient, 1.5, 10, 200, 100, 0, 0);

            //【5】依次在图中绘制出圆
            for (int i = 0; i < circles.Length; i++)
            {
                //参数定义
                Point2f center = circles[i].Center;
                float radius = circles[i].Radius;
                //绘制圆心
                Cv2.Circle(srcImage, (int)center.X, (int)center.Y, 5, new Scalar(0, 255, 0), -1, LineTypes.Link8, 0);
                //绘制圆轮廓
                Cv2.Circle(srcImage, (int)center.X, (int)center.Y, (int)radius, new Scalar(155, 50, 255), 5, LineTypes.Link8, 0);
            }

            //【6】显示效果图  
            // Cv2.Resize(srcImage, srcImage, new Size(srcImage.Cols * 0.5, srcImage.Rows * 0.5), 0, 0, InterpolationFlags.Area);
            Cv2.ImShow("【C# 效果图】", srcImage);

            Cv2.WaitKey(0);
        }
    }
}

/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //【1】载入原始图和Mat变量定义   
            Mat srcImage = Cv2.ImRead("../../../images/home.jpg");

            //临时变量和目标图的定义
            Mat midImage = new Mat();
            Mat dstImage = new Mat();

            //【2】进行边缘检测和转化为灰度图
            //进行一此canny边缘检测
            Cv2.Canny(srcImage, midImage, 50, 200, 3);
            //转化边缘检测后的图为灰度图
            Cv2.CvtColor(midImage, dstImage, ColorConversionCodes.GRAY2BGR);

            //【3】进行霍夫线变换
            //定义一个矢量结构lines用于存放得到的线段矢量集合
            LineSegmentPoint[] lines = Cv2.HoughLinesP(midImage, 1, Cv2.PI / 180, 80, 50, 10);

            //【4】依次在图中绘制出每条线段
            for (int i = 0; i < lines.Length; i++)
            {
                Point p1 = lines[i].P1;
                Point p2 = lines[i].P2;
                Cv2.Line(dstImage, p1, p2, new Scalar(186, 88, 255), 2, LineTypes.AntiAlias);
            }

            //【5】显示原始图  
            Cv2.ImShow("【原始图】", srcImage);

            //【6】边缘检测后的图 
            Cv2.ImShow("【边缘检测后的图】", midImage);

            //【7】显示效果图  
            Cv2.ImShow("【C# 效果图】", dstImage);

            Cv2.WaitKey(0);
        }
    }
}

/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：直方图对比
博客：http://www.bilibili996.com/Course?id=3750176000216
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
            // 【1】声明储存基准图像和另外两张对比图像的矩阵( RGB 和 HSV )
            Mat srcImage_base = Cv2.ImRead("../../../images/book1.jpg");
            Mat srcImage_test1 = Cv2.ImRead("../../../images/book2.jpg");
            Mat srcImage_test2 = Cv2.ImRead("../../../images/book3.jpg");
            //显示载入的3张图像
            Cv2.ImShow("基准图像", srcImage_base);
            Cv2.ImShow("测试图像1", srcImage_test1);
            Cv2.ImShow("测试图像2", srcImage_test2);

            // 【3】将图像由BGR色彩空间转换到 HSV色彩空间
            Mat hsvImage_base = new Mat();
            Mat hsvImage_test1 = new Mat();
            Mat hsvImage_test2 = new Mat();
            Cv2.CvtColor(srcImage_base, hsvImage_base, ColorConversionCodes.BGR2HSV);
            Cv2.CvtColor(srcImage_test1, hsvImage_test1, ColorConversionCodes.BGR2HSV);
            Cv2.CvtColor(srcImage_test2, hsvImage_test2, ColorConversionCodes.BGR2HSV);

            //【4】创建包含基准图像下半部的半身图像(HSV格式)
            Rect faces = new Rect()
            {
                X = 0,
                Y = hsvImage_base.Rows / 2,
                Width = hsvImage_base.Cols,
                Height = hsvImage_base.Rows / 2,
            };
            Mat hsvImage_halfDown = new Mat(hsvImage_base, faces);

            //【5】初始化计算直方图需要的实参
            // 对hue通道使用30个bin,对saturatoin通道使用32个bin
            int h_bins = 50;
            int s_bins = 60;
            int[] histSize = { h_bins, s_bins };
            // hue的取值范围从0到256, saturation取值范围从0到180
            Rangef h_ranges = new Rangef(0, 256);
            Rangef s_ranges = new Rangef(0, 180);
            Rangef[] ranges = { h_ranges, s_ranges };
            // 使用第0和第1通道
            int[] channels = { 0, 1 };

            // 【6】创建储存直方图的 MatND 类的实例:
            Mat baseHist = new Mat();
            Mat halfDownHist = new Mat();
            Mat testHist1 = new Mat();
            Mat testHist2 = new Mat();

            // 【7】计算基准图像，两张测试图像，半身基准图像的HSV直方图:
            Cv2.CalcHist(new Mat[] { hsvImage_base }, channels, null, baseHist, 2, histSize, ranges, true, false);
            Cv2.Normalize(baseHist, baseHist, 0, 1, NormTypes.MinMax, -1, null);

            Cv2.CalcHist(new Mat[] { hsvImage_halfDown }, channels, null, halfDownHist, 2, histSize, ranges, true, false);
            Cv2.Normalize(halfDownHist, halfDownHist, 0, 1, NormTypes.MinMax, -1, null);

            Cv2.CalcHist(new Mat[] { hsvImage_test1 }, channels, null, testHist1, 2, histSize, ranges, true, false);
            Cv2.Normalize(testHist1, testHist1, 0, 1, NormTypes.MinMax, -1, null);

            Cv2.CalcHist(new Mat[] { hsvImage_test2 }, channels, null, testHist2, 2, histSize, ranges, true, false);
            Cv2.Normalize(testHist2, testHist2, 0, 1, NormTypes.MinMax, -1, null);

            //【8】按顺序使用4种对比标准将基准图像的直方图与其余各直方图进行对比:
            for (int i = 0; i < 4; i++)
            {
                //进行图像直方图的对比
                HistCompMethods compare_method = (HistCompMethods)i;
                double base_base = Cv2.CompareHist(baseHist, baseHist, compare_method);
                double base_half = Cv2.CompareHist(baseHist, halfDownHist, compare_method);
                double base_test1 = Cv2.CompareHist(baseHist, testHist1, compare_method);
                double base_test2 = Cv2.CompareHist(baseHist, testHist2, compare_method);

                //输出结果
                Console.WriteLine($"方法[{i}]的匹配结果如下");
                Console.WriteLine($"【基准图 - 基准图】：{base_base}");
                Console.WriteLine($"【基准图 - 半身图】：{base_half}");
                Console.WriteLine($"【基准图 - 测试图1】：{base_test1}");
                Console.WriteLine($"【基准图 - 测试图2】：{base_test2}\r\n");
            }
            Console.WriteLine("检测结束。");
            Cv2.WaitKey(0);
        }
    }
}

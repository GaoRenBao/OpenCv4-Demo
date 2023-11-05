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
        static string WINDOW_NAME = "【亚像素级角点检测】";

        static Mat g_srcImage = new Mat();
        static Mat g_grayImage = new Mat();
        static int g_maxCornerNumber = 33;
        static int g_maxTrackbarNumber = 500;
        static RNG rng = new RNG(12345);

        static void Main(string[] args)
        {
            //【1】载入源图像并将其转换为灰度图
            g_srcImage = Cv2.ImRead("../../../images/home7.jpg");
            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);

            //【2】创建窗口和滑动条，并进行显示和回调函数初始化
            Cv2.NamedWindow(WINDOW_NAME, WindowFlags.AutoSize);
            Cv2.CreateTrackbar("最大角点数:", WINDOW_NAME, ref g_maxCornerNumber, g_maxTrackbarNumber, on_GoodFeaturesToTrack);
            on_GoodFeaturesToTrack(0, IntPtr.Zero);

            Cv2.ImShow(WINDOW_NAME, g_srcImage);
            Cv2.WaitKey();
        }

        static void on_GoodFeaturesToTrack(int pos, IntPtr userData)
        {
            //【1】对变量小于等于1时的处理
            if (g_maxCornerNumber <= 1) { g_maxCornerNumber = 1; }

            //【2】Shi-Tomasi算法（goodFeaturesToTrack函数）的参数准备
            double qualityLevel = 0.01;//角点检测可接受的最小特征值
            double minDistance = 10;//角点之间的最小距离
            int blockSize = 3;//计算导数自相关矩阵时指定的邻域范围
            double k = 0.04;//权重系数

            Mat copy = new Mat();
            g_srcImage.CopyTo(copy);  //复制源图像到一个临时变量中，作为感兴趣区域

            //【3】进行Shi-Tomasi角点检测
            Point2f[] corners = Cv2.GoodFeaturesToTrack(
                g_grayImage,//输入图像
                g_maxCornerNumber, //角点的最大数量
                qualityLevel,//角点检测可接受的最小特征值
                minDistance,//角点之间的最小距离
                null,
                blockSize,//计算导数自相关矩阵时指定的邻域范围
                false,
                k);//权重系数

            //【4】输出文字信息
            System.Diagnostics.Debug.WriteLine($"此次检测到的角点数量为：{corners.Length}");

            //【5】绘制检测到的角点
            int r = 4;
            for (int i = 0; i < corners.Length; i++)
            {
                //以随机的颜色绘制出角点
                byte cb = (byte)rng.Uniform(0, 255);
                byte cg = (byte)rng.Uniform(0, 255);
                byte cr = (byte)rng.Uniform(0, 255);
                Cv2.Circle(copy, (int)corners[i].X, (int)corners[i].Y, r, new Scalar(cb, cg, cr), -1, LineTypes.Link8, 0);
            }

            //【6】显示（更新）窗口
            Cv2.ImShow(WINDOW_NAME, copy);
            copy.Dispose();

            //【7】亚像素角点检测的参数设置
            Size winSize = new Size(5, 5);
            Size zeroZone = new Size(-1, -1);
            TermCriteria criteria = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 40, 0.001);

            //【8】计算出亚像素角点位置
            Cv2.CornerSubPix(g_grayImage, corners, winSize, zeroZone, criteria);

            //【9】输出角点信息
            for (int i = 0; i < corners.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine($"精确角点坐标：{corners[i].X},{corners[i].Y}");
            }
        }
    }
}

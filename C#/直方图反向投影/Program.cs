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
        static string WINDOW_NAME1 = "【原始图】";
        static Mat g_srcImage = new Mat();
        static Mat g_hsvImage = new Mat();
        static Mat g_hueImage = new Mat();
        static int g_bins = 30;//直方图组距

        static void Main(string[] args)
        {
            //【1】读取源图像，并转换到 HSV 空间
            g_srcImage = Cv2.ImRead("../../../images/hand.jpg");
            if (g_srcImage.Empty())
            {
                Console.WriteLine("读取图片错误，请确定目录下是否有imread函数指定图片存在~！");
                return;
            }

            //【2】得到原图的灰度图像并进行平滑
            g_hsvImage = new Mat();
            Cv2.CvtColor(g_srcImage, g_hsvImage, ColorConversionCodes.BGR2HSV);

            //【2】分离 Hue 色调通道
            g_hueImage = Mat.Zeros(g_hsvImage.Size(), g_hsvImage.Type());
            int[] ch = { 0, 0 };
            Mat[] input = { g_hsvImage };
            Mat[] output = { g_hueImage };
            Cv2.MixChannels(input, output, ch);

            //【3】创建 Trackbar 来输入bin的数目
            // opencv3：WindowMode.AutoSize
            // opencv4：WindowFlags.AutoSize
            Cv2.NamedWindow(WINDOW_NAME1, WindowFlags.AutoSize);
            Cv2.CreateTrackbar("色调组距:", WINDOW_NAME1, ref g_bins, 180, on_BinChange);
            on_BinChange(0, IntPtr.Zero);
            Cv2.WaitKey(0);
        }

        // opencv3：on_BinChange(int pos, object userData)
        // opencv4：on_BinChange(int pos, IntPtr userData)
        private static void on_BinChange(int pos, IntPtr userData)
        {
            int[] channels = { 0 };
            Mat hist = new Mat();
            int[] histSize = { Math.Max(g_bins, 2) };
            Rangef hue_range = new Rangef(0, 180);
            Rangef[] ranges = { hue_range };

            //【2】计算直方图并归一化
            Cv2.CalcHist(new Mat[] { g_hueImage },//输入的数组
                channels, // 通道索引
                null,     // 不使用掩膜
                hist,  // 输出的目标直方图
                1,        // 需要计算的直方图的维度为2
                histSize,     // 存放每个维度的直方图尺寸的数组
                ranges,   // 每一维数值的取值范围数组
                true,     // 指示直方图是否均匀的标识符，true表示均匀的直方图
                false);   // 累计标识符，false表示直方图在配置阶段会被清零

            Cv2.Normalize(hist, hist, 0, 255, NormTypes.MinMax, -1);

            //【3】计算反向投影
            Mat backproj = new Mat();
            Cv2.CalcBackProject(new Mat[] { g_hueImage }, channels, hist, backproj, ranges, true);

            //【4】显示反向投影
            Cv2.ImShow("反向投影图", backproj);

            //【5】绘制直方图的参数准备
            int w = 400; int h = 400;
            int bin_w = (int)Math.Round((double)w / histSize[0]);

            Mat histImg = new Mat(new Size(w, h), MatType.CV_8UC3);
            histImg.SetIdentity(new Scalar(0));

            //【6】绘制直方图
            for (int i = 0; i < g_bins; i++)
            {
                Rect rect = new Rect()
                {
                    X = Math.Min(i * bin_w, (i + 1) * bin_w),
                    Y = (int)Math.Min(h, h - Math.Round(hist.At<float>(i) * h / 255.0)),
                    Height = Math.Abs((int)Math.Round(hist.At<float>(i) * h / 255.0)),
                    Width = bin_w
                };
                Cv2.Rectangle(histImg, rect, new Scalar(100, 123, 255), -1);
            }

            //【7】显示直方图窗口
            Cv2.ImShow("直方图", histImg);
        }
    }
}

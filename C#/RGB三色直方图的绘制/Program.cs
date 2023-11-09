/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：RGB三色直方图的绘制
博客：http://www.bilibili996.com/Course?id=2488638000215
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
            // 【1】载入素材图并显示
            Mat srcImage = Cv2.ImRead("../../../images/6919.jpg");
            if (srcImage.Empty())
            {
                Console.WriteLine("Could not open or find the image!");
                return;
            }
            Cv2.ImShow("素材图", srcImage);

            //【2】参数准备
            int bins = 256;
            int[] hist_size = { bins };
            Rangef range = new Rangef(0, 256);
            Rangef[] ranges = { range };
            Mat redHist = new Mat();
            Mat grayHist = new Mat();
            Mat blueHist = new Mat();

            int[] channels_r = { 0 };
            //【3】进行直方图的计算（红色分量部分）
            Cv2.CalcHist(new Mat[] { srcImage },//输入的数组
                channels_r, // 通道索引
                null,     // 不使用掩膜
                redHist,  // 输出的目标直方图
                1,        // 需要计算的直方图的维度为2
                hist_size, // 存放每个维度的直方图尺寸的数组
                ranges,   // 每一维数值的取值范围数组
                true,     // 指示直方图是否均匀的标识符，true表示均匀的直方图
                false);   // 累计标识符，false表示直方图在配置阶段会被清零

            int[] channels_g = { 1 };
            //【4】进行直方图的计算（绿色分量部分）
            Cv2.CalcHist(new Mat[] { srcImage },//输入的数组
                channels_g, // 通道索引
                null,      // 不使用掩膜
                grayHist,  // 输出的目标直方图
                1,        // 需要计算的直方图的维度为2
                hist_size,// 存放每个维度的直方图尺寸的数组
                ranges,   // 每一维数值的取值范围数组
                true,     // 指示直方图是否均匀的标识符，true表示均匀的直方图
                false);   // 累计标识符，false表示直方图在配置阶段会被清零

            int[] channels_b = { 2 };
            //【5】进行直方图的计算（蓝色分量部分）
            Cv2.CalcHist(new Mat[] { srcImage },//输入的数组
                channels_b, // 通道索引
                null,     // 不使用掩膜
                blueHist,  // 输出的目标直方图
                1,        // 需要计算的直方图的维度为2
                hist_size,// 存放每个维度的直方图尺寸的数组
                ranges,   // 每一维数值的取值范围数组
                true,     // 指示直方图是否均匀的标识符，true表示均匀的直方图
                false);   // 累计标识符，false表示直方图在配置阶段会被清零

            //-----------------------绘制出三色直方图------------------------

            //参数准备
            double minValue_red, minValue_green, minValue_blue;
            double maxValue_red, maxValue_green, maxValue_blue;

            Cv2.MinMaxLoc(redHist, out minValue_red, out maxValue_red);
            Cv2.MinMaxLoc(grayHist, out minValue_green, out maxValue_green);
            Cv2.MinMaxLoc(blueHist, out minValue_blue, out maxValue_blue);
            int scale = 1;
            int histHeight = 256;
            Mat histImage = new Mat(histHeight, bins * 3, MatType.CV_8UC3);
            histImage.SetIdentity(new Scalar(0));

            //正式开始绘制
            for (int i = 0; i < bins; i++)
            {
                //参数准备
                float binValue_red = redHist.At<float>(i);
                float binValue_green = grayHist.At<float>(i);
                float binValue_blue = blueHist.At<float>(i);
                int intensity_red = (int)Math.Round(binValue_red * histHeight / maxValue_red);  //要绘制的高度
                int intensity_green = (int)Math.Round(binValue_green * histHeight / maxValue_green);  //要绘制的高度
                int intensity_blue = (int)Math.Round(binValue_blue * histHeight / maxValue_blue);  //要绘制的高度

                //绘制红色分量的直方图
                int a1 = histHeight - intensity_red;
                int b1 = histHeight - 1;
                int min1 = a1 > b1 ? b1 : a1;
                int max1 = a1 > b1 ? a1 : b1;
                Cv2.Rectangle(histImage, new Point(i * scale, min1),
                    new Point((i + 1) * scale - 1, max1),
                    new Scalar(255, 0, 0));

                //绘制绿色分量的直方图
                int a2 = histHeight - intensity_green;
                int b2 = histHeight - 1;
                int min2 = a2 > b2 ? b2 : a2;
                int max2 = a2 > b2 ? a2 : b2;
                Cv2.Rectangle(histImage, new Point((i + bins) * scale, min2),
                    new Point((i + bins + 1) * scale - 1, max2),
                    new Scalar(0, 255, 0));

                //绘制蓝色分量的直方图
                int a3 = histHeight - intensity_blue;
                int b3 = histHeight - 1;
                int min3 = a3 > b3 ? b3 : a3;
                int max3 = a3 > b3 ? a3 : b3;
                Cv2.Rectangle(histImage, new Point((i + bins * 2) * scale, min3),
                    new Point((i + bins * 2 + 1) * scale - 1, max3),
                    new Scalar(0, 0, 255));
            }

            //在窗口中显示出绘制好的直方图
            Cv2.ImShow("图像的RGB直方图", histImage);
            Cv2.WaitKey(0);
        }
    }
}

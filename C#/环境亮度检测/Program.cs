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
        /// 视频操作
        /// </summary>
        public static VideoCapture Cap = new VideoCapture();

        // 环境亮度值计算
        public static double BrightnessDetection(Mat srcImage)
        {
            Mat dstImage = new Mat();
            Cv2.CvtColor(srcImage, dstImage, ColorConversionCodes.BGR2GRAY);

            // bgr保存了所有0~255个像素值的数量
            int[] bgr = new int[256];
            for (int i = 0; i < bgr.Length; i++) bgr[i] = 0;
            // a为所有亮度值的和
            double a = 0;
            // num 为像素点个数
            int num = 0;
            for (int i = 0; i < dstImage.Rows; i++)
            {
                for (int j = 0; j < dstImage.Cols; j++)
                {
                    // 剔除最大和最小值
                    if (dstImage.At<byte>(i, j) == 0) continue;
                    if (dstImage.At<byte>(i, j) == 255) continue;
                    num++;
                    //在计算过程中，考虑128为亮度均值点
                    a += (double)(dstImage.At<byte>(i, j) - 128.0);
                    int x = (int)dstImage.At<byte>(i, j);
                    bgr[x]++;
                }
            }
            // 计算像素点平均值
            double da = a / num;
            double D = Math.Abs(da);
            // 将平均值乘以每个亮度点的个数，得到整个图像的亮度值，起到均衡亮度值
            // 避免由于某个区域特别亮，导致最终计算结果偏差过大
            double Ma = 0;
            for (int i = 0; i < 256; i++)
            {
                Ma += Math.Abs(i - 128 - da) * bgr[i];
            }
            // 将整个图像的亮度值除以个数，最终得到每个像素点的亮度值
            Ma /= (double)(dstImage.Rows * dstImage.Cols);
            double M = Math.Abs(Ma) + 0.0001;
            double K = D / M;
            double cast = (da > 0) ? K : -K;
            return cast;
        }

        static void Main(string[] args)
        {
            Cap.Open(0);
            if (!Cap.IsOpened())
            {
                Console.WriteLine("摄像头打开失败.");
                Console.Read();
                return;
            }

            Cap.Set(VideoCaptureProperties.FrameWidth, 1280);
            Cap.Set(VideoCaptureProperties.FrameHeight, 720);

            Mat Image = new Mat();
            while (true)
            {
                if (Cap.Read(Image))
                {
                    double value = BrightnessDetection(Image);
                    Cv2.PutText(Image, $"Value:{value}", new Point(10, 30), HersheyFonts.HersheyComplex, 1, new Scalar(0, 0, 255), 2);
                    Cv2.ImShow("Image", Image);
                    Cv2.WaitKey(1);
                }
            }
        }
    }
}

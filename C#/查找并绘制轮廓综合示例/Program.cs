/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：查找并绘制轮廓综合示例
博客：http://www.bilibili996.com/Course?id=0573457000194
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace ConsoleApp
{
    internal class Program
    {
        static string WINDOW_NAME1 = "【原始图窗口】";//为窗口标题定义的宏 
        static string WINDOW_NAME2 = "【轮廓图】";//为窗口标题定义的宏 

        static Mat drawing = new Mat();
        static Mat g_srcImage = new Mat();
        static Mat g_grayImage = new Mat();
        static Mat g_cannyMat_output = new Mat();
        static int g_nThresh = 80;
        static int g_nThresh_max = 255;
        static Random g_rng = new Random();

        static void on_ThreshChange(int pos, IntPtr userData)
        {
#if true
            // 用Canny算子检测边缘
            Cv2.Canny(g_grayImage, g_cannyMat_output, g_nThresh, g_nThresh * 2, 3);

            // 寻找轮廓
            Point[][] g_vContours = new Point[][] { };
            HierarchyIndex[] g_vHierarchy;
            Cv2.FindContours(g_cannyMat_output, out g_vContours, out g_vHierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple, null);

            // 绘出轮廓
            drawing = new Mat(g_srcImage.Size(), MatType.CV_8UC3);
            for (int i = 0; i < g_vContours.Length; i++)
            {
                //随机生成bgr值
                int b = g_rng.Next(255);//随机返回一个0~255之间的值
                int g = g_rng.Next(255);//随机返回一个0~255之间的值
                int r = g_rng.Next(255);//随机返回一个0~255之间的值
                Cv2.DrawContours(drawing, g_vContours, i, new Scalar(b, g, r), 2, LineTypes.Link8, g_vHierarchy);
            }
#else
            g_grayImage.CopyTo(g_cannyMat_output);
            Cv2.Threshold(g_cannyMat_output, g_cannyMat_output, g_nThresh, 255, ThresholdTypes.Binary);

            // 寻找轮廓
            drawing = new Mat(g_cannyMat_output.Size(), MatType.CV_8UC3);
            Point[][] contours = new Point[][] { };
            HierarchyIndex[] hierarcy;
            Cv2.FindContours(g_cannyMat_output, out contours, out hierarcy, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple, null);

            // 绘出轮廓
            for (int index = 0; index < contours.Length; index++)
            {
                //随机生成bgr值
                int b = g_rng.Next(255);//随机返回一个0~255之间的值
                int g = g_rng.Next(255);//随机返回一个0~255之间的值
                int r = g_rng.Next(255);//随机返回一个0~255之间的值
                Cv2.DrawContours(drawing, contours, index, new Scalar(b, g, r), -1, LineTypes.Link8, hierarcy);
            }
#endif
            // 显示效果图
            Cv2.ImShow(WINDOW_NAME2, drawing);
        }

        static void Main(string[] args)
        {
            // 加载源图像
            g_srcImage = Cv2.ImRead("../../../images/flowers2.jpg");

            // 转成灰度
            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);

            // 创建窗口
            Cv2.NamedWindow(WINDOW_NAME1, WindowFlags.AutoSize);
            Cv2.ImShow(WINDOW_NAME1, g_srcImage);

            //创建滚动条并初始化
            Cv2.CreateTrackbar("canny阈值:", WINDOW_NAME1, ref g_nThresh, g_nThresh_max, on_ThreshChange);
          
            // 轮询等待用户按键，如果ESC键按下则退出程序
            while (true)
            {
                if ((char)Cv2.WaitKey(20) == 27)
                {
                    break;
                }
            }
        }
    }
}

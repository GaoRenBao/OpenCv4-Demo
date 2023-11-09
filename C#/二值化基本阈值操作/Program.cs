/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：二值化基本阈值操作
博客：http://www.bilibili996.com/Course?id=4215192000168
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        static string WINDOW_NAME = "【程序窗口】"; //为窗口标题定义的宏 
        static int g_nThresholdValue = 100;
        static int g_nThresholdType = 3;
        static Mat g_srcImage = new Mat();
        static Mat g_grayImage = new Mat();
        static Mat g_dstImage = new Mat();

        static void on_Threshold(int pos, IntPtr userData)
        {
            //调用阈值函数
            Cv2.Threshold(g_grayImage, g_dstImage, g_nThresholdValue, 255, (ThresholdTypes)g_nThresholdType);

            //更新效果图
            Cv2.ImShow(WINDOW_NAME, g_dstImage);
        }

        static void ShowHelpText()
        {
            Console.WriteLine($"\n\n\t\t\t   当前使用的OpenCV版本为：{Cv2.GetVersionString()}");
            Console.WriteLine("\n\n  ----------------------------------------------------------------------------\n");

            //输出一些帮助信息  
            Console.WriteLine("\n\t欢迎来到【基本阈值操作】示例程序~\n\n");
            Console.WriteLine("\n\t按键操作说明: \n\n");
            Console.WriteLine("\t\t键盘按键【ESC】- 退出程序\n");
            Console.WriteLine("\t\t滚动条模式0- 二进制阈值\n");
            Console.WriteLine("\t\t滚动条模式1- 反二进制阈值\n");
            Console.WriteLine("\t\t滚动条模式2- 截断阈值\n");
            Console.WriteLine("\t\t滚动条模式3- 反阈值化为0\n");
            Console.WriteLine("\t\t滚动条模式4- 阈值化为0\n");
        }

        static void Main(string[] args)
        {
            //【0】显示欢迎和帮助文字
            ShowHelpText();

            //【1】读入源图片
            g_srcImage = Cv2.ImRead("../../../images/lake.jpg");
            if (g_srcImage.Data == null)
            {
                Console.WriteLine("Oh，no，读取g_srcImage图片错误~！");
                Console.Read();
                return;
            }
            Cv2.ImShow("原始图", g_srcImage);

            //【2】存留一份原图的灰度图
            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);

            //【3】创建窗口并显示原始图
            Cv2.NamedWindow(WINDOW_NAME, WindowFlags.AutoSize);

            //【4】创建滑动条来控制阈值
            Cv2.CreateTrackbar("模式", WINDOW_NAME, ref g_nThresholdType, 4, on_Threshold);
            Cv2.CreateTrackbar("参数值", WINDOW_NAME, ref g_nThresholdValue, 255, on_Threshold);

            //【5】初始化自定义的阈值回调函数
            on_Threshold(0, new IntPtr());

            // 【6】轮询等待用户按键，如果ESC键按下则退出程序
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

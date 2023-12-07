/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：程序性能检测及优化
博客：http://www.bilibili996.com/Course?id=2284309000315
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
            demo1();
            demo2();
            Console.Read();
        }

        /// <summary>
        /// 使用getTickCount方法来计算程序运行时间。
        /// </summary>
        static void demo1()
        {
            Mat img1 = Cv2.ImRead("../../../images/ml.png");
            var e1 = Cv2.GetTickCount();
            for (int i = 5; i < 49; i += 2)
                Cv2.MedianBlur(img1, img1, i);

            var e2 = Cv2.GetTickCount();
            var t = (e2 - e1) / Cv2.GetTickFrequency();  //  时钟频率 或者 每秒钟的时钟数
            Console.WriteLine(t);
        }

        /// <summary>
        /// 使用函数cv2.useOptimized() 来查看优化是否开启了，使用函数cv2.setUseOptimized()来开启优化
        /// </summary>
        static void demo2()
        {
            Console.WriteLine(Cv2.UseOptimized());
            Cv2.SetUseOptimized(false);
            Console.WriteLine(Cv2.UseOptimized());
        }
    }
}

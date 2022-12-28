using OpenCvSharp;
using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat img1 = Cv2.ImRead("ml.png");
            var e1 = Cv2.GetTickCount();
            for (int i = 5; i < 49; i += 2)
                Cv2.MedianBlur(img1, img1, i);

            var e2 = Cv2.GetTickCount();
            var t = (e2 - e1) / Cv2.GetTickFrequency();  //  时钟频率 或者 每秒钟的时钟数
            Console.WriteLine(t);
            Console.Read();

            //Console.WriteLine(Cv2.UseOptimized());
            //Cv2.SetUseOptimized(false);
            //Console.WriteLine(Cv2.UseOptimized());
            //Console.Read();
        }
    }
}

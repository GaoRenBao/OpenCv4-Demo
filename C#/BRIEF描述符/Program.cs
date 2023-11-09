/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：BRIEF描述符
博客：http://www.bilibili996.com/Course?id=c0c16b9f18ad4b5da127eb7792333229
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
            Mat img = Cv2.ImRead("../../../images/blox.jpg", 0);

            // 创建BRIEF描述符提取器
            var brief = OpenCvSharp.XFeatures2D.BriefDescriptorExtractor.Create();

            // Initiate FAST detector
            var star = OpenCvSharp.XFeatures2D.StarDetector.Create();
            var kp = star.Detect(img);

            // compute the descriptors with BRIEF
            Mat des = new Mat();
            brief.Compute(img, ref kp, des);
            Console.WriteLine(brief.DescriptorSize);
            Console.WriteLine($"{des.Width},{des.Height}");
            Console.Read();
        }
    }
}

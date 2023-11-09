/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：角点检测的FAST算法（FAST特征检测器）
博客：http://www.bilibili996.com/Course?id=421a041d9cd54125a4d8f035ca7e2b24
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
            Mat img = Cv2.ImRead("../../../images/blox.jpg");

            // Initiate FAST object with default values
            FastFeatureDetector fast = FastFeatureDetector.Create();
            // find and draw the keypoints
            KeyPoint[] kp = fast.Detect(img);
            var img2 = new Mat();
            Cv2.DrawKeypoints(img, kp, img2, Scalar.FromRgb(0, 0, 255));
            // Print all default params
            Console.WriteLine($"Threshold: {fast.Threshold}");
            Console.WriteLine($"nonmaxSuppression: {fast.NonmaxSuppression}");
            Console.WriteLine($"neighborhood: {fast.GetType()}");
            Console.WriteLine($"Total Keypoints with nonmaxSuppression: {kp.Length}");
            Cv2.ImShow("fast_true.png", img2);

            // Disable nonmaxSuppression
            fast.NonmaxSuppression = false;
            kp = fast.Detect(img);
            Console.WriteLine($"Total Keypoints without nonmaxSuppression: {kp.Length}");


            var img3 = new Mat();
            Cv2.DrawKeypoints(img, kp, img3, Scalar.FromRgb(0, 0, 255));
            Cv2.ImShow("fast_false.png", img3);
            Cv2.WaitKey(0);

            // 第一幅图是使用了非最大值抑制的结果
            // 第二幅没有使用非最大值抑制。
        }
    }
}

/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：ORB
博客：http://www.bilibili996.com/Course?id=e64cab8fe66f4380ba8a7e18469f2d21
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat img = Cv2.ImRead("../../../images/blox.jpg", 0);

            // Initiate ORB detector
            var orb = ORB.Create();

            // find the keypoints with ORB
            var kp = orb.Detect(img);

            // compute the descriptors with ORB
            Mat des = new Mat();
            orb.Compute(img, ref kp, des);

            // draw only keypoints location,not size and orientation
            Mat img2 = new Mat();
            Cv2.DrawKeypoints(img, kp, img2, new Scalar(0, 255, 0), DrawMatchesFlags.Default);

            Cv2.ImShow("img", img2);
            Cv2.WaitKey(0);
        }
    }
}

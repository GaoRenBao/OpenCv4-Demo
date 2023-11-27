/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：立体图像中的深度地图
博客：http://www.bilibili996.com/Course?id=
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
            // read two input images as grayscale images
            Mat imgL = Cv2.ImRead("../../../images/tsukuba_l.png", 0);
            Mat imgR = Cv2.ImRead("../../../images/tsukuba_r.png", 0);

            // Initiate and StereoBM object
            StereoBM stereo = StereoBM.Create(0, 21);

            // compute the disparity map
            Mat disparity = new Mat();
            stereo.Compute(imgL, imgR, disparity);

            Cv2.ImShow("gray", disparity);
            Cv2.ImWrite("gray.png", disparity);
            Cv2.WaitKey(0);
        }
    }
}

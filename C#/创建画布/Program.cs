/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
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
            // 全黑.可以用在屏保
            Mat mat = new Mat(new Size(200, 200), MatType.CV_8UC3, new Scalar(0, 0, 0));
            Cv2.ImShow("black", mat);

            // 全白
            mat = new Mat(new Size(200, 200), MatType.CV_8UC3, new Scalar(255, 255, 255));
            Cv2.ImShow("white", mat);
            Cv2.WaitKey(0);
        }
    }
}

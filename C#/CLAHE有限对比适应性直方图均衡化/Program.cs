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
            Mat img = Cv2.ImRead("../../../images/tsukuba_l.png", 0);
            //create a CLAHE object (Arguments are optional)
            var clahe = Cv2.CreateCLAHE(2.0, new Size(8, 8));
            Mat cl1 = new Mat();
            clahe.Apply(img, cl1);
            Cv2.ImShow("clahe_2", cl1);
            Cv2.WaitKey(0);
        }
    }
}

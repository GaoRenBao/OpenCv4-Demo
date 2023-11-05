/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat img = Cv2.ImRead("../../../images/orange.jpg");
            // 设置需要裁剪的轮廓
            Rect rect = new Rect()
            {
                X = 100,
                Y = 100,
                Width = 200,
                Height = 200
            };
            // 裁剪
            Mat roi = new Mat(img, rect);
            Cv2.ImShow("img", img);
            Cv2.ImShow("roi.jpg", roi);
            Cv2.WaitKey(0);
        }
    }
}

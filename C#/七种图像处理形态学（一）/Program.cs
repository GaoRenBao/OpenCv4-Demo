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
            // 载入原图
            Mat srcImage = Cv2.ImRead("../../../images/dog.jpg");

            // 显示原图
            Cv2.ImShow("【原图】", srcImage);

            // 定义核大小
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(15, 15));

            // 进行腐蚀操作 
            Mat out1 = new Mat();
            Cv2.Erode(srcImage, out1, element);

            // 进行膨胀操作 
            Mat out2 = new Mat();
            Cv2.Dilate(srcImage, out2, element);

            // 显示效果图
            Cv2.ImShow("腐蚀【效果图】", out1);
            Cv2.ImShow("膨胀【效果图】", out2);

            Cv2.WaitKey(0);
        }
    }
}

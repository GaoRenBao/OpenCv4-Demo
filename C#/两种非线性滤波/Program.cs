/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：两种非线性滤波
博客：http://www.bilibili996.com/Course?id=5395388000126
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
            Mat image = Cv2.ImRead("../../../images/girl3.jpg");

            // 显示原图
            Cv2.ImShow("【原图】", image);

            //进行中值滤波操作
            Mat out1 = new Mat();
            Cv2.MedianBlur(image, out1, 7);

            //进行双边滤波操作
            Mat out2 = new Mat();
            Cv2.BilateralFilter(image, out2, 25, 25 * 2, 25 / 2);

            //显示效果图
            Cv2.ImShow("中值滤波【效果图】", out1);
            Cv2.ImShow("双边滤波【效果图】", out2);

            Cv2.WaitKey(0);
        }
    }
}

/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：LUT 图像灰度调整
博客：http://www.bilibili996.com/Course?id=5825870000239
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
            //查找表，数组的下标对应图片里面的灰度值
            //例如lutData[20]=0;表示灰度为20的像素其对应的值0.
            //可能这样说的不清楚仔细看下代码就清楚了。
            byte[] lutData = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                if (i <= 120) lutData[i] = 0;
                if (i > 120 && i <= 200) lutData[i] = 120;
                if (i > 200) lutData[i] = 255;
            }

            Mat a = Cv2.ImRead("../../../images/1050.jpg");
            Mat b = new Mat();
            Mat lut = new Mat(1, 256, MatType.CV_8UC1, lutData);
            Cv2.ImShow("a", a);
            Cv2.LUT(a, lut, b);
            //Cv2.LUT(a, lutData, b);
            Cv2.ImShow("b2", b);
            Cv2.WaitKey();
        }
    }
}

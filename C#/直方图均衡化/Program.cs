/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：直方图均衡化
博客：http://www.bilibili996.com/Course?id=0735601000188
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
            //【1】载入原始图
            Mat srcImage = Cv2.ImRead("../../../images/Astral.jpg");

            //【2】显示原始图  
            Cv2.CvtColor(srcImage, srcImage, ColorConversionCodes.BGR2GRAY);
            Cv2.ImShow("【原始图】", srcImage);

            // 【3】进行直方图均衡化
            Mat dstImage = new Mat();
            Cv2.EqualizeHist(srcImage, dstImage);

            //【5】显示效果图
            Cv2.ImShow("【C# 效果图】", dstImage);
            Cv2.WaitKey(0);
        }
    }
}

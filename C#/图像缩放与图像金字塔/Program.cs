/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：图像缩放与图像金字塔
博客：http://www.bilibili996.com/Course?id=5486413000167
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
            Mat dstImage1 = new Mat();
            Mat dstImage2 = new Mat();
            Mat dstImage3 = new Mat();
            Mat dstImage4 = new Mat();

            // 载入原图
            Mat srcImage = Cv2.ImRead("../../../images/dota_logo.jpg");

            //显示原始图  
            Cv2.ImShow("【原始图】", srcImage);

            // 使用resize进行尺寸调整操作
            Cv2.Resize(srcImage, dstImage1, new Size(srcImage.Cols * 0.8, srcImage.Rows * 0.8), 0, 0, InterpolationFlags.Area);
            Cv2.Resize(srcImage, dstImage2, new Size(srcImage.Cols * 1.2, srcImage.Rows * 1.2), 0, 0, InterpolationFlags.Area);

            // 进行向上取样操作
            Cv2.PyrUp(srcImage, dstImage3, new Size(srcImage.Cols * 2, srcImage.Rows * 2));

            //进行向下取样操作
            Cv2.PyrDown(srcImage, dstImage4, new Size(srcImage.Cols * 0.5, srcImage.Rows * 0.5));

            //显示效果图  
            Cv2.ImShow("【效果图1】", dstImage1);
            Cv2.ImShow("【效果图2】", dstImage2);
            Cv2.ImShow("【效果图3】", dstImage3);
            Cv2.ImShow("【效果图4】", dstImage4);

            Cv2.WaitKey(0);
        }
    }
}

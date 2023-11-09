/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：cornerHarris角点检测
博客：http://www.bilibili996.com/Course?id=0862817000219
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
            // 以灰度模式载入图像并显示
            Mat srcImage = Cv2.ImRead("../../../images/home4.jpg", 0);
            Cv2.ImShow("原始图", srcImage);

            // 进行Harris角点检测找出角点
            // OpenCvSharp3-AnyCPU 版本找不到 CornerHarris 方法.
            // 必须采用 OpenCvSharp4 版本才行。
            Mat cornerStrength = new Mat();
            Cv2.CornerHarris(srcImage, cornerStrength, 2, 3, 0.01);

            // 对灰度图进行阈值操作，得到二值图并显示  
            Mat harrisCorner = new Mat();
            Cv2.Threshold(cornerStrength, harrisCorner, 0.00001, 255, ThresholdTypes.Binary);
            Cv2.ImShow("角点检测后的二值效果图", harrisCorner);

            Cv2.WaitKey(0);
        }
    }
}

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
        /// <summary>
        /// 颜色识别(除红色外的其他单色)
        /// </summary>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        static Mat ColorFindContours(Mat srcImage,
            int iLowH, int iHighH,
            int iLowS, int iHighS,
            int iLowV, int iHighV)
        {
            Mat bufImg = new Mat();
            Mat imgHSV = new Mat();
            //转为HSV
            Cv2.CvtColor(srcImage, imgHSV, ColorConversionCodes.BGR2HSV);
            Cv2.InRange(imgHSV, new Scalar(iLowH, iLowS, iLowV), new Scalar(iHighH, iHighS, iHighV), bufImg);
            return bufImg;
        }

        /// <summary>
        /// 颜色识别（红色）
        /// </summary>
        /// <param name="srcImage"></param>
        /// <returns></returns>
        static Mat ColorFindContours2(Mat srcImage)
        {
            Mat des1 = ColorFindContours(srcImage,
              350 / 2, 360 / 2,         // 色调最小值~最大值
              (int)(255 * 0.70), 255,   // 饱和度最小值~最大值
              (int)(255 * 0.60), 255);  // 亮度最小值~最大值

            Mat des2 = ColorFindContours(srcImage,
               0, 16 / 2,               // 色调最小值~最大值
              (int)(255 * 0.70), 255,   // 饱和度最小值~最大值
              (int)(255 * 0.60), 255);  // 亮度最小值~最大值

            return des1 + des2;
        }

        static void demo1()
        {
            Mat g_srcImage = Cv2.ImRead("../../../images/color.jpg");
            Cv2.ImShow("原始图1", g_srcImage);

            Mat des = ColorFindContours(g_srcImage,
                45 / 2, 60 / 2,           // 色调最小值~最大值
                (int)(255 * 0.60), 255,   // 饱和度最小值~最大值
                (int)(255 * 0.90), 255);  // 亮度最小值~最大值

            Cv2.ImShow("demo1", des);
        }

        static void demo2()
        {
            Mat g_srcImage = Cv2.ImRead("../../../images/color.jpg");
            Cv2.ImShow("原始图2", g_srcImage);
            Mat des = ColorFindContours2(g_srcImage);
            Cv2.ImShow("demo2", des);
        }

        static void Main(string[] args)
        {
            demo1();
            demo2();
            Cv2.WaitKey();
        }
    }
}

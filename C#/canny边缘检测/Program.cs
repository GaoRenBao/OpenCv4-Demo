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
            // 【1】读取图像
            Mat srcImage = Cv2.ImRead("../../../images/orange.jpg");
            Mat srcImage1 = srcImage.Clone();

            // 【2】显示原图
            Cv2.ImShow("原图", srcImage);

            // 【3】创建与src同类型和大小的矩阵(dst)
            Mat dstImage = new Mat(srcImage.Cols, srcImage.Rows, srcImage.Type());

            // 【4】将原图像转换为灰度图像
            Mat grayImage = new Mat();
            Cv2.CvtColor(srcImage1, grayImage, ColorConversionCodes.BGR2GRAY);

            // 【5】先用使用 3x3内核来降噪
            Mat edge = new Mat();
            Cv2.Blur(grayImage, edge, new OpenCvSharp.Size() { Width = 3, Height = 3 });

            // 【6】运行Canny算子
            Cv2.Canny(edge, edge, 3, 9, 3);

            // 【7】使用Canny算子输出的边缘图g_cannyDetectedEdges作为掩码，来将原图g_srcImage拷到目标图g_dstImage中
            srcImage1.CopyTo(dstImage, edge);

            // 【8】显示效果图 
            Cv2.ImShow("【效果图】整体方向Sobel", dstImage);
            Cv2.WaitKey(0);

            // 【9】在pictureBox1中显示效果图
            //Bitmap map = BitmapConverter.ToBitmap(dstImage);
            //pictureBox1.Image = map;
        }
    }
}

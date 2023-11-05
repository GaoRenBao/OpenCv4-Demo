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
            Mat grad_x = new Mat();
            Mat grad_y = new Mat();
            Mat abs_grad_x = new Mat();
            Mat abs_grad_y = new Mat();
            Mat dst = new Mat();

            //【1】读取图像
            Mat src = Cv2.ImRead("../../../images/girl2.jpg");
            //【2】显示原图
            Cv2.ImShow("原图", src);
            //【3】求 X方向梯度
            Cv2.Sobel(src, grad_x, MatType.CV_16S, 1, 0, 3, 1, 1, BorderTypes.Default);
            Cv2.ConvertScaleAbs(grad_x, abs_grad_x);
            Cv2.ImShow("【效果图】 X方向Sobel", abs_grad_x);
            //【4】求Y方向梯度
            Cv2.Sobel(src, grad_y, MatType.CV_16S, 0, 1, 3, 1, 1, BorderTypes.Default);
            Cv2.ConvertScaleAbs(grad_y, abs_grad_y);
            Cv2.ImShow("【效果图】Y方向Sobel", abs_grad_y);
            //【5】合并梯度(近似)
            Cv2.AddWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, dst);
            Cv2.ImShow("【效果图】整体方向Sobel", dst);

            Cv2.ImShow("效果", dst);
            Cv2.WaitKey(0);

            //【6】在pictureBox1中显示效果图
            //Bitmap map = BitmapConverter.ToBitmap(dst);
            //pictureBox1.Image = map;
        }
    }
}

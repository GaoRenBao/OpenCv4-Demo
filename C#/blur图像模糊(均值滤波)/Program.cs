/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：blur图像模糊（均值滤波）
博客：http://www.bilibili996.com/Course?id=4355990000006
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
            // 读取图像
            Mat srcImage = Cv2.ImRead("../../../images/girl.jpg");
            // 显示原图
            Cv2.ImShow("原图", srcImage);
            // 均值滤波
            Mat dstImage = new Mat();
            Cv2.Blur(srcImage, dstImage, new OpenCvSharp.Size() { Width = 7, Height = 7 });

            Cv2.ImShow("效果", dstImage);
            Cv2.WaitKey(0);

            // 在pictureBox1中显示效果图
            //Bitmap map = BitmapConverter.ToBitmap(dstImage);
            //pictureBox1.Image = map;
            //// 弹窗显示效果图
            //using (new Window("效果", dstImage))
            //{
            //    Cv2.WaitKey();
            //}
        }
    }
}

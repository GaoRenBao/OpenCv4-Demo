/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：图像腐蚀
博客：http://www.bilibili996.com/Course?id=4347382000005
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
            Mat srcImage = Cv2.ImRead("../../../images/girl.jpg");

            // 在窗口中显示原画
            Cv2.ImShow("原图", srcImage);

            // 进行腐蚀操作
            Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size() { Width = 15, Height = 15 });
            Mat dstImage = new Mat();
            Cv2.Erode(srcImage, dstImage, element);

            Cv2.ImShow("效果", dstImage);
            Cv2.WaitKey(0);

            // 输出图像到pictureBox控件
            //Bitmap map = BitmapConverter.ToBitmap(dstImage);
            //pictureBox1.Image = map;

            //// 弹窗显示图像
            //using (new Window("效果", dstImage))
            //{
            //    Cv2.WaitKey();
            //}
        }
    }
}

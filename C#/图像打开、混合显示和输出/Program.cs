/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：图像打开、混合显示和输出
博客：http://www.bilibili996.com/Course?id=4335202000004
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
            // 读取图片
            Mat image1 = Cv2.ImRead("../../../images/a.jpg");
            Mat image2 = new Mat("../../../images/b.jpg");
            // 设置图片2需要显示的区域
            Mat imageROI = image1[new Rect() { X = 800, Y = 350, Height = image2.Rows, Width = image2.Cols }];
            // 重叠两张图片
            Cv2.AddWeighted(imageROI, 0.7, image2, 0.3, 0.0, imageROI);
            Cv2.ImShow("合并", image1);
            Cv2.WaitKey(0);

            // 显示图片到pictureBox
            //Bitmap map = BitmapConverter.ToBitmap(image1);
            //pictureBox1.Image = map;
            //// 弹窗显示
            //using (new Window("合并", image1))
            //{
            //    Cv2.WaitKey();
            //}
        }
    }
}

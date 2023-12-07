/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：打开一张图片
博客：http://www.bilibili996.com/Course?id=4316440000002
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
            // 读取一张图片
            Mat srcImage = new Mat("../../../images/a.jpg");
            // 显示图片
            Cv2.ImShow("image", srcImage);
            // 按键检测，结束
            Cv2.WaitKey(0);

            // pictureBox赋值方法
            //Bitmap map = BitmapConverter.ToBitmap(srcImage);
            //PictureBox pictureBox1 = new PictureBox();
            //pictureBox1.Image = map;
        }
    }
}

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
        private static Mat createAlphaMat()
        {
            Mat mat = new Mat(480, 640, MatType.CV_8UC4);
            for (int i = 0; i < mat.Rows; ++i)
            {
                for (int j = 0; j < mat.Cols; ++j)
                {
                    var rgba = new Vec4b();
                    // 蓝色
                    rgba.Item0 = 0xff;
                    // 绿色
                    rgba.Item1 = (byte)(((float)mat.Cols - j) / (float)mat.Cols * 0xff);
                    // 红色
                    rgba.Item2 = (byte)(((float)mat.Rows - i) / (float)mat.Rows * 0xff);
                    // 透明度
                    rgba.Item3 = (byte)((float)0.5 * (float)(rgba[1] + rgba[2]));
                    // 设置
                    mat.Set(i, j, rgba);
                }
            }
            return mat;
        }

        static void Main(string[] args)
        {
            Mat srcImage = createAlphaMat();
            Cv2.ImShow("透明Alpha值图.png", srcImage);
            Cv2.WaitKey();
        }
    }
}

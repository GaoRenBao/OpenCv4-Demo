/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System.Collections.Generic;

namespace demo
{
    internal class Program
    {
        public static string WINDOW_NAME1 = "【原始图窗口】";                 //为窗口标题定义的宏 
        public static string WINDOW_NAME2 = "【经过Warp后的图像】";        //为窗口标题定义的宏 
        public static string WINDOW_NAME3 = "【经过Warp和Rotate后的图像】";        //为窗口标题定义的宏 

        static void Main(string[] args)
        {
            //【1】参数准备
            //定义两组点，代表两个三角形
            List<Point2f> srcTriangle = new List<Point2f>();
            List<Point2f> dstTriangle = new List<Point2f>();

            //定义一些Mat变量
            Mat rotMat = new Mat(2, 3, MatType.CV_32FC1);
            Mat warpMat = new Mat(2, 3, MatType.CV_32FC1);
            Mat dstImage_warp_rotate = new Mat();

            //【2】加载源图像并作一些初始化
            Mat srcImage = Cv2.ImRead("../../../images/tree.jpg");

            // 设置目标图像的大小和类型与源图像一致
            Mat dstImage_warp = new Mat(srcImage.Size(), srcImage.Type());

            //【3】设置源图像和目标图像上的三组点以计算仿射变换
            srcTriangle.Add(new Point2f(0, 0));
            srcTriangle.Add(new Point2f(srcImage.Cols - 1, 0));
            srcTriangle.Add(new Point2f(0, srcImage.Rows - 1));

            dstTriangle.Add(new Point2f((float)(srcImage.Cols * 0.0), (float)(srcImage.Rows * 0.33)));
            dstTriangle.Add(new Point2f((float)(srcImage.Cols * 0.65), (float)(srcImage.Rows * 0.35)));
            dstTriangle.Add(new Point2f((float)(srcImage.Cols * 0.15), (float)(srcImage.Rows * 0.6)));

            //【4】求得仿射变换
            warpMat = Cv2.GetAffineTransform(srcTriangle, dstTriangle);

            //【5】对源图像应用刚刚求得的仿射变换
            Cv2.WarpAffine(srcImage, dstImage_warp, warpMat, dstImage_warp.Size());

            //【6】对图像进行缩放后再旋转
            // 计算绕图像中点顺时针旋转50度缩放因子为0.6的旋转矩阵
            Point center = new Point(dstImage_warp.Cols / 2, dstImage_warp.Rows / 2);
            double angle = -50.0;
            double scale = 0.6;
            // 通过上面的旋转细节信息求得旋转矩阵
            rotMat = Cv2.GetRotationMatrix2D(center, angle, scale);
            // 旋转已缩放后的图像
            Cv2.WarpAffine(dstImage_warp, dstImage_warp_rotate, rotMat, dstImage_warp.Size());

            //【7】显示结果
            Cv2.ImShow(WINDOW_NAME1, srcImage);
            Cv2.ImShow(WINDOW_NAME2, dstImage_warp);
            Cv2.ImShow(WINDOW_NAME3, dstImage_warp_rotate);

            // 等待用户按任意按键退出程序
            Cv2.WaitKey(0);
        }
    }
}

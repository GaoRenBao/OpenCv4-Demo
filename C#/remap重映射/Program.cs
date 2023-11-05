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
            //【1】载入原始图
            Mat srcImage = Cv2.ImRead("../../../images/car.jpg");

            //【2】显示原始图  
            Cv2.ImShow("【原始图】", srcImage);

            //【2】创建和原始图一样的效果图，x重映射图，y重映射图
            Mat dstImage = new Mat(srcImage.Size(), srcImage.Type());
            Mat map_x = new Mat(srcImage.Size(), MatType.CV_32FC1);
            Mat map_y = new Mat(srcImage.Size(), MatType.CV_32FC1);

            //【3】双层循环，遍历每一个像素点，改变map_x & map_y的值
            for (int j = 0; j < srcImage.Rows; j++)
            {
                for (int i = 0; i < srcImage.Cols; i++)
                {
                    //改变map_x & map_y的值. 
                    map_x.Set(j, i, (float)(i));
                    map_y.Set(j, i, (float)(srcImage.Rows - j));
                }
            }

            //【4】进行重映射操作
            Cv2.Remap(srcImage, dstImage, map_x, map_y, InterpolationFlags.Linear, BorderTypes.Constant, new Scalar(0, 0, 0));

            //【5】显示效果图
            Cv2.ImShow("【C# 效果图】", dstImage);
            Cv2.WaitKey(0);
        }
    }
}

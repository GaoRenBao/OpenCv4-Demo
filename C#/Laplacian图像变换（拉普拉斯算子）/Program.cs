/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：Laplacian图像变换（拉普拉斯算子）
博客：http://www.bilibili996.com/Course?id=4232193000169
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
            //【0】变量的定义
            Mat src = new Mat();
            Mat src_gray = new Mat();
            Mat dst = new Mat();
            Mat abs_dst = new Mat();

            //【1】载入原始图  
            src = Cv2.ImRead("../../../images/girl5.jpg");

            //【2】显示原始图 
            Cv2.ImShow("【原始图】图像Laplace变换", src);

            //【3】使用高斯滤波消除噪声
            Cv2.GaussianBlur(src, src, new Size(3, 3), 0, 0, BorderTypes.Default);

            //【4】转换为灰度图
            Cv2.CvtColor(src, src_gray, ColorConversionCodes.RGB2GRAY);

            //【5】使用Laplace函数
            Cv2.Laplacian(src_gray, dst, MatType.CV_16S, 3, 1, 0, BorderTypes.Default);

            //【6】计算绝对值，并将结果转换成8位
            Cv2.ConvertScaleAbs(dst, abs_dst);

            //【7】显示效果图
            Cv2.ImShow("【效果图】图像Laplace变换", abs_dst);
            Cv2.WaitKey(0);
        }
    }
}

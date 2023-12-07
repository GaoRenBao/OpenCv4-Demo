/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：离散傅里叶变换
博客：http://www.bilibili996.com/Course?id=3391564000241
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
            //【1】以灰度模式读取原始图像并显示
            Mat srcImage = Cv2.ImRead("../../../images/girl.jpg", 0);
            Cv2.ImShow("原始图像", srcImage);

            //【2】将输入图像延扩到最佳的尺寸，边界用0补充
            int m = Cv2.GetOptimalDFTSize(srcImage.Rows);
            int n = Cv2.GetOptimalDFTSize(srcImage.Cols);

            Mat padded = new Mat();
            Cv2.CopyMakeBorder(srcImage, padded, 0, m - srcImage.Rows, 0, n - srcImage.Cols, BorderTypes.Constant, Scalar.All(0));

            //【3】为傅立叶变换的结果(实部和虚部)分配存储空间。
            //将planes数组组合合并成一个多通道的数组complexI
            Mat paddedF32 = new Mat();
            padded.ConvertTo(paddedF32, MatType.CV_32F);
            Mat[] planes = { paddedF32, Mat.Zeros(padded.Size(), MatType.CV_32F) };
            Mat complex = new Mat();
            Cv2.Merge(planes, complex);

            //【4】进行就地离散傅里叶变换
            Cv2.Dft(complex, complex);

            //【5】将复数转换为幅值，即=> log(1 + sqrt(Re(DFT(I))^2 + Im(DFT(I))^2))
            Cv2.Split(complex, out planes);

            Mat magnitudeImage = new Mat();
            Cv2.Magnitude(planes[0], planes[1], magnitudeImage);

            //【6】进行对数尺度(logarithmic scale)缩放
            magnitudeImage += Scalar.All(1);
            Cv2.Log(magnitudeImage, magnitudeImage);//求自然对数

            //【7】剪切和重分布幅度图象限
            //若有奇数行或奇数列，进行频谱裁剪      
            magnitudeImage = magnitudeImage[new Rect(0, 0, magnitudeImage.Cols & -2, magnitudeImage.Rows & -2)];

            //重新排列傅立叶图像中的象限，使得原点位于图像中心  
            int cx = magnitudeImage.Cols / 2;
            int cy = magnitudeImage.Rows / 2;
            Mat q0 = new Mat(magnitudeImage, new Rect(0, 0, cx, cy));   // ROI区域的左上
            Mat q1 = new Mat(magnitudeImage, new Rect(cx, 0, cx, cy));  // ROI区域的右上
            Mat q2 = new Mat(magnitudeImage, new Rect(0, cy, cx, cy));  // ROI区域的左下
            Mat q3 = new Mat(magnitudeImage, new Rect(cx, cy, cx, cy)); // ROI区域的右下

            //交换象限（左上与右下进行交换）
            Mat tmp = new Mat();
            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);

            //交换象限（右上与左下进行交换）
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            //【8】归一化，用0到1之间的浮点值将矩阵变换为可视的图像格式
            Cv2.Normalize(magnitudeImage, magnitudeImage, 0, 1, NormTypes.MinMax);

            //【9】显示效果图
            Cv2.ImShow("频谱幅值", magnitudeImage);
            Cv2.WaitKey();
        }
    }
}

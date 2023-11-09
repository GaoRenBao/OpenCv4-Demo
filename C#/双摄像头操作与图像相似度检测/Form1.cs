/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：双摄像头操作与图像相似度检测
博客：http://www.bilibili996.com/Course?id=0431478000248
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 双摄像头调用
        /// <summary>
        /// 双摄像头调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            VideoCapture Cap0 = new VideoCapture();
            VideoCapture Cap1 = new VideoCapture();

            // 打开ID为0的摄像头
            Cap0.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap0.IsOpened())
            {
                MessageBox.Show("摄像头0打开失败.");
                return;
            }

            // 打开ID为1的摄像头
            Cap1.Open(1);
            // 判断摄像头是否成功打开
            if (!Cap1.IsOpened())
            {
                MessageBox.Show("摄像头1打开失败.");
                return;
            }

            Cap0.Set(VideoCaptureProperties.FrameWidth, 320);  // 设置采集的图像宽度：320
            Cap0.Set(VideoCaptureProperties.FrameHeight, 240); // 设置采集的图像高度：240
            Cap1.Set(VideoCaptureProperties.FrameWidth, 320);  // 设置采集的图像宽度：320
            Cap1.Set(VideoCaptureProperties.FrameHeight, 240); // 设置采集的图像高度：240

            Mat frame0 = new Mat(), frame1 = new Mat();

            while (Cap0.IsOpened() && Cap1.IsOpened())
            {
                if (Cap0.Read(frame0))
                {
                    Cv2.ImShow("frame0", frame0);
                    Cv2.SetWindowTitle("frame0", "On Top");
                }
                if (Cap1.Read(frame1))
                {
                    Cv2.ImShow("frame1", frame1);
                    Cv2.MoveWindow("frame1", frame0.Cols, 0);
                }

                if (Cv2.WaitKey(2) == 27)// ESC按钮
                    break;
            }

            // When everything done, release the capture
            Cap0.Release();
            Cap1.Release();
        }
        #endregion

        #region 计算相似度

        /// <summary>
        /// 计算相似度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            VideoCapture Cap = new VideoCapture();
            Cap.Open(0);

            // 打开ID为1的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                MessageBox.Show("摄像头打开失败.");
                return;
            }

            Mat frame = new Mat();
            Mat temp = new Mat();
            Mat gray = new Mat();

            if (Cap.Read(frame))
            {
                Cv2.CvtColor(frame, temp, ColorConversionCodes.BGR2GRAY);
                Cv2.Resize(temp, temp, new Size(temp.Cols * 0.5, temp.Rows * 0.5), 0, 0, InterpolationFlags.Area);
                Cv2.ImShow("temp", temp);
            }

            while (Cap.IsOpened())
            {
                if (Cap.Read(frame))
                {
                    Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
                    Cv2.Resize(gray, gray, new Size(gray.Cols * 0.5, gray.Rows * 0.5), 0, 0, InterpolationFlags.Area);
                    Cv2.ImShow("gray", gray);
                    using (similarity si = new similarity())
                    {
                        label1.Text = $"mse:{si.mse(temp, gray)}\r\n";
                    }
                    using (similarity si = new similarity())
                    {
                        label1.Text += $"ssim:{si.ssim(temp, gray)}\r\n";
                    }
                    using (similarity si = new similarity())
                    {
                        label1.Text += $"psnr:{si.psnr(temp, gray)}";
                    }
                }
                Cv2.WaitKey(30);
            }
            Cap.Release();
        }
        #endregion
    }

    /// <summary>
    /// 相似度
    /// </summary>
    public  class similarity : IDisposable
    {
        void IDisposable.Dispose() { }
        public void Dispose() { }

        /// <summary>
        /// 均方误差（MSE）
        /// </summary>
        /// <param name="imageA"></param>
        /// <param name="imageB"></param>
        /// <returns></returns>
        public double mse(Mat imageA, Mat imageB)
        {
            // 在使用OpenCV时可以通过矩阵操作来避免for循环嵌套计算。
            // 需要注意的是乘除操作一般要注意将图像本身的uint8转换成float后再做，否则精度误差可能会导致较大偏差。
            Mat M1 = imageA.Clone();
            Mat M2 = imageB.Clone();
            Mat Diff = new Mat();

            // 提前转换为32F精度
            M1.ConvertTo(M1, MatType.CV_32F);
            M2.ConvertTo(M2, MatType.CV_32F);
            Diff.ConvertTo(Diff, MatType.CV_32F);

            Cv2.Absdiff(M1, M2, Diff); //  Diff = | M1 - M2 |
            Diff = Diff.Mul(Diff);     // | M1 - M2 |.^2
            Scalar S = Cv2.Sum(Diff);  //分别计算每个通道的元素之和

            double sse;   // square error
            if (Diff.Channels() == 3)
                sse = S.Val0 + S.Val1 + S.Val2;  // sum of all channels
            else
                sse = S.Val0;
            long nTotalElement = M2.Channels() * M2.Total();
            double mse = (sse / (double)nTotalElement);
            return mse;
        }

        // 结构相似度指数（SSIM），是一种衡量两幅图像相似度的指标，
        // 也是一种全参考的图像质量评价指标，它分别从亮度、对比度、结构三方面度量图像相似性
        // 参考： https://www.freesion.com/article/50591020225/
        // C1，C2和C3为常数，是为了避免分母为0而维持稳定。
        // L = 255（ 是像素值的动态范围，一般都取为255）。
        // K1 = 0.01, K2 = 0.03。结构相似性的范围为 - 1 到 1 。
        // 当两张图像一模一样时，SSIM的值等于1。
        public double ssim(Mat i1, Mat i2)
        {
            // 跑久了，会报错。。。。
            const double C1 = 6.5025, C2 = 58.5225;
            int d = MatType.CV_32F;
            Mat I1 = new Mat(), I2 = new Mat();
            i1.ConvertTo(I1, d);
            i2.ConvertTo(I2, d);
            Mat I1_2 = I1.Mul(I1);
            Mat I2_2 = I2.Mul(I2);
            Mat I1_I2 = I1.Mul(I2);
            Mat mu1 = new Mat(), mu2 = new Mat();
            Cv2.GaussianBlur(I1, mu1, new Size(11, 11), 1.5);
            Cv2.GaussianBlur(I2, mu2, new Size(11, 11), 1.5);
            Mat mu1_2 = mu1.Mul(mu1);
            Mat mu2_2 = mu2.Mul(mu2);
            Mat mu1_mu2 = mu1.Mul(mu2);
            Mat sigma1_2 = new Mat(), sigam2_2 = new Mat(), sigam12 = new Mat();
            Cv2.GaussianBlur(I1_2, sigma1_2, new Size(11, 11), 1.5);
            sigma1_2 -= mu1_2;

            Cv2.GaussianBlur(I2_2, sigam2_2, new Size(11, 11), 1.5);
            sigam2_2 -= mu2_2;

            Cv2.GaussianBlur(I1_I2, sigam12, new Size(11, 11), 1.5);
            sigam12 -= mu1_mu2;
            Mat t1, t2, t3;
            t1 = 2 * mu1_mu2 + C1;
            t2 = 2 * sigam12 + C2;
            t3 = t1.Mul(t2);

            t1 = mu1_2 + mu2_2 + C1;
            t2 = sigma1_2 + sigam2_2 + C2;
            t1 = t1.Mul(t2);

            Mat ssim_map = new Mat();
            Cv2.Divide(t3, t1, ssim_map);
            Scalar mssim = Cv2.Mean(ssim_map);

            double ssim = (mssim.Val0 + mssim.Val1 + mssim.Val2) / 3;
            return ssim;
        }

        // 峰值信噪比，一种评价图像的客观标准，用来评估图像的保真性。
        // 峰值信噪比经常用作图像压缩等领域中信号重建质量的测量方法，
        // 它常简单地通过均方差（MSE）进行定义，使用两个m×n单色图像I和K。PSNR的单位为分贝
        // PSNR值越大，就代表失真越少，图像压缩中典型的峰值信噪比值在 30 到 40dB 之间，小于30dB时考虑图像无法忍受。
        public double psnr(Mat I1, Mat I2)
        {
            //注意，当两幅图像一样时这个函数计算出来的psnr为0 
            Mat s1 = new Mat();
            Cv2.Absdiff(I1, I2, s1);
            s1.ConvertTo(s1, MatType.CV_32F);//转换为32位的float类型，8位不能计算平方  
            s1 = s1.Mul(s1);
            Scalar s = Cv2.Sum(s1);  //计算每个通道的和  
            double sse = s.Val0 + s.Val1 + s.Val2;
            if (sse <= 1e-10) // for small values return zero  
                return 0;
            else
            {
                double mse = sse / (double)(I1.Channels() * I1.Total()); //  sse/(w*h*3)  
                double psnr = 10.0 * Math.Log10((255 * 255) / mse);
                return psnr;
            }
        }
    }
}
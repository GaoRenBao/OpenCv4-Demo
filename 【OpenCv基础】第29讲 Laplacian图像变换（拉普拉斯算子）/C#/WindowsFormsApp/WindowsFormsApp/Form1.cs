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

        private void button1_Click(object sender, EventArgs e)
        {
			//【0】变量的定义
			Mat src = new Mat();
			Mat src_gray = new Mat();
			Mat dst = new Mat();
			Mat abs_dst = new Mat();

			//【1】载入原始图  
			src = Cv2.ImRead("1.jpg");  //工程目录下应该有一张名为1.jpg的素材图

			//【2】显示原始图 
			Cv2.ImShow("【原始图】图像Laplace变换", src);

			//【3】使用高斯滤波消除噪声
			Cv2.GaussianBlur(src, src,new  Size(3, 3), 0, 0, BorderTypes.Default);

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


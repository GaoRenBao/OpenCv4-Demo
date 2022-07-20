using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Threading.Tasks;
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
			//【1】载入原始图
			Mat srcImage = Cv2.ImRead("1.jpg");

			//【2】显示原始图  
			Cv2.CvtColor(srcImage, srcImage, ColorConversionCodes.BGR2GRAY);
			Cv2.ImShow("【原始图】", srcImage);

            // 【3】进行直方图均衡化
            Mat dstImage = new Mat();
            Cv2.EqualizeHist(srcImage, dstImage);

            //【5】显示效果图
            Cv2.ImShow("【C# 效果图】", dstImage);
			Cv2.WaitKey(0);
		}
    }
}


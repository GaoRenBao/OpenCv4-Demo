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
			// 载入原图
			Mat image = Cv2.ImRead("1.jpg");

			// 显示原图
			Cv2.ImShow("【原图】", image);

			// 进行方框滤波操作
			Mat out1 = new Mat();
			Cv2.BoxFilter(image, out1, -1, new Size(5, 5));

			// 进行均值滤波操作
			Mat out2 = new Mat();
			Cv2.Blur(image, out2, new Size(7, 7));

			//进行高斯滤波操作
			// sigmaX，表示高斯核函数在X方向的的标准偏差。
			// sigmaY，表示高斯核函数在Y方向的的标准偏差。
			// 若sigmaY为零，就将它设为sigmaX。
			// 如果sigmaX和sigmaY都是0，那么就由ksize.width和ksize.height计算出来。
			Mat out3 = new Mat();
			Cv2.GaussianBlur(image, out3, new Size(5, 5), 0, 0);

			//显示效果图
			Cv2.ImShow("方框滤波【效果图】", out1);
			Cv2.ImShow("均值滤波【效果图】", out2);
			Cv2.ImShow("高斯滤波【效果图】", out3);

			Cv2.WaitKey(0);
		}
    }
}


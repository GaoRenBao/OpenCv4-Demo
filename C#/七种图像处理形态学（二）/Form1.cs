using OpenCvSharp;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
		public int g_element = 15;  //核
		Mat srcImage; // 原图
		
		public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			// 载入原图
			srcImage = Cv2.ImRead("../../../images/girl4.jpg");
			// 显示原图
			Cv2.ImShow("【原图】", srcImage);
			// 异步操作
			new Task(() => { MyMorphology(); }).Start();
		}

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
			g_element = hScrollBar1.Value;
			label1.Text = g_element.ToString();
		}

		private void MyMorphology()
        {
			// 一定要放在while外面初始化，不然会出现内存问题
			Mat out1 = new Mat();
			Mat out2 = new Mat();
			Mat out3 = new Mat();
			Mat out4 = new Mat();
			Mat out5 = new Mat();
			Mat out6 = new Mat();
			Mat out7 = new Mat();
			Mat out8 = new Mat();
			Mat des = new Mat();

			while (true)
            {
				// 定义核大小
				Mat element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(g_element, g_element));

				// 进行形态学腐蚀操作 
				Cv2.MorphologyEx(srcImage, out1, MorphTypes.Erode, element);

				// 进行形态学膨胀操作 
				Cv2.MorphologyEx(srcImage, out2, MorphTypes.Dilate, element);

				// 进行形态学开运算操作 
				Cv2.MorphologyEx(srcImage, out3, MorphTypes.Open, element);

				// 进行形态学闭运算操作 
				Cv2.MorphologyEx(srcImage, out4, MorphTypes.Close, element);

				// 进行形态学梯度操作 
				Cv2.MorphologyEx(srcImage, out5, MorphTypes.Gradient, element);

				// 进行形态学顶帽操作 
				Cv2.MorphologyEx(srcImage, out6, MorphTypes.TopHat, element);

				// 进行形态学黑帽操作 
				Cv2.MorphologyEx(srcImage, out7, MorphTypes.BlackHat, element);

				// 进行形态学击中击不中操作 
				Cv2.CvtColor(srcImage, des, ColorConversionCodes.BGR2GRAY);
				Cv2.MorphologyEx(des, out8, MorphTypes.HitMiss, element);

				// 显示效果图
				Cv2.ImShow("腐蚀【效果图】", out1);
				Cv2.ImShow("膨胀【效果图】", out2);
				Cv2.ImShow("开运算【效果图】", out3);
				Cv2.ImShow("闭运算【效果图】", out4);
				Cv2.ImShow("梯度【效果图】", out5);
				Cv2.ImShow("顶帽【效果图】", out6);
				Cv2.ImShow("黑帽【效果图】", out7);
				Cv2.ImShow("击中击不中【效果图】", out8);
				Cv2.WaitKey(30);
			}
		}
	}
}


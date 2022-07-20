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
			//【1】载入原始图和Mat变量定义   
			Mat srcImage = Cv2.ImRead("1.jpg");  //工程目录下应该有一张名为1.jpg的素材图

			//临时变量和目标图的定义
			Mat midImage = new Mat();
			Mat dstImage = new Mat();

			//【2】进行边缘检测和转化为灰度图
			//进行一此canny边缘检测
			Cv2.Canny(srcImage, midImage, 50, 200, 3);
			//转化边缘检测后的图为灰度图
			Cv2.CvtColor(midImage, dstImage, ColorConversionCodes.GRAY2BGR);

			//【3】进行霍夫线变换
			//定义一个矢量结构lines用于存放得到的线段矢量集合
			LineSegmentPolar[] lines =  Cv2.HoughLines(midImage, 1, Cv2.PI / 180, 150, 0, 0);

			//【4】依次在图中绘制出每条线段
			for (int i = 0; i < lines.Length; i++)
			{
				float rho = lines[i].Rho, theta = lines[i].Theta;
				Point pt1, pt2;
				double a = Math.Cos(theta), b = Math.Sin(theta);
				double x0 = a * rho, y0 = b * rho;
				pt1.X = (int)Math.Round(x0 + 1000 * (-b));
				pt1.Y = (int)Math.Round(y0 + 1000 * (a));
				pt2.X = (int)Math.Round(x0 - 1000 * (-b));
				pt2.Y = (int)Math.Round(y0 - 1000 * (a));
				Cv2.Line(dstImage, pt1, pt2, new Scalar(55, 100, 195), 1, LineTypes.AntiAlias);
			}

			//【5】显示原始图  
			Cv2.ImShow("【原始图】", srcImage);

			//【6】边缘检测后的图 
			Cv2.ImShow("【边缘检测后的图】", midImage);

			//【7】显示效果图  
			Cv2.ImShow("【效果图】", dstImage);

			Cv2.WaitKey(0);
		}
    }
}


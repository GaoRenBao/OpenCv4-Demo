using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private string WINDOW_NAME = "【Shi-Tomasi角点检测】";

        Mat g_srcImage = new Mat();
        Mat g_grayImage = new Mat();
        int g_maxCornerNumber = 33; //当前阈值
        int g_maxTrackbarNumber = 500; //最大阈值

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //【1】载入原图像
            g_srcImage = Cv2.ImRead("1.jpg");

            //【2】存留一张灰度图
            Cv2.CvtColor(g_srcImage, g_grayImage, ColorConversionCodes.BGR2GRAY);

            //【3】创建窗口和滚动条
            Cv2.NamedWindow(WINDOW_NAME, WindowFlags.AutoSize);
            int v = Cv2.CreateTrackbar("最大角点数:", WINDOW_NAME, ref g_maxCornerNumber, g_maxTrackbarNumber, on_GoodFeaturesToTrack);
			on_GoodFeaturesToTrack(0, IntPtr.Zero);

            Cv2.WaitKey(0);
        }

        public void on_GoodFeaturesToTrack(int pos, IntPtr userData)
        {
			//【1】对变量小于等于1时的处理
			if (g_maxCornerNumber <= 1) { g_maxCornerNumber = 1; }

			//【2】Shi-Tomasi算法（goodFeaturesToTrack函数）的参数准备
			double qualityLevel = 0.01;//角点检测可接受的最小特征值
			double minDistance = 10;//角点之间的最小距离
			int blockSize = 3;//计算导数自相关矩阵时指定的邻域范围
			double k = 0.04;//权重系数
			Mat copy = new Mat();
			g_srcImage.CopyTo(copy);  //复制源图像到一个临时变量中，作为感兴趣区域

			//【3】进行Shi-Tomasi角点检测 
			// 输出检测到的角点的输出向量
			Point2f[] corners = Cv2.GoodFeaturesToTrack(g_grayImage,//输入图像
				g_maxCornerNumber,//角点的最大数量
				qualityLevel,//角点检测可接受的最小特征值
				minDistance,//角点之间的最小距离
				null,//感兴趣区域
				blockSize,//计算导数自相关矩阵时指定的邻域范围
				false,//不使用Harris角点检测
				k);//权重系数


			//【4】输出文字信息
			System.Diagnostics.Debug.WriteLine($"此次检测到的角点数量为：{corners.Length}");

			//【5】绘制检测到的角点
			int r = 4;
			RNG rng = new RNG(12345);
			for (int i = 0; i < corners.Length; i++)
			{
				//以随机的颜色绘制出角点
				byte cb = (byte)rng.Uniform(0, 255);
				byte cg = (byte)rng.Uniform(0, 255);
				byte cr = (byte)rng.Uniform(0, 255);
				Cv2.Circle(copy, (int)corners[i].X, (int)corners[i].Y , r, new Scalar(cb, cg, cr), -1, LineTypes.Link8, 0);
			}

			//【6】显示（更新）窗口
			Cv2.ImShow(WINDOW_NAME, copy);
		}
    }
}
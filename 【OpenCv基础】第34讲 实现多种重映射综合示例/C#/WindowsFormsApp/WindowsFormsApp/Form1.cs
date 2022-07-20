using OpenCvSharp;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
		public Mat g_srcImage = new Mat();
		public Mat g_dstImage = new Mat();
		public Mat g_map_x = new Mat();
		public Mat g_map_y = new Mat();

		public int key = 0;
		public Form1()
        {
            InitializeComponent();
        }

		// 描述：根据按键来更新map_x与map_x的值
		public int update_map(int key)
		{
			//双层循环，遍历每一个像素点
			for (int j = 0; j < g_srcImage.Rows; j++)
			{
				for (int i = 0; i < g_srcImage.Cols; i++)
				{
					switch (key)
					{
						case 1: // 键盘【1】键按下，进行第一种重映射操作
							if (i > g_srcImage.Cols * 0.25 && i < g_srcImage.Cols * 0.75 && j > g_srcImage.Rows * 0.25 && j < g_srcImage.Rows * 0.75)
							{
								g_map_x.Set(j, i, (float)(2 * (i - g_srcImage.Cols * 0.25) + 0.5));
								g_map_y.Set(j, i, (float)(2 * (j - g_srcImage.Rows * 0.25) + 0.5));
							}
							else
							{
								g_map_x.Set(j, i, (float)(0));
								g_map_y.Set(j, i, (float)(0));
							}
							break;
						case 2:// 键盘【2】键按下，进行第二种重映射操作
							g_map_x.Set(j, i, (float)(i));
							g_map_y.Set(j, i, (float)(g_srcImage.Rows - j));
							break;
						case 3:// 键盘【3】键按下，进行第三种重映射操作
							g_map_x.Set(j, i, (float)(g_srcImage.Cols - i));
							g_map_y.Set(j, i, (float)(j));
							break;
						case 4:// 键盘【4】键按下，进行第四种重映射操作
							g_map_x.Set(j, i, (float)(g_srcImage.Cols - i));
							g_map_y.Set(j, i, (float)(g_srcImage.Rows - j));
							break;
					}
				}
			}
			return 1;
		}

		private void button1_Click(object sender, EventArgs e)
        {
			//【1】载入原始图
			g_srcImage = Cv2.ImRead("1.jpg");

			//【2】显示原始图  
			Cv2.ImShow("【原始图】", g_srcImage);

			//【2】创建和原始图一样的效果图，x重映射图，y重映射图
			g_dstImage = new Mat(g_srcImage.Size(), g_srcImage.Type());
			g_map_x = new Mat(g_srcImage.Size(), MatType.CV_32FC1);
			g_map_y = new Mat(g_srcImage.Size(), MatType.CV_32FC1);

			// 异步操作
			new Task(() => {
				while(true)
                {
					if (key != 0)
					{
						// 根据按下的键盘按键来更新 map_x & map_y的值. 然后调用remap( )进行重映射
						update_map(key);

						// 进行重映射操作
						Cv2.Remap(g_srcImage, g_dstImage, g_map_x, g_map_y, InterpolationFlags.Linear, BorderTypes.Constant, new Scalar(0, 0, 0));

						// 显示效果图
						Cv2.ImShow("【C# 效果图】", g_dstImage);
						key = 0;
					}
					Cv2.WaitKey(30);
				}
			}).Start();
		}
        private void 第一种映射方式_Click(object sender, EventArgs e)
        {
			key = 1;
		}
        private void 第二种映射方式_Click(object sender, EventArgs e)
        {
			key = 2;
		}
		private void 第三种映射方式_Click(object sender, EventArgs e)
        {
			key = 3;
		}
		private void 第四种映射方式_Click(object sender, EventArgs e)
        {
			key = 4;
		}
	}
}


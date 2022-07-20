using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
 
namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
		string WINDOW_NAME1 = "【绘制图1】";// 为窗口标题定义的宏 
		string WINDOW_NAME2 = "【绘制图2】";// 为窗口标题定义的宏 
		int WINDOW_WIDTH = 600;             // 定义窗口大小的宏

		public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			// 创建空白的Mat图像
			Mat atomImage = new Mat(WINDOW_WIDTH, WINDOW_WIDTH, MatType.CV_8UC3);
			Mat rookImage = new Mat(WINDOW_WIDTH, WINDOW_WIDTH, MatType.CV_8UC3);

			// --------------<1>绘制化学中的原子示例图--------------
			// 先绘制出椭圆
			DrawEllipse(atomImage, 90);
			DrawEllipse(atomImage, 0);
			DrawEllipse(atomImage, 45);
			DrawEllipse(atomImage, -45);

			// 再绘制圆心
			DrawFilledCircle(atomImage, new Point(WINDOW_WIDTH / 2, WINDOW_WIDTH / 2));

            // --------------<2>绘制组合图--------------
            //先绘制出多边形
            DrawPolygon(rookImage);

            // 绘制矩形
            Cv2.Rectangle(rookImage,
                new Point(0, 7 * WINDOW_WIDTH / 8),
                new Point(WINDOW_WIDTH, WINDOW_WIDTH),
                new Scalar(0, 255, 255), -1, LineTypes.Link8);

            // 绘制一些线段
            DrawLine(rookImage, new Point(0, 15 * WINDOW_WIDTH / 16), new Point(WINDOW_WIDTH, 15 * WINDOW_WIDTH / 16));
            DrawLine(rookImage, new Point(WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8), new Point(WINDOW_WIDTH / 4, WINDOW_WIDTH));
            DrawLine(rookImage, new Point(WINDOW_WIDTH / 2, 7 * WINDOW_WIDTH / 8), new Point(WINDOW_WIDTH / 2, WINDOW_WIDTH));
            DrawLine(rookImage, new Point(3 * WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8), new Point(3 * WINDOW_WIDTH / 4, WINDOW_WIDTH));

            // 显示绘制出的图像
            Cv2.ImShow(WINDOW_NAME1, atomImage);
			Cv2.MoveWindow(WINDOW_NAME1, 100, 200);
			Cv2.ImShow(WINDOW_NAME2, rookImage);
			Cv2.MoveWindow(WINDOW_NAME2, WINDOW_WIDTH + 200, 200);
			Cv2.WaitKey(0);
		}

		// 自定义的绘制函数，实现了绘制不同角度、相同尺寸的椭圆
		void DrawEllipse(Mat img, double angle)
		{
			Cv2.Ellipse(img,
				new Point(WINDOW_WIDTH / 2, WINDOW_WIDTH / 2),
				new Size(WINDOW_WIDTH / 4, WINDOW_WIDTH / 16),
				angle, 0, 360, new Scalar(255, 129, 0), 2, LineTypes.Link8);
		}

		// 自定义的绘制函数，实现了实心圆的绘制
		void DrawFilledCircle(Mat img, Point center)
		{
			Cv2.Circle(img, center.X, center.Y, WINDOW_WIDTH / 32, new Scalar(0, 0, 255), -1, LineTypes.Link8);
		}

		// 自定义的绘制函数，实现了凹多边形的绘制
		void DrawPolygon(Mat img)
		{
			//创建一些点
			List<List<Point>> pts = new List<List<Point>>() 
			{
				new List<Point>
				{
					 new Point(WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8),
					 new Point(3 * WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8),
					 new Point(3 * WINDOW_WIDTH / 4, 13 * WINDOW_WIDTH / 16),
					 new Point(11 * WINDOW_WIDTH / 16, 13 * WINDOW_WIDTH / 16),
					 new Point(19 * WINDOW_WIDTH / 32, 3 * WINDOW_WIDTH / 8),
					 new Point(3 * WINDOW_WIDTH / 4, 3 * WINDOW_WIDTH / 8),
					 new Point(3 * WINDOW_WIDTH / 4, WINDOW_WIDTH / 8),
					 new Point(26 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8),
					 new Point(26 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4),
					 new Point(22 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4),
					 new Point(22 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8),
					 new Point(18 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8),
					 new Point(18 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4),
					 new Point(14 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4),
					 new Point(14 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8),
					 new Point(WINDOW_WIDTH / 4, WINDOW_WIDTH / 8),
					 new Point(WINDOW_WIDTH / 4, 3 * WINDOW_WIDTH / 8),
					 new Point(13 * WINDOW_WIDTH / 32, 3 * WINDOW_WIDTH / 8),
					 new Point(5 * WINDOW_WIDTH / 16, 13 * WINDOW_WIDTH / 16),
					 new Point(WINDOW_WIDTH / 4, 13 * WINDOW_WIDTH / 16)
				}
			};

			// 绘制多边形填充
			Cv2.FillPoly(img, pts, new Scalar(255, 255, 255), LineTypes.Link8);
		}

		// 自定义的绘制函数，实现了线的绘制
		void DrawLine(Mat img, Point start, Point end)
		{
			Cv2.Line(img, start.X,start.Y, end.X, end.Y, new Scalar(0, 0, 0), 2, LineTypes.Link8);
		}
	}
}
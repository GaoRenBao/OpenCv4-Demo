using OpenCvSharp;
using System;
using System.Collections.Generic;
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

        private void button3_Click(object sender, EventArgs e)
        {
            new Task(() => { MyConvexHull(); }).Start();
        }

        private void MyConvexHull()
        {
            RNG rng = new RNG(12345);
            Mat image = Mat.Zeros(600, 600, MatType.CV_8UC3);

            while (true)
            {
                List<Point> points = new List<Point>(); //点值

                //参数初始化
                int count = rng.Uniform(10, 50);//随机生成点的数量
                points.Clear();

                //随机生成点坐标
                for (int i = 0; i < count; i++)
                {
                    Point point;
                    point.X = rng.Uniform(image.Cols / 4, image.Cols * 3 / 4);
                    point.Y = rng.Uniform(image.Rows / 4, image.Rows * 3 / 4);
                    points.Add(point);
                }

                // 绘制出随机颜色的点
                image = Mat.Zeros(600, 600, MatType.CV_8UC3);
                for (int i = 0; i < count; i++)
                    Cv2.Circle(image, points[i], 3, new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255)), -1, LineTypes.AntiAlias);

                //对给定的 2D 点集，寻找最小面积的包围圆
                Point2f center;
                float radius = 0;
                Cv2.MinEnclosingCircle(points,out center, out radius);

                //绘制出最小面积的包围圆
                Cv2.Circle(image, (int)center.X, (int)center.Y, (int)radius, new Scalar(rng.Uniform(0, 255), rng.Uniform(0, 255), rng.Uniform(0, 255)), 2, LineTypes.AntiAlias);

                //显示效果图
                Cv2.ImShow("矩形包围示例", image);
                Cv2.WaitKey(0);
            }
        }
    }
}


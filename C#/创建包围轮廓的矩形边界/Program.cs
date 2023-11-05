/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System.Collections.Generic;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
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

                // 绘制出最小面积的包围矩形
                RotatedRect box = Cv2.MinAreaRect(points);
                Point2f[] line = box.Points();
                Cv2.Line(image, (Point)line[0], (Point)line[1], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Line(image, (Point)line[1], (Point)line[2], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Line(image, (Point)line[2], (Point)line[3], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Line(image, (Point)line[3], (Point)line[0], new Scalar(0, 0, 255), 2, LineTypes.Link8);

                //显示效果图
                Cv2.ImShow("矩形包围示例", image);
                Cv2.WaitKey(0);
            }
        }
    }
}

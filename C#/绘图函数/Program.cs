/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：绘图函数
博客：http://www.bilibili996.com/Course?id=2699750000249
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        public static RNG rng = new RNG(12345);

        static void Main(string[] args)
        {
            demo1();
            //demo2();
            //demo3();
            //demo4();
        }

        public static void click_event(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            if (@event == MouseEventTypes.LButtonDown)
                System.Diagnostics.Debug.WriteLine($"{x},{y}");
        }

        private static void demo1()
        {
            Cv2.NamedWindow("Canvas", WindowFlags.GuiExpanded);
            Cv2.SetMouseCallback("Canvas", new MouseCallback(click_event));
            Mat canvas = new Mat(new Size(300, 300), MatType.CV_8UC3);

            while (true)
            {
                for (int i = 0; i < 25; i++)
                {
                    byte b = (byte)rng.Uniform(100, 255);
                    byte g = (byte)rng.Uniform(100, 255);
                    byte r = (byte)rng.Uniform(100, 255);

                    int radius = (byte)rng.Uniform(5, 200);
                    Scalar color = new Scalar(b, g, r);
                    Point pt = new Point((byte)rng.Uniform(0, 300), (byte)rng.Uniform(0, 300));
                    Cv2.Circle(canvas, pt, radius, color, -1);
                }
                Cv2.ImShow("Canvas", canvas);
                if (Cv2.WaitKey(1000) == 'q')
                    break;
            }
        }

        private static void demo2()
        {
            Console.WriteLine("freetype 操作，暂无");
        }

        private static void demo3()
        {
            int r1 = 70;
            int r2 = 30;
            int ang = 60;
            int d = 170;
            int h = (int)(d / 2 * Math.Sqrt(3));
            Point dot_red = new Point(256, 128);
            Point dot_green = new Point((int)(dot_red.X - d / 2), dot_red.Y + h);
            Point dot_blue = new Point((int)(dot_red.X + d / 2), dot_red.Y + h);
            Scalar red = new Scalar(0, 0, 255);
            Scalar green = new Scalar(0, 255, 0);
            Scalar blue = new Scalar(255, 0, 0);
            Scalar black = new Scalar(0, 0, 0);

            int full = -1;
            Mat img = new Mat(new Size(512, 512), MatType.CV_8UC3, new Scalar(0, 0, 0));

            Cv2.Circle(img, dot_red, r1, red, full);
            Cv2.Circle(img, dot_green, r1, green, full);
            Cv2.Circle(img, dot_blue, r1, blue, full);
            Cv2.Circle(img, dot_red, r2, black, full);
            Cv2.Circle(img, dot_green, r2, black, full);
            Cv2.Circle(img, dot_blue, r2, black, full);

            Cv2.Ellipse(img, dot_red, new Size(r1, r1), ang, 0, ang, black, full);
            Cv2.Ellipse(img, dot_green, new Size(r1, r1), 360 - ang, 0, ang, black, full);
            Cv2.Ellipse(img, dot_blue, new Size(r1, r1), 360 - 2 * ang, ang, 0, black, full);

            HersheyFonts font = HersheyFonts.HersheySimplex;
            Cv2.PutText(img, "OpenCV", new Point(15, 450), font, 4, new Scalar(255, 255, 255), 10);

            Cv2.ImShow("opencv_logo.jpg", img);
            Cv2.WaitKey(0);
        }

        private static void demo4()
        {
            Mat img = new Mat(new Size(512, 512), MatType.CV_8UC3, new Scalar(0, 0, 0));

            Cv2.Line(img, new Point(0, 0), new Point(511, 511), new Scalar(255, 0, 0), 5);
            // polylines() 可以 用来画很多条线。只需要把想 画的线放在一 个列表中， 将 列表传给函数就可以了。
            // 每条线 会被独立绘制。 这会比用 cv2.line() 一条一条的绘制 要快一些。

            Cv2.ArrowedLine(img, new Point(21, 13), new Point(151, 401), new Scalar(255, 0, 0), 5);
            Cv2.Rectangle(img, new Point(384, 0), new Point(510, 128), new Scalar(0, 255, 0), 3);
            Cv2.Circle(img, new Point(447, 63), 63, new Scalar(0, 0, 255), -1);

            // 一个参数是中心点的位置坐标。 下一个参数是长轴和短轴的长度。椭圆沿逆时针方向旋转的角度。
            // 椭圆弧演顺时针方向起始的角度和结束角度 如果是 0 很 360 就是整个椭圆
            Cv2.Ellipse(img, new Point(256, 256), new Size(100, 50), 0, 0, 180, 255, -1);

            // 这里 reshape 的第一个参数为 - 1, 表明这一维的长度是根据后面的维度的计算出来的。
            // 注意 如果第三个参数是 False 我们得到的多边形是不闭合的 ，首 尾不相  连 。

            HersheyFonts font = HersheyFonts.HersheySimplex;
            // 或使用 bottomLeftOrigin=True,文字会上下颠倒
            Cv2.PutText(img, "bottomLeftOrigin", new Point(10, 400), font, 1, new Scalar(255, 255, 255), 1, LineTypes.Link8, true);
            Cv2.PutText(img, "OpenCV", new Point(10, 500), font, 4, new Scalar(255, 255, 255), 2);

            Cv2.NamedWindow("example", 0);
            Cv2.ImShow("example", img);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}

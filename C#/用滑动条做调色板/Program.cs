/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        public static Mat img = new Mat();
        // 矩形和圆形模式切换
        public static bool mode = false;
        public static bool drawing = false;
        public static int ix = -1, iy = -1;
        public static void draw_circle(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            int r = Cv2.GetTrackbarPos("R", "image");
            int g = Cv2.GetTrackbarPos("G", "image");
            int b = Cv2.GetTrackbarPos("B", "image");

            // 当按下左键是返回起始位置坐标
            if (@event == MouseEventTypes.LButtonDown)
            {
                drawing = true;
                ix = x;
                iy = y;
            }
            if (@event == MouseEventTypes.MouseMove)
            {
                if (drawing)
                {
                    if (mode)
                    {
                        Cv2.Rectangle(img, new Point(ix, iy), new Point(x, y), new Scalar(b, g, r), -1);
                    }
                    else
                    {
                        // 绘制圆圈,小圆点连在一起就成了线,3 代表了笔画的粗细
                        Cv2.Circle(img, new Point(x, y), 3, new Scalar(b, g, r), -1);

                        //下面注释掉的代码是起始点为圆心,起点到终点为半径的
                        //int r = (int)(Math.Sqrt((x - ix) * (x - ix) + (y - iy) * (y - iy)));
                        //Cv2.Circle(img, new Point(x, y), r, new Scalar(b, g, r), -1);
                    }
                }
            }
            // 当鼠标松开停止绘画。
            if (@event == MouseEventTypes.LButtonUp)
            {
                drawing = false;
            }
        }

        public static void Main(string[] args)
        {
            img = new Mat(new Size(512, 512), MatType.CV_8UC3, new Scalar(0, 0, 0));
            Cv2.NamedWindow("image", WindowFlags.AutoSize);

            Cv2.CreateTrackbar("R", "image", 255, null);
            Cv2.CreateTrackbar("G", "image", 255, null);
            Cv2.CreateTrackbar("B", "image", 255, null);
            Cv2.SetMouseCallback("image", new MouseCallback(draw_circle));
            while (true)
            {
                Cv2.ImShow("image", img);
                if (Cv2.WaitKey(1) == 'm')
                {
                    mode = !mode;
                }
            }
        }
    }
}

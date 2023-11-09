using OpenCvSharp;
using System;

namespace demo
{
    /// <summary>
    /// 通过按键‘m’切换绘制圆和矩形
    /// </summary>
    public static class demo1
    {
        public static Mat img = new Mat();
        // 当鼠标按下时变为 True
        public static bool drawing = false;
        // 矩形和圆形模式切换
        public static bool mode = false;
        // 如果 mode 为 true 绘制矩形。按下'm' 变成绘制曲线。 mode=True
        public static int ix = -1, iy = -1;
        public static void onMouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            // 当按下左键是返回起始位置坐标
            if (@event == MouseEventTypes.LButtonDown)
            {
                drawing = true;
                ix = x;
                iy = y;
            }
            //  当鼠标左键按下并移动是绘制图形。event 可以查看移动,flag 查看是否按下
            if (@event == MouseEventTypes.MouseMove)
            {
                if (drawing)
                {
                    if (mode)
                    {
                        Cv2.Rectangle(img, new Point(ix, iy), new Point(x, y), new Scalar(0, 255, 0), -1);
                    }
                    else
                    {
                        // 绘制圆圈,小圆点连在一起就成了线,3 代表了笔画的粗细
                        Cv2.Circle(img, new Point(x, y), 3, new Scalar(0, 0, 255), -1);

                        //下面注释掉的代码是起始点为圆心,起点到终点为半径的
                        //int r = (int)(Math.Sqrt((x - ix) * (x - ix) + (y - iy) * (y - iy)));
                        //Cv2.Circle(img, new Point(x, y), r, new Scalar(0, 0, 255), -1);
                    }
                }
            }
            // 当鼠标松开停止绘画。
            if (@event == MouseEventTypes.LButtonUp)
            {
                drawing = false;
                //if (mode)
                //{
                //    Cv2.Rectangle(img, new Point(ix, iy), new Point(x, y), new Scalar(0, 255, 0), -1);
                //}
                //else
                //{
                //    Cv2.Circle(img, new Point(x, y), 5, new Scalar(0, 0, 255), -1);
                //}
            }
        }

        public static void Run()
        {
            img = new Mat(new Size(512, 512), MatType.CV_8UC3);
            mode = false;

            Cv2.NamedWindow("image");
            Cv2.SetMouseCallback("image", new MouseCallback(onMouse));
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

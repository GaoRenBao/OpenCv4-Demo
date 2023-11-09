using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    /// <summary>
    /// 通过双击绘制圆
    /// </summary>
    public static class demo2
    {
        public static Mat img = new Mat();

        // 只用做一件事:在双击过的地方绘制一个圆圈。
        public static void onMouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            if (@event == MouseEventTypes.LButtonDoubleClick)
            {
                Cv2.Circle(img, new Point(x, y), 100, new Scalar(255, 0, 0), -1);
            }
        }

        public static void Run()
        {
            img = new Mat(new Size(512, 512), MatType.CV_8UC3);

            Cv2.NamedWindow("image");
            Cv2.SetMouseCallback("image", new MouseCallback(onMouse));
            while (true)
            {
                Cv2.ImShow("image", img);
                Cv2.WaitKey(1);
            }
        }
    }
}

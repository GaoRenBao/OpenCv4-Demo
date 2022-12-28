using OpenCvSharp;
using System;

namespace ConsoleApp
{
    internal class Program
    {
        public static Mat img = new Mat();

        // 鼠标左右键回调函数
        public static void onMouse(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            if (@event == MouseEventTypes.LButtonDown)
            {
                Console.WriteLine($"{x},{y}");
            }
            if (@event == MouseEventTypes.RButtonDown)
            {
                Vec3b vec = img.Get<Vec3b>(y, x);
                byte blue = vec.Item0;
                byte green = vec.Item1;
                byte red = vec.Item2;
                Console.WriteLine($"{red},{green},{blue}");
                string strRGB = $"{red},{green},{blue}";

                var font = HersheyFonts.HersheySimplex;
                Cv2.PutText(img, strRGB, new Point(x, y), font, 1, new Scalar(255, 255, 255), 2);
                Cv2.ImShow("original", img);
            }
        }

        public static void Main(string[] args)
        {
            img = Cv2.ImRead("messi5.jpg");
            Cv2.ImShow("original", img);

            Cv2.NamedWindow("original");
            Cv2.SetMouseCallback("original", new MouseCallback(onMouse));
            Cv2.WaitKey(0);
            Console.Read();
        }
    }
}

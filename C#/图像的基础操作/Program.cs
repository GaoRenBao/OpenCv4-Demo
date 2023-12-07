/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：图像的基础操作
博客：http://www.bilibili996.com/Course?id=5178073000253
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            demo1();
            demo2();
            demo3();
        }

        // 使所有像素的红色通道值都为 0
        public static void demo1()
        {
            Mat img = Cv2.ImRead("../../../images/messi5.jpg");
            Mat[] mv;
            Cv2.Split(img, out mv); // 比较耗时的操作
            Cv2.Merge(mv, img);
            // 获取三通道颜色
            Mat b = mv[0];
            Mat g = mv[1];
            Mat r = mv[2];

            //使所有像素的红色通道值都为 0,你不必先拆分再赋值。
            mv[0] = new Mat(mv[0].Size(), mv[0].Type(), new Scalar(0));
            Cv2.Merge(mv, img);
            Cv2.ImShow("DEMO1", img);
            Cv2.WaitKey(0);
        }

        // 输出图像的长宽以及通道数
        public static void demo2()
        {
            Mat img = Cv2.ImRead("../../../images/messi5.jpg");
            System.Console.WriteLine($"行/高:{img.Rows}");
            System.Console.WriteLine($"列/宽:{img.Cols}");
            System.Console.WriteLine($"通道:{img.Channels()}");
            System.Console.Read();
        }

        // 边界扩张与虚化
        public static void demo3()
        {
            Mat replicate = new Mat();
            Mat reflect = new Mat();
            Mat reflect101 = new Mat();
            Mat wrap = new Mat();
            Mat constant = new Mat();

            Mat img = Cv2.ImRead("../../../images/messi5.jpg");
            Cv2.CopyMakeBorder(img, replicate, 50, 50, 50, 50, BorderTypes.Replicate);
            Cv2.CopyMakeBorder(img, reflect, 50, 50, 50, 50, BorderTypes.Reflect);
            Cv2.CopyMakeBorder(img, reflect101, 50, 50, 50, 50, BorderTypes.Reflect101);
            Cv2.CopyMakeBorder(img, wrap, 50, 50, 50, 50, BorderTypes.Wrap);
            Cv2.CopyMakeBorder(img, constant, 50, 50, 50, 50, BorderTypes.Constant, new Scalar(255, 0, 0)); // 边界颜色

            Cv2.ImShow("replicate", replicate);
            Cv2.ImShow("reflect", reflect);
            Cv2.ImShow("reflect101", reflect101);
            Cv2.ImShow("wrap", wrap);
            Cv2.ImShow("constant", constant);
            Cv2.WaitKey(0);
        }
    }
}

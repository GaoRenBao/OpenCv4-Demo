/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：MSER区域检测
博客：http://www.bilibili996.com/Course?id=09740476aebb4690a6f57e56063b28c8
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat img = Cv2.ImRead("../../../images/WQbGH.jpg");
            img = new Mat(img, new Rect() { X = 5, Y = 5, Width = img.Width - 10, Height = img.Height - 10 });

            MSER mser = MSER.Create();

            // Resize the image so that MSER can work better
            Mat img2 = new Mat();
            Cv2.Resize(img, img2, new Size(img.Cols * 2, img.Rows * 2)); // 扩大

            Mat gray = new Mat();
            Cv2.CvtColor(img2, gray, ColorConversionCodes.BGR2GRAY);
            Mat vis = new Mat();
            img2.CopyTo(vis);

            mser.DetectRegions(gray, out Point[][] msers, out Rect[] bboxes);

            Point[][] hulls = new Point[msers.Length][];
            for (int i = 0; i < msers.Length; i++)
            {
                hulls[i] = Cv2.ConvexHull(msers[i]);
            }
            Cv2.Polylines(vis, hulls, true, new Scalar(0, 255, 0));

            Mat img3 = new Mat();
            Cv2.Resize(vis, img3, img.Size());
            Cv2.NamedWindow("img", 0);
            Cv2.ImShow("img", img3);
            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}

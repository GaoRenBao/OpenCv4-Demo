/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：LSD快速直线检测
博客：http://www.bilibili996.com/Course?id=600776a4089d4f99b46a0fd854e8c74d
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
            Mat img0 = Cv2.ImRead("../../../images/home.jpg");
            Mat img = new Mat();
            Cv2.CvtColor(img0, img, ColorConversionCodes.BGR2GRAY);
            Cv2.ImShow("pokerQ", img0);

            // Create default parametrization LSD
            LineSegmentDetector lsd = LineSegmentDetector.Create(LineSegmentDetectorModes.RefineNone);
            lsd.Detect(img, out Vec4f[] lines, out double[] width, out double[] prec, out double[] nfa);

            for (int i = 0; i < lines.Length; i++)
            {
                int x0 = (int)lines[i].Item0;
                int y0 = (int)lines[i].Item1;
                int x1 = (int)lines[i].Item2;
                int y1 = (int)lines[i].Item3;
                Cv2.Line(img0, new Point(x0, y0), new Point(x1, y1), new Scalar(0, 255, 0), 2, LineTypes.AntiAlias);
                Cv2.ImShow("LSD", img0);
                Cv2.WaitKey(10);
            }

            // 绘图线检测结果
            Mat mats = new Mat { };
            lsd.Detect(img, mats);
            lsd.DrawSegments(img, mats);
            Cv2.ImShow("LSD", img);
            Cv2.WaitKey(0);
        }
    }
}

/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
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
            Mat img_rgb = Cv2.ImRead("../../../images/mario.png");
            Mat img_gray = new Mat();
            Cv2.CvtColor(img_rgb, img_gray, ColorConversionCodes.BGR2GRAY);
            Mat template = Cv2.ImRead("../../../images/mario_coin.png", 0);

            int w = template.Width;
            int h = template.Height;

            Mat res = new Mat();
            Cv2.MatchTemplate(img_gray, template, res, TemplateMatchModes.CCoeffNormed);

            double threshold = 0.8;
            Cv2.Threshold(res, res, threshold, 1.0, ThresholdTypes.Tozero);
            res.FindNonZero().GetArray(out Point[] points);

            for (int i = 0; i < points.Length; i++)
            {
                Point pt = points[i];
                Cv2.Rectangle(img_rgb, pt, new Point(pt.X + w, pt.Y + h), new Scalar(0, 255, 0), 2);
            }

            Cv2.ImShow("result", img_rgb);
            Cv2.WaitKey(0);
        }
    }
}

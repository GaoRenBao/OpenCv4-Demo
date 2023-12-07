/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：删除图像中的水印
博客：http://www.bilibili996.com/Course?id=b608bdd358e041478931886b8b08058c
作者：高仁宝
时间：2023.11

来源：https://stackoverflow.com/questions/32125281/removing-watermark-out-of-an-image-using-opencv
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat im = Cv2.ImRead("../../../images/YZeOg.jpg");

            // 原图有点大。。。
            Cv2.Resize(im, im, new Size(im.Cols * 0.5, im.Rows * 0.5));
            Cv2.ImShow("im", im);

            Mat gr = new Mat();
            Mat bg = new Mat();
            Mat bw = new Mat();
            Mat dark = new Mat();
            Cv2.CvtColor(im, gr, ColorConversionCodes.BGR2GRAY);

            // approximate the background
            gr.CopyTo(bg);
            for (int r = 1; r < 5; r++)
            {
                Mat kernel2 = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(2 * r + 1, 2 * r + 1));
                Cv2.MorphologyEx(bg, bg, MorphTypes.Close, kernel2);
                Cv2.MorphologyEx(bg, bg, MorphTypes.Open, kernel2);
            }

            // difference = background - initial
            Mat dif = bg - gr;
            // threshold the difference image so we get dark letters
            Cv2.Threshold(dif, bw, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
            // threshold the background image so we get dark region
            Cv2.Threshold(bg, dark, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

            // extract pixels in the dark region
            byte[] darkpix = new byte[Cv2.CountNonZero(dark)];
            int index = 0;
            for (int r = 0; r < dark.Rows; r++)
            {
                for (int c = 0; c < dark.Cols; c++)
                {
                    if (dark.At<byte>(r, c) > 0)
                    {
                        darkpix[index++] = gr.At<byte>(r, c);
                    }
                }
            }
            Mat mat = Mat.FromArray(darkpix);
            // threshold the dark region so we get the darker pixels inside it
            Cv2.Threshold(mat, mat, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            mat.GetArray(out darkpix);

            // paste the extracted darker pixels
            index = 0;
            for (int r = 0; r < dark.Rows; r++)
            {
                for (int c = 0; c < dark.Cols; c++)
                {
                    if (dark.At<byte>(r, c) > 0)
                    {
                        bw.At<byte>(r, c) = darkpix[index++];
                    }
                }
            }

            Cv2.ImShow("BW", bw);
            Cv2.WaitKey(0);
        }
    }
}

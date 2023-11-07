/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：几何变换
博客：http://www.bilibili996.com/Course?id=1154656000256
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
            demo1();
            //demo2();
            //demo3();
            //demo4();
        }

        #region 图像的平移操作
        public static void demo1()
        {
            // 移动了100,50 个像素。
            Mat img = Cv2.ImRead("../../../images/messi5.jpg", 0);

            // Define the transformation matrix
            Mat M = new Mat(2, 3, MatType.CV_32FC1);
            M.Set<float>(0, 0, 1);
            M.Set<float>(0, 1, 0);
            M.Set<float>(0, 2, 100);
            M.Set<float>(1, 0, 0);
            M.Set<float>(1, 1, 1);
            M.Set<float>(1, 2, 50);

            // 应用平移变换  
            Mat dst = new Mat();
            Cv2.WarpAffine(img, dst, M, img.Size());

            Cv2.ImShow("img", dst);
            Cv2.WaitKey(0);
        }
        #endregion

        #region 旋转图像操作
        public static void demo2()
        {
            Mat img = Cv2.ImRead("../../../images/messi5.jpg", 0);
            Mat dst = new Mat();
            Mat rotMat = Cv2.GetRotationMatrix2D(new Point2f(img.Cols / 2, img.Rows / 2), 45, 0.5);
            Cv2.WarpAffine(img, dst, rotMat, new Size(img.Cols, img.Rows));
            Cv2.ImShow("img", dst);
            Cv2.WaitKey(0);
        }
        #endregion

        #region 仿射变换的基本操作
        public static void demo3()
        {
            Mat img = Cv2.ImRead("../../../images/drawing.png");

            Mat pts1 = new Mat(3, 2, MatType.CV_32FC1);
            pts1.Set<float>(0, 0, 50);
            pts1.Set<float>(0, 1, 50);
            pts1.Set<float>(1, 0, 200);
            pts1.Set<float>(1, 1, 50);
            pts1.Set<float>(2, 0, 50);
            pts1.Set<float>(2, 1, 200);

            Mat pts2 = new Mat(3, 2, MatType.CV_32FC1);
            pts2.Set<float>(0, 0, 10);
            pts2.Set<float>(0, 1, 100);
            pts2.Set<float>(1, 0, 200);
            pts2.Set<float>(1, 1, 50);
            pts2.Set<float>(2, 0, 100);
            pts2.Set<float>(2, 1, 250);

            Mat M = Cv2.GetAffineTransform(pts1, pts2);
            Mat dst = new Mat();
            Cv2.WarpAffine(img, dst, M, img.Size());

            Cv2.ImShow("Input", img);
            Cv2.ImShow("Output", dst);
            Cv2.WaitKey(0);
        }
        #endregion

        #region 透视变换的基本操作
        public static void demo4()
        {
            Mat img = Cv2.ImRead("../../../images/sudoku.jpg");

            Mat pts1 = new Mat(4, 2, MatType.CV_32FC1);
            pts1.Set<float>(0, 0, 56);
            pts1.Set<float>(0, 1, 65);
            pts1.Set<float>(1, 0, 368);
            pts1.Set<float>(1, 1, 52);
            pts1.Set<float>(2, 0, 28);
            pts1.Set<float>(2, 1, 387);
            pts1.Set<float>(3, 0, 389);
            pts1.Set<float>(3, 1, 390);

            Mat pts2 = new Mat(4, 2, MatType.CV_32FC1);
            pts2.Set<float>(0, 0, 0);
            pts2.Set<float>(0, 1, 0);
            pts2.Set<float>(1, 0, 300);
            pts2.Set<float>(1, 1, 0);
            pts2.Set<float>(2, 0, 0);
            pts2.Set<float>(2, 1, 300);
            pts2.Set<float>(3, 0, 300);
            pts2.Set<float>(3, 1, 300);

            Mat M = Cv2.GetPerspectiveTransform(pts1, pts2);
            Mat dst = new Mat();
            Cv2.WarpPerspective(img, dst, M, new Size(300, 300));

            Cv2.ImShow("Input", img);
            Cv2.ImShow("Output2", dst);
            Cv2.WaitKey(0);
        }
        #endregion
    }
}

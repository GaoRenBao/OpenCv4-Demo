/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：对极几何
博客：http://www.bilibili996.com/Course?id=52c06e9172914d5eb6443fbd8c466974
作者：高仁宝
时间：2023.11
资料：
https://docs.opencv.org/4.5.5/da/de9/tutorial_py_epipolar_geometry.html
https://blog.csdn.net/zhoujinwang/article/details/128349114
*/

using OpenCvSharp;
using OpenCvSharp.Features2D;
using OpenCvSharp.Flann;
using System.Linq;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat img1 = Cv2.ImRead("../images/myleft.jpg", 0);
            Mat img2 = Cv2.ImRead("../images/myright.jpg", 0);

            var sift = SIFT.Create();
            Mat des1 = new Mat();
            Mat des2 = new Mat();
            KeyPoint[] kp1, kp2;
            sift.DetectAndCompute(img1, null, out kp1, des1);
            sift.DetectAndCompute(img2, null, out kp2, des2);

            IndexParams index_params = new IndexParams();
            index_params.SetAlgorithm(0);

            SearchParams search_params = new SearchParams(50);
            FlannBasedMatcher flann = new FlannBasedMatcher(index_params, search_params);

            DMatch[][] matches = flann.KnnMatch(des1, des2, 2);

            Point2f[] pts1 = new Point2f[] { };
            Point2f[] pts2 = new Point2f[] { };
            for (int i = 0; i < matches.Length; i++)
            {
                if (matches[i][0].Distance < 0.7 * matches[i][1].Distance)
                {
                    pts2.Append(kp2[matches[i][0].TrainIdx].Pt);
                    pts1.Append(kp1[matches[i][0].QueryIdx].Pt);
                }
            }

            // 匹配点列表，用它来计算【基础矩阵】
            Mat F = Cv2.FindFundamentalMat(pts1, pts2, FundamentalMatMethods.LMedS);

            //Point2d[] ptss1 = new Point2d[] { };
            //Point2d[] ptss2 = new Point2d[] { };
            //for (int i = 0; i < pts1.Length; i++)
            //{
            //    ptss1.Append(new Point2d(pts1[i].X, pts1[i].Y));
            //}
            //for (int i = 0; i < pts2.Length; i++)
            //{
            //    ptss2.Append(new Point2d(pts2[i].X, pts2[i].Y));
            //}

            // 计算极线
            Mat epilines1 = new Mat();
            Mat epilines2 = new Mat();
            // 这里报错。。。
            Cv2.ComputeCorrespondEpilines(pts1, 1, F, epilines1);
            Cv2.ComputeCorrespondEpilines(pts2, 2, F, epilines2);

            // 显示图像和极线
            for (int i = 0; i < pts1.Length; i++)
            {
                Cv2.Line(img1, new Point(0, (int)(-epilines1.At<float>(i, 0) / epilines1.At<float>(i, 1))),
                    new Point(img1.Cols, (int)((-epilines1.At<float>(i, 2) - epilines1.At<float>(i, 0) * img1.Cols) / epilines1.At<float>(i, 1))), Scalar.Red);

                Cv2.Line(img2, new Point(0, (int)(-epilines2.At<float>(i, 0) / epilines2.At<float>(i, 1))),
                    new Point(img2.Cols, (int)((-epilines2.At<float>(i, 2) - epilines2.At<float>(i, 0) * img2.Cols) / epilines2.At<float>(i, 1))), Scalar.Red);
            }

            Cv2.ImShow("img1", img1);
            Cv2.ImShow("img2", img2);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}

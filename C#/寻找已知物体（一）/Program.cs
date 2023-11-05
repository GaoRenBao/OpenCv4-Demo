/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System.Collections.Generic;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // queryImage
            Mat img1 = Cv2.ImRead("../../../images/book_box.jpg");
            // trainImage
            Mat img2 = Cv2.ImRead("../../../images/book2.jpg");
            var Sift = OpenCvSharp.Features2D.SIFT.Create();

            KeyPoint[] kp1, kp2;
            Mat des1 = new Mat();
            Mat des2 = new Mat();
            Sift.DetectAndCompute(img1, null, out kp1, des1);
            Sift.DetectAndCompute(img2, null, out kp2, des2);

            //BFMatcher with default params
            BFMatcher bf = new BFMatcher();
            DMatch[][] matches = bf.KnnMatch(des1, des2, 2);

            List<DMatch[]> good = new List<DMatch[]>();
            //Apply ratio test
            //比值测试，首先获取与 A距离最近的点 B （最近）和 C （次近），
            //只有当 B/C 小于阀值时（0.75）才被认为是匹配，
            //因为假设匹配是一一对应的，真正的匹配的理想距离为0
            for (int i = 0; i < matches.Length; i++)
            {
                if (matches[i][0].Distance < 0.75 * matches[i][1].Distance)
                    good.Add(matches[i]);
            }

            //cv2.drawMatchesKnn expects list of lists as matches.
            Mat img3 = new Mat();
            Cv2.DrawMatchesKnn(img1, kp1, img2, kp2, good.ToArray(), img3, flags: DrawMatchesFlags.NotDrawSinglePoints);

            Cv2.ImShow("out", img3);
            Cv2.WaitKey(0);


        }
    }
}

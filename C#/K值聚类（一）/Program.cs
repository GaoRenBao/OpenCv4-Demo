/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：K值聚类（一）
博客：http://www.bilibili996.com/Course?id=7d02676837f34dcc84606d906d877c3b
作者：高仁宝
时间：2023.11

参考代码：
https://github.com/VahidN/OpenCVSharp-Samples/tree/master/OpenCVSharpSample11
https://github.com/Itseez/opencv_extra/blob/master/learning_opencv_v2/ch13_ex13_1.cpp
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int maxClusters = 5;
            var rng = new RNG(state: (ulong)DateTime.Now.Ticks);
            while (true)
            {
                var clustersCount = rng.Uniform(a: 2, b: maxClusters + 1);
                var samplesCount = rng.Uniform(a: 1, b: 1001);

                var points = new Mat(rows: samplesCount, cols: 1, type: MatType.CV_32FC2);
                clustersCount = Math.Min(clustersCount, samplesCount);

                var img = new Mat(rows: 500, cols: 500, type: MatType.CV_8UC4, s: Scalar.All(0));

                // generate random sample from multi-gaussian distribution
                for (var k = 0; k < clustersCount; k++)
                {
                    var pointChunk = points.RowRange(
                            startRow: k * samplesCount / clustersCount,
                            endRow: (k == clustersCount - 1)
                                ? samplesCount
                                : (k + 1) * samplesCount / clustersCount);

                    var center = new Point
                    {
                        X = rng.Uniform(a: 0, b: img.Cols),
                        Y = rng.Uniform(a: 0, b: img.Rows)
                    };
                    rng.Fill(
                        mat: pointChunk,
                        distType: DistributionType.Normal,
                        a: new Scalar(center.X, center.Y),
                        b: new Scalar(img.Cols * 0.05f, img.Rows * 0.05f));
                }

                Cv2.RandShuffle(dst: points, iterFactor: 1, ref rng);

                var labels = new Mat();
                var centers = new Mat(rows: clustersCount, cols: 1, type: points.Type());
                Cv2.Kmeans(
                    data: points,
                    k: clustersCount,
                    bestLabels: labels,
                    criteria: new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 10, 1.0),
                    attempts: 3,
                    flags: KMeansFlags.PpCenters,
                    centers: centers);


                Scalar[] colors =
                {
                       new Scalar(0, 0, 255),
                       new Scalar(0, 255, 0),
                       new Scalar(255, 100, 100),
                       new Scalar(255, 0, 255),
                       new Scalar(0, 255, 255)
                    };

                for (var i = 0; i < samplesCount; i++)
                {
                    var clusterIdx = labels.At<int>(i);
                    Point2f ipt = points.At<Point2f>(i);
                    Cv2.Circle(
                        img: img,
                        center: new Point(ipt.X, ipt.Y),
                        radius: 2,
                        color: colors[clusterIdx],
                        lineType: LineTypes.AntiAlias,
                        thickness: 1);
                }
                Cv2.ImShow("img", img);
                Cv2.ImWrite("img.jpg", img);
                var key = (char)Cv2.WaitKey();
                if (key == 27 || key == 'q' || key == 'Q') // 'ESC'
                {
                    break;
                }
            }
        }
    }
}

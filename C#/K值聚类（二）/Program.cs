/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：K值聚类（二）
博客：http://www.bilibili996.com/Course?id=fcd7745b72f4493f81683b2e3e80f911
作者：高仁宝
时间：2023.11

参考代码：https://github.com/VahidN/OpenCVSharp-Samples/tree/master/OpenCVSharpSample11
*/

using OpenCvSharp;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var src = new Mat(@"../../../images/fruits.jpg", ImreadModes.AnyDepth | ImreadModes.AnyColor);
            Cv2.ImShow("Source", src);
            Cv2.WaitKey(1); // do events

            Cv2.Blur(src, src, new Size(15, 15));
            Cv2.ImShow("Blurred Image", src);
            Cv2.WaitKey(1); // do events

            // Converts the MxNx3 image into a Kx3 matrix where K=MxN and
            // each row is now a vector in the 3-D space of RGB.
            // change to a Mx3 column vector (M is number of pixels in image)
            var columnVector = src.Reshape(cn: 3, rows: src.Rows * src.Cols);

            // convert to floating point, it is a requirement of the k-means method of OpenCV.
            var samples = new Mat();
            columnVector.ConvertTo(samples, MatType.CV_32FC3);

            TermCriteria criteria = new TermCriteria(type: CriteriaTypes.Eps | CriteriaTypes.MaxIter, maxCount: 10, epsilon: 1.0);
            int K = 2; // 2、4、6、8

            var bestLabels = new Mat();
            var centers = new Mat();
            Cv2.Kmeans(
                data: samples,
                k: K,
                bestLabels: bestLabels,
                criteria: criteria,
                attempts: 3,
                flags: KMeansFlags.PpCenters,
                centers: centers);

            var clusteredImage = new Mat(src.Rows, src.Cols, src.Type());
            for (var size = 0; size < src.Cols * src.Rows; size++)
            {
                var clusterIndex = bestLabels.At<int>(0, size);
                var newPixel = new Vec3b
                {
                    Item0 = (byte)(centers.At<float>(clusterIndex, 0)), // B
                    Item1 = (byte)(centers.At<float>(clusterIndex, 1)), // G
                    Item2 = (byte)(centers.At<float>(clusterIndex, 2)) // R
                };
                clusteredImage.Set(size / src.Cols, size % src.Cols, newPixel);
            }

            Cv2.ImShow(string.Format("Clustered Image [k:{0}]", K), clusteredImage);
            Cv2.ImWrite("out.jpg", clusteredImage);
            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}

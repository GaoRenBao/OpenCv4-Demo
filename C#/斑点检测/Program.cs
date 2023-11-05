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
        public static void Main(string[] args)
        {
            // Read image
            Mat im = Cv2.ImRead("../../../images/blob.jpg", ImreadModes.Grayscale);
            // 创建SimpleBlobDetector.Params对象并设置参数
            var detectorParams = new SimpleBlobDetector.Params
            {
                //MinDistBetweenBlobs = 10, // 10 pixels between blobs
                //MinRepeatability = 1,

                //MinThreshold = 100,
                //MaxThreshold = 255,
                //ThresholdStep = 5,

                FilterByColor = false,
                //BlobColor = 255 // to extract light blobs

                // 斑点面积
                FilterByArea = true,
                MinArea = 100, // 10 pixels squared
                MaxArea = 10000,

                // 斑点圆度
                FilterByCircularity = false,
                //MinCircularity = 0.001f,
                //MaxCircularity = 0.001f,

                //斑点凸度
                FilterByConvexity = false,
                //MinConvexity = 0.001f,
                //MaxConvexity = 10,

                //斑点惯性率
                FilterByInertia = false,
                //MinInertiaRatio = 0.001f,
                //MaxInertiaRatio  = 0.001f,
            };
            // Set up the detector with default parameters.
            SimpleBlobDetector detector = SimpleBlobDetector.Create(detectorParams);
            // Detect blobs.
            var keypoints = detector.Detect(im);

            // Draw detected blobs as red circles.
            // cv2.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS ensures the size of the circle corresponds to the size of blob
            var im_with_keypoints = new Mat();
            Cv2.DrawKeypoints(
                    image: im,
                    keypoints: keypoints,
                    outImage: im_with_keypoints,
                    color: Scalar.FromRgb(255, 0, 0),
                    flags: DrawMatchesFlags.DrawRichKeypoints);

            Cv2.ImShow("Keypoints.jpg", im_with_keypoints);
            Cv2.WaitKey(0);
        }
    }
}

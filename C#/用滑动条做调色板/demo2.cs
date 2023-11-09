using OpenCvSharp;

namespace demo
{
    /// <summary>
    /// 调节画布颜色
    /// </summary>
    public static class demo2
    {
        public static void Run()
        {
            string swit = "0 : OFF \n1 : ON";
            Mat img = new Mat(new Size(512, 300), MatType.CV_8UC3, new Scalar(0, 0, 0));
            Cv2.NamedWindow("image", WindowFlags.AutoSize);

            Cv2.CreateTrackbar("R", "image", 255, null);
            Cv2.CreateTrackbar("G", "image", 255, null);
            Cv2.CreateTrackbar("B", "image", 255, null);
            Cv2.CreateTrackbar(swit, "image", 1, null);

            while (true)
            {
                int r = Cv2.GetTrackbarPos("R", "image");
                int g = Cv2.GetTrackbarPos("G", "image");
                int b = Cv2.GetTrackbarPos("B", "image");
                int s = Cv2.GetTrackbarPos(swit, "image");

                if (s == 0)
                    img = new Mat(new Size(512, 300), MatType.CV_8UC3, new Scalar(0, 0, 0));
                else
                    img = new Mat(new Size(512, 300), MatType.CV_8UC3, new Scalar(b, g, r));

                Cv2.ImShow("image", img);
                Cv2.WaitKey(1);
                img.Dispose();
            }
        }
    }
}

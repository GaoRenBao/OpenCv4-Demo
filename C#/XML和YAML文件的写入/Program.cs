/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
博客：http://www.bilibili996.com/Course/article_list?id=20224789774006
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //开始文件写入
            // opencv3：FileStorage.Mode.Write
            // opencv4：FileStorage.Modes.Write
            using (var fs = new FileStorage("test.yaml", FileStorage.Modes.Write))
            {
                fs.Add("frameCount").Add(5);
                fs.Add("calibrationDate").Add(DateTime.Now.ToString());

                Array data1 = new double[] { 1000, 0, 320, 0, 1000, 240, 0, 0, 1 };
                Mat cameraMatrix = new Mat(3, 3, MatType.CV_64FC1, data1);
                fs.Add("cameraMatrix").Add(cameraMatrix);

                Array data2 = new double[] { 0.1, 0.01, -0.001, 0, 0 };
                Mat distCoeffs = new Mat(5, 1, MatType.CV_64FC1, data2);
                fs.Add("distCoeffs").Add(distCoeffs);

                Random g_rng = new Random();
                fs.Add("features").Add("[");
                for (int i = 0; i < 3; i++)
                {
                    int x = g_rng.Next(255) % 640;
                    int y = g_rng.Next(255) % 480;
                    byte lbp = (byte)(g_rng.Next(255) % 256);
                    fs.Add("{:")
                    .Add("x").Add(x)
                    .Add("y").Add(y)
                    .Add("lbp").Add("[:");
                    for (int j = 0; j < 8; j++)
                        fs.Add((lbp >> j) & 1);
                    fs.Add("]")
                    .Add("}");
                }
                fs.Add("]");
            }
        }
    }
}

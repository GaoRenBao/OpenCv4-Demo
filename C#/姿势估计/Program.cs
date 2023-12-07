/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：姿势估计
博客：http://www.bilibili996.com/Course?id=f2a6235f09a849a9a93974ce8b00272f
作者：高仁宝
时间：2023.11

参考代码：https://github.com/shimat/opencvsharp/tree/main/test/OpenCvSharp.Tests/calib3d/Calib3dTest.cs
*/

using OpenCvSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> paths = new List<string>()
            {
                "../../../images/lefts/",
                "../../../images/rights/"
            };
            List<string> imagesList = new List<string>();
            foreach (string path in paths)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fis = di.GetFiles("*.jpg");
                Mat srcImage = new Mat();
                foreach (FileInfo fi in fis)
                {
                    imagesList.Add(fi.FullName);
                }
            }

            TermCriteria criteria = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 30, 0.001);

            // 棋盘格内角点数量（行数和列数）
            Size BoardSize = new Size(9, 6);

            // 棋盘格尺寸（单位：米）
            float squareSize = 0.025f;

            Mat img = new Mat();
            Mat gray = new Mat();

            List<List<Point3f>> objpoints = new List<List<Point3f>>();
            List<List<Point2f>> imgpoints = new List<List<Point2f>>();

            for (int i = 0; i < imagesList.Count; i++)
            {
                img = new Mat(imagesList[i]);
                if (!img.Empty())
                {
                    Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

                    var corners = new Mat<Point2f>();
                    bool found = Cv2.FindChessboardCorners(gray, BoardSize, corners);
                    if (found == true)
                    {
                        var objectPointsArray = Create3DChessboardCorners(BoardSize, squareSize).ToArray();
                        var imagePointsArray = corners.ToArray();

                        //var objectPoints = Mat<Point3f>.FromArray(objectPointsArray);
                        //var imagePoints = Mat<Point2f>.FromArray(imagePointsArray);

                        objpoints.Add(objectPointsArray.ToList());
                        imgpoints.Add(imagePointsArray.ToList());
                    }
                }
            }

            var mtx = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            var dist = new double[5];
            Cv2.CalibrateCamera(objpoints, imgpoints, gray.Size(), mtx,
                dist, out var rvecs, out var tvecs, CalibrationFlags.None);

            List<Point3f> objPts = new List<Point3f>();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    objPts.Add(new Point3f(j, i, 0));
                }
            }

            // X、Y、Z三轴方向，占3格
            var axis = new[]
            {
                new Point3f(3,0,0),
                new Point3f(0,3,0),
                new Point3f(0,0,-3),
            };

            foreach (string fname in imagesList)
            {
                img = Cv2.ImRead(fname);
                Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);
                Point2f[] corners;
                bool found = Cv2.FindChessboardCorners(img, BoardSize, out corners);
                if (found == true)
                {
                    var corners2 = Cv2.CornerSubPix(gray, corners, new Size(11, 11), new Size(-1, -1), criteria);

                    var rvec = new double[] { };
                    var tvec = new double[] { };
                    Cv2.SolvePnP(objPts, corners2, mtx, dist, ref rvec, ref tvec);

                    Cv2.ProjectPoints(axis, rvec, tvec, mtx, dist, out var imgPts, out var jacobian);
                    img = draw(img, corners2, imgPts);
                    Cv2.ImShow("img", img);
                    if (Cv2.WaitKey(0) == 27)
                        break;
                }
            }
        }

        private static IEnumerable<Point3f> Create3DChessboardCorners(Size boardSize, float squareSize)
        {
            for (int y = 0; y < boardSize.Height; y++)
            {
                for (int x = 0; x < boardSize.Width; x++)
                {
                    yield return new Point3f(x * squareSize, y * squareSize, 0);
                }
            }
        }

        static Mat draw(Mat img, Point2f[] corners, Point2f[] imgpts)
        {
            Point l0 = new Point((int)corners[0].X, (int)corners[0].Y);
            Point l1 = new Point((int)imgpts[0].X, (int)imgpts[0].Y);
            Point l2 = new Point((int)imgpts[1].X, (int)imgpts[1].Y);
            Point l3 = new Point((int)imgpts[2].X, (int)imgpts[2].Y);
            Cv2.Line(img, l0, l1, new Scalar(255, 0, 0), 5);
            Cv2.Line(img, l0, l2, new Scalar(0, 255, 0), 5);
            Cv2.Line(img, l0, l3, new Scalar(0, 0, 255), 5);
            return img;
        }
    }
}

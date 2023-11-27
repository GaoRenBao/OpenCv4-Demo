/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：摄像机标定
博客：http://www.bilibili996.com/Course?id=3b7dadd95f6a4508867768c07a59db3e
作者：高仁宝
时间：2023.11

参考博客：https://blog.csdn.net/jimtien/article/details/119009460

*/

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //demo1();
            demo2();
        }

        static List<Point3f> GetChessboardObjectPoints(Size boardSize, float squareSize)
        {
            List<Point3f> objectPoints = new List<Point3f>() { };
            for (int i = 0; i < boardSize.Height; ++i)
            {
                for (int j = 0; j < boardSize.Width; ++j)
                {
                    objectPoints.Add(new Point3f(squareSize * j, squareSize * i, 0.0f));
                }
            }
            return objectPoints;
        }

        public static Mat[] calcBoardCornerPositions(Size BoardSize, float SquareSize, int imagesCount)
        {
            Mat[] corners = new Mat[imagesCount];
            for (int k = 0; k < imagesCount; k++)
            {
                Point3f[] p = new Point3f[BoardSize.Height * BoardSize.Width];
                for (int i = 0; i < BoardSize.Height; i++)
                {
                    for (int j = 0; j < BoardSize.Width; j++)
                    {
                        p[i * BoardSize.Width + j] = new Point3f(j * SquareSize, i * SquareSize, 0);
                    }
                }
                corners[k] = Mat.FromArray<Point3f>(p);
            }
            return corners;
        }

        static void demo1()
        {
            List<string> paths = new List<string>()
            {
                "../../../images/lefts/",
                "../../../images/rights/"
            };
            List<string> images = new List<string>();
            foreach (string path in paths)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fis = di.GetFiles("*.jpg");
                Mat srcImage = new Mat();
                foreach (FileInfo fi in fis)
                {
                    images.Add(fi.FullName);
                }
            }
            TermCriteria criteria = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 30, 0.001);

            // 棋盘格内角点数量（行数和列数）
            Size chessboardSize = new Size(7, 6);

            // 棋盘格尺寸（单位：米）
            float squareSize = 0.025f;

            // 存储棋盘格角点坐标的容器
            List<List<Point3f>> objpoints = new List<List<Point3f>>();
            // 存储检测到的棋盘格角点坐标的容器
            List<List<Point2f>> imgpoints = new List<List<Point2f>>();
            Mat gray = new Mat();

            foreach (var image in images)
            {
                Mat img = Cv2.ImRead(image);
                if (!img.Empty())
                {
                    Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

                    // Find the chess board corners
                    Point2f[] corners = new Point2f[] { };
                    bool ret = Cv2.FindChessboardCorners(gray, chessboardSize, out corners);
                    // If found, add object points, image points(after refining them)
                    if (ret == true)
                    {
                        Cv2.CornerSubPix(gray, corners, new Size(11, 11), new Size(-1, -1), criteria);
                        // Draw and display the corners
                        Cv2.DrawChessboardCorners(img, chessboardSize, corners, ret);

                        objpoints.Add(GetChessboardObjectPoints(chessboardSize, squareSize));
                        imgpoints.Add(corners.ToList());

                        Cv2.ImShow("img", img);
                        Cv2.WaitKey(100);
                    }
                }
            }
        }

        static void demo2()
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

            Size BoardSize = new Size(9, 6);
            int SquareSize = 25; //每格的宽度应设置为实际的毫米数

            Mat img = new Mat();
            Mat gray = new Mat();

            List<Mat> imagesPointsM = new List<Mat>();
            List<Point2f[]> imgpoints = new List<Point2f[]>();

            for (int i = 0; i < imagesList.Count; i++)
            {
                img = new Mat(imagesList[i]);
                if (!img.Empty())
                {
                    Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

                    Point2f[] pointBuf;
                    bool found = Cv2.FindChessboardCorners(img, BoardSize, out pointBuf);
                    if (found == true)
                    {
                        Mat viewGray = new Mat();
                        Cv2.CvtColor(img, viewGray, ColorConversionCodes.BGR2GRAY);
                        Cv2.CornerSubPix(viewGray, pointBuf, new Size(11, 11), new Size(-1, -1), criteria);
                        imgpoints.Add(pointBuf);
                        imagesPointsM.Add(Mat.FromArray<Point2f>(pointBuf));

                        Cv2.DrawChessboardCorners(img, BoardSize, pointBuf, found);
                        Cv2.ImShow("img", img);
                        Cv2.WaitKey(10);
                    }
                }
            }

            // 执行相机标定
            Mat[] rvecs = new Mat[] { };
            Mat[] tvecs = new Mat[] { };
            Mat mtx = new Mat();
            Mat dist = new Mat();
            Mat[] objpoints = calcBoardCornerPositions(BoardSize, SquareSize, imagesList.Count);
            Cv2.CalibrateCamera(objpoints, imagesPointsM.ToArray(), new Size(gray.Cols, gray.Rows), mtx, dist, out rvecs, out tvecs, CalibrationFlags.None);

            // 畸变校正
            img = Cv2.ImRead("../../../images/lefts/left12.jpg");
            int h = img.Rows;
            int w = img.Cols;
            Mat mapx = new Mat();
            Mat mapy = new Mat();
            Rect roi = new Rect();
            Mat newcameramtx = Cv2.GetOptimalNewCameraMatrix(mtx, dist, new Size(w, h), 1, new Size(w, h), out roi);

            // undistort
            Mat dst = new Mat();
            Cv2.Undistort(img, dst, mtx, dist, newcameramtx);

            // crop the image
            dst = new Mat(dst, roi);
            Cv2.ImShow("calibresult1.png", dst);

            Cv2.InitUndistortRectifyMap(mtx, dist, new Mat(), newcameramtx, new Size(w, h), 5, mapx, mapy);
            Cv2.Remap(img, dst, mapx, mapy, InterpolationFlags.Linear);

            // crop the image
            dst = new Mat(dst, roi);
            Cv2.ImShow("calibresult2.png", dst);

            double mean_error = 0;
            for (int i = 0; i < objpoints.Length; ++i)
            {
                Mat imagePoints2 = new Mat();
                Cv2.ProjectPoints(objpoints[i], rvecs[i], tvecs[i], mtx, dist, imagePoints2);
                double error = Cv2.Norm(imagesPointsM[i], imagePoints2, NormTypes.L2);
                Console.WriteLine($"total error :{mean_error / objpoints.Count()}");
            }

            Cv2.WaitKey();
        }
    }
}

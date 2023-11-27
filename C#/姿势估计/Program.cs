/*
OpenCv版本 OpenCvSharp4.4.8.0.20230708
内容：姿势估计
博客：http://www.bilibili996.com/Course?id=f2a6235f09a849a9a93974ce8b00272f
作者：高仁宝
时间：2023.11
*/

//【没搞定。。。】

//using OpenCvSharp;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace demo
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            List<string> paths = new List<string>()
//            {
//                "../../../images/lefts/",
//                "../../../images/rights/"
//            };
//            List<string> imagesList = new List<string>();
//            foreach (string path in paths)
//            {
//                DirectoryInfo di = new DirectoryInfo(path);
//                FileInfo[] fis = di.GetFiles("*.jpg");
//                Mat srcImage = new Mat();
//                foreach (FileInfo fi in fis)
//                {
//                    imagesList.Add(fi.FullName);
//                }
//            }

//            TermCriteria criteria = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 30, 0.001);

//            Size BoardSize = new Size(9, 6);
//            int SquareSize = 25; //每格的宽度应设置为实际的毫米数

//            Mat img = new Mat();
//            Mat gray = new Mat();

//            List<Mat> imagesPointsM = new List<Mat>();
//            //List<List<Point3f>> objpoints = new List<List<Point3f>>();
//            List<List<Point2f>> imgpoints = new List<List<Point2f>>();

//            for (int i = 0; i < imagesList.Count; i++)
//            {
//                img = new Mat(imagesList[i]);
//                if (!img.Empty())
//                {
//                    Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

//                    Point2f[] pointBuf;
//                    bool found = Cv2.FindChessboardCorners(img, BoardSize, out pointBuf);
//                    if (found == true)
//                    {
//                        Mat viewGray = new Mat();
//                        Cv2.CvtColor(img, viewGray, ColorConversionCodes.BGR2GRAY);
//                        Cv2.CornerSubPix(viewGray, pointBuf, new Size(11, 11), new Size(-1, -1), criteria);
//                        imgpoints.Add(pointBuf.ToList());
//                        imagesPointsM.Add(Mat.FromArray<Point2f>(pointBuf));
//                        //objpoints.Add(GetChessboardObjectPoints(BoardSize, ((float)SquareSize / 1000.0f)));

//                        Cv2.DrawChessboardCorners(img, BoardSize, pointBuf, found);
//                        Cv2.ImShow("img", img);
//                        Cv2.WaitKey(10);
//                    }
//                }
//            }

//            // 执行相机标定
//            Mat[] rvecs = new Mat[] { };
//            Mat[] tvecs = new Mat[] { };
//            Mat mtx = new Mat();
//            Mat dist = new Mat();
//            Mat[] objpoints = calcBoardCornerPositions(BoardSize, SquareSize, imagesList.Count);
//            Cv2.CalibrateCamera(objpoints, imagesPointsM.ToArray(), new Size(gray.Cols, gray.Rows), mtx, dist, out rvecs, out tvecs, CalibrationFlags.None);

//            //Vec3d[] rvecs = new Vec3d[] { };
//            //Vec3d[] tvecs = new Vec3d[] { };
//            //double[,] mtx = new double[,] { };
//            //double[] dist = new double[] { };
//            //Cv2.CalibrateCamera(objpoints, imgpoints, new Size(gray.Cols, gray.Rows), mtx, dist, out rvecs, out tvecs, CalibrationFlags.None);

//            Mat objq = objpoints[0];//new Mat(9 * 6, 3, MatType.CV_64FC3);

//            Mat axis = new Mat(3, 3, MatType.CV_32F);
//            axis.At<int>(0, 0) = 3;
//            axis.At<int>(1, 1) = 3;
//            axis.At<int>(2, 2) = -3;

//            Mat rvecs2 = new Mat() { };
//            Mat tvecs2 = new Mat() { };
//            Mat imgpts = new Mat() { };
//            Mat jac = new Mat() { };

//            foreach (string fname in imagesList)
//            {
//                img = Cv2.ImRead(fname);
//                Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);
//                Point2f[] corners;
//                bool found = Cv2.FindChessboardCorners(img, BoardSize, out corners);
//                if (found == true)
//                {
//                    var corners2 = Cv2.CornerSubPix(gray, corners, new Size(11, 11), new Size(-1, -1), criteria);

//                    // Find the rotation and translation vectors.
//                    Cv2.SolvePnP(objq, Mat.FromArray<Point2f>(corners2), mtx, dist, rvecs2, tvecs2);

//                    // project 3D points to image plane
//                    Cv2.ProjectPoints(axis, rvecs2, tvecs2, mtx, dist, imgpts, jac);

//                    Cv2.ImShow("imgpts", imgpts);

//                    //img = draw(img, corners2, imgpts);
//                    Cv2.ImShow("img", img);
//                    if (Cv2.WaitKey(0) == 27)
//                        break;
//                }
//            }
//        }

//        static List<Point3f> GetChessboardObjectPoints(Size boardSize, float squareSize)
//        {
//            List<Point3f> objectPoints = new List<Point3f>() { };
//            for (int i = 0; i < boardSize.Height; ++i)
//            {
//                for (int j = 0; j < boardSize.Width; ++j)
//                {
//                    objectPoints.Add(new Point3f(squareSize * j, squareSize * i, 0.0f));
//                }
//            }
//            return objectPoints;
//        }

//        public static Mat[] calcBoardCornerPositions(Size BoardSize, float SquareSize, int imagesCount)
//        {
//            Mat[] corners = new Mat[imagesCount];
//            for (int k = 0; k < imagesCount; k++)
//            {
//                Point3f[] p = new Point3f[BoardSize.Height * BoardSize.Width];
//                for (int i = 0; i < BoardSize.Height; i++)
//                {
//                    for (int j = 0; j < BoardSize.Width; j++)
//                    {
//                        p[i * BoardSize.Width + j] = new Point3f(j * SquareSize, i * SquareSize, 0);
//                    }
//                }
//                corners[k] = Mat.FromArray<Point3f>(p);
//            }
//            return corners;
//        }

//        static Mat draw(Mat img, Point2f[] corners, Point2f[] imgpts)
//        {
//            Point l0 = new Point((int)corners[0].X, (int)corners[0].Y);
//            Point l1 = new Point((int)imgpts[0].X, (int)imgpts[0].Y);
//            Point l2 = new Point((int)imgpts[1].X, (int)imgpts[1].Y);
//            Point l3 = new Point((int)imgpts[2].X, (int)imgpts[2].Y);
//            Cv2.Line(img, l0, l1, new Scalar(255, 0, 0), 5);
//            Cv2.Line(img, l0, l2, new Scalar(0, 255, 0), 5);
//            Cv2.Line(img, l0, l3, new Scalar(0, 0, 255), 5);
//            return img;
//        }
//    }
//}

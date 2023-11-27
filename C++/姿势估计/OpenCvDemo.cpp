/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：姿势估计
博客：http://www.bilibili996.com/Course?id=f2a6235f09a849a9a93974ce8b00272f
作者：高仁宝
时间：2023.11
*/

//【没搞定。。。】

//#include <opencv2/opencv.hpp>
//#include <opencv2/ml.hpp>
//#include <opencv2/cvconfig.h>
//#include <opencv2/imgproc/imgproc.hpp>
//#include <opencv2/highgui/highgui.hpp>    
//
//#include <io.h>
//#include <string>
//#include <iostream>
//#include <ctime>
//
//using namespace cv;
//using namespace std;
//
//std::vector<cv::Point3f> GetChessboardObjectPoints(const cv::Size& boardSize, float squareSize) {
//    std::vector<cv::Point3f> objectPoints;
//    for (int i = 0; i < boardSize.height; ++i) {
//        for (int j = 0; j < boardSize.width; ++j) {
//            objectPoints.push_back(Point3f(squareSize * j, squareSize * i, 0.0f));
//        }
//    }
//    return objectPoints;
//}
//
//void demo1()
//{
//    // 获取图片目录
//    std::string inPath1 = "..\\images\\lefts\\left*.jpg";
//    std::string inPath2 = "..\\images\\rights\\right*.jpg";
//    intptr_t handle1, handle2;
//    struct _finddata_t fileinfo1, fileinfo2;
//    handle1 = _findfirst(inPath1.c_str(), &fileinfo1);
//    handle2 = _findfirst(inPath2.c_str(), &fileinfo2);
//
//    vector<string> images;
//    do {
//        string Name = fileinfo1.name;
//        images.push_back("..\\images\\lefts\\" + Name);
//    } while (!_findnext(handle1, &fileinfo1));
//
//    do {
//        string Name = fileinfo2.name;
//        images.push_back("..\\images\\lefts\\" + Name);
//    } while (!_findnext(handle2, &fileinfo2));
//
//    TermCriteria criteria = TermCriteria(TermCriteria::EPS | TermCriteria::MAX_ITER, 30, 0.001);
//
//    // 棋盘格内角点数量（行数和列数）
//    cv::Size chessboardSize(9, 6);
//
//    // 棋盘格尺寸（单位：米）
//    float squareSize = 0.025f;
//
//    // 存储棋盘格角点坐标的容器
//    std::vector<std::vector<cv::Point3f>> objpoints;
//    // 存储检测到的棋盘格角点坐标的容器
//    std::vector<std::vector<cv::Point2f>> imgpoints;
//    Mat gray;
//
//    for (int i = 0; i < images.size(); i++)
//    {
//        Mat img = imread(images[i]);
//        if (!img.empty())
//        {
//            cvtColor(img, gray, COLOR_BGR2GRAY);
//
//            // Find the chess board corners
//            std::vector<cv::Point2f> corners;
//            bool ret = findChessboardCorners(gray, chessboardSize, corners);
//
//            // If found, add object points, image points(after refining them)
//            if (ret == true)
//            {
//                cornerSubPix(gray, corners, Size(11, 11), Size(-1, -1), criteria);
//                // Draw and display the corners
//                drawChessboardCorners(img, chessboardSize, corners, ret);
//
//                objpoints.push_back(GetChessboardObjectPoints(chessboardSize, squareSize));
//                imgpoints.push_back(corners);
//
//                imshow("img", img);
//                waitKey(10);
//            }
//        }
//    }
//
//    // 执行相机标定
//    cv::Mat mtx, dist;
//    std::vector<cv::Mat> rvecs, tvecs;
//    cv::calibrateCamera(objpoints, imgpoints, cv::Size(gray.cols, gray.rows), mtx, dist, rvecs, tvecs);
//
//
//
//    for (int i = 0; i < images.size(); i++)
//    {
//        Mat img = imread(images[i]);
//        cvtColor(img, gray, COLOR_BGR2GRAY);
//        // Find the chess board corners
//        std::vector<cv::Point2f> corners;
//        bool ret = findChessboardCorners(gray, chessboardSize, corners);
//        if (ret == true)
//        {
//            cornerSubPix(gray, corners, Size(11, 11), Size(-1, -1), criteria);
//
//            cv::Mat rvec;  // 输出的旋转向量
//            cv::Mat tvec;  // 输出的平移向量
//            solvePnP(objpoints[0], imgpoints[0], mtx, dist, rvec, tvec);
//       
//
//            // Find the rotation and translation vectors.
//            solvePnP(objq, Mat.FromArray<Point2f>(corners2), mtx, dist, rvecs2, tvecs2);
//
//            // project 3D points to image plane
//            Cv2.ProjectPoints(axis, rvecs2, tvecs2, mtx, dist, imgpts, jac);
//
//            //img = draw(img, corners2, imgpts);
//            Cv2.ImShow("img", img);
//            if (Cv2.WaitKey(0) == 27)
//                break;
//        }
//    }
//
//
//
//
//
//
//
//
//   // // 畸变校正
//   // Mat img = imread("../images/lefts/left12.jpg");
//   // int h = img.rows;
//   // int w = img.cols;
//   // cv::Mat mapx, mapy;
//   // Rect roi;
//   // cv::Mat newcameramtx = getOptimalNewCameraMatrix(mtx, dist, Size(w,h), 1, Size(w, h), &roi);
//
//   // // undistort
//   // Mat dst;
//   // undistort(img, dst, mtx, dist, newcameramtx);
//
//   // // crop the image
//   // dst = dst(roi);
//   // imshow("calibresult1.png", dst);
//
//   // initUndistortRectifyMap(mtx, dist, cv::Mat(), newcameramtx, Size(w, h), 5, mapx, mapy);
//   // remap(img, dst, mapx, mapy, INTER_LINEAR);
//
//   // // crop the image
//   // dst = dst(roi);
//   // imshow("calibresult2.png", dst);
//   //// 你会发现结果图像中所有的边界变直了
//
//   // // 反向投影误差
//   // // 我们可以利用反向投影误差对我们找到的参数的准确性进行估计。得到的结果越接近 0越好。
//   // // 有了内部参数，畸变参数和旋转变换矩阵，我们就可以使用cv2.projectPoints()将对象点转换到图像点。
//   // // 然后就可以计算变换得到图像与角点检测算法的绝对差了。
//   // // 然后我们计算所有标定图像的误差平均值。
//   // int mean_error = 0;
//   // for (int i = 0; i < objpoints.size(); i++)
//   // {
//   //     vector<Point2f> imgpoints2;
//   //     projectPoints(objpoints[i], rvecs[i], tvecs[i], mtx, dist, imgpoints2);
//   //    int error = norm(imgpoints[i], imgpoints2, NORM_L2) / imgpoints2.size();
//   //    mean_error += error;
//   //    cout << "total error :" << mean_error / objpoints.size() << endl;
//   // }
//   // waitKey(0);
//}
//
//int main()
//{
//    demo1();
//}
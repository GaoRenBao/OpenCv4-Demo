/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：姿势估计
博客：http://www.bilibili996.com/Course?id=f2a6235f09a849a9a93974ce8b00272f
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/ml.hpp>
#include <opencv2/cvconfig.h>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>    

#include <io.h>
#include <string>
#include <iostream>
#include <ctime>

using namespace cv;
using namespace std;

vector<Point3f> Create3DChessboardCorners(Size boardSize, float squareSize)
{
    std::vector<cv::Point3f> objectPoints;
    for (int y = 0; y < boardSize.height; y++)
    {
        for (int x = 0; x < boardSize.width; x++)
        {
            objectPoints.push_back(Point3f(x * squareSize, y * squareSize, 0));
        }
    }
    return objectPoints;
}

Mat draw(Mat img, vector<Point2f> corners, vector<Point2f> imgpts)
{
    Point l0((int)corners[0].x, (int)corners[0].y);
    Point l1((int)imgpts[0].x, (int)imgpts[0].y);
    Point l2((int)imgpts[1].x, (int)imgpts[1].y);
    Point l3((int)imgpts[2].x, (int)imgpts[2].y);
    line(img, l0, l1, Scalar(255, 0, 0), 5);
    line(img, l0, l2, Scalar(0, 255, 0), 5);
    line(img, l0, l3, Scalar(0, 0, 255), 5);
    return img;
}

int main()
{
    // 获取图片目录
    std::string inPath1 = "..\\images\\lefts\\left*.jpg";
    std::string inPath2 = "..\\images\\rights\\right*.jpg";
    intptr_t handle1, handle2;
    struct _finddata_t fileinfo1, fileinfo2;
    handle1 = _findfirst(inPath1.c_str(), &fileinfo1);
    handle2 = _findfirst(inPath2.c_str(), &fileinfo2);

    vector<string> images;
    do {
        string Name = fileinfo1.name;
        images.push_back("..\\images\\lefts\\" + Name);
    } while (!_findnext(handle1, &fileinfo1));

    do {
        string Name = fileinfo2.name;
        images.push_back("..\\images\\lefts\\" + Name);
    } while (!_findnext(handle2, &fileinfo2));

    TermCriteria criteria = TermCriteria(TermCriteria::EPS | TermCriteria::MAX_ITER, 30, 0.001);

    // 棋盘格内角点数量（行数和列数）
    cv::Size BoardSize(9, 6);

    // 棋盘格尺寸（单位：米）
    float squareSize = 0.025f;

    // 存储棋盘格角点坐标的容器
    std::vector<std::vector<cv::Point3f>> objpoints;
    // 存储检测到的棋盘格角点坐标的容器
    std::vector<std::vector<cv::Point2f>> imgpoints;
    Mat gray;

    for (int i = 0; i < images.size(); i++)
    {
        Mat img = imread(images[i]);
        if (!img.empty())
        {
            cvtColor(img, gray, COLOR_BGR2GRAY);

            // Find the chess board corners
            std::vector<cv::Point2f> corners;;
            bool found = findChessboardCorners(gray, BoardSize, corners);
            if (found == true)
            {
                vector<Point3f> objectPointsArray = Create3DChessboardCorners(BoardSize, squareSize);
                objpoints.push_back(objectPointsArray);
                imgpoints.push_back(corners);
            }
        }
    }

    cv::Mat mtx, dist;
    std::vector<cv::Mat> rvecs, tvecs;
    cv::calibrateCamera(objpoints, imgpoints, gray.size(), mtx, dist, rvecs, tvecs);

    vector<Point3f> objPts;
    for (int i = 0; i < 6; i++)
    {
        for (int j = 0; j < 9; j++)
        {
            objPts.push_back(Point3f((float)j, (float)i, (float)0));
        }
    }

    // X、Y、Z三轴方向，占3格
    vector<Point3f> axis;
    axis.push_back(Point3f(3, 0, 0));
    axis.push_back(Point3f(0, 3, 0));
    axis.push_back(Point3f(0, 0, -3));

    for (int i = 0; i < images.size(); i++)
    {
        Mat img = imread(images[i]);
        cvtColor(img, gray, COLOR_BGR2GRAY);

        std::vector<cv::Point2f> corners;
        bool found = findChessboardCorners(gray, BoardSize, corners);
        if (found == true)
        {
            cornerSubPix(gray, corners, Size(11, 11), Size(-1, -1), criteria);

            cv::Mat rvec;  // 输出的旋转向量
            cv::Mat tvec;  // 输出的平移向量
            solvePnP(objPts, corners, mtx, dist, rvec, tvec);

            // project 3D points to image plane
            cv::Mat imgpts;
            cv::Mat jac;
            projectPoints(axis, rvec, tvec, mtx, dist, imgpts, jac);

            img = draw(img, corners, imgpts);
            imshow("img", img);
            if (waitKey(0) == 27)
                break;
        }
    }
}
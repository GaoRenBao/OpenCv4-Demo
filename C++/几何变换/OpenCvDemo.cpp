/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：几何变换
博客：http://www.bilibili996.com/Course?id=1154656000256
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/cvconfig.h>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>    

#include <io.h>
#include <string>
#include <iostream>

using namespace cv;
using namespace std;

// 图像的平移操作
void demo1()
{
    // 移动了100,50 个像素。
    Mat img = cv::imread("../images/messi5.jpg", 0);

    // Define the transformation matrix
    Mat M(2, 3, CV_32FC1);
    M.at<float>(0, 0) = 1;
    M.at<float>(0, 1) = 0;
    M.at<float>(0, 2) = 100;
    M.at<float>(1, 0) = 0;
    M.at<float>(1, 1) = 1;
    M.at<float>(1, 2) = 50;

    // 应用平移变换  
    Mat dst;
    cv::warpAffine(img, dst, M, img.size());

    cv::imshow("img", dst);
    cv::waitKey(0);
}

// 旋转图像操作
void demo2()
{
    Mat img = cv::imread("../images/messi5.jpg", 0);
    Mat dst;
    Mat rotMat = cv::getRotationMatrix2D(Point2f(img.cols / 2, img.rows / 2), 45, 0.6);
    cv::warpAffine(img, dst, rotMat, Size(img.cols * 2, img.rows * 2));
    cv::imshow("img", dst);
    cv::waitKey(0);
}

// 仿射变换的基本操作
void demo3()
{
    Mat img = cv::imread("../images/drawing.png");

    Mat pts1(3, 2, CV_32FC1);
    pts1.at<float>(0, 0) = 50;
    pts1.at<float>(0, 1) = 50;
    pts1.at<float>(1, 0) = 200;
    pts1.at<float>(1, 1) = 50;
    pts1.at<float>(2, 0) = 50;
    pts1.at<float>(2, 1) = 200;

    Mat pts2(3, 2, CV_32FC1);
    pts2.at<float>(0, 0) = 10;
    pts2.at<float>(0, 1) = 100;
    pts2.at<float>(1, 0) = 200;
    pts2.at<float>(1, 1) = 50;
    pts2.at<float>(2, 0) = 100;
    pts2.at<float>(2, 1) = 250;

    Mat M = cv::getAffineTransform(pts1, pts2);
    Mat dst;
    cv::warpAffine(img, dst, M, img.size());

    cv::imshow("Input", img);
    cv::imshow("Output", dst);
    cv::waitKey(0);
}

// 透视变换的基本操作
void demo4()
{
    Mat img = cv::imread("../images/sudoku.jpg");

    Mat pts1(4, 2, CV_32FC1);
    pts1.at<float>(0, 0) = 56;
    pts1.at<float>(0, 1) = 65;
    pts1.at<float>(1, 0) = 368;
    pts1.at<float>(1, 1) = 52;
    pts1.at<float>(2, 0) = 28;
    pts1.at<float>(2, 1) = 387;
    pts1.at<float>(3, 0) = 389;
    pts1.at<float>(3, 1) = 390;

    Mat pts2(4, 2, CV_32FC1);
    pts2.at<float>(0, 0) = 0;
    pts2.at<float>(0, 1) = 0;
    pts2.at<float>(1, 0) = 300;
    pts2.at<float>(1, 1) = 0;
    pts2.at<float>(2, 0) = 0;
    pts2.at<float>(2, 1) = 300;
    pts2.at<float>(3, 0) = 300;
    pts2.at<float>(3, 1) = 300;

    Mat M = cv::getPerspectiveTransform(pts1, pts2);
    Mat dst;
    cv::warpPerspective(img, dst, M, Size(300, 300));

    cv::imshow("Input", img);
    cv::imshow("Output", dst);
    cv::waitKey(0);
}

int main()
{
    demo1();
    //demo2();
    //demo3();
    //demo4();
}
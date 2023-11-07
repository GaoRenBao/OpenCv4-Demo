/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：凸缺陷/凸包检测
博客：http://www.bilibili996.com/Course?id=3128323000263
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/cvconfig.h>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>    

#include <string>
#include <cmath>
#include <iostream>

using namespace cv;
using namespace std;

void demo1()
{
    Mat img = cv::imread("../images/star3.jpg");
    Mat img_gray;
    cv::cvtColor(img, img_gray, COLOR_BGR2GRAY);

    Mat thresh;
    cv::threshold(img_gray, thresh, 127, 255, THRESH_BINARY);

    vector<Mat> contours;
    Mat hierarcy;
    cv::findContours(thresh, contours, hierarcy, RETR_CCOMP, CHAIN_APPROX_NONE, Point(0, 0));
    Mat cnt = contours[0];

    // 函数 cv::isContourConvex() 可以可以用来检测一个曲线是不是凸的。它只能返回 True 或 False。
    bool k = cv::isContourConvex(cnt);
    cout << "K=" << k << endl;

    // 函数 cv::convexHull() 可以用来检测一个曲线的凸包
    Mat hull;
    cv::convexHull(cnt, hull, false);

    // 创建结果图像并绘制轮廓和凸包  
    cv::drawContours(img, contours, 0, Scalar(0, 0, 255), 2); // 绘制原始轮廓  
    vector<Mat> hulls;
    hulls.push_back(hull);
    cv::drawContours(img, hulls, 0, Scalar(0, 255, 0), 2); // 绘制凸包  

    cv::imshow("img1", img);
}

void demo2()
{
    Mat img = cv::imread("../images/star3.jpg");
    Mat img_gray;
    cv::cvtColor(img, img_gray, COLOR_BGR2GRAY);

    Mat thresh;
    cv::threshold(img_gray, thresh, 127, 255, THRESH_BINARY);

    vector<vector<Point>> contours;
    vector<Vec4i> hierarcy;
    cv::findContours(thresh, contours, hierarcy, RETR_CCOMP, CHAIN_APPROX_NONE, Point(0, 0));
    vector<Point> cnt = contours[0];

    // 函数 cv::isContourConvex() 可以可以用来检测一个曲线是不是凸的。它只能返回 True 或 False。
    bool k = cv::isContourConvex(cnt);
    cout << "K=" << k << endl;

    // 函数 cv::convexHull() 可以用来检测一个曲线的凸包
    vector<int> hull;
    convexHull(Mat(cnt), hull, false);

    // 创建结果图像并绘制轮廓和凸包  
    vector<Vec4i> defects;
    cv::convexityDefects(cnt, hull, defects);

    for (int j = 0; j < defects.size(); j++)
    {
        Point start = cnt[defects[j][0]];
        Point end = cnt[defects[j][1]];
        Point far = cnt[defects[j][2]];
        //Point fart = cnt[defects[j][3]];
        cv::line(img, start, end, Scalar(0, 255, 0), 2);
        cv::circle(img, far, 5, Scalar(0, 0, 255), -1);
    }
    cv::imshow("img2", img);
}

int main()
{
    demo1();
    demo2();
    cv::waitKey(0);
}
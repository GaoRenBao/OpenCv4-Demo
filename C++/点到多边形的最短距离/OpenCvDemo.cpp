/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：点到多边形的最短距离
博客：http://www.bilibili996.com/Course?id=3657332000262
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

int main()
{
    Mat srcImage = cv::imread("../images/lightning.png");
    Mat img;
    cv::cvtColor(srcImage, img, COLOR_BGR2GRAY);

    Mat threshold;
    cv::threshold(img, threshold, 127, 255, THRESH_BINARY); // 把黑白颜色反转
    vector<vector<Point>> contours;
    vector<Vec4i> hierarcy;
    findContours(threshold, contours, hierarcy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));
    cout << "contours len:" << contours.size() << endl;
    vector<Point> cnt = contours[0];
    Point point(50, 50);

    cv::circle(srcImage, point, 5, Scalar(0, 0, 255), -1);
    cv::imshow("srcImage", srcImage);

    double dist = cv::pointPolygonTest(cnt, point, true);
    cout << "距离:" << dist << endl;
    cv::waitKey(0);
}
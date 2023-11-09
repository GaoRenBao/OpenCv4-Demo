/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：椭圆拟合与直线拟合
博客：http://www.bilibili996.com/Course?id=0975485000265
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
    Mat rImg = cv::imread("../images/lightning.png");
    Mat img;
    cv::cvtColor(rImg, img, COLOR_BGR2GRAY);

    Mat thresh;
    cv::threshold(img, thresh, 127, 255, THRESH_BINARY);

    vector<vector<Point>> contours;
    vector<Vec4i> hierarcy;
    cv::findContours(thresh, contours, hierarcy, RETR_CCOMP, CHAIN_APPROX_NONE, Point(0, 0));
    cout << "contours len:" << contours.size() << endl;
    vector<Point> cnt = contours[0];

    // 椭圆拟合
    // 使用的函数为 cv::ellipse()  返回值其实就是旋转边界矩形的内切圆
    RotatedRect ellipse = cv::fitEllipse(cnt);
    float angle = ellipse.angle;
    cv::ellipse(rImg, ellipse, Scalar(0, 255, 0), 2);

    // 直线拟合
    // 我们可以根据一组点拟合出一条直线 同样我们也可以为图像中的白色点 拟合出一条直线。
    cv::Vec4f line;
    cv::fitLine(cnt, line, cv::DIST_L2, 0, 0.01, 0.01);
    int lefty = (int)((-line[2] * line[1] / line[0]) + line[3]);
    int righty = (int)(((img.cols - line[2]) * line[1] / line[0]) + line[3]);
    cv::line(rImg, Point(img.cols - 1, righty), Point(0, lefty), Scalar(0, 0, 255), 2);

    cv::imshow("rImg", rImg);
    cv::waitKey(0);
}


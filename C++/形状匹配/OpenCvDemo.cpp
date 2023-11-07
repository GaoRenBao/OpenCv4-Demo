/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：形状匹配
博客：http://www.bilibili996.com/Course?id=0930272000264
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
    Mat img1 = cv::imread("../images/star.jpg", 0);
    //Mat img2 = cv::imread("../images/star_b.jpg",0);
    Mat img2 = cv::imread("../images/star_c.jpg", 0);

    cv::imshow("img1", img1);
    cv::imshow("img2", img2);

    Mat thresh1;
    Mat thresh2;
    cv::threshold(img1, thresh1, 127, 255, THRESH_BINARY);
    cv::threshold(img2, thresh2, 127, 255, THRESH_BINARY);

    vector<vector<Point>> contours;
    vector<Vec4i> hierarcy;
    cv::findContours(thresh1, contours, hierarcy, RETR_CCOMP, CHAIN_APPROX_NONE, Point(0, 0));
    vector<Point> cnt1 = contours[0];
    cout << "contours len1：" << contours.size() << endl;

    cv::findContours(thresh2, contours, hierarcy, RETR_CCOMP, CHAIN_APPROX_NONE, Point(0, 0));
    vector<Point> cnt2 = contours[0];
    cout << "contours len2：" << contours.size() << endl;

    double ret = cv::matchShapes(cnt1, cnt2, 1, 0.0);
    cout << "ret：" << ret << endl;

    cv::waitKey(0);
}
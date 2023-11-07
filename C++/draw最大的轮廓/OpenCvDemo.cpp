/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：draw最大的轮廓
博客：http://www.bilibili996.com/Course?id=5528010000260
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

struct areaPack
{
    int i;
    double area;
};

int main()
{
    Mat org = cv::imread("../images/cards.png");

    Mat imgray;
    cv::cvtColor(org, imgray, COLOR_BGR2GRAY);
    cv::imshow("imgray", imgray);

    // 白色背景
    Mat threshold;
    cv::threshold(imgray, threshold, 244, 255, THRESH_BINARY_INV); // 把黑白颜色反转
    cv::imshow("after threshold", threshold);
    vector<vector<Point>> contours;
    vector<Vec4i> hierarcy;
    findContours(threshold, contours, hierarcy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));

    vector<areaPack> areas;
    for (int i = 0; i < contours.size(); i++)
    {
        areaPack p;
        p.i = i;
        p.area = cv::contourArea(contours[i]); // 面积大小
        areas.push_back(p);
    }
    // 按面积大小，从大到小排序
    sort(areas.begin(), areas.end(), [](const areaPack& a, const areaPack& b) {
        return a.area > b.area;
    });

    vector<areaPack> a2 = areas;
    for (int i = 0; i < a2.size(); i++)
    {
        if (a2[i].area < 150)
            continue;
        Mat img22;
        org.copyTo(img22); //逐个contour 显示
        cv::drawContours(img22, contours, a2[i].i, Scalar(0, 0, 255), 3);
        cv::imshow("drawContours", img22);
        if (cv::waitKey(200) == 'q')
            break;
    }

    // 获取最大或某个contour，剪切
    int idx = a2[1].i;
    // Create mask where white is what we want, black otherwise
    Mat mask(org.size(), org.type(), Scalar(0, 0, 0));
    // Draw filled contour in mask
    cv::drawContours(mask, contours, idx, Scalar(0, 255, 0), -1);

    // Extract out the object and place into output image
    Mat matout(org.size(), org.type(), Scalar(0, 0, 0));
    org.copyTo(matout, mask);
    cv::imshow("out_contour.jpg", matout);

    //roi方法
    idx = a2[4].i;
    Rect rect = cv::boundingRect(contours[idx]);
    Mat matroi = org(rect);
    cv::imshow("out_contour-roi4.jpg", matroi);
    cv::waitKey(0);
}
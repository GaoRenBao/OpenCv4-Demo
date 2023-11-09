/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：多对象模板匹配
博客：http://www.bilibili996.com/Course?id=bd204811ae5949549f1a2fbeedaeb470
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat img_rgb = cv::imread("../images/mario.png");
    Mat img_gray;
    cv::cvtColor(img_rgb, img_gray, COLOR_BGR2GRAY);
    Mat temp = cv::imread("../images/mario_coin.png", 0);

    int w = temp.cols;
    int h = temp.rows;
    Mat res;
    cv::matchTemplate(img_gray, temp, res, cv::TM_CCOEFF_NORMED);

    double threshold = 0.8;
    cv::threshold(res, res, threshold, 1.0, cv::THRESH_TOZERO);

    vector<cv::Point> points;
    cv::findNonZero(res, points);

    for (int i = 0; i < points.size(); i++)
    {
        Point pt = points[i];
        cv::rectangle(img_rgb, pt, Point(pt.x + w, pt.y + h), Scalar(0, 255, 0), 2);
    }

    cv::imshow("result", img_rgb);
    cv::waitKey(0);
}
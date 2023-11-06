/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：ROI矩形截取
博客：http://www.bilibili996.com/Course?id=db851d3a71c7471ab0bb11ca4d19e650
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat img = cv::imread("../images/messi5.jpg");
    // 设置需要裁剪的轮廓
    Rect rect;
    rect.x = 100;
    rect.y = 100;
    rect.width = 200;
    rect.height = 200;
    // 裁剪
    Mat roi = img(rect);

    cv::imshow("img", img);
    cv::imshow("roi.jpg", roi);
    cv::waitKey(0);
}
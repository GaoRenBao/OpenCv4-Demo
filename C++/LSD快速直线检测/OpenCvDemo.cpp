/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：LSD快速直线检测
博客：http://www.bilibili996.com/Course?id=600776a4089d4f99b46a0fd854e8c74d
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat img0 = cv::imread("../images/home.jpg");
    Mat img;
    cv::cvtColor(img0, img, cv::COLOR_BGR2GRAY);
    cv::imshow("pokerQ", img0);

    // Create default parametrization LSD
    Ptr<LineSegmentDetector> lsd = createLineSegmentDetector(LSD_REFINE_NONE);
    vector<Vec4f> lines;
    lsd->detect(img, lines);

    for (int i = 0; i < lines.size(); i++)
    {
        int x0 = lines[i][0];
        int y0 = lines[i][1];
        int x1 = lines[i][2];
        int y1 = lines[i][3];
        cv::line(img0, Point(x0, y0), Point(x1, y1), Scalar(0, 255, 0), 2, cv::LINE_AA);
        cv::imshow("LSD", img0);
        cv::waitKey(30);
    }

    // 绘图线检测结果
    lsd->drawSegments(img, lines);
    cv::imshow("LSD", img);
    cv::waitKey(0);
}
/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：warpPerspective透视变换
博客：http://www.bilibili996.com/Course?id=1854259000227
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>  
#include <opencv2/imgproc/imgproc.hpp>  
#include <opencv2/highgui/highgui.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat srcImage = imread("../images/Astral3.jpg");

    // 起始坐标
    vector<Point2f> org;
    org.push_back(Point2f((float)(srcImage.cols * 0.2), (float)(srcImage.rows * 0.2)));
    org.push_back(Point2f((float)(srcImage.cols * 0.8), (float)(srcImage.rows * 0.2)));
    org.push_back(Point2f((float)(srcImage.cols * 0.8), (float)(srcImage.rows * 0.8)));
    org.push_back(Point2f((float)(srcImage.cols * 0.2), (float)(srcImage.rows * 0.8)));

    // 目标坐标
    vector<Point2f> dst;
    dst.push_back(Point2f(0, 0));
    dst.push_back(Point2f(srcImage.cols, 0));
    dst.push_back(Point2f(srcImage.cols, srcImage.rows));
    dst.push_back(Point2f(0, srcImage.rows));

    line(srcImage, Point(org[0].x, org[0].y), Point(org[1].x, org[1].y), Scalar(0, 0, 255), 2, LINE_AA);
    line(srcImage, Point(org[1].x, org[1].y), Point(org[2].x, org[2].y), Scalar(0, 0, 255), 2, LINE_AA);
    line(srcImage, Point(org[2].x, org[2].y), Point(org[3].x, org[3].y), Scalar(0, 0, 255), 2, LINE_AA);
    line(srcImage, Point(org[3].x, org[3].y), Point(org[0].x, org[0].y), Scalar(0, 0, 255), 2, LINE_AA);
    imshow("透视线", srcImage);

    Mat warpR = getPerspectiveTransform(org, dst);
    warpPerspective(srcImage, srcImage, warpR, srcImage.size());
    imshow("识别结果", srcImage);

    waitKey(0);
    return 0;
}
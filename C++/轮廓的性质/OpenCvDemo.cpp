/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：轮廓的性质
博客：http://www.bilibili996.com/Course?id=4292381000261
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

void test1(vector<Point> cnt)
{
    // 边界矩形的宽高比
    Rect rect = cv::boundingRect(cnt);
    float aspect_ratio = (float)rect.width / (float)rect.height;
    cout << "边界矩形的宽高比:" << aspect_ratio << endl;
}
void test2(vector<Point> cnt)
{
    // Extent轮廓面积与边界矩形面积的比
    double area = cv::contourArea(cnt);
    Rect rect = cv::boundingRect(cnt);
    double rect_area = rect.width * rect.height;
    double extent = area / rect_area;
    cout << "Extent轮廓面积与边界矩形面积的比:" << extent << endl;
}
void test3(vector<Point> cnt)
{
    // Solidity轮廓面积与凸包面积的比。
    double area = cv::contourArea(cnt);
    vector<Point> hull;
    cv::convexHull(cnt, hull);
    double hull_area = cv::contourArea(hull);
    double solidity = area / hull_area;
    cout << "Solidity轮廓面积与凸包面积的比:" << solidity << endl;
}
void test4(vector<Point> cnt)
{
    // Equivalent Diameter与轮廓面积相等的圆形的直径
    double area = cv::contourArea(cnt);
    double equi_diameter = sqrt(4 * area / CV_PI);
    cout << "Equivalent Diameter与轮廓面积相等的圆形的直径:" << equi_diameter << endl;
}
void test5(vector<Point> cnt)
{
    // Orientation对象的方向下的方法会返回长轴和短轴的长度
    RotatedRect rect = cv::fitEllipse(cnt);
    cout << "Orientation对象的方向下的方法会返回长轴和短轴的长度:" << rect.center << "," << rect.size << "," << rect.angle << endl;
}
void test6(Mat imgray, vector<Point> cnt)
{
    // Mask and Pixel Points掩模和像素点
    Mat mask(imgray.size(), CV_8U);
    mask = Scalar::all(0);

    vector<vector<Point>> cnts;
    cnts.push_back(cnt);
    cv::drawContours(mask, cnts, 0, 255, -1);

    // 最大值和最小值及它们的位置
    double minValue;
    double maxValue;
    Point minLocation;
    Point maxLocation;
    minMaxLoc(imgray, &minValue, &maxValue, &minLocation, &maxLocation, Mat());
    cout << "最大值和最小值及它们的位置:" << minValue << "," << maxValue << "," << minLocation << "," << maxLocation << endl;

    // 我们也可以使用相同的掩模求一个对象的平均颜色或平均灰度
    Scalar mean_val = cv::mean(imgray, mask);
    cout << "平均色:" << mean_val << endl;
}

void test7(Mat img, vector<Point> cnt)
{
    Point topmost = *std::min_element(cnt.begin(), cnt.end(), [](const Point& a, const Point& b) { return a.y < b.y; });// 最上面
    Point bottommost = *std::min_element(cnt.begin(), cnt.end(), [](const Point& a, const Point& b) { return a.y > b.y; }); // 最下面
    Point leftmost = *std::min_element(cnt.begin(), cnt.end(), [](const Point& a, const Point& b) { return a.x < b.x; });// 最左面
    Point rightmost = *std::min_element(cnt.begin(), cnt.end(), [](const Point& a, const Point& b) { return a.x > b.x; }); // 最右面

    cout << "最上面:" << topmost << endl;
    cout << "最下面:" << bottommost << endl;
    cout << "最左:" << leftmost << endl;
    cout << "最右:" << rightmost << endl;

    cv::circle(img, topmost, 5, Scalar(0, 0, 255), -1);
    cv::circle(img, bottommost, 5, Scalar(0, 0, 255), -1);
    cv::circle(img, leftmost, 5, Scalar(0, 0, 255), -1);
    cv::circle(img, rightmost, 5, Scalar(0, 0, 255), -1);
    cv::imshow("img", img);
    cv::waitKey(0);
}

int main()
{
    Mat srcImage = cv::imread("../images/star3.jpg");
    Mat img;
    cv::cvtColor(srcImage, img, COLOR_BGR2GRAY);

    Mat thresh;
    cv::threshold(img, thresh, 127, 255, THRESH_BINARY);
    vector<vector<Point>> contours;
    vector<Vec4i> hierarcy;
    findContours(thresh, contours, hierarcy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));
    cout << "contours len:" << contours.size() << endl;
    vector<Point> cnt = contours[0];  // contours 有三个，挑个最大的，方便演示

    test1(cnt);
    test2(cnt);
    test3(cnt);
    test4(cnt);
    test5(cnt);
    test6(img, cnt);
    test7(srcImage, cnt);
}
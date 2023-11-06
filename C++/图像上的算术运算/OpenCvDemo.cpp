/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：图像上的算术运算
博客：http://www.bilibili996.com/Course?id=4683988000254
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/cvconfig.h>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>    

#include <io.h>
#include <string>
#include <iostream>

using namespace cv;
using namespace std;

void myImshow(string name, Mat img)
{
    Mat temp;
    cv::resize(img, temp, Size(img.cols * 1, img.rows * 1), 0, 0, 3);
    cv::imshow(name, temp);
}

// 图像相减1：图像的减法运算
void demo1()
{
    Mat img1 = cv::imread("../images/subtract1.jpg", 0); // 灰度图
    Mat img2 = cv::imread("../images/subtract2.jpg", 0); // 灰度图

    myImshow("subtract1", img1);
    myImshow("subtract2", img2);

    Mat st = img2 - img1;
    myImshow("after subtract", st);

    Mat threshold;
    cv::threshold(st, threshold, 50, 255, THRESH_BINARY);

    myImshow("after threshold", threshold);
    cv::waitKey(0);
}

// 图像相减2：通过图像相减，查找扑克牌位置
void demo2()
{
    Mat img1 = cv::imread("../images/subtract1.jpg", 0);
    Mat img22 = cv::imread("../images/subtract2.jpg");

    Mat img2;
    cv::cvtColor(img22, img2, COLOR_BGR2GRAY);

    Mat st;
    cv::subtract(img2, img1, st);
    //cv::Subtract(img1, img2, st); // 相反

    // 把小于5的像素点设为0
    for (int i = 0; i < st.rows; ++i)
    {
        for (int j = 0; j < st.cols; ++j)
        {
            char rgb = st.at<char>(i, j);
            if (rgb > 0 && rgb <= 5)
                st.at<char>(i, j) = 0;
        }
    }

    Mat threshold;
    cv::threshold(st, threshold, 50, 255, THRESH_BINARY);

    vector<vector<Point>> contours;
    vector<Vec4i> hierarcy;
    findContours(threshold, contours, hierarcy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));

    //for (int i = 0; i < contours.size(); i++)
    //{
    //    int area = cv::contourArea(contours[i]);
    //    if (area < 100)
    //        continue;

    //    cv::drawContours(img22, contours, i, Scalar(0, 0, 255), 3);
    //    myImshow("approxPolyDP", img22);
    //    cv::waitKey(0);
    //}

    // TODO 截取原图，把长方形纠正
    vector<Point> cnt = contours[0];
    vector<Point> hull;
    cv::convexHull(cnt, hull);
    double epsilon = 0.001 * cv::arcLength(hull, true);
    std::vector<cv::Point> simplified_cnt;
    cv::approxPolyDP(hull, simplified_cnt, epsilon, true);

    epsilon = 0.1 * cv::arcLength(cnt, true);
    vector<Point> approx;
    cv::approxPolyDP(cnt, approx, epsilon, true);
    vector<vector<Point>> approxs;
    approxs.push_back(approx);

    cv::drawContours(img22, approxs, 0, Scalar(255, 0, 0), 3);
    myImshow("approxPolyDP", img22);
    cv::waitKey(0);
}

//  returns just the difference of the two images
Mat diff(Mat img, Mat img1)
{
    Mat diff;
    cv::absdiff(img, img1, diff);
    return diff;
}
// removes the background but requires three images
Mat diff_remove_bg(Mat img0, Mat img, Mat img1)
{
    Mat d1 = diff(img0, img);
    Mat d2 = diff(img, img1);
    Mat a;
    cv::bitwise_and(d1, d2, a);
    return a;
}
// 图像相减3：通过图像相减，凸显扑克牌位置
void demo3()
{
    Mat img1 = cv::imread("../images/subtract1.jpg", 0);
    Mat img2 = cv::imread("../images/subtract2.jpg", 0);
    myImshow("subtract1", img1);
    myImshow("subtract2", img2);
    Mat st = diff_remove_bg(img2, img1, img2);
    myImshow("after subtract", st);
    cv::waitKey(0);
}

// 调用摄像头，通过图像相减，标记扑克牌位置
void demo4()
{
    VideoCapture cap;
    cap.open(0);
    if (!cap.isOpened())
    {
        cout << "摄像头打开失败." << endl;
        while (true);
        return;
    }

    //cap.set(CAP_PROP_FRAME_WIDTH, 1280);  // 宽度
    //cap.set(CAP_PROP_FRAME_HEIGHT, 720); // 高度
    //cap.set(CAP_PROP_EXPOSURE, 0);     // 曝光

    Mat bgimg0;
    Mat bgimg;
    int frame_no = 10; // 第10帧稳定图像
    while (frame_no > 0)
    {
        cap >> bgimg0;
        if (!bgimg0.empty())
        {
            frame_no--;
        }
    }
    cv::cvtColor(bgimg0, bgimg, COLOR_BGR2GRAY);

    Mat frame;
    Mat gray;
    Mat st;
    Mat img;
    while (true)
    {
        cap >> frame;
        if (!frame.empty())
        {
            cv::cvtColor(frame, gray, COLOR_BGR2GRAY);
            cv::subtract(gray, bgimg, st);

            Mat threshold;
            cv::threshold(st, threshold, 50, 255, THRESH_BINARY);

            vector<vector<Point>> contours;
            vector<Vec4i> hierarcy;
            findContours(threshold, contours, hierarcy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));
            cv::drawContours(st, contours, -1, Scalar(255, 255, 255), 3);
            img = st;

            for (int i = 0; i < contours.size(); i++)
            {
                int area = cv::contourArea(contours[i]);
                if (area < 200)
                    continue;

                double peri = cv::arcLength(contours[i], true);
                vector<Point> approx;
                cv::approxPolyDP(contours[i], approx, 0.04 * peri, true);
                if (approx.size() == 4)
                {
                    Rect rect = cv::boundingRect(approx);
                    cv::rectangle(frame, Point(rect.x, rect.y),
                        Point(rect.x + rect.width, rect.y + rect.height),
                        Scalar(0, 0, 255), 2);
                }
            }

            // TODO 对比前几/十几帧，新放一张扑克，知道是那张
            // 等待图像稳定，不放牌后，再计算
            myImshow("frame", frame);
            myImshow("subtract", img);
            myImshow("threshold", threshold);

            cv::waitKey(1);
        }
    }
}

int main()
{
    demo1();
    //demo2();
    //demo3();
    //demo4();
}
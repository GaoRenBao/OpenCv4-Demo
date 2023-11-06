/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：光流法运动目标检测
博客：http://www.bilibili996.com/Course?id=4918981000067
作者：高仁宝
时间：2023.11
*/

#include <opencv2/video/video.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/core/core.hpp>
#include <iostream>
#include <cstdio>

using namespace std;
using namespace cv;

void tracking(Mat& frame, Mat& output);
bool addNewPoints();
bool acceptTrackedPoint(int i);

string window_name = "optical flow tracking";
Mat gray;	    // 当前图片
Mat gray_prev;	// 预测图片
vector<Point2f> points[2];	// point0为特征点的原来位置，point1为特征点的新位置
vector<Point2f> initial;	// 初始化跟踪点的位置
vector<Point2f> features;	// 检测的特征
int maxCount = 500;	        // 检测的最大特征数
double qLevel = 0.01;	    // 特征检测的等级
double minDist = 10.0;	    // 两特征点之间的最小距离
vector<uchar> status;	    // 跟踪特征的状态，特征的流发现为1，否则为0
vector<float> err;

int main()
{
    Mat frame;
    Mat result;
    VideoCapture capture("../images/lol.avi");

    if (capture.isOpened())	// 摄像头读取文件开关
    {
        capture >> frame;
        imshow(window_name, frame);
        waitKey(0);
        while (true)
        {
            capture >> frame;
            if (frame.empty()) break;
            tracking(frame, result);
            imshow(window_name, result);

            if ((char)waitKey(50) == 27)
            {
                break;
            }
        }
    }
    return 0;
}

//--------------------------------------
// function: tracking
// brief: 跟踪
// parameter: frame	输入的视频帧
//			  output 有跟踪结果的视频帧
// return: void
//--------------------------------------
void tracking(Mat& frame, Mat& output)
{
    //此句代码的OpenCV3版为：
    cvtColor(frame, gray, COLOR_BGR2GRAY);
    //此句代码的OpenCV2版为：
    //cvtColor(frame, gray, CV_BGR2GRAY);
    frame.copyTo(output);
    // 添加特征点
    if (addNewPoints())
    {
        goodFeaturesToTrack(gray, features, maxCount, qLevel, minDist);
        points[0].insert(points[0].end(), features.begin(), features.end());
        initial.insert(initial.end(), features.begin(), features.end());
    }
    if (gray_prev.empty())
    {
        gray.copyTo(gray_prev);
    }
    // l-k光流法运动估计
    calcOpticalFlowPyrLK(gray_prev, gray, points[0], points[1], status, err);
    // 去掉一些不好的特征点
    int k = 0;
    for (size_t i = 0; i < points[1].size(); i++)
    {
        if (acceptTrackedPoint(i))
        {
            initial[k] = initial[i];
            points[1][k++] = points[1][i];
        }
    }
    points[1].resize(k);
    initial.resize(k);
    // 显示特征点和运动轨迹
    for (size_t i = 0; i < points[1].size(); i++)
    {
        line(output, initial[i], points[1][i], Scalar(0, 0, 255));
        circle(output, points[1][i], 3, Scalar(0, 255, 0), -1);
    }

    // 把当前跟踪结果作为下一此参考
    swap(points[1], points[0]);
    swap(gray_prev, gray);
}

//-------------------------------------
// function: addNewPoints
// brief: 检测新点是否应该被添加
// parameter:
// return: 是否被添加标志
//-------------------------------------
bool addNewPoints()
{
    return points[0].size() <= 10;
}

//--------------------------------------
// function: acceptTrackedPoint
// brief: 决定哪些跟踪点被接受
// parameter:
// return:
//-------------------------------------
bool acceptTrackedPoint(int i)
{
    return status[i] && ((abs(points[0][i].x - points[1][i].x) + abs(points[0][i].y - points[1][i].y)) > 2);
}
/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：几何变换
博客：http://www.bilibili996.com/Course?id=1154656000256
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/core/utils/logger.hpp> 

#include <string>
#include <iostream>

using namespace cv;
using namespace std;

// FastNlMeansDenoisingColored
void demo1()
{
    Mat img = cv::imread("../images/die.png");
    cv::cvtColor(img, img, COLOR_BGR2RGB);

    Mat dst;
    cv::fastNlMeansDenoisingColored(img, dst, 10, 10, 7, 21);

    cv::imshow("img", img);
    cv::imshow("dst", dst);
    cv::waitKey(0);
}

// fastNlMeansDenoisingMulti
void demo2()
{
    VideoCapture cap = VideoCapture("../images/vtest.avi");

    // 读取三帧图像
    double pos = cap.get(VideoCaptureProperties::CAP_PROP_FRAME_COUNT);

    vector<Mat> srcImgs;
    Mat frame;
    for (int i = 0; i < 5; i++)
    {
        cap.set(VideoCaptureProperties::CAP_PROP_POS_FRAMES, (int)(pos / 6 * i));
        cap >> frame;
        cv::cvtColor(frame, frame, COLOR_BGR2GRAY);
        srcImgs.push_back(frame);
    }

    // 一共5帧图像，我们取第二帧
    Mat dst;
    cv::fastNlMeansDenoisingMulti(srcImgs, dst, 2, 5, 4, 7, 35);

    cv::imshow("srcImgs", srcImgs[2]);
    cv::imshow("dst", dst);
    cv::waitKey(0);
}

int main()
{
    // 关闭opencv日志
    cv::utils::logging::setLogLevel(cv::utils::logging::LOG_LEVEL_SILENT);

    //demo1();
    demo2();
}
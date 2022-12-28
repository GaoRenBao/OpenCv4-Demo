#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

void main()
{
    VideoCapture Cap;
    // 打开ID为0的摄像头
    Cap.open(0);
    // 判断摄像头是否成功打开
    if (!Cap.isOpened())
    {
        cout << "摄像头打开失败" << endl;
        return;
    }

    // 摄像头图像
    Mat frame;
    // 设置蓝色的阈值
    Scalar lower = Scalar(90, 50, 50);
    Scalar upper = Scalar(130, 255, 255);

    while (true)
    {
        Cap >> frame;
        if (!frame.empty())
        {
            Mat hsv, res, mask;

            // 换到 HSV
            cvtColor(frame, hsv, COLOR_BGR2HSV);
            // 根据阈值构建掩模
            inRange(hsv, lower, upper, mask);
            // 对原图像和掩模位运算
            bitwise_and(frame, frame, res, mask);
            // 显示图像
            imshow("frame", frame);
            moveWindow("frame", 0, 0); // 原地
            imshow("mask", mask);
            moveWindow("mask", frame.cols, 0);// 右边
            imshow("res", res);
            moveWindow("res", 0, frame.rows);// 下边
            waitKey(10);
        }
    }
}

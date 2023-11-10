/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：使用GrabCut算法进行交互式前景提取
博客：http://www.bilibili996.com/Course?id=ebe8b6e9ef6843f486142989d05d438a
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

void demo1()
{
    Mat img = cv::imread("../images/messi5.jpg");
    Mat mask;     // 存储掩码图像
    Mat bgdModel; // 存储背景模型
    Mat fgdModel; // 存储前景模型

    // 使用GrabCut算法进行图像分割，指定感兴趣区域的矩形框作为初始化
    cv::grabCut(img, mask, Rect(50, 50, 450, 290), bgdModel, fgdModel, 5, GC_INIT_WITH_RECT);

    // 创建结果图像
    Mat mask2(mask.rows, mask.cols, CV_8UC1);

    for (int i = 0; i < mask.cols; i++)
    {
        for (int j = 0; j < mask.rows; j++)
        {
            // 获取掩码图像的像素值
            char v = mask.at<char>(j, i);
            // 根据掩码像素值设置结果图像的像素值
            switch (v)
            {
            case 0: // 0 表示背景
            case 2: // 2 表示可能的背景
                mask2.at<char>(j, i) = 0; // 设置为黑色
                break;

            case 1: // 1 表示前景
            case 3: // 3 表示可能的前景
                mask2.at<char>(j, i) = 1; // 设置为白
                break;
            }
        }
    }
    // 按Python逻辑，使用相乘算法
    cv::cvtColor(mask2, mask2, COLOR_GRAY2BGR);
    cv::multiply(img, mask2, img);
    cv::imshow("out", img);
}

void demo2()
{
    Mat img = cv::imread("../images/messi5.jpg");
    Mat mask;     // 存储掩码图像
    Mat bgdModel; // 存储背景模型
    Mat fgdModel; // 存储前景模型

    // 使用GrabCut算法进行图像分割，指定感兴趣区域的矩形框作为初始化
    cv::grabCut(img, mask, Rect(50, 50, 450, 290), bgdModel, fgdModel, 5, GC_INIT_WITH_RECT);

    // 创建结果图像
    Mat mask2(mask.rows, mask.cols, CV_8UC1);

    for (int i = 0; i < mask.cols; i++)
    {
        for (int j = 0; j < mask.rows; j++)
        {
            // 获取掩码图像的像素值
            char v = mask.at<char>(j, i);
            // 根据掩码像素值设置结果图像的像素值
            switch (v)
            {
            case 0: // 0 表示背景
            case 2: // 2 表示可能的背景
                mask2.at<char>(j, i) = 0; // 设置为黑色
                break;

            case 1: // 1 表示前景
            case 3: // 3 表示可能的前景
                mask2.at<char>(j, i) = 1; // 设置为白
                break;
            }
        }
    }
    // 按Python逻辑，使用相乘算法
    cv::cvtColor(mask2, mask2, COLOR_GRAY2BGR);
    cv::multiply(img, mask2, img);

    Mat newmask = cv::imread("image/newmask2.jpg", 0);
    Mat rMask;
    cv::bitwise_and(mask, mask, rMask, newmask);
    cv::grabCut(img, rMask, Rect(), bgdModel, fgdModel, 5, GC_INIT_WITH_MASK);
    mask = Mat(rMask.rows, rMask.cols, CV_8UC1);

    for (int i = 0; i < rMask.cols; i++)
    {
        for (int j = 0; j < rMask.rows; j++)
        {
            // 获取掩码图像的像素值
            char v = rMask.at<char>(j, i);
            // 根据掩码像素值设置结果图像的像素值
            switch (v)
            {
            case 0: // 0 表示背景
            case 2: // 2 表示可能的背景
                mask.at<char>(j, i) = 0; // 设置为黑色
                break;

            case 1: // 1 表示前景
            case 3: // 3 表示可能的前景
                mask.at<char>(j, i) = 1; // 设置为白
                break;
            }
        }
    }

    cv::cvtColor(mask, mask, COLOR_GRAY2BGR);
    cv::multiply(img, mask, img);
    cv::imshow("result", img);
}

int main()
{
    demo2();
    cv::waitKey(0);
}
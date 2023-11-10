/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：stitching 全景拼接
博客：http://www.bilibili996.com/Course?id=3291377000366
作者：高仁宝
时间：2023.11
*/

#include <iostream>
#include <fstream>
#include <opencv2/core/core.hpp>
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/stitching.hpp"
#include<Windows.h>

using namespace std;
using namespace cv;

/// <summary>
/// 全景拼接，去掉黑边
/// </summary>
void stitch1()
{
    vector<Mat> imgs;
    Mat img1 = imread("../images/stitching1.jpg");
    Mat img2 = imread("../images/stitching2.jpg");
    Mat img3 = imread("../images/stitching3.jpg");
    imgs.push_back(img1);
    imgs.push_back(img2);
    imgs.push_back(img3);
    Mat pano;
    Ptr<Stitcher> stitcher = Stitcher::create();
    Stitcher::Status status = stitcher->stitch(imgs, pano);
    if (status != Stitcher::OK)
    {
        cout << "Can't stitch images, error code = " << status << endl;
        return;
    }

    // 全景图轮廓提取
    Mat stitched, gray, thresh;
    copyMakeBorder(pano, stitched, 10, 10, 10, 10, BORDER_CONSTANT, (0, 0, 0));
    cvtColor(stitched, gray, COLOR_BGR2GRAY);

    threshold(gray, thresh, 0, 255, THRESH_BINARY);

    //定义轮廓和层次结构
    vector<vector<Point>> cnts;
    vector<Vec4i> hierarchy;
    findContours(thresh, cnts, hierarchy, RETR_EXTERNAL, CHAIN_APPROX_SIMPLE);

    // 创建单通道黑色图像, 轮廓最小正矩形
    Mat mask(thresh.rows, thresh.cols, CV_8U);
    mask = Scalar::all(0);

    // 取出list中的轮廓二值图
    Rect boundRect = boundingRect(Mat(cnts[0]));

    //绘制矩形
    rectangle(mask, boundRect.tl(), boundRect.br(), Scalar(255, 255, 255), -1, 8, 0);

    // 腐蚀处理，直到minRect的像素值都为0
    Mat minRect, sub;
    mask.copyTo(minRect);
    mask.copyTo(sub);
    while (countNonZero(sub) > 0) {
        // 行腐蚀操作 
        Mat element = getStructuringElement(MORPH_RECT, Size(5, 5));
        erode(minRect, minRect, element);
        subtract(minRect, thresh, sub);
    }

    // 提取minRect轮廓并裁剪
    findContours(minRect, cnts, hierarchy, RETR_EXTERNAL, CHAIN_APPROX_SIMPLE, Point(0, 0));
    boundRect = boundingRect(cnts[0]);
    // 裁剪
    stitched = stitched(boundRect);
    imshow("result1", stitched);
    imwrite("result1.jpg", stitched);
}

/// <summary>
/// 全景拼接，没去黑边
/// </summary>
void stitch2()
{
    vector<Mat> imgs;
    Mat img1 = imread("../images/stitching1.jpg");
    Mat img2 = imread("../images/stitching2.jpg");
    Mat img3 = imread("../images/stitching3.jpg");
    imgs.push_back(img1);
    imgs.push_back(img2);
    imgs.push_back(img3);
    Mat pano;
    Ptr<Stitcher> stitcher = Stitcher::create();
    Stitcher::Status status = stitcher->stitch(imgs, pano);
    if (status != Stitcher::OK)
    {
        cout << "Can't stitch images, error code = " << status << endl;
        return;
    }
    imshow("result2", pano);
    imwrite("result2.jpg", pano);
}

int main()
{
    stitch1();
    stitch2();
    waitKey();
    destroyAllWindows();
    return 0;
}
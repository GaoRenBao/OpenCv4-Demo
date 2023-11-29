/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：MSER区域检测
博客：http://www.bilibili996.com/Course?id=09740476aebb4690a6f57e56063b28c8
作者：高仁宝
时间：2023.11
*/

#include <iostream>    
#include <opencv2\opencv.hpp>    
#include <opencv2\highgui\highgui.hpp>    
#include <windows.h>

using namespace cv;
using namespace std;

int main()
{
    Mat img = imread("../images/WQbGH.jpg");
    
    Rect rect;
    rect.x = 5;
    rect.y = 5;
    rect.width = img.cols - 10;
    rect.height = img.rows - 10;
    img = img(rect);
    Ptr<MSER> mser = MSER::create();

    // Resize the image so that MSER can work better
    Mat img2;
    resize(img, img2, Size(img.cols * 2, img.rows * 2)); // 扩大

    Mat gray;
    cvtColor(img2, gray, COLOR_BGR2GRAY);

    Mat vis;
    img2.copyTo(vis);
    vector<vector<Point>> msers;
    vector<Rect> bboxes;
    mser->detectRegions(gray, msers, bboxes);

    vector<vector<Point>> hulls;
    for (int i = 0; i < msers.size(); i++)
    {
        vector<Point> hull;
        convexHull(msers[i], hull);
        hulls.push_back(hull);
    }
    polylines(vis, hulls, true, Scalar(0, 255, 0));

    Mat img3;
    resize(vis, img3, img.size());
    namedWindow("img", 0);
    imshow("img", img3);
    waitKey();
    destroyAllWindows();
}
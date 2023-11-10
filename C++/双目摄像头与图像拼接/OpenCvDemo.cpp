/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：双目摄像头与图像拼接
博客：http://www.bilibili996.com/Course?id=1048651000244
作者：高仁宝
时间：2023.11
*/

#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/Stitching.hpp"
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat img_1 = imread("../images/orb (1).jpg");
    Mat img_2 = imread("../images/orb (2).jpg");
    imshow("img_1", img_1);
    imshow("img_2", img_2);

    vector<Mat> imgs;
    imgs.push_back(img_1);
    imgs.push_back(img_2);

    Mat panoMat;
    Stitcher::Mode mode = Stitcher::SCANS;
    Ptr<Stitcher> stitcher = Stitcher::create(mode);
    Stitcher::Status status = stitcher->stitch(imgs, panoMat);
    if (status != Stitcher::OK)
    {
        cout << "拼接失败" << endl;
        waitKey(0);
        return 0;
    }

    imshow("拼接结果", panoMat);
    waitKey(0);
    return 0;
}
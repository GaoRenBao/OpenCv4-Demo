/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：对极几何
博客：http://www.bilibili996.com/Course?id=52c06e9172914d5eb6443fbd8c466974
作者：高仁宝
时间：2023.11
资料：
https://docs.opencv.org/4.5.5/da/de9/tutorial_py_epipolar_geometry.html
https://blog.csdn.net/zhoujinwang/article/details/128349114
*/

#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/features2d/features2d.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat img1 = cv::imread("../images/myleft.jpg", 0);
    Mat img2 = cv::imread("../images/myright.jpg", 0);

    Ptr<SIFT> sift = SIFT::create();
    Mat des1, des2;
    vector<KeyPoint> kp1, kp2;
    sift->detectAndCompute(img1, cv::Mat(), kp1, des1);
    sift->detectAndCompute(img2, cv::Mat(), kp2, des2);

    Ptr<flann::IndexParams> index_params = makePtr<flann::KDTreeIndexParams>(5);
    index_params->setAlgorithm(0);

    Ptr<flann::SearchParams> search_params = makePtr<flann::SearchParams>(50);
    FlannBasedMatcher flann = FlannBasedMatcher(index_params, search_params);

    vector<vector<DMatch>> matches;
    flann.knnMatch(des1, des2, matches, 2);
   
    vector<Point2f> pts1;
    vector<Point2f> pts2;
    for (int i = 0; i < matches.size(); i++)
    {
        if (matches[i][0].distance < 0.7 * matches[i][1].distance)
        {
            pts2.push_back(kp2[matches[i][0].trainIdx].pt);
            pts1.push_back(kp1[matches[i][0].queryIdx].pt);
        }
    }

    // 匹配点列表，用它来计算【基础矩阵】
    Mat mask;
    cv::Mat F = cv::findFundamentalMat(pts1, pts2, mask, FM_LMEDS);

    std::vector<cv::Vec<float, 3>> epilines1, epilines2;
    cv::computeCorrespondEpilines(pts1, 2, F, epilines1);
    cv::computeCorrespondEpilines(pts2, 1, F, epilines2);

    cv::RNG& rng = theRNG();
    for (int i = 0; i < pts1.size(); i++)
    {
        Scalar color = Scalar(rng(256), rng(256), rng(256));
        line(img1, Point(0, -epilines1[i][2] / epilines1[i][1]),
            Point(img1.cols, -(epilines1[i][2] + epilines1[i][0] * img1.cols) / epilines1[i][1]), color);

        line(img2, Point(0, -epilines2[i][2] / epilines2[i][1]),
            Point(img2.cols, -(epilines2[i][2] + epilines2[i][0] * img2.cols) / epilines2[i][1]), color);
    }

    cv::imshow("img1", img1);
    cv::imshow("img2", img2);
    cv::waitKey(0);
}
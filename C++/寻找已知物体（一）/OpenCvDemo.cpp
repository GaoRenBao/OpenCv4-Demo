/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：寻找已知物体（一）
博客：http://www.bilibili996.com/Course?id=058f71e965d74a4284cfe77621ff200f
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/features2d.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    // queryImage
    Mat img1 = cv::imread("../images/book_box.jpg");
    // trainImage
    Mat img2 = cv::imread("../images/book2.jpg");

    Ptr<SIFT> sift = SIFT::create();
    vector<KeyPoint> kp1, kp2;
    Mat des1, des2;
    sift->detectAndCompute(img1, Mat(), kp1, des1);
    sift->detectAndCompute(img2, Mat(), kp2, des2);

    //BFMatcher with default params
    BFMatcher bf;
    vector<vector<DMatch>> matches;
    bf.knnMatch(des1, des2, matches, 2);

    vector<vector<DMatch>> good;
    for (int i = 0; i < matches.size(); i++)
    {
        if (matches[i][0].distance < 0.75 * matches[i][1].distance)
            good.push_back(matches[i]);
    }

    //cv2.drawMatchesKnn expects list of lists as matches.
    Mat img3;
    drawMatches(img1 = img1, kp1, img2, kp2, good, img3,
        Scalar::all(-1), Scalar::all(-1), std::vector<std::vector<char> >(),
        DrawMatchesFlags::NOT_DRAW_SINGLE_POINTS);

    cv::imshow("img", img3);
    cv::waitKey(0);
}
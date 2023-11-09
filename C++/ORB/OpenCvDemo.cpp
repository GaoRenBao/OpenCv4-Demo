/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：ORB
博客：http://www.bilibili996.com/Course?id=e64cab8fe66f4380ba8a7e18469f2d21
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    Mat img = cv::imread("../images/blox.jpg", 0);

    // Initiate ORB detector
    Ptr<ORB> orb = cv::ORB::create();

    // find the keypoints with ORB
    vector<KeyPoint> kp;
    orb->detect(img, kp);

    // compute the descriptors with ORB
    Mat des;
    orb->compute(img, kp, des);

    // draw only keypoints location,not size and orientation
    Mat img2;
    drawKeypoints(img, kp, img2, Scalar(0, 255, 0), DrawMatchesFlags::DEFAULT);

    cv::imshow("img", img2);
    cv::waitKey(0);
}
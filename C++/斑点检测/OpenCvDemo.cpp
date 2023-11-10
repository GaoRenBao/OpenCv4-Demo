/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：斑点检测
博客：http://www.bilibili996.com/Course?id=5ecd1f6fca2641a2a228c60ae09ce4c1
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    // Read image
    Mat im = cv::imread("../images/blob.jpg", IMREAD_GRAYSCALE);

    // 创建SimpleBlobDetector.Params对象并设置参数
    cv::SimpleBlobDetector::Params detectorParams;
    //detectorParams.thresholdStep = 5;
    //detectorParams.minThreshold = 100;
    //detectorParams.maxThreshold = 255;
    //detectorParams.minRepeatability = 1;
    //detectorParams.minDistBetweenBlobs = 10;
    detectorParams.filterByColor = false;
    //detectorParams.blobColor = 0;
    //斑点面积
    detectorParams.filterByArea = true;
    detectorParams.minArea = 100;
    detectorParams.maxArea = 100000;
    //斑点圆度
    detectorParams.filterByCircularity = false;
    //detectorParams.minCircularity = 0;
    //detectorParams.maxCircularity = (float)0;
    //斑点惯性率
    detectorParams.filterByInertia = false;
    //detectorParams.minInertiaRatio = 0;
    //detectorParams.maxInertiaRatio = (float)0;
    //斑点凸度
    detectorParams.filterByConvexity = false;
    //detectorParams.minConvexity = 0;
    //detectorParams.maxConvexity = (float)0;

    // Set up the detector with default parameters.
    cv::Ptr<cv::SimpleBlobDetector> detector = cv::SimpleBlobDetector::create(detectorParams);

    vector<cv::KeyPoint> key_points;
    detector->detect(im, key_points);
    cv::Mat im_with_keypoints;
    //绘制结果
    cv::drawKeypoints(im, key_points, im_with_keypoints, cv::Scalar(0, 0, 255), DrawMatchesFlags::DRAW_RICH_KEYPOINTS);

    cv::imshow("Keypoints", im_with_keypoints);
    cv::waitKey(0);
}
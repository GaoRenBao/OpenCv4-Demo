/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：角点检测的FAST算法（FAST特征检测器）
博客：http://www.bilibili996.com/Course?id=421a041d9cd54125a4d8f035ca7e2b24
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/features2d.hpp>
#include <opencv2/core/utils/logger.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    // 关闭opencv日志
    cv::utils::logging::setLogLevel(cv::utils::logging::LOG_LEVEL_SILENT);

    Mat img = cv::imread("../images/blox.jpg", 0);

    // Initiate FAST object with default values
    cv::Ptr<cv::FastFeatureDetector> fast = cv::FastFeatureDetector::create();

    // find and draw the keypoints
    std::vector<cv::KeyPoint> kp;
    fast->detect(img, kp);

    cv::Mat img2;
    cv::drawKeypoints(img, kp, img2, Scalar(255, 0, 0));
    // Print all default params
    cout << "Threshold:" << fast->getThreshold() << endl;
    cout << "nonmaxSuppression:" << fast->getNonmaxSuppression() << endl;
    cout << "neighborhood:" << fast->getType() << endl;
    cout << "Total Keypoints with nonmaxSuppression:" << kp.size() << endl;
    cv::imshow("fast_true.png", img2);

    // Disable nonmaxSuppression
    fast->setNonmaxSuppression(false);
    fast->detect(img, kp);
    cout << "Total Keypoints without nonmaxSuppression:" << kp.size() << endl;

    cv::Mat img3;
    cv::drawKeypoints(img, kp, img3, Scalar(255, 0, 0));
    cv::imshow("fast_false.png", img3);
    // 第一幅图是使用了非最大值抑制的结果
    // 第二幅没有使用非最大值抑制。
    cv::waitKey(0);
}
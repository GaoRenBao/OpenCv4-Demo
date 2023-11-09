/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：使用掩膜绘制直方图
博客：http://www.bilibili996.com/Course?id=2605222000268
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <string>

using namespace cv;
using namespace std;

/// <summary>
///  绘制直方图
/// </summary>
/// <param name="histImage"></param>
/// <param name="hist"></param>
/// <param name="color"></param>
/// <returns></returns>
void DrawHist(Mat histImage, Mat hist, Scalar color)
{
    int binW = cvRound(histImage.cols / hist.rows);
    //归一化
    normalize(hist, hist, 0, hist.rows, NORM_MINMAX, -1, Mat());
    for (int i = 1; i < hist.rows; i++)
    {
        Point pt1 = Point(binW * (i - 1), histImage.rows - cvRound(hist.at<float>(i - 1)));
        Point pt2 = Point(binW * (i), histImage.rows - cvRound(hist.at<float>(i)));
        line(histImage, pt1, pt2, color, 1, 8, 0);
    }
}


int main()
{
    Mat img = cv::imread("../images/home3.jpg", 0);
    cv::Mat mask = cv::Mat(img.size(), CV_8UC1, cv::Scalar(0, 0, 0));

    std::vector<cv::Point> points;
    points.push_back(cv::Point(100, 100));
    points.push_back(cv::Point(400, 100));
    points.push_back(cv::Point(400, 300));
    points.push_back(cv::Point(100, 300));
    cv::fillConvexPoly(mask, points.data(), points.size(), cv::Scalar(255));

    Mat masked_img;
    cv::bitwise_and(img, img, masked_img, mask);

    // Calculate histogram with mask and without mask
    // Check third argument for mask

    float range[] = { 0, 256 };
    int channels[] = { 0 };
    int hist_size[] = { 256 };
    const float* ranges[] = { range };

    MatND hist_full, hist_mask;
    calcHist(&img, 1, channels, Mat(), hist_full, 1, hist_size, ranges);
    calcHist(&img, 1, channels, mask, hist_mask, 1, hist_size, ranges);

    cv::Mat hist = cv::Mat(img.size(), CV_8UC3, cv::Scalar(255, 255, 255));
    DrawHist(hist, hist_full, Scalar(0, 0, 255));
    DrawHist(hist, hist_mask, Scalar(0, 255, 0));

    cv::imshow("img", img);
    cv::imshow("mask", mask);
    cv::imshow("masked_img", masked_img);
    cv::imshow("hist", hist);
    cv::waitKey(0);
}
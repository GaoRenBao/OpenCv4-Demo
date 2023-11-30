/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：K值聚类（一）
博客：http://www.bilibili996.com/Course?id=7d02676837f34dcc84606d906d877c3b
作者：高仁宝
时间：2023.11

参考博客：https://blog.csdn.net/qq_40622955/article/details/122853991
*/

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/ml/ml.hpp>

using namespace cv;
using namespace std;

int main()
{
    Mat src = cv::imread("../images/fruits.jpg");
    cv::imshow("Source", src);
    cv::waitKey(1); // do events

    cv::blur(src, src, Size(15, 15));
    cv::imshow("Blurred Image", src);
    cv::waitKey(1); // do events

    // Converts the MxNx3 image into a Kx3 matrix where K=MxN and
    // each row is now a vector in the 3-D space of RGB.
    // change to a Mx3 column vector (M is number of pixels in image)
    Mat columnVector = src.reshape(3, src.rows * src.cols);

    // convert to floating point, it is a requirement of the k-means method of OpenCV.
    Mat samples;
    columnVector.convertTo(samples, CV_32FC3);

    TermCriteria criteria = TermCriteria(TermCriteria::EPS | TermCriteria::MAX_ITER, 10, 1.0);
    int K = 2; // 2、4、6、8

    Mat bestLabels;
    Mat centers;
    cv::kmeans(samples, K, bestLabels, criteria, 3, KmeansFlags::KMEANS_PP_CENTERS, centers);

    //create a color map
    std::vector<cv::Scalar> colorMaps;
    uchar b, g, r;;
    //clusterCount is equal to centers.rows
    for (int i = 0; i < centers.rows; i++)
    {
        b = (uchar)centers.at<float>(i, 0);
        g = (uchar)centers.at<float>(i, 1);
        r = (uchar)centers.at<float>(i, 2);
        colorMaps.push_back(cv::Scalar(b, g, r));
    }
    // Show  result
    int index = 0;
    Mat clusteredImage(src.rows, src.cols, src.type());
    uchar* ptr = NULL;
    int* label = NULL;
    for (int row = 0; row < src.rows; row++) {
        ptr = clusteredImage.ptr<uchar>(row);
        for (int col = 0; col < src.cols; col++) {
            index = row * src.cols + col;
            label = bestLabels.ptr<int>(index);
            *(ptr + col * 3) = colorMaps[*label][0];
            *(ptr + col * 3 + 1) = colorMaps[*label][1];
            *(ptr + col * 3 + 2) = colorMaps[*label][2];
        }
    }

    cv::imshow("Clustered Image", clusteredImage);
    cv::waitKey();
    cv::destroyAllWindows();
}



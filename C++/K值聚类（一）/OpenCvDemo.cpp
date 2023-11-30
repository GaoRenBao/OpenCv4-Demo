/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：K值聚类（一）
博客：http://www.bilibili996.com/Course?id=7d02676837f34dcc84606d906d877c3b
作者：高仁宝
时间：2023.11
*/

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/ml/ml.hpp>

using namespace cv;
using namespace std;

int main()
{
    const int maxClusters = 5;
    cv::RNG rng(12345);
    while (true)
    {
        int clustersCount = rng.uniform(2, maxClusters + 1);
        int samplesCount = rng.uniform(1, 1001);

        Mat points(samplesCount,1, CV_32FC2);
        clustersCount = min(clustersCount, samplesCount);

        Mat img(500, 500, CV_8UC4, Scalar::all(0));

        // generate random sample from multi-gaussian distribution
        for (int k = 0; k < clustersCount; k++)
        {
            Mat pointChunk = points.rowRange(
                k * samplesCount / clustersCount, 
                (k == clustersCount - 1) ? samplesCount : (k + 1) * samplesCount / clustersCount);

            Point center(rng.uniform(0, img.cols), rng.uniform(0, img.rows));
           
            rng.fill(
                pointChunk,
                cv::RNG::NORMAL,
                Scalar(center.x, center.y),
                Scalar(img.cols * 0.05f, img.rows * 0.05f));
        }
        cv::randShuffle(points, 1, &rng);

        Mat labels;
        Mat centers(clustersCount, 1, points.type());
        cv::kmeans(
            points,
            clustersCount,
            labels,
            TermCriteria(TermCriteria::EPS | TermCriteria::MAX_ITER, 10, 1.0),
            3,
             KmeansFlags::KMEANS_PP_CENTERS,
            centers);


        vector<Scalar> colors;
        colors.push_back(Scalar(0, 0, 255));
        colors.push_back(Scalar(0, 255, 0));
        colors.push_back(Scalar(255, 100, 100));
        colors.push_back(Scalar(255, 0, 255));
        colors.push_back(Scalar(0, 255, 255));

        for (int i = 0; i < samplesCount; i++)
        {
            int clusterIdx = labels.at<int>(i);
            Point2f ipt = points.at<Point2f>(i);
            cv::circle(
                img,
                Point(ipt.x, ipt.y),
                2,
                colors[clusterIdx],
                LineTypes::LINE_AA,
                1);
        }
        cv::imshow("img", img);
        char key = (char)cv::waitKey();
        if (key == 27 || key == 'q' || key == 'Q') // 'ESC'
        {
            break;
        }
    }
    return 0;
}



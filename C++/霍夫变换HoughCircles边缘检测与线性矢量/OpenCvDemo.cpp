/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：霍夫变换HoughCircles边缘检测与线性矢量
博客：http://www.bilibili996.com/Course?id=4885001000177
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace cv;
using namespace std;

int main()
{
    //【1】载入原始图、Mat变量定义   
    Mat srcImage = imread("../images/HoughCircles.jpg"); 
    Mat midImage;//临时变量和目标图的定义

    //【2】显示原始图
    imshow("【原始图】", srcImage);

    //【3】转为灰度图并进行图像平滑
    // opencv3：CV_BGR2GRAY
    // opencv4：COLOR_BGR2GRAY
    cvtColor(srcImage, midImage, COLOR_BGR2GRAY);//转化边缘检测后的图为灰度图
    GaussianBlur(midImage, midImage, Size(9, 9), 2, 2);

    //【4】进行霍夫圆变换
    vector<Vec3f> circles;
    // opencv3：CV_HOUGH_GRADIENT
    // opencv4：HOUGH_GRADIENT
    HoughCircles(midImage, circles, HOUGH_GRADIENT, 1.5, 10, 200, 100, 0, 0);

    //【5】依次在图中绘制出圆
    for (size_t i = 0; i < circles.size(); i++)
    {
        //参数定义
        Point center(cvRound(circles[i][0]), cvRound(circles[i][1]));
        int radius = cvRound(circles[i][2]);
        //绘制圆心
        circle(srcImage, center, 5, Scalar(0, 255, 0), -1, 8, 0);
        //绘制圆轮廓
        circle(srcImage, center, radius, Scalar(155, 50, 255), 5, 8, 0);
    }

    //【6】显示效果图  
    // resize(srcImage, srcImage, Size(srcImage.cols * 0.5, srcImage.rows * 0.5), (0, 0), (0, 0), 3);
    imshow("【C++ 效果图】", srcImage);

    waitKey(0);
    return 0;
}
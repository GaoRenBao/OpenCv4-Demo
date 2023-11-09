/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：颜色识别
博客：http://www.bilibili996.com/Course?id=0177963000226
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>  
#include <opencv2/imgproc/imgproc.hpp>  

using namespace cv;

// 颜色识别(除红色外的其他单色)
Mat ColorFindContours(Mat srcImage,
    int iLowH, int iHighH,
    int iLowS, int iHighS,
    int iLowV, int iHighV)
{
    Mat bufImg;
    Mat imgHSV;
    //转为HSV
    cvtColor(srcImage, imgHSV, COLOR_BGR2HSV);
    inRange(imgHSV, Scalar(iLowH, iLowS, iLowV), Scalar(iHighH, iHighS, iHighV), bufImg);
    return bufImg;
}

// 颜色识别（红色）
Mat ColorFindContours2(Mat srcImage)
{
    Mat des1 = ColorFindContours(srcImage,
        350 / 2, 360 / 2,         // 色调最小值~最大值
        (int)(255 * 0.70), 255,   // 饱和度最小值~最大值
        (int)(255 * 0.60), 255);  // 亮度最小值~最大值

    Mat des2 = ColorFindContours(srcImage,
        0, 16 / 2,                // 色调最小值~最大值
        (int)(255 * 0.70), 255,   // 饱和度最小值~最大值
        (int)(255 * 0.60), 255);  // 亮度最小值~最大值

    return des1 + des2;
}

int main()
{
    //载入色卡
    Mat srcImage = imread("../images/color.jpg");
    imshow("原始图", srcImage);

    // 黄色识别
    Mat des = ColorFindContours(srcImage,
        45 / 2, 60 / 2,          // 色调最小值~最大值
        (int)(255 * 0.60), 255,  // 饱和度最小值~最大值
        (int)(255 * 0.90), 255); // 亮度最小值~最大值
    imshow("des1", des);

    // 红色识别
    des = ColorFindContours2(srcImage);
    imshow("des2", des);

    waitKey(0);
    return 0;
}
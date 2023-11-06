/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：两种非线性滤波
博客：http://www.bilibili996.com/Course?id=5395388000126
作者：高仁宝
时间：2023.11
*/

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

using namespace cv;

int main()
{
    // 载入原图
    Mat image = imread("../images/girl3.jpg");

    // 创建窗口
    namedWindow("【原图】");
    namedWindow("中值滤波【效果图】");
    namedWindow("双边滤波【效果图】");

    // 显示原图
    imshow("【原图】", image);

    //进行中值滤波操作
    Mat out1;
    medianBlur(image, out1, 7);

    //进行双边滤波操作
    Mat out2;
    bilateralFilter(image, out2, 25, 25 * 2, 25 / 2);

    //显示效果图
    imshow("中值滤波【效果图】", out1);
    imshow("双边滤波【效果图】", out2);

    waitKey(0);
    return 0;
}
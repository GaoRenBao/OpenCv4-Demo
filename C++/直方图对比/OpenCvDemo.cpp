/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：直方图对比
博客：http://www.bilibili996.com/Course?id=3750176000216
作者：高仁宝
时间：2023.11
*/

#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
using namespace cv;

int main()
{
    //【1】声明储存基准图像和另外两张对比图像的矩阵( RGB 和 HSV )
    Mat srcImage_base, hsvImage_base;
    Mat srcImage_test1, hsvImage_test1;
    Mat srcImage_test2, hsvImage_test2;
    Mat hsvImage_halfDown;

    //【2】载入基准图像(srcImage_base) 和两张测试图像srcImage_test1、srcImage_test2，并显示
    srcImage_base = imread("../images/book1.jpg");
    srcImage_test1 = imread("../images/book2.jpg");
    srcImage_test2 = imread("../images/book3.jpg");

    //显示载入的3张图像
    imshow("基准图像", srcImage_base);
    imshow("测试图像1", srcImage_test1);
    imshow("测试图像2", srcImage_test2);

    // 【3】将图像由BGR色彩空间转换到 HSV色彩空间
    cvtColor(srcImage_base, hsvImage_base, COLOR_BGR2HSV);
    cvtColor(srcImage_test1, hsvImage_test1, COLOR_BGR2HSV);
    cvtColor(srcImage_test2, hsvImage_test2, COLOR_BGR2HSV);

    //【4】创建包含基准图像下半部的半身图像(HSV格式)
    hsvImage_halfDown = hsvImage_base(Range(hsvImage_base.rows / 2, hsvImage_base.rows - 1), Range(0, hsvImage_base.cols - 1));

    //【5】初始化计算直方图需要的实参
    // 对hue通道使用30个bin,对saturatoin通道使用32个bin
    int h_bins = 50; int s_bins = 60;
    int histSize[] = { h_bins, s_bins };
    // hue的取值范围从0到256, saturation取值范围从0到180
    float h_ranges[] = { 0, 256 };
    float s_ranges[] = { 0, 180 };
    const float* ranges[] = { h_ranges, s_ranges };
    // 使用第0和第1通道
    int channels[] = { 0, 1 };

    // 【6】创建储存直方图的 MatND 类的实例:
    MatND baseHist;
    MatND halfDownHist;
    MatND testHist1;
    MatND testHist2;

    // 【7】计算基准图像，两张测试图像，半身基准图像的HSV直方图:
    calcHist(&hsvImage_base, 1, channels, Mat(), baseHist, 2, histSize, ranges, true, false);
    normalize(baseHist, baseHist, 0, 1, NORM_MINMAX, -1, Mat());

    calcHist(&hsvImage_halfDown, 1, channels, Mat(), halfDownHist, 2, histSize, ranges, true, false);
    normalize(halfDownHist, halfDownHist, 0, 1, NORM_MINMAX, -1, Mat());

    calcHist(&hsvImage_test1, 1, channels, Mat(), testHist1, 2, histSize, ranges, true, false);
    normalize(testHist1, testHist1, 0, 1, NORM_MINMAX, -1, Mat());

    calcHist(&hsvImage_test2, 1, channels, Mat(), testHist2, 2, histSize, ranges, true, false);
    normalize(testHist2, testHist2, 0, 1, NORM_MINMAX, -1, Mat());


    //【8】按顺序使用4种对比标准将基准图像的直方图与其余各直方图进行对比:
    for (int i = 0; i < 4; i++)
    {
        //进行图像直方图的对比
        int compare_method = i;
        double base_base = compareHist(baseHist, baseHist, compare_method);
        double base_half = compareHist(baseHist, halfDownHist, compare_method);
        double base_test1 = compareHist(baseHist, testHist1, compare_method);
        double base_test2 = compareHist(baseHist, testHist2, compare_method);

        //输出结果
        printf("方法 [%d] 的匹配结果如下：\r\n", i);
        printf("【基准图 - 基准图】：%f：\r\n", base_base);
        printf("【基准图 - 半身图】：%f：\r\n", base_half);
        printf("【基准图 - 测试图1】：%f：\r\n", base_test1);
        printf("【基准图 - 测试图2】：%f：\r\n\r\n", base_test2);
    }

    printf("检测结束。");
    waitKey(0);
    return 0;
}
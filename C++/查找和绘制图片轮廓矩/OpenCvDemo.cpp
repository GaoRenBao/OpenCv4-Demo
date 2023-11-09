/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：查找和绘制图片轮廓矩
博客：http://www.bilibili996.com/Course?id=4739103000210
作者：高仁宝
时间：2023.11
*/

#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include <iostream>
using namespace cv;
using namespace std;

#define WINDOW_NAME1 "【原始图】"     //为窗口标题定义的宏 
#define WINDOW_NAME2 "【图像轮廓】"   //为窗口标题定义的宏 

Mat g_srcImage; Mat g_grayImage;
int g_nThresh = 100;
int g_nMaxThresh = 255;
RNG g_rng(12345);
Mat g_cannyMat_output;
vector<vector<Point> > g_vContours;
vector<Vec4i> g_vHierarchy;

void on_ThreshChange(int, void*)
{
    // 使用Canndy检测边缘
    Canny(g_grayImage, g_cannyMat_output, g_nThresh, g_nThresh * 2, 3);

    // 找到轮廓
    findContours(g_cannyMat_output, g_vContours, g_vHierarchy, RETR_TREE, CHAIN_APPROX_SIMPLE, Point(0, 0));

    // 计算矩
    vector<Moments> mu(g_vContours.size());
    for (unsigned int i = 0; i < g_vContours.size(); i++)
    {
        mu[i] = moments(g_vContours[i], false);
    }

    //  计算中心矩
    vector<Point2f> mc(g_vContours.size());
    for (unsigned int i = 0; i < g_vContours.size(); i++)
    {
        mc[i] = Point2f(static_cast<float>(mu[i].m10 / mu[i].m00), static_cast<float>(mu[i].m01 / mu[i].m00));
    }

    // 绘制轮廓
    Mat drawing = Mat::zeros(g_cannyMat_output.size(), CV_8UC3);
    for (unsigned int i = 0; i < g_vContours.size(); i++)
    {
        Scalar color = Scalar(g_rng.uniform(0, 255), g_rng.uniform(0, 255), g_rng.uniform(0, 255));//随机生成颜色值
        drawContours(drawing, g_vContours, i, color, 2, 8, g_vHierarchy, 0, Point());//绘制外层和内层轮廓
        circle(drawing, mc[i], 4, color, -1, 8, 0);;//绘制圆
    }

    // 显示到窗口中
    namedWindow(WINDOW_NAME2, WINDOW_AUTOSIZE);
    imshow(WINDOW_NAME2, drawing);

    // 通过m00计算轮廓面积并且和OpenCV函数比较
    printf("\t 输出内容: 面积和轮廓长度\n");
    for (unsigned int i = 0; i < g_vContours.size(); i++)
    {
        printf(">通过m00计算出轮廓[%d]的面积: (M_00)=%.2f\n", i, mu[i].m00);
        printf("OpenCV函数计算出的面积=%.2f\n", contourArea(g_vContours[i]));
        printf("长度:%.2f\n\n", arcLength(g_vContours[i], true));

        Scalar color = Scalar(g_rng.uniform(0, 255), g_rng.uniform(0, 255), g_rng.uniform(0, 255));
        drawContours(drawing, g_vContours, i, color, 2, 8, g_vHierarchy, 0, Point());
        circle(drawing, mc[i], 4, color, -1, 8, 0);
    }
}

int main(int argc, char** argv)
{
    //【0】改变console字体颜色
    system("color 71");

    // 读入原图像, 返回3通道图像数据
    g_srcImage = imread("../images/lake2.jpg", 1);

    // 把原图像转化成灰度图像并进行平滑
    cvtColor(g_srcImage, g_grayImage, COLOR_BGR2GRAY);
    blur(g_grayImage, g_grayImage, Size(3, 3));

    // 创建新窗口
    namedWindow(WINDOW_NAME1, WINDOW_AUTOSIZE);
    imshow(WINDOW_NAME1, g_srcImage);

    //创建滚动条并进行初始化
    createTrackbar(" 阈值", WINDOW_NAME1, &g_nThresh, g_nMaxThresh, on_ThreshChange);
    on_ThreshChange(0, 0);

    waitKey(0);
    return(0);
}
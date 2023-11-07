/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：二值化基本阈值操作
博客：http://www.bilibili996.com/Course?id=4215192000168
作者：高仁宝
时间：2023.11
*/

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>
using namespace cv;
using namespace std;

#define WINDOW_NAME "【程序窗口】"        //为窗口标题定义的宏 

int g_nThresholdValue = 100;
int g_nThresholdType = 3;
Mat g_srcImage, g_grayImage, g_dstImage;

static void ShowHelpText();//输出帮助文字
void on_Threshold(int, void*);//回调函数

int main()
{
    //【0】改变console字体颜色
    system("color 1F");

    //【0】显示欢迎和帮助文字
    ShowHelpText();

    //【1】读入源图片
    g_srcImage = imread("../images/lake.jpg");
    if (!g_srcImage.data) { 
        printf("读取图片错误，请确定目录下是否有imread函数指定的图片存在~！ \n"); 
        return false; 
    }
    imshow("原始图", g_srcImage);

    //【2】存留一份原图的灰度图
    // opencv3：CV_RGB2GRAY
    // opencv4：COLOR_RGB2GRAY
    cvtColor(g_srcImage, g_grayImage, COLOR_RGB2GRAY);

    //【3】创建窗口并显示原始图
    // opencv3：CV_WINDOW_AUTOSIZE
    // opencv4：WINDOW_AUTOSIZE
    namedWindow(WINDOW_NAME, WINDOW_AUTOSIZE);

    //【4】创建滑动条来控制阈值
    createTrackbar("模式", WINDOW_NAME, &g_nThresholdType, 4, on_Threshold);
    createTrackbar("参数值", WINDOW_NAME, &g_nThresholdValue, 255, on_Threshold);

    //【5】初始化自定义的阈值回调函数
    on_Threshold(0, 0);

    // 【6】轮询等待用户按键，如果ESC键按下则退出程序
    while (1)
    {
        if ((char)waitKey(20) == 27)
        { 
            break; 
        }
    }
}

void on_Threshold(int, void*)
{
    //调用阈值函数
    threshold(g_grayImage, g_dstImage, g_nThresholdValue, 255, g_nThresholdType);

    //更新效果图
    imshow(WINDOW_NAME, g_dstImage);
}

static void ShowHelpText()
{
    printf("\n\n\t\t\t   当前使用的OpenCV版本为：" CV_VERSION);
    printf("\n\n  ----------------------------------------------------------------------------\n");

    //输出一些帮助信息  
    printf("\n\t欢迎来到【基本阈值操作】示例程序~\n\n");
    printf("\n\t按键操作说明: \n\n"
        "\t\t键盘按键【ESC】- 退出程序\n"
        "\t\t滚动条模式0- 二进制阈值\n"
        "\t\t滚动条模式1- 反二进制阈值\n"
        "\t\t滚动条模式2- 截断阈值\n"
        "\t\t滚动条模式3- 反阈值化为0\n"
        "\t\t滚动条模式4- 阈值化为0\n");
}
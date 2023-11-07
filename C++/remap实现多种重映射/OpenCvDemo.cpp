/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：remap实现多种重映射
博客：http://www.bilibili996.com/Course?id=5673205000181
作者：高仁宝
时间：2023.11
*/

#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include <iostream>

using namespace cv;
using namespace std;

#define WINDOW_NAME "【程序窗口】"        //为窗口标题定义的宏 

Mat g_srcImage, g_dstImage;
Mat g_map_x, g_map_y;

int update_map(int key);
static void ShowHelpText();//输出帮助文字

int main(int argc, char** argv)
{
    //改变console字体颜色
    system("color 5F");

    //显示帮助文字
    ShowHelpText();

    //【1】载入原始图
    g_srcImage = imread("../images/Robot.jpg", 1);
    if (!g_srcImage.data) { 
        printf("读取图片错误，请确定目录下是否有imread函数指定的图片存在~！ \n"); 
        return false; 
    }
    imshow("原始图", g_srcImage);

    //【2】创建和原始图一样的效果图，x重映射图，y重映射图
    g_dstImage.create(g_srcImage.size(), g_srcImage.type());
    g_map_x.create(g_srcImage.size(), CV_32FC1);
    g_map_y.create(g_srcImage.size(), CV_32FC1);

    //【3】创建窗口并显示
    // opencv3：CV_WINDOW_AUTOSIZE
    // opencv4：WINDOW_AUTOSIZE
    namedWindow(WINDOW_NAME, WINDOW_AUTOSIZE);
    imshow(WINDOW_NAME, g_srcImage);

    //【4】轮询按键，更新map_x和map_y的值，进行重映射操作并显示效果图
    while (1)
    {
        //获取键盘按键  
        int key = waitKey(0);

        //判断ESC是否按下，若按下便退出  
        if ((key & 255) == 27)
        {
            cout << "程序退出...........\n";
            break;
        }

        //根据按下的键盘按键来更新 map_x & map_y的值. 然后调用remap( )进行重映射
        update_map(key);
        // opencv3：CV_INTER_LINEAR
        // opencv4：INTER_LINEAR
        remap(g_srcImage, g_dstImage, g_map_x, g_map_y, INTER_LINEAR, BORDER_CONSTANT, Scalar(0, 0, 0));

        //显示效果图
        imshow(WINDOW_NAME, g_dstImage);
    }
    return 0;
}

// 描述：根据按键来更新map_x与map_x的值
int update_map(int key)
{
    //双层循环，遍历每一个像素点
    for (int j = 0; j < g_srcImage.rows; j++)
    {
        for (int i = 0; i < g_srcImage.cols; i++)
        {
            switch (key)
            {
            case '1': // 键盘【1】键按下，进行第一种重映射操作
                if (i > g_srcImage.cols * 0.25 && i < g_srcImage.cols * 0.75 && j > g_srcImage.rows * 0.25 && j < g_srcImage.rows * 0.75)
                {
                    g_map_x.at<float>(j, i) = static_cast<float>(2 * (i - g_srcImage.cols * 0.25) + 0.5);
                    g_map_y.at<float>(j, i) = static_cast<float>(2 * (j - g_srcImage.rows * 0.25) + 0.5);
                }
                else
                {
                    g_map_x.at<float>(j, i) = 0;
                    g_map_y.at<float>(j, i) = 0;
                }
                break;
            case '2':// 键盘【2】键按下，进行第二种重映射操作
                g_map_x.at<float>(j, i) = static_cast<float>(i);
                g_map_y.at<float>(j, i) = static_cast<float>(g_srcImage.rows - j);
                break;
            case '3':// 键盘【3】键按下，进行第三种重映射操作
                g_map_x.at<float>(j, i) = static_cast<float>(g_srcImage.cols - i);
                g_map_y.at<float>(j, i) = static_cast<float>(j);
                break;
            case '4':// 键盘【4】键按下，进行第四种重映射操作
                g_map_x.at<float>(j, i) = static_cast<float>(g_srcImage.cols - i);
                g_map_y.at<float>(j, i) = static_cast<float>(g_srcImage.rows - j);
                break;
            }
        }
    }
    return 1;
}

// 描述：输出一些帮助信息 
static void ShowHelpText()
{
    //输出一些帮助信息  
    printf("\n\t欢迎来到重映射示例程序~\n\n");
    printf("\n\t按键操作说明: \n\n"
        "\t\t键盘按键【ESC】- 退出程序\n"
        "\t\t键盘按键【1】-  第一种映射方式\n"
        "\t\t键盘按键【2】- 第二种映射方式\n"
        "\t\t键盘按键【3】- 第三种映射方式\n"
        "\t\t键盘按键【4】- 第四种映射方式\n");
}
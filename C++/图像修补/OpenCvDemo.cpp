/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：图像修补
博客：http://www.bilibili996.com/Course?id=0063295000212
作者：高仁宝
时间：2023.11
*/

#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/photo/photo.hpp"
#include <iostream>
using namespace cv;
using namespace std;

#define WINDOW_NAME0 "【原始图参考】"        //为窗口标题定义的宏 
#define WINDOW_NAME1 "【原始图】"        //为窗口标题定义的宏 
#define WINDOW_NAME2 "【修补后的效果图】"        //为窗口标题定义的宏 

Mat srcImage0, srcImage1, inpaintMask;
Point previousPoint(-1, -1);//原来的点坐标

static void ShowHelpText()
{
    //输出一些帮助信息
    printf("\n\n\n\t欢迎来到【图像修复】示例程序~\n");
    printf("\n\t请在进行图像修复操作之前，在【原始图】窗口中进行适量的绘制"
        "\n\n\t按键操作说明: \n\n"
        "\t\t【鼠标左键】-在图像上绘制白色线条\n\n"
        "\t\t键盘按键【ESC】- 退出程序\n\n"
        "\t\t键盘按键【1】或【SPACE】-进行图像修复操作 \n\n");
}

static void On_Mouse(int event, int x, int y, int flags, void*)
{
    //鼠标左键弹起消息
    if (event == EVENT_LBUTTONUP || !(flags & EVENT_FLAG_LBUTTON))
        previousPoint = Point(-1, -1);
    //鼠标左键按下消息
    else if (event == EVENT_LBUTTONDOWN)
        previousPoint = Point(x, y);
    //鼠标按下并移动，进行绘制
    else if (event == EVENT_MOUSEMOVE && (flags & EVENT_FLAG_LBUTTON))
    {
        Point pt(x, y);
        if (previousPoint.x < 0)
            previousPoint = pt;
        //绘制白色线条
        line(inpaintMask, previousPoint, pt, Scalar::all(255), 5, 8, 0);
        line(srcImage1, previousPoint, pt, Scalar::all(255), 5, 8, 0);
        previousPoint = pt;
        imshow(WINDOW_NAME1, srcImage1);
    }
}

int main(int argc, char** argv)
{
    //改变console字体颜色
    system("color 2F");

    //显示帮助文字
    ShowHelpText();

    //载入原始图并进行掩膜的初始化
    Mat srcImage = imread("../images/Astral2.jpg", -1);
    if (!srcImage.data) 
    { 
        printf("读取图片错误，请确定目录下是否有imread函数指定图片存在~！ \n"); 
        return false; 
    }
    srcImage0 = srcImage.clone();
    srcImage1 = srcImage.clone();
    inpaintMask = Mat::zeros(srcImage1.size(), CV_8U);

    //显示原始图参考
    imshow(WINDOW_NAME0, srcImage0);
    //显示原始图
    imshow(WINDOW_NAME1, srcImage1);
    //设置鼠标回调消息
    setMouseCallback(WINDOW_NAME1, On_Mouse, 0);

    //轮询按键，根据不同的按键进行处理
    while (1)
    {
        //获取按键键值
        char c = (char)waitKey();

        //键值为ESC，程序退出
        if (c == 27)
            break;

        //键值为2，恢复成原始图像
        if (c == '2')
        {
            inpaintMask = Scalar::all(0);
            srcImage.copyTo(srcImage1);
            imshow(WINDOW_NAME1, srcImage1);
        }

        //键值为1或者空格，进行图像修补操作
        if (c == '1' || c == ' ')
        {
            Mat inpaintedImage;
            inpaint(srcImage1, inpaintMask, inpaintedImage, 3, INPAINT_TELEA);
            imshow(WINDOW_NAME2, inpaintedImage);
        }
    }
    return 0;
}
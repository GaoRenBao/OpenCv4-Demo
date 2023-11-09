/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：用滑动条做调色板
博客：http://www.bilibili996.com/Course?id=1395481000252
作者：高仁宝
时间：2023.11
*/

#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

#define DEMOID 0

// 绘制不同颜色的圆和矩形演示代码如下
#if DEMOID == 0
// 当鼠标按下时变为 True
bool drawing = false;
// 矩形和圆形模式切换
bool mode = false;
// 如果 mode 为 true 绘制矩形。按下'm' 变成绘制曲线。 mode=True
int ix = -1, iy = -1;

void on_MouseHandle(int event, int x, int y, int flags, void* param)
{
    int r = getTrackbarPos("R", "image");
    int g = getTrackbarPos("G", "image");
    int b = getTrackbarPos("B", "image");

    Mat& img = *(cv::Mat*)param;
    if (event == EVENT_LBUTTONDOWN)
    {
        drawing = true;
        ix = x;
        iy = y;
    }
    if (event == EVENT_MOUSEMOVE)
    {
        if (drawing)
        {
            if (mode)
            {
                rectangle(img, Point(ix, iy), Point(x, y), Scalar(b, g, r), -1);
            }
            else
            {
                // 绘制圆圈,小圆点连在一起就成了线,3 代表了笔画的粗细
                circle(img, Point(x, y), 3, Scalar(b, g, r), -1);

                //下面注释掉的代码是起始点为圆心,起点到终点为半径的
                //int r = (int)(Math.Sqrt((x - ix) * (x - ix) + (y - iy) * (y - iy)));
                //circle(img, new Point(x, y), r, Scalar(b, g, r), -1);
            }
        }
    }
    // 当鼠标松开停止绘画。
    if (event == EVENT_LBUTTONUP)
    {
        drawing = false;
    }
}

void main()
{
    Mat img(512, 512, CV_8UC3);
    namedWindow("image");
    createTrackbar("R", "image", NULL, 255);
    createTrackbar("G", "image", NULL, 255);
    createTrackbar("B", "image", NULL, 255);

    setMouseCallback("image", on_MouseHandle, (void*)&img);
    while (true)
    {
        imshow("image", img);
        if (waitKey(1) == 'm')
        {
            mode = !mode;
        }
    }
}
#endif

// 调节画布颜色
#if DEMOID == 1
void main()
{
    string swit = "0 : OFF \n1 : ON";
    Mat img(300, 512, CV_8UC3);
    namedWindow("image");
    createTrackbar("R", "image", NULL, 255);
    createTrackbar("G", "image", NULL, 255);
    createTrackbar("B", "image", NULL, 255);
    createTrackbar(swit, "image", NULL, 1);

    while (true)
    {
        int r = getTrackbarPos("R", "image");
        int g = getTrackbarPos("G", "image");
        int b = getTrackbarPos("B", "image");
        int s = getTrackbarPos(swit, "image");

        if (s == 0)
            img = Scalar::all(0);
        else
            img = Scalar(b, g, r);

        imshow("image", img);
        waitKey(1);
    }
}
#endif





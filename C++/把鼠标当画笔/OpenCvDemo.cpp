/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：把鼠标当画笔
博客：http://www.bilibili996.com/Course?id=3993445000251
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

#if DEMOID == 0
// 当鼠标按下时变为 True
bool drawing = false;
// 矩形和圆形模式切换
bool mode = false;
// 如果 mode 为 true 绘制矩形。按下'm' 变成绘制曲线。 mode=True
int ix = -1, iy = -1;

void on_MouseHandle(int event, int x, int y, int flags, void* param)
{
    Mat& image = *(cv::Mat*)param;
    switch (event)
    {
        // 当按下左键是返回起始位置坐标
    case EVENT_LBUTTONDOWN:
    {
        drawing = true;
        ix = x;
        iy = y;
    }
    break;

    // 当鼠标左键按下并移动是绘制图形。event 可以查看移动,flag 查看是否按下
    case EVENT_MOUSEMOVE:
    {
        if (drawing)
        {
            if (mode)
            {
                rectangle(image, Point(ix, iy), Point(x, y), Scalar(0, 255, 0), -1);
            }
            else
            {
                // 绘制圆圈,小圆点连在一起就成了线,3 代表了笔画的粗细
                circle(image, Point(x, y), 3, Scalar(0, 0, 255), -1);

                //下面注释掉的代码是起始点为圆心,起点到终点为半径的
                //int r = (int)(sqrt((x - ix) * (x - ix) + (y - iy) * (y - iy)));
                //circle(image, Point(x, y), r, Scalar(0, 0, 255), -1);
            }
        }
    }
    break;

    // 当鼠标松开停止绘画。
    case EVENT_LBUTTONUP:
    {
        drawing = false;
        /*if (mode)
        {
            rectangle(image, Point(ix, iy), Point(x, y), Scalar(0, 255, 0), -1);
        }
        else
        {
            circle(image, Point(x, y), 5, Scalar(0, 0, 255), -1);
        }*/
    }
    break;
    }
}

int main()
{
    Mat img = Mat(512, 512, CV_8UC3);
    img = Scalar::all(0);
    mode = false;

    namedWindow("image");
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

#if DEMOID == 1
// 只用做一件事:在双击过的地方绘制一个圆圈。
void on_MouseHandle(int event, int x, int y, int flags, void* param)
{
    Mat& image = *(cv::Mat*)param;
    if (event == EVENT_LBUTTONDBLCLK)
    {
        circle(image, Point(x, y), 100, Scalar(0, 0, 255), -1);
    }
}

void main()
{
    Mat img = Mat(512, 512, CV_8UC3);
    img = Scalar::all(0);
    namedWindow("image");
    setMouseCallback("image", on_MouseHandle, (void*)&img);
    while (true)
    {
        imshow("image", img);
        waitKey(1);
    }
}
#endif

#if DEMOID == 2
// 当鼠标按下时变为 True
bool drawing = false;
// 矩形和圆形模式切换
bool mode = false;
// 如果 mode 为 true 绘制矩形。按下'm' 变成绘制曲线。 mode=True
int ix = -1, iy = -1;

void on_MouseHandle(int event, int x, int y, int flags, void* param)
{
    Mat& image = *(cv::Mat*)param;
    if (event == EVENT_LBUTTONDOWN)
    {
        cout << to_string(x) << "," << to_string(y) << endl;
    }
    if (event == EVENT_RBUTTONDOWN)
    {
        Vec3b vec = image.at<Vec3b>(y, x);
        uchar blue = vec[0];
        uchar green = vec[1];
        uchar red = vec[2];
        string strRGB = to_string(red) + "," + to_string(green) + "," + to_string(blue);
        cout << strRGB << endl;
        putText(image, strRGB, Point(x, y), FONT_HERSHEY_SIMPLEX, 1, Scalar(255, 255, 255), 2, LINE_AA);
        imshow("original", image);
    }
}

void main()
{
    Mat img = imread("../images/messi5.jpg");
    imshow("original", img);

    namedWindow("original");
    setMouseCallback("original", on_MouseHandle, (void*)&img);
    waitKey(0);
}
#endif

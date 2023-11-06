/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：绘图函数
博客：http://www.bilibili996.com/Course?id=2699750000249
作者：高仁宝
时间：2023.11
*/

#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

// 用左键点击屏幕，打印坐标
void click_event(int event, int x, int y, int flags, void*)
{
    if (event == EVENT_LBUTTONDOWN)
        printf("%d,%d\r\n", x, y);
}
void demo1()
{
    namedWindow("Canvas", WINDOW_GUI_EXPANDED);
    setMouseCallback("Canvas", click_event);

    Mat canvas(300, 300, CV_8UC3);
    while (true)
    {
        for (size_t i = 0; i < 25; i++)
        {
            int radius = theRNG().uniform(5, 200);
            Scalar color = Scalar(theRNG().uniform(0, 256), theRNG().uniform(0, 256), theRNG().uniform(0, 256));
            Point pt = Point(theRNG().uniform(0, 300), theRNG().uniform(0, 300));
            circle(canvas, pt, radius, color, -1);
        }
        imshow("Canvas", canvas);
        if (waitKey(1000) == 'q')
            break;
    }
}

void demo2()
{
    /* freetype 操作，暂无 */
}

void demo3()
{
    int r1 = 70;
    int r2 = 30;
    int ang = 60;
    int d = 170;
    int h = int(d / 2 * sqrt(3));
    Point dot_red = Point(256, 128);
    Point dot_green = Point(int(dot_red.x - d / 2), dot_red.y + h);
    Point dot_blue = Point(int(dot_red.x + d / 2), dot_red.y + h);
    Scalar red = Scalar(0, 0, 255);
    Scalar green = Scalar(0, 255, 0);
    Scalar blue = Scalar(255, 0, 0);
    Scalar black = Scalar(0, 0, 0);

    int full = -1;
    Mat img(512, 512, CV_8UC3);
    img = Scalar::all(0);
    circle(img, dot_red, r1, red, full);
    circle(img, dot_green, r1, green, full);
    circle(img, dot_blue, r1, blue, full);
    circle(img, dot_red, r2, black, full);
    circle(img, dot_green, r2, black, full);
    circle(img, dot_blue, r2, black, full);

    ellipse(img, dot_red, Point(r1, r1), ang, 0, ang, black, full);
    ellipse(img, dot_green, Point(r1, r1), 360 - ang, 0, ang, black, full);
    ellipse(img, dot_blue, Point(r1, r1), 360 - 2 * ang, ang, 0, black, full);

    int font = FONT_HERSHEY_SIMPLEX;
    putText(img, "OpenCV", Point(15, 450), font, 4, Scalar(255, 255, 255), 10);

    imshow("opencv_logo.jpg", img);
    waitKey(0);
}

void demo4()
{
    Mat img(512, 512, CV_8UC3);
    img = Scalar::all(0);

    line(img, Point(0, 0), Point(511, 511), Scalar(255, 0, 0), 5);
    // polylines() 可以 用来画很多条线。只需要把想 画的线放在一 个列表中， 将 列表传给函数就可以了。
    // 每条线 会被独立绘制。 这会比用 cv2.line() 一条一条的绘制 要快一些。

    arrowedLine(img, Point(21, 13), Point(151, 401), Scalar(255, 0, 0), 5);
    rectangle(img, Point(384, 0), Point(510, 128), Scalar(0, 255, 0), 3);
    circle(img, Point(447, 63), 63, Scalar(0, 0, 255), -1);

    // 一个参数是中心点的位置坐标。 下一个参数是长轴和短轴的长度。椭圆沿逆时针方向旋转的角度。
    // 椭圆弧演顺时针方向起始的角度和结束角度 如果是 0 很 360 就是整个椭圆
    ellipse(img, Point(256, 256), Point(100, 50), 0, 0, 180, 255, -1);

    // 这里 reshape 的第一个参数为 - 1, 表明这一维的长度是根据后面的维度的计算出来的。
    // 注意 如果第三个参数是 False 我们得到的多边形是不闭合的 ，首 尾不相  连 。

    int font = FONT_HERSHEY_SIMPLEX;
    // 或使用 bottomLeftOrigin=True,文字会上下颠倒
    putText(img, "bottomLeftOrigin", Point(10, 400), font, 1, Scalar(255, 255, 255), 1, true);
    putText(img, "OpenCV", Point(10, 500), font, 4, Scalar(255, 255, 255), 2);

    namedWindow("example", 0);
    imshow("example", img);

    waitKey(0);
    destroyAllWindows();
}

void main()
{
    //demo1();
    //demo2();
    //demo3();
    demo4();
}
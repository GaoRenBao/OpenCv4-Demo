#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

void main()
{
    Mat img1 = imread("ml.png");
    int64 e1 = getTickCount();
    for (int i = 5; i < 49; i += 2)
        medianBlur(img1, img1, i);

    int64 e2 = getTickCount();
    double t = (e2 - e1) / getTickFrequency();  //  时钟频率 或者 每秒钟的时钟数
    printf("%f", t);

    //printf("%d\n", useOptimized());
    //setUseOptimized(false);
    //printf("%d\n", useOptimized());
}

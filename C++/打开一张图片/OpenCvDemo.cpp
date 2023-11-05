#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

int main()
{
    // 读取一张图片
    Mat srcImage = imread("../images/a.jpg");

    // 创建一个名字为image的窗口
    namedWindow("image");

    // 显示图片
    imshow("image", srcImage);

    // 等待任意按钮结束
    waitKey(0);
}
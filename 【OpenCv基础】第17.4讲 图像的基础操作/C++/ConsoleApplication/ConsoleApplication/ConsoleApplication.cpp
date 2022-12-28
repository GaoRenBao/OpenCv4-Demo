#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

// 使所有像素的红色通道值都为 0
void demo1()
{
    Mat img = imread("messi5.jpg");
    Mat mv[3];
    split(img, mv); // 比较耗时的操作
    merge(mv, 3, img);
    // 获取三通道颜色
    Mat b = mv[0];
    Mat g = mv[1];
    Mat r = mv[2];

    //使所有像素的红色通道值都为 0,你不必先拆分再赋值。
    mv[0] = Scalar::all(0);
    merge(mv, 3, img);
    imshow("DEMO1", img);
    waitKey(0);
}

// 输出图像的长宽以及通道数
void demo2()
{
    Mat img = imread("messi5.jpg");
    cout << "行/高:" << img.rows << endl;
    cout << "列/宽:" << img.cols << endl;
    cout << "通道:" << img.channels() << endl;
}

// 边界扩张与虚化
void demo3()
{
    Mat replicate;
    Mat reflect;
    Mat reflect101;
    Mat wrap;
    Mat constant;

    Mat img = imread("messi5.jpg");
    copyMakeBorder(img, replicate, 50, 50, 50, 50, BORDER_REPLICATE); 
    copyMakeBorder(img, reflect, 50, 50, 50, 50, BORDER_REFLECT);
    copyMakeBorder(img, reflect101, 50, 50, 50, 50, BORDER_REFLECT_101);
    copyMakeBorder(img, wrap, 50, 50, 50, 50, BORDER_WRAP);
    copyMakeBorder(img, constant, 50, 50, 50, 50, BORDER_CONSTANT, Scalar(255, 0, 0)); // 边界颜色
    imshow("replicate", replicate);
    imshow("reflect", reflect);
    imshow("reflect101", reflect101);
    imshow("wrap", wrap);
    imshow("constant", constant);
    waitKey(0);
}

int main()
{
    demo3();
    return 0;
}

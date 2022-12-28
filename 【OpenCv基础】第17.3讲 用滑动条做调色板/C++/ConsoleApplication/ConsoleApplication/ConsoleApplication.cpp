#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <iostream>

using namespace cv;
using namespace std;

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

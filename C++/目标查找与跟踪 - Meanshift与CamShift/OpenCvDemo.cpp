/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：目标查找与跟踪 - Meanshift与CamShift
博客：http://www.bilibili996.com/Course?id=0699ef6cd6e1407bbfb39a5e39b81e9a
作者：高仁宝
时间：2023.11
*/

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <video/tracking.hpp>
#include <iostream>

using namespace std;
using namespace cv;

int main()
{
    //保存目标轨迹  
    std::vector<Point> pt;

    VideoCapture capture("../images/Meanshift_CamShift.mp4");
    if (!capture.isOpened())
    {
        cout << "Open video failed!" << endl;
        return 0;
    }

    // take first frame of the video
    Mat frame;
    capture.read(frame);

    //setup initial location of window
    Rect track_window(65, 275, 105, 105); // simply hardcoded the values

    //set up the ROI for tracking
    Mat roi = frame(track_window);
    //imshow("frame", frame);
    //imshow("roi", roi);
    //waitKey(0);

    Mat hsv_roi;
    cvtColor(roi, hsv_roi, COLOR_BGR2HSV);

    // 将低亮度的值忽略掉
    Mat mask;
    inRange(hsv_roi, Scalar(0, 100, 0), Scalar(100, 255, 255), mask);

    MatND roi_hist;
    int channels[] = { 0 };
    int histSize[] = { 180 };
    float range[] = { 0, 180 };
    const float* ranges[] = { range };

    // 归一化
    calcHist(&hsv_roi, 1, channels, Mat(), roi_hist, 1, histSize, ranges);
    normalize(roi_hist, roi_hist, 0, 255, NORM_MINMAX);

    //Setup the termination criteria, either 10 iteration or move by atleast 1 pt
    TermCriteria term_crit(TermCriteria::EPS | TermCriteria::COUNT, 10, 1);

    Mat hsv;
    Mat dst;
    Mat img1;
    Mat img2;
    Rect track_window1 = track_window;
    Rect track_window2 = track_window;
    while (true)
    {
        capture.read(frame);
        if (frame.empty())
            break;
        frame.copyTo(img1);
        frame.copyTo(img2);

        cvtColor(frame, hsv, COLOR_BGR2HSV);

        float hrange[] = { 0, 180 };
        const float* hranges = hrange;
        std::vector<cv::Mat> hue;
        cv::split(hsv, hue);
        calcBackProject(&hue[1], 1, 0, roi_hist, dst, &hranges);
        //calcBackProject(vector<Mat>{hue[1]}, vector<int>{0}, roi_hist, dst, vector<float>{ 0, 180 }, 1);
        
        // meanShift 效果
        meanShift(dst, track_window1, term_crit);
        putText(img1, "MeanShift", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, LINE_AA);
        rectangle(img1, track_window1, 255, 2);
        imshow("img1", img1);

        // CamShift 效果
        cv::Point2f lines[4];
        RotatedRect ret = CamShift(dst, track_window2, term_crit);
        ret.points(lines);
        putText(img2, "CamShift", Point(10, 30), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2);
        line(img2, lines[0], lines[1], Scalar(0, 0, 255), 2, LINE_8);
        line(img2, lines[1], lines[2], Scalar(0, 0, 255), 2, LINE_8);
        line(img2, lines[2], lines[3], Scalar(0, 0, 255), 2, LINE_8);
        line(img2, lines[3], lines[0], Scalar(0, 0, 255), 2, LINE_8);

        // 绘制路径，原文链接：https://blog.csdn.net/akadiao/article/details/78991095
        Rect rect;
        rect.x = ret.center.x - ret.size.width / 2.0;
        rect.y = ret.center.y - ret.size.height / 2.0;
        rect.width = ret.size.width;
        rect.height = ret.size.height;
        pt.push_back(Point(rect.x + rect.width / 2, rect.y + rect.height / 2));
        for (int i = 0; i < pt.size() - 1; i++)
        {
            line(img2, pt[i], pt[i + 1], Scalar(0, 255, 0), 2);
        }
        imshow("img2", img2);

        // 按ESC退出
        if (waitKey(30) == 27)
            break;
    }
    return 0;
}

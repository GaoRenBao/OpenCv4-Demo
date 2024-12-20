﻿/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：人脸识别
博客：http://www.bilibili996.com/Course?id=4962812000072
作者：高仁宝
时间：2023.11
*/

#include "opencv2/objdetect/objdetect.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"

#include <iostream>
#include <stdio.h>

using namespace std;
using namespace cv;

void detectAndDisplay(Mat frame);

//--------------------------------【全局变量声明】----------------------------------------------
//		描述：声明全局变量
//-------------------------------------------------------------------------------------------------
//注意，需要把"haarcascade_frontalface_alt.xml"和"haarcascade_eye_tree_eyeglasses.xml"这两个文件复制到工程路径下
String face_cascade_name = "Files/haarcascade_frontalface_alt.xml";
String eyes_cascade_name = "Files/haarcascade_eye_tree_eyeglasses.xml";
CascadeClassifier face_cascade;
CascadeClassifier eyes_cascade;
string window_name = "Capture - Face detection";
RNG rng(12345);


//-----------------------------------【main( )函数】--------------------------------------------
//		描述：控制台应用程序的入口函数，我们的程序从这里开始
//-------------------------------------------------------------------------------------------------
int main(void)
{
    VideoCapture capture;
    Mat frame;

    //-- 1. 加载级联（cascades）
    if (!face_cascade.load(face_cascade_name)) { printf("--(!)Error loading\n"); return -1; };
    if (!eyes_cascade.load(eyes_cascade_name)) { printf("--(!)Error loading\n"); return -1; };

    //-- 2. 读取视频
    capture.open(0);
    if (capture.isOpened())
    {
        for (;;)
        {
            capture >> frame;

            //-- 3. 对当前帧使用分类器（Apply the classifier to the frame）
            if (!frame.empty())
            {
                detectAndDisplay(frame);
            }
            else
            {
                printf(" --(!) No captured frame -- Break!"); break;
            }

            int c = waitKey(10);
            if ((char)c == 'c') { break; }

        }
    }
    waitKey(10);
    return 0;
}


void detectAndDisplay(Mat frame)
{
    std::vector<Rect> faces;
    Mat frame_gray;

    cvtColor(frame, frame_gray, COLOR_BGR2GRAY);
    equalizeHist(frame_gray, frame_gray);

    //-- 人脸检测
    //此句代码的OpenCV2版为：
    //face_cascade.detectMultiScale(frame_gray, faces, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));
    //此句代码的OpenCV3版为：
    face_cascade.detectMultiScale(frame_gray, faces, 1.1, 2, 0 | CASCADE_SCALE_IMAGE, Size(30, 30));


    for (size_t i = 0; i < faces.size(); i++)
    {
        Point center(faces[i].x + faces[i].width / 2, faces[i].y + faces[i].height / 2);
        ellipse(frame, center, Size(faces[i].width / 2, faces[i].height / 2), 0, 0, 360, Scalar(255, 0, 255), 2, 8, 0);

        Mat faceROI = frame_gray(faces[i]);
        std::vector<Rect> eyes;

        //-- 在脸中检测眼睛
        //此句代码的OpenCV2版为：
        //eyes_cascade.detectMultiScale(faceROI, eyes, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));
        //此句代码的OpenCV3版为：
        eyes_cascade.detectMultiScale(faceROI, eyes, 1.1, 2, 0 | CASCADE_SCALE_IMAGE, Size(30, 30));

        for (size_t j = 0; j < eyes.size(); j++)
        {
            Point eye_center(faces[i].x + eyes[j].x + eyes[j].width / 2, faces[i].y + eyes[j].y + eyes[j].height / 2);
            int radius = cvRound((eyes[j].width + eyes[j].height) * 0.25);
            circle(frame, eye_center, radius, Scalar(255, 0, 0), 3, 8, 0);
        }
    }
    //-- 显示最终效果图
    imshow(window_name, frame);
}
/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：使用Haar分类器之行人检测
博客：http://www.bilibili996.com/Course?id=c6abbd334a724e8696c1165b7b6b558c
作者：高仁宝
时间：2023.11
*/

#include <opencv2/opencv.hpp>
#include <opencv2/ml.hpp>
#include <opencv2/cvconfig.h>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>    

#include <io.h>
#include <string>
#include <iostream>
#include <ctime>

using namespace cv;
using namespace std;

void demo1()
{
    // 获取图片目录
    std::string inPath = "..\\images\\haar\\*.jpg";
    intptr_t handle;
    struct _finddata_t fileinfo;
    handle = _findfirst(inPath.c_str(), &fileinfo);

    vector<string> list_images;
    do {
        string Name = fileinfo.name;
        list_images.push_back("..\\images\\haar\\" + Name);
    } while (!_findnext(handle, &fileinfo));

    //initialize the HOG descriptor/person detector
    HOGDescriptor hog;
    hog.setSVMDetector(HOGDescriptor::getDefaultPeopleDetector());
    for (int i = 0; i < list_images.size(); i++)
    {
        //load the image and resize it to (1) reduce detection time
       // and (2) improve detection accuracy
        Mat image = imread(list_images[i]);
   
        vector<Rect> rects;
        vector<double> foundWeights;
        hog.detectMultiScale(image, rects, foundWeights, 0, Size(4, 4), Size(8, 8), 1.05);

        Mat orig;
        image.copyTo(orig);
        for (int i = 0; i < rects.size(); i++)
        {
            // draw the original bounding boxes
            rectangle(orig,
                Point(rects[i].x, rects[i].y),
                Point(rects[i].x + rects[i].width, rects[i].y + rects[i].height),
                Scalar(0, 0, 255), 2);
        }
        imshow("orig", orig);

        vector<float> scores;
        for (int i = 0; i < foundWeights.size(); i++)
            scores.push_back(foundWeights[i]);

        vector<int> indices;
        vector<float> updated_scores;
        cv::dnn::softNMSBoxes(rects, scores, updated_scores, 0.65, 0.1, indices);
        vector<Rect> pick;
        for (int i = 0; i < indices.size(); i++)
        {
            cout << foundWeights[indices[i]] << endl;
            pick.push_back(rects[indices[i]]);
        }

        // C++ 中没有提供 nonMaximumSuppression 方法
        // 网上找的nonMaximumSuppression算法，没啥卵用~~
        // https://blog.csdn.net/JulyLi2019/article/details/133353948
        for (int i = 0; i < pick.size(); i++)
        {
            // draw the original bounding boxes
            rectangle(image,
                Point(pick[i].x, pick[i].y),
                Point(pick[i].x + pick[i].width, pick[i].y + pick[i].height),
                Scalar(0, 255, 0), 2);
        }
        imshow("image", image);
        if (waitKey(0) == 'q')
            break;
    }
}

void demo2()
{
    HOGDescriptor hog;
    hog.setSVMDetector(HOGDescriptor::getDefaultPeopleDetector());
    VideoCapture cap("../images/礼让斑马线！齐齐哈尔城市文明的伤！.mp4");
    double fps = cap.get(VideoCaptureProperties::CAP_PROP_FPS);  // 25.0
    cout << "Frames per second using video.get(cv2.CAP_PROP_FPS) :" << fps << endl;
    double num_frames = cap.get(VideoCaptureProperties::CAP_PROP_FRAME_COUNT);
    cout << "共有" << num_frames << "帧" << endl;

    double frame_height = cap.get(VideoCaptureProperties::CAP_PROP_FRAME_HEIGHT);
    double frame_width = cap.get(VideoCaptureProperties::CAP_PROP_FRAME_WIDTH);
    cout << "高：" << frame_height << " 宽："  << frame_width  << endl;

    // 跳过多少帧
    int skips = 20.0;
    Mat image;
    Mat orig;

    while (cap.isOpened())
    {
        cap >> image;  //读取当前帧
        double current = cap.get(VideoCaptureProperties::CAP_PROP_POS_FRAMES);
        if ((int)current % skips != 0.0)
            continue;

        vector<Rect> rects;
        vector<double> foundWeights;
        hog.detectMultiScale(image, rects, foundWeights, 0, Size(4, 4), Size(8, 8), 1.05);

        Mat orig;
        image.copyTo(orig);
        for (int i = 0; i < rects.size(); i++)
        {
            // draw the original bounding boxes
            rectangle(orig,
                Point(rects[i].x, rects[i].y),
                Point(rects[i].x + rects[i].width, rects[i].y + rects[i].height),
                Scalar(0, 0, 255), 2);
        }
        imshow("orig", orig);
        
        vector<float> scores;
        for (int i = 0; i < foundWeights.size(); i++)
            scores.push_back(foundWeights[i]);

        vector<int> indices;
        vector<float> updated_scores;
        cv::dnn::softNMSBoxes(rects, scores, updated_scores, 0.65, 0.1, indices);
        vector<Rect> pick;
        for (int i = 0; i < indices.size(); i++)
        {
            cout << foundWeights[indices[i]] << endl;
            pick.push_back(rects[indices[i]]);
        }

        for (int i = 0; i < pick.size(); i++)
        {
            // draw the original bounding boxes
            rectangle(image,
                Point(pick[i].x, pick[i].y),
                Point(pick[i].x + pick[i].width, pick[i].y + pick[i].height),
                Scalar(0, 255, 0), 2);
        }
        imshow("image", image);
        if (waitKey(10) == 'q')
            break;
    }
}

int main()
{
    // demo1();
    demo2(); // 好慢啊...
}
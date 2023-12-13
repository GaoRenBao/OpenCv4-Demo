/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：OpenCV中的稠密光流
博客：http://www.bilibili996.com/Course?id=6af89dda880846cca6da479b654d68d0
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
#include <fstream>
#include <cstdlib>

using namespace cv;
using namespace std;

/// <summary>
/// To run this example first download the hand model available here: http://posefs1.perception.cs.cmu.edu/OpenPose/models/hand/pose_iter_102000.caffemodel
/// Or also available here https://github.com/CMU-Perceptual-Computing-Lab/openpose/tree/master/models
/// Add the files to the bin folder
/// 代码来源：https://github.com/shimat/opencvsharp_samples/blob/master/SamplesCore/Samples/HandPose.cs
/// </summary>
void Demo1()
{
    const string model = "../images/dnn/github/pose_iter_102000.caffemodel";
    const string modelTxt = "../images/dnn/github/pose_deploy.prototxt";
    const string sampleImage = "../images/dnn/github/hand.jpg";
    const int nPoints = 22;
    const double thresh = 0.01;

    vector<vector<int>> posePairs = {
       {0, 1}, {1, 2},{2, 3},{3, 4}, //thumb
       {0, 5},{5, 6},{6, 7},{7, 8}, //index
       {0, 9},{9, 10},{10, 11},{11, 12}, //middle
       {0, 13},{13, 14},{14, 15},{15, 16}, //ring
       {0, 17},{17, 18},{18, 19},{19, 20}, //small
    };

    Mat frame = imread(sampleImage);

    Mat frameCopy;
    frame.copyTo(frameCopy);
    int frameWidth = frame.cols;
    int frameHeight = frame.rows;

    float aspectRatio = frameWidth / (float)frameHeight;
    int inHeight = 368;
    int inWidth = ((int)(aspectRatio * inHeight) * 8) / 8;

    cv::dnn::dnn4_v20211220::Net net = cv::dnn::readNetFromCaffe(modelTxt, model);
    Mat inpBlob = cv::dnn::blobFromImage(frame, 1.0 / 255, Size(inWidth, inHeight),
         Scalar(0, 0, 0), false, false);

    net.setInput(inpBlob);
    Mat output = net.forward();
    int H = output.size[2];
    int W = output.size[3];

    vector<Point> points;
    for (int n = 0; n < nPoints; n++)
    {
        // Probability map of corresponding body's part.
        Mat probMap = Mat(H, W, CV_32F, output.ptr(0, n));
        resize(probMap, probMap, Size(frameWidth, frameHeight));

        double minVal, maxVal;
        Point minLoc, maxLoc;
        minMaxLoc(probMap, &minVal, &maxVal, &minLoc, &maxLoc);

        if (maxVal > thresh)
        {
            circle(frameCopy, Point(maxLoc.x, maxLoc.y), 8, Scalar(0, 255, 255), -1, LineTypes::LINE_8);
            putText(frameCopy, to_string(n), Point(maxLoc.x, maxLoc.y), FONT_HERSHEY_COMPLEX, 1, Scalar(0, 0, 255), 2, LINE_AA);
        }
        points.push_back(maxLoc);
    }

    int nPairs = 20; //(POSE_PAIRS).Length / POSE_PAIRS[0].Length;

    for (int n = 0; n < nPairs; n++)
    {
        // lookup 2 connected body/hand parts
        Point partA = points[posePairs[n][0]];
        Point partB = points[posePairs[n][1]];

        if (partA.x <= 0 || partA.y <= 0 || partB.x <= 0 || partB.y <= 0)
            continue;

        line(frame, partA, partB, Scalar(0, 255, 255), 8);
        circle(frame, Point(partA.x, partA.y), 8, Scalar(0, 0, 255), -1);
        circle(frame, Point(partB.x, partB.y), 8, Scalar(0, 0, 255), -1);
    }

    imshow("frame", frame);
    //imwrite("frame.jpg", frame);
    waitKey();
}

/// <summary>
/// To run this example first download the face model available here: https://github.com/spmallick/learnopencv/tree/master/FaceDetectionComparison/models
/// Add the files to the bin folder.
/// You should also prepare the input images (faces.jpg) yourself.
/// 代码来源：https://github.com/shimat/opencvsharp_samples/blob/master/SamplesCore/Samples/FaceDetectionDNN.cs
/// </summary>
void Demo2()
{
    const string configFile = "../images/dnn/github/deploy.prototxt";
    const string faceModel = "../images/dnn/github/res10_300x300_ssd_iter_140000_fp16.caffemodel";
    const string image = "../images/airline-stewardess-bikini.jpg"; //"faces.jpg"; //找不到faces.jpg图片

    // Read sample image
    Mat frame = imread(image);
    int frameHeight = frame.rows;
    int frameWidth = frame.cols;
    cv::dnn::dnn4_v20211220::Net faceNet = cv::dnn::readNetFromCaffe(configFile, faceModel);
    Mat blob = cv::dnn::blobFromImage(frame, 1.0, Size(300, 300), Scalar(104, 117, 123), false, false);
    faceNet.setInput(blob, "data");

    Mat detection = faceNet.forward("detection_out");
    Mat detectionMat = Mat(detection.size[2], detection.size[3], CV_32F, detection.ptr(0));
    for (int i = 0; i < detectionMat.rows; i++)
    {
        float confidence = detectionMat.at<float>(i, 2);

        if (confidence > 0.7)
        {
            int x1 = (int)(detectionMat.at<float>(i, 3) * frameWidth);
            int y1 = (int)(detectionMat.at<float>(i, 4) * frameHeight);
            int x2 = (int)(detectionMat.at<float>(i, 5) * frameWidth);
            int y2 = (int)(detectionMat.at<float>(i, 6) * frameHeight);
            rectangle(frame, Point(x1, y1), Point(x2, y2), Scalar(0, 255, 0), 2, LineTypes::LINE_4);
        }
    }

    imshow("frame", frame);
    //imwrite("frame.jpg", frame);
    waitKey();
}

std::string& trim(std::string& s)
{
    if (s.empty())
    {
        return s;
    }
    s.erase(0, s.find_first_not_of(" "));
    s.erase(s.find_last_not_of(" ") + 1);
    return s;
}

/// <summary>
/// Find best class for the blob (i. e. class with maximal probability)
/// </summary>
/// <param name="probBlob"></param>
/// <param name="classId"></param>
/// <param name="classProb"></param>
void GetMaxClass(Mat probBlob, int* classId, double* classProb)
{
    // reshape the blob to 1x1000 matrix
    Mat probMat = probBlob.reshape(1, 1);
    double minVal, maxVal;
    Point minLoc, maxLoc;
    minMaxLoc(probMat, &minVal, &maxVal, &minLoc, &maxLoc);
    *classProb = maxVal;
    *classId = maxLoc.x;
}

/// <summary>
/// 参考来源：https://github.com/shimat/opencvsharp_samples/blob/master/SamplesCore/Samples/CaffeSample.cs
/// </summary>
void Demo3()
{
    //load the input image from disk
    Mat image = imread("../images/dnn/traffic_light.png");

    vector<string> rows;
    std::ifstream file("../images/dnn/synset_words.txt"); // 打开文件
    if (file.is_open()) {
        std::string line;
        while (getline(file, line)) { // 逐行读取文本
            rows.push_back(trim(line));
        }
        file.close(); // 关闭文件
    }
    else {
        std::cout << "无法打开文件" << std::endl;
        return;
    }

    vector<string> classes;
    for (int i = 0; i < rows.size(); i++)
    {
        string s = rows[i];
        s.erase(0, s.find_first_of(" ") + 1);
        size_t index = s.find_first_of(",");
        if (index != std::string::npos){
            s.erase(index);
        }
        classes.push_back(s);
    }

    // our CNN requires fixed spatial dimensions for our input image(s)
    // so we need to ensure it is resized to 224x224 pixels while
    // performing mean subtraction (104, 117, 123) to normalize the input;
    // after executing this command our "blob" now has the shape:
    // (1, 3, 224, 224)
    Mat blob = cv::dnn::blobFromImage(image, 1, Size(224, 224), Scalar(104, 117, 123));
    // load our serialized model from disk
    cout << "[INFO] loading model..." << endl;

    string prototxt = "../images/dnn/bvlc_googlenet.prototxt";
    string caffeModel = "../images/dnn/bvlc_googlenet.caffemodel";
    cv::dnn::dnn4_v20211220::Net net = cv::dnn::readNetFromCaffe(prototxt, caffeModel);

    // set the blob as input to the network and perform a forward-pass to
    // obtain our output classification

    net.setInput(blob);

    int64 start = getTickCount();
    Mat preds = net.forward();
    int64 end = getTickCount();
    double t = (end - start) / getTickFrequency();  //  时钟频率 或者 每秒钟的时钟数
    printf("[INFO] classification took {%f} seconds", t);

    // 方法1
    int classId;
    double classProb;
    GetMaxClass(preds, &classId, &classProb);

    stringstream ss;
    ss << fixed << setprecision(2) << (classProb * 100);
    string text = to_string(classId) + "," + classes[classId] + "," + ss.str() + "%";
    putText(image, text, Point(5, 25), HersheyFonts::FONT_HERSHEY_SIMPLEX, 0.7, Scalar(0, 0, 255), 2);

    // 方法2：
    //for (int idx = 0; idx < preds.cols; idx++)
    //{
    //    float confidence = preds.at<float>(0, idx);
    //    if (confidence > 0.7)
    //    {
    //        stringstream ss;
    //        ss << fixed << setprecision(2) << (confidence * 100);
    //        string text = to_string(idx) + "," + classes[idx] + "," + ss.str() + "%";
    //        putText(image, text, Point(5, 25), HersheyFonts::FONT_HERSHEY_SIMPLEX, 0.7, Scalar(0, 0, 255), 2);
    //        break;
    //    }
    //}

    // github上的代码有目标矩形标注，我这里没测试成功。。。。

    imshow("image", image);
    waitKey();
}

int main()
{
    //Demo1();
    //Demo2();
    Demo3();
}
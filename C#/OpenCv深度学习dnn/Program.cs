/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：OpenCv深度学习dnn
博客：http://www.bilibili996.com/Course?id=a3c2583540704a5fb8f5262ab7235a4f
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace demo
{
    internal class Program
    {
        /// <summary>
        /// To run this example first download the hand model available here: http://posefs1.perception.cs.cmu.edu/OpenPose/models/hand/pose_iter_102000.caffemodel
        /// Or also available here https://github.com/CMU-Perceptual-Computing-Lab/openpose/tree/master/models
        /// Add the files to the bin folder
        /// 代码来源：https://github.com/shimat/opencvsharp_samples/blob/master/SamplesCore/Samples/HandPose.cs
        /// </summary>
        static void Demo1()
        {
            const string model = "../../../images/dnn/github/pose_iter_102000.caffemodel";
            const string modelTxt = "../../../images/dnn/github/pose_deploy.prototxt";
            const string sampleImage = "../../../images/dnn/github/hand.jpg";
            const int nPoints = 22;
            const double thresh = 0.01;

            int[][] posePairs =
            {
                new[] {0, 1}, new[] {1, 2}, new[] {2, 3}, new[] {3, 4}, //thumb
                new[] {0, 5}, new[] {5, 6}, new[] {6, 7}, new[] {7, 8}, //index
                new[] {0, 9}, new[] {9, 10}, new[] {10, 11}, new[] {11, 12}, //middle
                new[] {0, 13}, new[] {13, 14}, new[] {14, 15}, new[] {15, 16}, //ring
                new[] {0, 17}, new[] {17, 18}, new[] {18, 19}, new[] {19, 20}, //small
            };

            var frame = Cv2.ImRead(sampleImage);
            var frameCopy = frame.Clone();
            int frameWidth = frame.Cols;
            int frameHeight = frame.Rows;

            float aspectRatio = frameWidth / (float)frameHeight;
            int inHeight = 368;
            int inWidth = ((int)(aspectRatio * inHeight) * 8) / 8;

            var net = CvDnn.ReadNetFromCaffe(modelTxt, model);
            var inpBlob = CvDnn.BlobFromImage(frame, 1.0 / 255, new Size(inWidth, inHeight),
                new Scalar(0, 0, 0), false, false);

            net.SetInput(inpBlob);

            var output = net.Forward();
            int H = output.Size(2);
            int W = output.Size(3);

            var points = new List<Point>();

            for (int n = 0; n < nPoints; n++)
            {
                // Probability map of corresponding body's part.
                var probMap = new Mat(H, W, MatType.CV_32F, output.Ptr(0, n));
                Cv2.Resize(probMap, probMap, new Size(frameWidth, frameHeight));
                Cv2.MinMaxLoc(probMap, out _, out var maxVal, out _, out var maxLoc);

                if (maxVal > thresh)
                {
                    Cv2.Circle(frameCopy, maxLoc.X, maxLoc.Y, 8, new Scalar(0, 255, 255), -1,
                        LineTypes.Link8);
                    Cv2.PutText(frameCopy, Cv2.Format(n), new OpenCvSharp.Point(maxLoc.X, maxLoc.Y),
                        HersheyFonts.HersheyComplex, 1, new Scalar(0, 0, 255), 2, LineTypes.AntiAlias);
                }

                points.Add(maxLoc);
            }

            int nPairs = 20; //(POSE_PAIRS).Length / POSE_PAIRS[0].Length;

            for (int n = 0; n < nPairs; n++)
            {
                // lookup 2 connected body/hand parts
                Point partA = points[posePairs[n][0]];
                Point partB = points[posePairs[n][1]];

                if (partA.X <= 0 || partA.Y <= 0 || partB.X <= 0 || partB.Y <= 0)
                    continue;

                Cv2.Line(frame, partA, partB, new Scalar(0, 255, 255), 8);
                Cv2.Circle(frame, partA.X, partA.Y, 8, new Scalar(0, 0, 255), -1);
                Cv2.Circle(frame, partB.X, partB.Y, 8, new Scalar(0, 0, 255), -1);
            }

            Cv2.ImShow("frame", frame);
            Cv2.ImWrite("frame.jpg", frame);
            Cv2.WaitKey();
        }

        /// <summary>
        /// To run this example first download the face model available here: https://github.com/spmallick/learnopencv/tree/master/FaceDetectionComparison/models
        /// Add the files to the bin folder.
        /// You should also prepare the input images (faces.jpg) yourself.
        /// 代码来源：https://github.com/shimat/opencvsharp_samples/blob/master/SamplesCore/Samples/FaceDetectionDNN.cs
        /// </summary>
        static void Demo2()
        {
            const string configFile = "../../../images/dnn/github/deploy.prototxt";
            const string faceModel = "../../../images/dnn/github/res10_300x300_ssd_iter_140000_fp16.caffemodel";
            const string image = "../../../images/airline-stewardess-bikini.jpg"; //"faces.jpg"; //找不到faces.jpg图片

            // Read sample image
            var frame = Cv2.ImRead(image);
            int frameHeight = frame.Rows;
            int frameWidth = frame.Cols;
            var faceNet = CvDnn.ReadNetFromCaffe(configFile, faceModel);
            var blob = CvDnn.BlobFromImage(frame, 1.0, new Size(300, 300), new Scalar(104, 117, 123), false, false);
            faceNet.SetInput(blob, "data");

            var detection = faceNet.Forward("detection_out");
            var detectionMat = new Mat(detection.Size(2), detection.Size(3), MatType.CV_32F,
                detection.Ptr(0));
            for (int i = 0; i < detectionMat.Rows; i++)
            {
                float confidence = detectionMat.At<float>(i, 2);

                if (confidence > 0.7)
                {
                    int x1 = (int)(detectionMat.At<float>(i, 3) * frameWidth);
                    int y1 = (int)(detectionMat.At<float>(i, 4) * frameHeight);
                    int x2 = (int)(detectionMat.At<float>(i, 5) * frameWidth);
                    int y2 = (int)(detectionMat.At<float>(i, 6) * frameHeight);

                    Cv2.Rectangle(frame, new Point(x1, y1), new Point(x2, y2), new Scalar(0, 255, 0), 2, LineTypes.Link4);
                }
            }

            Cv2.ImShow("frame", frame);
            Cv2.ImWrite("frame.jpg", frame);
            Cv2.WaitKey();
        }

        /// <summary>
        /// 参考来源：https://github.com/shimat/opencvsharp_samples/blob/master/SamplesCore/Samples/CaffeSample.cs
        /// </summary>
        static void Demo3()
        {
            //load the input image from disk
            Mat image = Cv2.ImRead("../../../images/dnn/traffic_light.png");

            List<string> rows = File.ReadAllText("../../../images/dnn/synset_words.txt").Trim().Split('\n').ToList();
            List<string> classes = new List<string>();
            foreach (var item in rows)
            {
                classes.Add(item.Substring(item.IndexOf(" ") + 1).Split(',')[0]);
            }

            // our CNN requires fixed spatial dimensions for our input image(s)
            // so we need to ensure it is resized to 224x224 pixels while
            // performing mean subtraction (104, 117, 123) to normalize the input;
            // after executing this command our "blob" now has the shape:
            // (1, 3, 224, 224)
            Mat blob = CvDnn.BlobFromImage(image, 1, new Size(224, 224), new Scalar(104, 117, 123));

            // load our serialized model from disk
            Console.WriteLine("[INFO] loading model...");

            string prototxt = "../../../images/dnn/bvlc_googlenet.prototxt";
            string caffeModel = "../../../images/dnn/bvlc_googlenet.caffemodel";
            Net net = CvDnn.ReadNetFromCaffe(prototxt, caffeModel);

            // set the blob as input to the network and perform a forward-pass to
            // obtain our output classification
            net.SetInput(blob);
            var start = Cv2.GetTickCount();
            Mat preds = net.Forward();
            var end = Cv2.GetTickCount();
            var t = (end - start) / Cv2.GetTickFrequency();  //  时钟频率 或者 每秒钟的时钟数
            Console.WriteLine($"[INFO] classification took {t} seconds");

            // 方法1
            GetMaxClass(preds, out int classId, out double classProb);
            string text = $"{classId},{classes[classId]},{Math.Round(classProb * 100, 2)}%";
            Cv2.PutText(image, text, new Point(5, 25), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);

            // 方法2：
            //for (int idx = 0; idx < preds.Cols; idx++)
            //{
            //    float confidence = preds.At<float>(0, idx);
            //    if (confidence > 0.7)
            //    {
            //        Cv2.PutText(image, $"{idx},{classes[idx]},{Math.Round(confidence * 100, 2)}%", 
            //            new Point(5, 25), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);
            //        break;
            //    }
            //}

            // github上的代码有目标矩形标注，我这里没测试成功。。。。

            Cv2.ImShow("image", image);
            Cv2.WaitKey();
        }

        /// <summary>
        /// Find best class for the blob (i. e. class with maximal probability)
        /// </summary>
        /// <param name="probBlob"></param>
        /// <param name="classId"></param>
        /// <param name="classProb"></param>
        private static void GetMaxClass(Mat probBlob, out int classId, out double classProb)
        {
            // reshape the blob to 1x1000 matrix
            var probMat = probBlob.Reshape(1, 1);
            Cv2.MinMaxLoc(probMat, out _, out classProb, out _, out var classNumber);
            classId = classNumber.X;
        }

        static void Main(string[] args)
        {
            //Demo1();
            Demo2();
            //Demo3();
        }
    }
}

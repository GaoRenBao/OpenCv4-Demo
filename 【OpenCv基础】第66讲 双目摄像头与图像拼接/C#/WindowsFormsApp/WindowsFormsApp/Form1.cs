using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static OpenCvSharp.Stitcher;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 视频操作
        /// </summary>
        public VideoCapture Cap = new VideoCapture();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 调用摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 打开ID为0的摄像头
            Cap.Open(1);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                MessageBox.Show("摄像头打开失败.");
                return;
            }

            Cap.Set(VideoCaptureProperties.FrameWidth, 3040);
            Cap.Set(VideoCaptureProperties.FrameHeight, 1080);

            Mat Image = new Mat();
            List<Mat> images;
            Mat panoMat = new Mat();
            Stitcher stitcher;

            while (true)
            {
                if (Cap.Read(Image))
                {
                    Cv2.Resize(Image, Image, new Size(Image.Cols * 0.5, Image.Rows * 0.5), 0, 0, InterpolationFlags.Area);

                    // 提取轮廓
                    Rect faces1 = new Rect()
                    {
                        X = 0,
                        Y = 0,
                        Width = (int)(Image.Width * 0.5),
                        Height = Image.Height
                    };
                    Rect faces2 = new Rect()
                    {
                        X = (int)(Image.Width * 0.5),
                        Y = 0,
                        Width = (int)(Image.Width * 0.5),
                        Height = Image.Height
                    };
                    Mat img_1 = new Mat(Image, faces1);
                    Mat img_2 = new Mat(Image, faces2);
                    Cv2.ImShow("img_1", img_1);
                    Cv2.ImShow("img_2", img_2);

              
                    images = new List<Mat>() { img_2, img_1 };
                    stitcher = Stitcher.Create(Mode.Scans);
                    if (stitcher.Stitch(images, panoMat) != Stitcher.Status.OK)
                    {
                        Cv2.WaitKey(1);
                        continue;
                    }

                    Cv2.ImWrite("1.jpg", img_1);
                    Cv2.ImWrite("2.jpg", img_2);
                    Cv2.ImWrite("拼接结果.jpg", panoMat);
                    Cv2.ImShow("拼接结果", panoMat);
                    return;
                }
            }         
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Mat img_1 = new Mat("1.jpg");
            Mat img_2 = new Mat("2.jpg");
            Cv2.ImShow("img_1", img_1);
            Cv2.ImShow("img_2", img_2);

            Mat panoMat = new Mat();
            List<Mat> images = new List<Mat>() { img_2, img_1 };
            Stitcher stitcher = Stitcher.Create(Mode.Scans);
            if (stitcher.Stitch(images, panoMat) != Stitcher.Status.OK)
            {
                MessageBox.Show("拼接失败！");
                return;
            }
            Cv2.ImShow("拼接结果", panoMat);
        }
    }
}
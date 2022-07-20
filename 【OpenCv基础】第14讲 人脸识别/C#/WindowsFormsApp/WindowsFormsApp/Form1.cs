using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public CascadeClassifier face_cascade = new CascadeClassifier();
        public CascadeClassifier eyes_cascade = new CascadeClassifier();

        public Form1()
        {
            InitializeComponent();
        }

        // 打开摄像头
        private void button1_Click(object sender, EventArgs e)
        {
            // 加载xml文件
            if (!face_cascade.Load(@"Files\haarcascade_frontalface_alt.xml"))
            {
                MessageBox.Show("load haarcascade_frontalface_alt.xml Error.");
                return;
            }
            if (!eyes_cascade.Load(@"Files\haarcascade_eye_tree_eyeglasses.xml"))
            {
                MessageBox.Show("load haarcascade_eye_tree_eyeglasses.xml Error.");
                return;
            }

            VideoCapture Cap = new VideoCapture();
            // 打开ID为0的摄像头
            Cap.Open(0);
            // 判断摄像头是否成功打开
            if (!Cap.IsOpened())
            {
                MessageBox.Show("摄像头打开失败.");
                return;
            }
            // 设置采集的图像尺寸为：640*480
            Cap.Set(CaptureProperty.FrameWidth, 640);
            Cap.Set(CaptureProperty.FrameHeight, 480);
            Cap.Set(CaptureProperty.Exposure, -3); // 曝光

            Mat frame = new Mat();
            var window = new Window("frame");
            while (true)
            {
                if (Cap.Read(frame))
                {
                    // 人脸识别
                    frame = detectAndDisplay(frame);

                    // 在Window窗口中播放视频(方法1)
                    window.ShowImage(frame);

                    // 在Window窗口中播放视频(方法2)
                    //Cv2.ImShow("frame", frame);

                    // 在pictureBox1中显示效果图
                    pictureBox1.Image = BitmapConverter.ToBitmap(frame);

                    if (Cv2.WaitKey(10) == 27)
                        break;
                }
            }
        }

        private Mat detectAndDisplay(Mat frame)
        {
            // 将原图像转换为灰度图像
            Mat frame_gray = new Mat();
            Cv2.CvtColor(frame, frame_gray, ColorConversionCodes.BGR2GRAY);
            // 直方图均衡化, 用于提高图像的质量
            Cv2.EqualizeHist(frame_gray, frame_gray);

            // 人脸检测
            Rect[] faces = face_cascade.DetectMultiScale(frame_gray, 1.1, 2, 0 | HaarDetectionType.ScaleImage, new Size(30, 30));
            for (int i = 0; i < faces.Length; i++)
            {
                // 绘制脸部区域
                Point center = new Point() { X = (faces[i].X + faces[i].Width / 2), Y = (faces[i].Y + faces[i].Width / 2) };
                Cv2.Ellipse(frame, center, new Size(faces[i].Width / 2, faces[i].Height / 2), 0, 0, 360, new Scalar(255, 0, 255), 2, LineTypes.Link8, 0);
                
                // 绘制眼睛区域
                Mat faceROI = new Mat(frame_gray, faces[i]);
                Cv2.ImShow("faceROI", faceROI);

                Rect[] eyes = eyes_cascade.DetectMultiScale(faceROI, 1.1, 2, 0 | HaarDetectionType.ScaleImage, new Size(30, 30));
                for (int j = 0; j < eyes.Length; j++)
                {
                    Point eye_center = new Point() { X = (faces[i].X + eyes[j].X + eyes[j].Width / 2), Y = (faces[i].Y + eyes[j].Y + eyes[j].Height / 2) };
                    int radius = (int)Math.Round((decimal)((eyes[j].Width + eyes[j].Height) * 0.25), 0, MidpointRounding.AwayFromZero);
                    Cv2.Circle(frame, eye_center.X, eye_center.Y, radius, new Scalar(0, 0, 255), 3, LineTypes.Link8, 0);
                }
            }
            return frame;
        }
    }
}
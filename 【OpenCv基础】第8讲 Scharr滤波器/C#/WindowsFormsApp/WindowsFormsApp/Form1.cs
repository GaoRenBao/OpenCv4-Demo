using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

// https://shimat.github.io/opencvsharp_docs/html/d69c29a1-7fb1-4f78-82e9-79be971c3d03.htm

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //【0】创建 grad_x 和 grad_y 矩阵
            Mat grad_x = new Mat();
            Mat grad_y = new Mat();
            Mat abs_grad_x = new Mat();
            Mat abs_grad_y = new Mat();
            Mat dst = new Mat();

            // 【1】载入原始图
            Mat src = Cv2.ImRead("1.jpg");

            // 【2】显示原图
            Cv2.ImShow("原图", src);

            // 【3】求 X方向梯度
            Cv2.Scharr(src, grad_x, MatType.CV_16S, 1, 0, 1, 0, BorderTypes.Default);
            Cv2.ConvertScaleAbs(grad_x, abs_grad_x);
            Cv2.ImShow("【效果图】 X方向Scharr", abs_grad_x);

            // 【4】求 Y方向梯度
            Cv2.Scharr(src, grad_y, MatType.CV_16S, 0, 1, 1, 0, BorderTypes.Default);
            Cv2.ConvertScaleAbs(grad_y, abs_grad_y);
            Cv2.ImShow("【效果图】 Y方向Scharr", abs_grad_y);

            // 【5】合并梯度(近似)
            Cv2.AddWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, dst);

            // 【6】显示效果图 
            Cv2.ImShow("【效果图】合并梯度后Scharr", dst);

            // 【7】在pictureBox1中显示效果图
            Bitmap map = BitmapConverter.ToBitmap(dst);
            pictureBox1.Image = map;
        }
    }
}

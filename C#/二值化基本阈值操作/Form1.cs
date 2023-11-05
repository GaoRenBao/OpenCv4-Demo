using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public int g_nThresholdValue = 100;
        public ThresholdTypes g_nThresholdType = ThresholdTypes.Binary;
        Mat g_srcImage;

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(ThresholdTypes)));
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 读入用户提供的图像
            g_srcImage = Cv2.ImRead("../../../images/lake.jpg");
            if (g_srcImage.Data == null)
            {
                MessageBox.Show("Oh，no，读取g_srcImage图片错误~！");
                return;
            }
            // 转灰度图
            Cv2.CvtColor(g_srcImage, g_srcImage, ColorConversionCodes.BGR2GRAY);

            Task t = new Task(() =>
            {
                ContrastAndBright();
            });
            t.Start();
        }

        // 模式
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_nThresholdType = (ThresholdTypes)Enum.Parse(typeof(ThresholdTypes), comboBox1.Text);
            label3.Text = ((int)g_nThresholdType).ToString();
        }

        // 参数值
        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            g_nThresholdValue = (int)hScrollBar2.Value;
            label4.Text = g_nThresholdValue.ToString();
        }

        private void ContrastAndBright()
        {
            Mat desImage = new Mat();
            while (true)
            {
                Cv2.Threshold(g_srcImage, desImage, g_nThresholdValue, 255, g_nThresholdType);
                // 输出图像到pictureBox控件
                pictureBox1.Image = BitmapConverter.ToBitmap(desImage);
                Cv2.WaitKey(30);
            }
        }
    }
}


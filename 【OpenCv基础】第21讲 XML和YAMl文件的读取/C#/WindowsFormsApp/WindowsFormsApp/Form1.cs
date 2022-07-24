using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void log(string a)
        {
            textBox1.Text += a + "\r\n";
        }
        private void log2(string a)
        {
            textBox1.Text += a;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // opencv3：FileStorage.Mode.Read
            // opencv4：FileStorage.Modes.Read
            using (var fs = new FileStorage("test.yaml", FileStorage.Modes.Read))
            {
                if (fs.IsOpened() == false)
                {
                    MessageBox.Show("文件打开失败.");
                    return;
                }

                using (var node = fs["frameCount"])
                {
                    log($"{node.Name},{node.Type},{node.ReadInt()}\r\n");
                }

                using (var node = fs["calibrationDate"])
                {
                    log($"{node.Name},{node.Type},{node.ReadString()}\r\n");
                }

                using (var node = fs["cameraMatrix"])
                {
                    log($"{node.Name}");
                    foreach (var a in node)
                    {
                        if (a.Type == FileNode.Types.Seq)
                        {
                            log2($"\t{a.Name}:[");
                            foreach (var b in a) { log2($"{b.ReadInt()},"); }
                            log($"]");
                        }
                        else
                        {
                            string str = ((a.Type == FileNode.Types.String) ? a.ReadString() : a.ReadInt().ToString());
                            log($"\t{a.Name},{a.Type},{str}");
                        }
                    }
                }

                using (var node = fs["distCoeffs"])
                {
                    log($"{node.Name}");
                    foreach (var a in node)
                    {
                        if (a.Type == FileNode.Types.Seq)
                        {
                            log2($"\t{a.Name}:[");
                            foreach (var b in a) { log2($"{b.ReadDouble()},"); }
                            log($"]");
                        }
                        else
                        {
                            string str = ((a.Type == FileNode.Types.String) ? a.ReadString() : a.ReadInt().ToString());
                            log($"\t{a.Name},{a.Type},{str}");
                        }
                    }
                }

                using (var node = fs["features"])
                {
                    log($"{node.Name}");
                    foreach (var a in node)
                    {
                        if (a.Type == FileNode.Types.Map)
                        {
                            foreach (var b in a)
                            {
                                if (b.Type == FileNode.Types.Seq)
                                {
                                    log2($"\t{b.Name}:[");
                                    foreach (var c in b) { log2($"{c.ReadDouble()},"); }
                                    log($"]");
                                }
                                else
                                {
                                    string str = ((b.Type == FileNode.Types.String) ? b.ReadString() : b.ReadInt().ToString());
                                    log($"\t{b.Name},{b.Type},{str}");
                                }
                            }
                        }
                        else
                        {
                            string str = ((a.Type == FileNode.Types.String) ? a.ReadString() : a.ReadInt().ToString());
                            log($"\t{a.Name},{a.Type},{str}");
                        }
                    }
                }
            }
        }
    }
}


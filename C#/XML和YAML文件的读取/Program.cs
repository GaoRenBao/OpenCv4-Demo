/*
OpenCv版本 OpenCvSharp 4.4.8.0.20230708
内容：XML和YAML文件的读取
博客：http://www.bilibili996.com/Course?id=5157334000101
作者：高仁宝
时间：2023.11
*/

using OpenCvSharp;
using System;

namespace demo
{
    internal class Program
    {
        private static void log(string a)
        {
            Console.WriteLine(a);
        }
        private static void log2(string a)
        {
            Console.Write(a);
        }

        static void Main(string[] args)
        {
            // opencv3：FileStorage.Mode.Read
            // opencv4：FileStorage.Modes.Read
            using (var fs = new FileStorage("test.yaml", FileStorage.Modes.Read))
            {
                if (fs.IsOpened() == false)
                {
                    Console.WriteLine("文件打开失败.");
                    Console.Read();
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

            Console.Read();
        }
    }
}

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public static Point2d Point2fToPoint2d(Point2f pf)
        {
            return new Point2d(((int)pf.X), ((int)pf.Y));
        }

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// SURF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 载入源图片并显示
            Mat srcImage1 = Cv2.ImRead("11.jpg");
            Mat srcImage2 = Cv2.ImRead("2.jpg");

            // 定义一个特征检测类对象
            KeyPoint[] keypoints_object, keypoints_scene;
            Mat descriptors_object = new Mat();
            Mat descriptors_scene = new Mat();
            var MySurf = OpenCvSharp.XFeatures2D.SURF.Create(400);

            // 方法1：计算描述符（特征向量），将Detect和Compute操作分开
            keypoints_object = MySurf.Detect(srcImage1);
            keypoints_scene = MySurf.Detect(srcImage2);
            MySurf.Compute(srcImage1, ref keypoints_object, descriptors_object);
            MySurf.Compute(srcImage2, ref keypoints_scene, descriptors_scene);

            // 方法2：计算描述符（特征向量），将Detect和Compute操作合并
            //MySurf.DetectAndCompute(srcImage1, null, out keypoints_object, descriptors_object);
            //MySurf.DetectAndCompute(srcImage2, null, out keypoints_scene, descriptors_scene);

            // 创建基于FLANN的描述符匹配对象
            FlannBasedMatcher matcher = new FlannBasedMatcher();
            DMatch[] matches = matcher.Match(descriptors_object, descriptors_scene);
            double max_dist = 0; double min_dist = 100;//最小距离和最大距离

            // 计算出关键点之间距离的最大值和最小值
            for (int i = 0; i < descriptors_object.Rows; i++)
            {
                double dist = matches[i].Distance;
                if (dist < min_dist) min_dist = dist;
                if (dist > max_dist) max_dist = dist;
            }

            System.Diagnostics.Debug.WriteLine($">Max dist 最大距离 : {max_dist}");
            System.Diagnostics.Debug.WriteLine($">Min dist 最小距离 : {min_dist}");

            // 存下匹配距离小于3*min_dist的点对
            List<DMatch> good_matches = new List<DMatch>();
            for (int i = 0; i < descriptors_object.Rows; i++)
            {
                if (matches[i].Distance < 3 * min_dist)
                {
                    good_matches.Add(matches[i]);
                }
            }

            // 绘制出匹配到的关键点
            Mat img_matches = new Mat();
            Cv2.DrawMatches(srcImage1, keypoints_object, srcImage2, keypoints_scene, good_matches, img_matches);

            //定义两个局部变量
            List<Point2f> obj = new List<Point2f>();
            List<Point2f> scene = new List<Point2f>();

            //从匹配成功的匹配对中获取关键点
            for (int i = 0; i < good_matches.Count; i++)
            {
                obj.Add(keypoints_object[good_matches[i].QueryIdx].Pt);
                scene.Add(keypoints_scene[good_matches[i].TrainIdx].Pt);
            }

            //计算透视变换 
            List<Point2d> objPts = obj.ConvertAll(Point2fToPoint2d);
            List<Point2d> scenePts = scene.ConvertAll(Point2fToPoint2d);
            Mat H = Cv2.FindHomography(objPts, scenePts, HomographyMethods.Ransac);

            //从待测图片中获取角点
            List<Point2f> obj_corners = new List<Point2f>();
            obj_corners.Add(new Point(0, 0));
            obj_corners.Add(new Point(srcImage1.Cols, 0));
            obj_corners.Add(new Point(srcImage1.Cols, srcImage1.Rows));
            obj_corners.Add(new Point(0, srcImage1.Rows));

            //进行透视变换
            Point2f[] scene_corners = Cv2.PerspectiveTransform(obj_corners, H);

            //绘制出角点之间的直线
            Point2f P0 = scene_corners[0] + new Point2f(srcImage1.Cols, 0);
            Point2f P1 = scene_corners[1] + new Point2f(srcImage1.Cols, 0);
            Point2f P2 = scene_corners[2] + new Point2f(srcImage1.Cols, 0);
            Point2f P3 = scene_corners[3] + new Point2f(srcImage1.Cols, 0);

            Cv2.Line(img_matches, new Point(P0.X, P0.Y), new Point(P1.X, P1.Y), new Scalar(255, 0, 123), 4);
            Cv2.Line(img_matches, new Point(P1.X, P1.Y), new Point(P2.X, P2.Y), new Scalar(255, 0, 123), 4);
            Cv2.Line(img_matches, new Point(P2.X, P2.Y), new Point(P3.X, P3.Y), new Scalar(255, 0, 123), 4);
            Cv2.Line(img_matches, new Point(P3.X, P3.Y), new Point(P0.X, P0.Y), new Scalar(255, 0, 123), 4);

            //显示最终结果
            Cv2.ImShow("效果图", img_matches);
        }

        /// <summary>
        /// SIFT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // 载入源图片并显示
            Mat srcImage1 = Cv2.ImRead("11.jpg");
            Mat srcImage2 = Cv2.ImRead("2.jpg");

            // 定义一个特征检测类对象
            KeyPoint[] keypoints_object, keypoints_scene;
            Mat descriptors_object = new Mat();
            Mat descriptors_scene = new Mat();
            var MySift = OpenCvSharp.Features2D.SIFT.Create(400);

            // 方法1：计算描述符（特征向量），将Detect和Compute操作分开
            keypoints_object = MySift.Detect(srcImage1);
            keypoints_scene = MySift.Detect(srcImage2);
            MySift.Compute(srcImage1, ref keypoints_object, descriptors_object);
            MySift.Compute(srcImage2, ref keypoints_scene, descriptors_scene);

            // 方法2：计算描述符（特征向量），将Detect和Compute操作合并
            //MySift.DetectAndCompute(srcImage1, null, out keypoints_object, descriptors_object);
            //MySift.DetectAndCompute(srcImage2, null, out keypoints_scene, descriptors_scene);

            // 创建基于FLANN的描述符匹配对象
            FlannBasedMatcher matcher = new FlannBasedMatcher();
            DMatch[] matches = matcher.Match(descriptors_object, descriptors_scene);
            double max_dist = 0; double min_dist = 100;//最小距离和最大距离

            // 计算出关键点之间距离的最大值和最小值
            for (int i = 0; i < descriptors_object.Rows; i++)
            {
                double dist = matches[i].Distance;
                if (dist < min_dist) min_dist = dist;
                if (dist > max_dist) max_dist = dist;
            }

            System.Diagnostics.Debug.WriteLine($">Max dist 最大距离 : {max_dist}");
            System.Diagnostics.Debug.WriteLine($">Min dist 最小距离 : {min_dist}");

            // 存下匹配距离小于3*min_dist的点对
            List<DMatch> good_matches = new List<DMatch>();
            for (int i = 0; i < descriptors_object.Rows; i++)
            {
                if (matches[i].Distance < 3 * min_dist)
                {
                    good_matches.Add(matches[i]);
                }
            }

            // 绘制出匹配到的关键点
            Mat img_matches = new Mat();
            Cv2.DrawMatches(srcImage1, keypoints_object, srcImage2, keypoints_scene, good_matches, img_matches);

            //定义两个局部变量
            List<Point2f> obj = new List<Point2f>();
            List<Point2f> scene = new List<Point2f>();

            //从匹配成功的匹配对中获取关键点
            for (int i = 0; i < good_matches.Count; i++)
            {
                obj.Add(keypoints_object[good_matches[i].QueryIdx].Pt);
                scene.Add(keypoints_scene[good_matches[i].TrainIdx].Pt);
            }

            //计算透视变换 
            List<Point2d> objPts = obj.ConvertAll(Point2fToPoint2d);
            List<Point2d> scenePts = scene.ConvertAll(Point2fToPoint2d);
            Mat H = Cv2.FindHomography(objPts, scenePts, HomographyMethods.Ransac);

            //从待测图片中获取角点
            List<Point2f> obj_corners = new List<Point2f>();
            obj_corners.Add(new Point(0, 0));
            obj_corners.Add(new Point(srcImage1.Cols, 0));
            obj_corners.Add(new Point(srcImage1.Cols, srcImage1.Rows));
            obj_corners.Add(new Point(0, srcImage1.Rows));

            //进行透视变换
            Point2f[] scene_corners = Cv2.PerspectiveTransform(obj_corners, H);

            //绘制出角点之间的直线
            Point2f P0 = scene_corners[0] + new Point2f(srcImage1.Cols, 0);
            Point2f P1 = scene_corners[1] + new Point2f(srcImage1.Cols, 0);
            Point2f P2 = scene_corners[2] + new Point2f(srcImage1.Cols, 0);
            Point2f P3 = scene_corners[3] + new Point2f(srcImage1.Cols, 0);

            Cv2.Line(img_matches, new Point(P0.X, P0.Y), new Point(P1.X, P1.Y), new Scalar(255, 0, 123), 4);
            Cv2.Line(img_matches, new Point(P1.X, P1.Y), new Point(P2.X, P2.Y), new Scalar(255, 0, 123), 4);
            Cv2.Line(img_matches, new Point(P2.X, P2.Y), new Point(P3.X, P3.Y), new Scalar(255, 0, 123), 4);
            Cv2.Line(img_matches, new Point(P3.X, P3.Y), new Point(P0.X, P0.Y), new Scalar(255, 0, 123), 4);

            //显示最终结果
            Cv2.ImShow("效果图", img_matches);
        }
    }
}
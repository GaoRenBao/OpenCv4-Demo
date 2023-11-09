# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：使用FLANN进行特征点匹配
# 博客：http://www.bilibili996.com/Course?id=1443670000237
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 【1】载入图像
img_1 = cv2.imread("../images/book2.jpg")
img_2 = cv2.imread("../images/book3.jpg")

# 定义一个特征检测类对象
sift = cv2.SIFT_create(300)

# 方法1：计算描述符（特征向量），将Detect和Compute操作分开
# keypoints_1 = sift.detect(img_1)
# keypoints_2 = sift.detect(img_2)
# (keypoints1, descriptors_1) = sift.compute(img_1, keypoints_1)
# (keypoints2, descriptors_2) = sift.compute(img_2, keypoints_2)

# 方法2：计算描述符（特征向量），将Detect和Compute操作合并
(keypoints_1, descriptors_1) = sift.detectAndCompute(img_1, None)
(keypoints_2, descriptors_2) = sift.detectAndCompute(img_2, None)

matcher = cv2.BFMatcher()  # 建立匹配关系
matches = matcher.match(descriptors_1, descriptors_2)  # 匹配描述子
matches = sorted(matches, key=lambda x: x.distance)  # 据距离来排序

max_dist = 0
min_dist = 100
for i in range(descriptors_1.shape[1]):
    dist = matches[i].distance
    if dist < min_dist:
        min_dist = dist;
    if dist > max_dist:
        max_dist = dist;

# 存下符合条件的匹配结果（即其距离小于2* min_dist的），使用radiusMatch同样可行
good_matches = []
for i in range(descriptors_1.shape[1]):
    if matches[i].distance < 2 * min_dist:
        good_matches.append(matches[i])

# 画出匹配关系
img_matches = cv2.drawMatches(img_1, keypoints_1, img_2, keypoints_2, good_matches, None)

cv2.imshow("img_matches", img_matches)
cv2.waitKey(0)
cv2.destroyAllWindows()
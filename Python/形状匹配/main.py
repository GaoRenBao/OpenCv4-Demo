# OpenCv版本：opencv-python 4.6.0.66
# 内容：形状匹配
# 博客：http://www.bilibili996.com/Course?id=0930272000264
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

img1 = cv2.imread('../images/star.jpg', 0)
img2 = cv2.imread('../images/star_b.jpg', 0)
# img2 = cv2.imread('../images/star_c.jpg', 0)

cv2.imshow('img1', img1)
cv2.imshow('img2', img2)

ret, thresh1 = cv2.threshold(img1, 127, 255, 0)
ret, thresh2 = cv2.threshold(img2, 127, 255, 0)

contours, hierarchy = cv2.findContours(thresh1, 2, 1)
cnt1 = contours[0]
print('contours len1:', len(contours))

contours, hierarchy = cv2.findContours(thresh2, 2, 1)
cnt2 = contours[0]
print('contours len2:', len(contours))

ret = cv2.matchShapes(cnt1, cnt2, 1, 0.0)
print(ret)
cv2.waitKey(0)

# Hu 矩是归一化中心矩的线性组合
# 之所以这样样做是为了能够获取到代表图像的某个特征的矩函数
# 这些矩函数对某些变化如缩放 旋转，镜像映射（ 除了 h1）具有不变形。

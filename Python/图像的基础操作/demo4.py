# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：图像的基础操作
# 博客：http://www.bilibili996.com/Course?id=5178073000253
# 作者：高仁宝
# 时间：2023.11

# -*- coding: utf-8 -*-
import cv2

img = cv2.imread('../images/messi5.jpg')

#
px = img[100, 100]
print(px)
blue = img[100, 100, 0]
print(blue)

#
img[100, 100] = [255, 255, 255]
print(img[100, 100])

# 获取像素值及修改的更好方法。
print(img.item(10, 10, 2))
img.itemset((10, 10, 2), 100)
print(img.item(10, 10, 2))
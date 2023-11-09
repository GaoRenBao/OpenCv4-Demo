# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：直方图对比
# 博客：http://www.bilibili996.com/Course?id=3750176000216
# 作者：高仁宝
# 时间：2023.11

import cv2

# 【1】载入基准图像(srcImage_base) 和两张测试图像srcImage_test1、srcImage_test2，并显示
srcImage_base = cv2.imread('../images/book1.jpg"')
srcImage_test1 = cv2.imread('../images/book2.jpg')
srcImage_test2 = cv2.imread('../images/book3.jpg')

# 【2】显示载入的3张图像
cv2.imshow('srcImage_base', srcImage_base)
cv2.imshow('srcImage_test1', srcImage_test1)
cv2.imshow('srcImage_test2', srcImage_test2)

# 【3】将图像由BGR色彩空间转换到 HSV色彩空间
hsvImage_base = cv2.cvtColor(srcImage_base, cv2.COLOR_BGR2HSV)
hsvImage_test1 = cv2.cvtColor(srcImage_test1, cv2.COLOR_BGR2HSV)
hsvImage_test2 = cv2.cvtColor(srcImage_test2, cv2.COLOR_BGR2HSV)

# 【4】创建包含基准图像下半部的半身图像(HSV格式)
x = 0
y = int(hsvImage_base.shape[0] / 2)
h = int(hsvImage_base.shape[0] / 2)
w = hsvImage_base.shape[1]
hsvImage_halfDown = hsvImage_base[y:y + h, x:x + w]

# 【5】初始化计算直方图需要的实参
histSize = [50, 60]
ranges = [0, 256, 0, 180]
# 使用第0和第1通道
channels = [0, 1]

# 【6】计算基准图像，两张测试图像，半身基准图像的HSV直方图:
baseHist = cv2.calcHist([hsvImage_base], channels, None, histSize, ranges)
cv2.normalize(baseHist, baseHist, 0, 1, cv2.NORM_MINMAX, -1);

halfDownHist = cv2.calcHist([hsvImage_halfDown], channels, None, histSize, ranges)
cv2.normalize(halfDownHist, halfDownHist, 0, 1, cv2.NORM_MINMAX, -1);

testHist1 = cv2.calcHist([hsvImage_test1], channels, None, histSize, ranges)
cv2.normalize(testHist1, testHist1, 0, 1, cv2.NORM_MINMAX, -1);

testHist2 = cv2.calcHist([hsvImage_test2], channels, None, histSize, ranges)
cv2.normalize(testHist2, testHist2, 0, 1, cv2.NORM_MINMAX, -1);

# 【7】按顺序使用4种对比标准将基准图像的直方图与其余各直方图进行对比:

for i in range(4):
    # 进行图像直方图的对比
    base_base = cv2.compareHist(baseHist, baseHist, i)
    base_half = cv2.compareHist(baseHist, halfDownHist, i)
    base_test1 = cv2.compareHist(baseHist, testHist1, i)
    base_test2 = cv2.compareHist(baseHist, testHist2, i)
    print("方法[", i, "]的匹配结果如下")
    print("【基准图 - 基准图】：", base_base)
    print("【基准图 - 半身图】：", base_half)
    print("【基准图 - 测试图1】：", base_test1)
    print("【基准图 - 测试图2】：", base_test2, "\n")

print("检测结束")
cv2.waitKey(0)

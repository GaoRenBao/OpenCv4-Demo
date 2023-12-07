# OpenCv版本：opencv-python 4.6.0.66
# 内容：二值化基本阈值操作
# 博客：http://www.bilibili996.com/Course?id=4215192000168
# 作者：高仁宝
# 时间：2023.11

import cv2

g_nThresholdValue = 100
g_nThresholdType = 3

g_srcImage = cv2.imread("../images/lake.jpg")
g_srcImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)


# 模式
def on_Threshold1(x):
    global g_srcImage, g_nThresholdValue, g_nThresholdType
    g_nThresholdType = x
    ret, g_maskImage = cv2.threshold(g_srcImage, g_nThresholdValue, 255, g_nThresholdType)
    cv2.imshow('image', g_maskImage)


# 修改亮度
def on_Threshold2(x):
    global g_srcImage, g_nThresholdValue, g_nThresholdType
    g_nThresholdValue = x
    ret, g_maskImage = cv2.threshold(g_srcImage, g_nThresholdValue, 255, g_nThresholdType)
    cv2.imshow('image', g_maskImage)


# 创建窗口
cv2.namedWindow('image')
cv2.createTrackbar('model', 'image', 0, 4, on_Threshold1)
cv2.createTrackbar('number', 'image', 0, 255, on_Threshold2)
cv2.setTrackbarPos('model', 'image', 3)
cv2.setTrackbarPos('number', 'image', 100)

cv2.waitKey(0)
cv2.destroyAllWindows()

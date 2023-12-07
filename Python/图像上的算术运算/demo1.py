# OpenCv版本：opencv-python 4.6.0.66
# 内容：图像上的算术运算
# 博客：http://www.bilibili996.com/Course?id=4683988000254
# 作者：高仁宝
# 时间：2023.11

import cv2

# img1=cv2.imread('../images/subtract1.jpg')
img1 = cv2.imread('../images/subtract1.jpg', 0)  # 灰度图
# img2=cv2.imread('../images/subtract2.jpg')
img2 = cv2.imread('../images/subtract2.jpg', 0)

cv2.imshow('subtract1', img1)
cv2.imshow('subtract2', img2)

#
st = img2 - img1
# st=img1-img2#相反
cv2.imshow('after subtract', st)

# 效果好一点
# ret,threshold=cv2.threshold(st,0, 127, cv2.THRESH_BINARY)
ret, threshold = cv2.threshold(st, 50, 255, cv2.THRESH_BINARY)
cv2.imshow('after threshold', threshold)

cv2.waitKey(0)

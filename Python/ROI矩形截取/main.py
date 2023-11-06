# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：ROI矩形截取
# 博客：http://www.bilibili996.com/Course?id=db851d3a71c7471ab0bb11ca4d19e650
# 作者：高仁宝
# 时间：2023.11

import cv2

img = cv2.imread('../images/messi5.jpg')

# 设置需要裁剪的轮廓
x = 100
y = 100
w = 200
h = 200
# 裁剪
roi = img[y:y + h, x:x + w].copy()

cv2.imshow('img', img)
cv2.imshow('roi.jpg', roi)
cv2.waitKey(0)
cv2.destroyAllWindows()
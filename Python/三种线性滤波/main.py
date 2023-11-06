# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：三种线性滤波
# 博客：http://www.bilibili996.com/Course?id=5359993000122
# 作者：高仁宝
# 时间：2023.11

import cv2

# 载入原图
image = cv2.imread('../images/girl3.jpg')

# 显示原图
cv2.imshow('image', image)

# 进行方框滤波操作 BoxFilter
out1 = cv2.boxFilter(image, -1, (5, 5))

# 进行均值滤波操作
out2 = cv2.blur(image, (7, 7))

# 进行高斯滤波操作
out3 = cv2.GaussianBlur(image, (5, 5), 0)

# 显示组合结果
cv2.imshow('boxFilter', out1)
cv2.imshow('blur', out2)
cv2.imshow('GaussianBlur', out3)

cv2.waitKey(0)
cv2.destroyAllWindows()

# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：霍夫变换HoughCircles边缘检测与线性矢量
# 博客：http://www.bilibili996.com/Course?id=4885001000177
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 【1】载入原始图和Mat变量定义
srcImage = cv2.imread('../images/HoughCircles.jpg')

# 【2】显示原始图
cv2.imshow("srcImage", srcImage)

# 【3】转化边缘检测后的图为灰度图
midImage = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)
# 进行高斯滤波操作
midImage = cv2.GaussianBlur(midImage, (9, 9), 2)

# 【3】进行霍夫线变换
circles = cv2.HoughCircles(midImage, cv2.HOUGH_GRADIENT, 1.5, 10,
                           param1=200, param2=100, minRadius=0, maxRadius=0)

circles = np.uint16(np.around(circles))
for i in circles[0, :]:
    cv2.circle(srcImage, (i[0], i[1]), 3, (0, 255, 0), -1)
    cv2.circle(srcImage, (i[0], i[1]), i[2], (155, 50, 255), 3)

# 【7】显示效果图
cv2.imshow("EndImage", srcImage)

cv2.waitKey(0)
cv2.destroyAllWindows()

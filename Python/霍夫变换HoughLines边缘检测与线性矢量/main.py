# OpenCv版本：opencv-python 4.6.0.66
# 内容：霍夫变换HoughLines边缘检测与线性矢量
# 博客：http://www.bilibili996.com/Course?id=3905534000171
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 【1】载入原始图和Mat变量定义
srcImage = cv2.imread('../images/home.jpg')

# 【2】进行边缘检测和转化为灰度图
midImage = cv2.Canny(srcImage, 50, 200, 3)  # 进行一此canny边缘检测

# 转化边缘检测后的图为灰度图
dstImage = cv2.cvtColor(midImage, cv2.COLOR_GRAY2BGR)

# 【3】进行霍夫线变换
lines = cv2.HoughLines(midImage, 1, np.pi / 180, 150)

# 【4】依次在图中绘制出每条线段
lines1 = lines[:, 0, :]  # 提取为为二维
for rho, theta in lines1[:]:
    a = np.cos(theta)
    b = np.sin(theta)
    x0 = a * rho
    y0 = b * rho
    x1 = int(x0 + 1000 * (-b))
    y1 = int(y0 + 1000 * a)
    x2 = int(x0 - 1000 * (-b))
    y2 = int(y0 - 1000 * a)
    cv2.line(dstImage, (x1, y1), (x2, y2), (55, 100, 195), 1)

# 【5】显示原始图
cv2.imshow("srcImage", srcImage)

# 【6】边缘检测后的图
cv2.imshow("midImage", midImage)

# 【7】显示效果图
cv2.imshow("dstImage", dstImage)

cv2.waitKey(0)
cv2.destroyAllWindows()

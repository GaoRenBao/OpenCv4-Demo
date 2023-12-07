# OpenCv版本：opencv-python 4.6.0.66
# 内容：椭圆拟合与直线拟合
# 博客：http://www.bilibili996.com/Course?id=0975485000265
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

rImg = cv2.imread('../images/lightning.png')
img = cv2.cvtColor(rImg, cv2.COLOR_BGR2GRAY)
ret, thresh = cv2.threshold(img, 127, 255, 0)
contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
print('contours len:', len(contours))
cnt = contours[0]

# 椭圆拟合
# 使用的函数为 cv2.ellipse()  返回值其实就是旋转边界矩形的内切圆
ellipse = cv2.fitEllipse(cnt)
angle = ellipse[2]
im = cv2.ellipse(rImg, ellipse, (0, 255, 0), 2)

# 直线拟合
# 我们可以根据一组点拟合出一条直线 同样我们也可以为图像中的白色点拟合出一条直线。
rows, cols = img.shape[:2]
[vx, vy, x, y] = cv2.fitLine(cnt, cv2.DIST_L2, 0, 0.01, 0.01)
lefty = int((-x * vy / vx) + y)
righty = int(((cols - x) * vy / vx) + y)
cv2.line(rImg, (cols - 1, righty), (0, lefty), (0, 0, 255), 2)

cv2.imshow('rImg', rImg)
cv2.imwrite('rImg.jpg', rImg)
cv2.waitKey(0)

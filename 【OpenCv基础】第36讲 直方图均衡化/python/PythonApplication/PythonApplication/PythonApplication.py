import cv2
import numpy as np

#【1】载入原始图
srcImage = cv2.imread("1.jpg")

# 【2】转为灰度图并显示出来
srcImage=cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)
cv2.imshow("srcImage", srcImage)

#【3】进行直方图均衡化
dstImage = cv2.equalizeHist(srcImage);

#【4】显示效果图
cv2.imshow('dstImage',dstImage)

cv2.waitKey(0)
cv2.destroyAllWindows()

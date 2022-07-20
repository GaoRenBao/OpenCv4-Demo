import cv2
import numpy as np

# 【1】载入原始图  
src=cv2.imread('1.jpg')

# 显示原始图 
cv2.imshow('src',src)

#【3】使用高斯滤波消除噪声
src = cv2.GaussianBlur(src,(3,3), 0)

#【4】转换为灰度图
src_gray = cv2.cvtColor(src,cv2.COLOR_BGR2GRAY)

#【5】使用Laplace函数
dst = cv2.Laplacian(src_gray, cv2.CV_16S, ksize = 3)

#【6】计算绝对值，并将结果转换成8位
abs_dst = cv2.convertScaleAbs(dst)

#【7】显示效果图
cv2.imshow("abs_dst", abs_dst);

cv2.waitKey(0);


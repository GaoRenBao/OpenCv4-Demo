# -*- coding: utf-8 -*-
import cv2
import numpy as np

'''
例如我们 检测一副图像中 眼睛的位置 我们 先应该在图像中找到脸 再在脸的区域中找眼睛 
而不是 直接在一幅图像中搜索。这样会提高程序的准确性和性能。
'''

img=cv2.imread('messi5.jpg')

ball=img[280:280+60,330:330+60] # 获取ROI区域
img[273:273+60,100:100+60]=ball # 将获取到的ROI区域替换到新的位置

# cv2.namedWindow("messi",0)
# cv2.imshow("messi",img)
cv2.imwrite("PythonApplication5.jpg",img)
cv2.waitKey(0)

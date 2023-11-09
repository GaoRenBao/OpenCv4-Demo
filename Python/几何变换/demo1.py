# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：几何变换
# 博客：http://www.bilibili996.com/Course?id=1154656000256
# 作者：高仁宝
# 时间：2023.11

"""
http://docs.opencv.org/3.2.0/da/d6e/tutorial_py_geometric_transformations.html
函数 cv2.warpAffine() 的第三个参数的是 出图像的大小 ，它的格式应是图像的(宽,高) 。
图像的宽对应的是列数, 高对应的是行数。
"""

import cv2
import numpy as np

# 移动了100,50 个像素。
img = cv2.imread('../images/messi5.jpg', 0)
rows, cols = img.shape

M = np.float32([[1, 0, 100], [0, 1, 50]])
dst = cv2.warpAffine(img, M, (cols, rows))

cv2.imshow('img', dst)
cv2.imwrite('img.jpg', dst)
cv2.waitKey(0)
cv2.destroyAllWindows()



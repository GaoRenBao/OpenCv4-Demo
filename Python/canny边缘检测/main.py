# OpenCv版本：opencv-python 4.6.0.66
# 内容：canny边缘检测
# 博客：http://www.bilibili996.com/Course?id=4371839000008
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np


def zh_ch(string):
    return string.encode("gbk").decode(errors="ignore")


# 载入原始图
srcImage = cv2.imread('../images/orange.jpg')
srcImage1 = srcImage.copy()

# 显示原始图
cv2.imshow(zh_ch('原始图'), srcImage)

# 【1】创建与src同类型和大小的矩阵(dst)
dstImage = np.zeros(srcImage.shape, np.uint8) * 0

# 【2】将原图像转换为灰度图像
grayImage = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)

# 【3】先用使用3x3内核来降噪（均值滤波）
edge = cv2.blur(grayImage, (3, 3))

# 【4】运行Canny算子
edge = cv2.Canny(edge, 3, 9, 3)

# Using the 'NOT' way
# dstImage=cv2.bitwise_not(dstImage,  srcImage1, edge)

# Using the 'copyto' way
edge = cv2.cvtColor(edge, cv2.COLOR_GRAY2BGR)
np.copyto(dstImage, srcImage1, where=np.array(edge, dtype='bool'))

# 【6】显示效果图
cv2.imshow('dstImage.png', dstImage)
cv2.waitKey(0)
cv2.destroyAllWindows()

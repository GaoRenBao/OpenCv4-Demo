# OpenCv版本：opencv-python 4.6.0.66
# 内容：remap重映射
# 博客：http://www.bilibili996.com/Course?id=4473896000179
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 【1】载入原始图
srcImage = cv2.imread("../images/car.jpg")
cv2.imshow("srcImage", srcImage)

# 【2】创建和原始图一样的效果图，x重映射图，y重映射图
sp = srcImage.shape[:2]
map_x = np.zeros((sp[0], sp[1]), np.float32)
map_y = np.zeros((sp[0], sp[1]), np.float32)

# 【3】双层循环，遍历每一个像素点，改变map_x & map_y的值
for y in range(0, int(sp[0] - 1)):
    for x in range(0, int(sp[1] - 1)):
        # 改变map_x & map_y的值.
        map_x[y, x] = x
        map_y[y, x] = sp[0] - y

# 【4】进行重映射操作
dstImage = cv2.remap(srcImage, map_x, map_y, cv2.INTER_LINEAR, cv2.BORDER_CONSTANT)

# 【5】显示效果图
cv2.imshow('dstImage', dstImage)

cv2.waitKey(0)
cv2.destroyAllWindows()

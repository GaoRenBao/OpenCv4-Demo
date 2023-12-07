# OpenCv版本：opencv-python 4.6.0.66
# 内容：remap实现多种重映射
# 博客：http://www.bilibili996.com/Course?id=5673205000181
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

# 【1】载入原始图
g_srcImage = cv2.imread("../images/Robot.jpg")
cv2.imshow("srcImage", g_srcImage)

# 【2】创建和原始图一样的效果图，x重映射图，y重映射图
sp = g_srcImage.shape[:2]
g_map_x = np.zeros((sp[0], sp[1]), np.float32)
g_map_y = np.zeros((sp[0], sp[1]), np.float32)


def update_map(key):
    # 双层循环，遍历每一个像素点，改变map_x & map_y的值
    for y in range(0, int(sp[0] - 1)):
        for x in range(0, int(sp[1] - 1)):
            if key == 49:
                if x > sp[1] * 0.25 and x < sp[1] * 0.75 and y > sp[0] * 0.25 and y < sp[0] * 0.75:
                    g_map_x[y, x] = 2 * (x - sp[1] * 0.25) + 0.5
                    g_map_y[y, x] = 2 * (y - sp[0] * 0.25) + 0.5
                else:
                    g_map_x[y, x] = 0
                    g_map_y[y, x] = 0

            if key == 50:
                g_map_x[y, x] = x
                g_map_y[y, x] = sp[0] - y

            if key == 51:
                g_map_x[y, x] = sp[1] - x
                g_map_y[y, x] = y

            if key == 52:
                g_map_x[y, x] = sp[1] - x
                g_map_y[y, x] = sp[0] - y


while True:
    # 获取键盘键值
    key = cv2.waitKey(0)

    # 根据按下的键盘按键来更新 map_x & map_y的值. 然后调用remap( )进行重映射
    update_map(key)

    # 进行重映射操作
    g_dstImage = cv2.remap(g_srcImage, g_map_x, g_map_y, cv2.INTER_LINEAR, cv2.BORDER_CONSTANT)

    # 显示效果图
    cv2.imshow('dstImage', g_dstImage)

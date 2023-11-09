# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：凸包检测
# 博客：http://www.bilibili996.com/Course?id=3741165000202
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import random


def zh_ch(string):
    return string.encode("gbk").decode(errors="ignore")


while (True):

    # 随机生成点的数量
    count = random.randint(30, 50)

    point = []
    for i in range(count):
        x = random.randint(600 / 4, 600 * 3 / 4)
        y = random.randint(600 / 4, 600 * 3 / 4)
        point.append([x, y])

    points = np.array(point, np.int32)  # 点值

    # 寻找图像凸包
    hull = cv2.convexHull(points, False)

    # 绘制出随机颜色的点
    image = np.zeros((600, 600, 3), np.uint8)
    for i in range(count):
        cv2.circle(image, (points[i][0], points[i][1]), 3,
                   (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)), -1)

    # 准备参数
    point0 = hull[len(hull) - 1]  # 连接凸包边的坐标点

    # 绘制轮廓和凸包
    for i in range(len(hull)):
        point = hull[i]
        image = cv2.line(image, (point0[0][0], point0[0][1]),
                         (point[0][0], point[0][1]),
                         (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)), 2)
        point0 = point

    cv2.imshow("image", image)
    cv2.waitKey(0)

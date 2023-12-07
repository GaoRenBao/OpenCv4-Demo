# OpenCv版本：opencv-python 4.6.0.66
# 内容：创建包围轮廓的圆形边
# 博客：http://www.bilibili996.com/Course?id=3929903000204
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import random

while True:

    # 随机生成点的数量
    count = random.randint(30, 50)

    point = []
    for i in range(count):
        x = random.randint(600 / 4, 600 * 3 / 4)
        y = random.randint(600 / 4, 600 * 3 / 4)
        point.append([x, y])

    points = np.array(point, np.int32)  # 点值

    # 绘制出随机颜色的点
    image = np.zeros((600, 600, 3), np.uint8)
    for i in range(count):
        cv2.circle(image, (points[i][0], points[i][1]), 3,
                   (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)), -1)

    # 对给定的 2D 点集，寻找最小面积的包围圆
    (x, y), radius = cv2.minEnclosingCircle(points)
    center = (int(x), int(y))

    cv2.circle(image, center, int(radius), (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)), 2)

    cv2.imshow("image", image)
    cv2.waitKey(0)

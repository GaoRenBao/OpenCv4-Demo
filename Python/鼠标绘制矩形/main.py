# OpenCv版本：opencv-python 4.6.0.66
# 内容：鼠标绘制矩形
# 博客：http://www.bilibili996.com/Course?id=5103658000095
# 作者：高仁宝
# 时间：2023.11

import numpy as np
import cv2
import random

g_rectangle = [0, 0, 0, 0]  # x,y,w,h
g_bDrawingBox = False
srcImage = np.zeros((600, 800, 3), np.uint8)
tempImage = np.zeros((600, 800, 3), np.uint8)


def DrawRectangle(img, box):
    cv2.rectangle(img, (box[0], box[1]), (box[0] + box[2], box[1] + box[3]),
                  (random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)), 1)
    return img


def GetRGBCvMouseCallback(event, x, y, flags, param):
    global g_rectangle, g_bDrawingBox, srcImage

    # 鼠标移动消息
    if event == cv2.EVENT_MOUSEMOVE:
        if g_bDrawingBox:
            g_rectangle[2] = x - g_rectangle[0]
            g_rectangle[3] = y - g_rectangle[1]

    # 左键按下消息
    if event == cv2.EVENT_LBUTTONDOWN:
        # 记录起始点
        g_bDrawingBox = True
        g_rectangle[0] = x
        g_rectangle[1] = y
        g_rectangle[2] = 0
        g_rectangle[3] = 0

    elif event == cv2.EVENT_LBUTTONUP:
        g_bDrawingBox = False
        if g_rectangle[2] < 0:
            g_rectangle[0] += g_rectangle[2]
            g_rectangle[2] *= -1

        if g_rectangle[3] < 0:
            g_rectangle[1] += g_rectangle[3]
            g_rectangle[3] *= -1

        # 调用函数进行绘制
        srcImage = DrawRectangle(srcImage, g_rectangle);


cv2.namedWindow('image')
cv2.setMouseCallback('image', GetRGBCvMouseCallback)

while (1):
    np.copyto(tempImage, srcImage)
    if g_bDrawingBox:
        # 当进行绘制的标识符为真，则进行绘制
        tempImage = DrawRectangle(tempImage, g_rectangle);

    cv2.imshow('image', tempImage)

    # 按下ESC键，程序退出
    k = cv2.waitKey(10) & 0xFF
    if k == 27:
        break

cv2.destroyAllWindows()

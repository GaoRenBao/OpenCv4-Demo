# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：OpenCV中的稠密光流
# 博客：http://www.bilibili996.com/Course?id=6af89dda880846cca6da479b654d68d0
# 作者：高仁宝
# 时间：2023.11

# OpenCV-Python-Tutorial-中文版.pdf P235

import cv2
import numpy as np

cap = cv2.VideoCapture("../images/vtest.avi")
ret, frame1 = cap.read()

prvs = cv2.cvtColor(frame1, cv2.COLOR_BGR2GRAY)
hsv = np.zeros_like(frame1)
hsv[..., 1] = 255

cv2.imshow('hsv', hsv)
cv2.waitKey(0)


while True:
    ret, frame2 = cap.read()
    next = cv2.cvtColor(frame2, cv2.COLOR_BGR2GRAY)
    flow = cv2.calcOpticalFlowFarneback(prvs, next, None, 0.5, 3, 15, 3, 5, 1.2, 0)
    mag, ang = cv2.cartToPolar(flow[..., 0], flow[..., 1])
    hsv[..., 0] = ang * 180 / np.pi / 2
    hsv[..., 2] = cv2.normalize(mag, None, 0, 255, cv2.NORM_MINMAX)
    bgr = cv2.cvtColor(hsv, cv2.COLOR_HSV2BGR)

    cv2.imshow('frame2', frame2)
    cv2.imshow('flow', bgr)
    k = cv2.waitKey(1) & 0xff
    if k == 27:
        break
    prvs = next

cap.release()
cv2.destroyAllWindows()


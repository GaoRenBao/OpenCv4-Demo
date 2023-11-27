# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：背景减除
# 博客：http://www.bilibili996.com/Course?id=56cb7ea6a7b84e2297af1334aaeb7609
# 作者：高仁宝
# 时间：2023.11

import cv2

cap = cv2.VideoCapture('../images/vtest.avi')
# cap = cv2.VideoCapture(0)#笔记本摄像头

fgbg = cv2.createBackgroundSubtractorMOG2()

while True:
    ret, frame = cap.read()

    # frame = cv2.flip(frame, flipCode=1)  # 左右翻转
    fgmask = fgbg.apply(frame)

    cv2.imshow('frame', frame)
    cv2.imshow('fgmask', fgmask)
    if cv2.waitKey(30) == 27:
        break
cap.release()
cv2.destroyAllWindows()

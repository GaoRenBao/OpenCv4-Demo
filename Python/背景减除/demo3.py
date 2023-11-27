# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：背景减除
# 博客：http://www.bilibili996.com/Course?id=56cb7ea6a7b84e2297af1334aaeb7609
# 作者：高仁宝
# 时间：2023.11

import cv2

cap = cv2.VideoCapture('../images/vtest.avi')
# cap = cv2.VideoCapture(0)#笔记本摄像头

kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (3, 3))
fgbg = cv2.bgsegm.createBackgroundSubtractorGMG()

while True:
    ret, frame = cap.read()

    fgmask = fgbg.apply(frame)
    fgmask = cv2.morphologyEx(fgmask, cv2.MORPH_OPEN, kernel)

    cv2.imshow('frame', frame)
    cv2.imshow('fgmask', fgmask)
    if cv2.waitKey(1) == 27:
        break

cap.release()
cv2.destroyAllWindows()

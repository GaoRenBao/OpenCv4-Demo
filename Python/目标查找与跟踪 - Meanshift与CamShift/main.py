# OpenCv版本：opencv-python 4.6.0.66
# 内容：目标查找与跟踪 - Meanshift与CamShift
# 博客：http://www.bilibili996.com/Course?id=0699ef6cd6e1407bbfb39a5e39b81e9a
# 作者：高仁宝
# 时间：2023.11

import numpy as np
import cv2

cap = cv2.VideoCapture('../images/Meanshift_CamShift.mp4')

# take first frame of the video
ret, frame = cap.read()
# setup initial location of window
c, r, w, h = 65, 275, 105, 105

track_window = (c, r, w, h)
# set up the ROI for tracking
roi = frame[r:r + h, c:c + w]
# cv2.imshow('frame', frame)
# cv2.imshow('roi', roi)

hsv_roi = cv2.cvtColor(roi, cv2.COLOR_BGR2HSV)

# 将低亮度的值忽略掉
mask = cv2.inRange(hsv_roi, np.array((0, 100, 0)), np.array((100, 255, 255)))

# 这个方法貌似也行
# roi2 = cv2.cvtColor(roi, cv2.COLOR_BGR2GRAY)
# ret, mask = cv2.threshold(roi2, 100, 255, cv2.THRESH_BINARY)
# mask = cv2.bitwise_not(mask)

# cv2.imshow('hsv_roi', hsv_roi)
# cv2.imshow('mask', mask)
# cv2.waitKey(0)

roi_hist = cv2.calcHist([hsv_roi], [0], mask, [180], [0, 180])
cv2.normalize(roi_hist, roi_hist, 0, 255, cv2.NORM_MINMAX)

# Setup the termination criteria, either 10 iteration or move by atleast 1 pt
term_crit = (cv2.TERM_CRITERIA_EPS | cv2.TERM_CRITERIA_COUNT, 10, 1)

track_window1 = track_window
track_window2 = track_window

while True:
    ret, frame = cap.read()
    if ret is True:
        img1 = np.copy(frame)
        img2 = np.copy(frame)

        hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
        dst = cv2.calcBackProject([hsv], [0], roi_hist, [0, 180], 1)

        # meanShift 效果
        ret, track_window = cv2.meanShift(dst, track_window1, term_crit)
        x, y, w, h = track_window
        img1 = cv2.rectangle(img1, (x, y), (x + w, y + h), (0, 0, 255), 2)
        img1 = cv2.putText(img1, "MeanShift", (10, 30), cv2.FONT_HERSHEY_COMPLEX, 1, (0, 0, 255), 2)
        cv2.imshow('img1', img1)

        # CamShift 效果
        ret, track_window = cv2.CamShift(dst, track_window2, term_crit)
        pts = cv2.boxPoints(ret)
        pts = np.int0(pts)
        print('len pts:', len(pts), pts)
        img2 = cv2.polylines(img2, [pts], True, (0, 255, 0), 2)
        img2 = cv2.putText(img2, "CamShift", (10, 30), cv2.FONT_HERSHEY_COMPLEX, 1, (0, 255, 0), 2)
        cv2.imshow('img2', img2)
        # 按ESC退出
        if cv2.waitKey(30) == 27:
            break
    else:
        break
cv2.destroyAllWindows()
cap.release()
# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：彩色目标追踪
# 博客：http://www.bilibili996.com/Course?id=4743192000049
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

xs, ys, ws, hs = 0, 0, 0, 0  # selection.x selection.y
xo, yo = 0, 0  # origin.x origin.y
selectObject = False
trackObject = 0


# 创建回调函数
def onMouse(event, x, y, flags, param):
    global xs, ys, ws, hs, selectObject, xo, yo, trackObject
    if selectObject == True:
        xs = min(x, xo)
        ys = min(y, yo)
        ws = abs(x - xo)
        hs = abs(y - yo)
    if event == cv2.EVENT_LBUTTONDOWN:
        xo, yo = x, y
        xs, ys, ws, hs = x, y, 0, 0
        selectObject = True
    elif event == cv2.EVENT_LBUTTONUP:
        selectObject = False
        trackObject = -1


# 打开摄像头
Cap = cv2.VideoCapture(0)
# 判断视频是否打开
if (Cap.isOpened() == False):
    print('Open Camera Error.')
else:
    Cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)  # 设置图像宽
    Cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)  # 设置图像高
    Cap.set(cv2.CAP_PROP_EXPOSURE, -3)  # 设置曝光值
    # 读取设置的参数
    size = (int(Cap.get(cv2.CAP_PROP_FRAME_WIDTH)), int(Cap.get(cv2.CAP_PROP_FRAME_HEIGHT)))
    baog = int(Cap.get(cv2.CAP_PROP_EXPOSURE))
    # 输出参数
    print('摄像头设置，尺寸:' + str(size))
    print('摄像头设置，曝光:' + str(baog))

    cv2.namedWindow("CamShift Demo", 1)
    cv2.setMouseCallback("CamShift Demo", onMouse)
    term_crit = (cv2.TERM_CRITERIA_EPS | cv2.TERM_CRITERIA_COUNT, 10, 1)

    # 读取图像
    while True:
        grabbed, frame = Cap.read()
        if frame is None:
            continue
        if trackObject != 0:
            hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
            mask = cv2.inRange(hsv, np.array((0., 30., 10.)), np.array((180., 256., 255.)))
            if trackObject == -1:
                track_window = (xs, ys, ws, hs)
                maskroi = mask[ys:ys + hs, xs:xs + ws]
                hsv_roi = hsv[ys:ys + hs, xs:xs + ws]
                roi_hist = cv2.calcHist([hsv_roi], [0], maskroi, [180], [0, 180])
                cv2.normalize(roi_hist, roi_hist, 0, 255, cv2.NORM_MINMAX)
                trackObject = 1
            dst = cv2.calcBackProject([hsv], [0], roi_hist, [0, 180], 1)
            dst &= mask
            ret, track_window = cv2.CamShift(dst, track_window, term_crit)
            pts = cv2.boxPoints(ret)
            pts = np.int0(pts)
            img2 = cv2.polylines(frame, [pts], True, 255, 2)

        if selectObject == True and ws > 0 and hs > 0:
            cv2.imshow('imshow1', frame[ys:ys + hs, xs:xs + ws])
            cv2.bitwise_not(frame[ys:ys + hs, xs:xs + ws], frame[ys:ys + hs, xs:xs + ws])
        cv2.imshow('CamShift Demo', frame)
        if cv2.waitKey(10) == 27:
            break
    cv2.destroyAllWindows()




import cv2
import numpy as np

xo,yo=0,0
addRemovePt = False

# ShiTomasi corner detection的参数
feature_params = dict(maxCorners=500,
                        qualityLevel=0.01,
                        minDistance=10,
                        blockSize=3)
# 光流法参数
lk_params = dict(winSize=(10, 10),
                 maxLevel=2,
                 criteria=(cv2.TERM_CRITERIA_EPS | cv2.TERM_CRITERIA_MAX_ITER, 20, 0.03))

#创建回调函数
def onMouse(event,x,y,flags,param):
    global xo,yo,addRemovePt
    if event == cv2.EVENT_LBUTTONDOWN:
        xo,yo = x, y
        addRemovePt = True

# 打开摄像头
Cap = cv2.VideoCapture(0)
# 判断视频是否打开
if (Cap.isOpened() == False):
    print('Open Camera Error.')
else:
    Cap.set(cv2.CAP_PROP_FRAME_WIDTH,640)  # 设置图像宽
    Cap.set(cv2.CAP_PROP_FRAME_HEIGHT,480) # 设置图像高
    Cap.set(cv2.CAP_PROP_EXPOSURE, -3)      # 设置曝光值
    # 读取设置的参数
    size = (int(Cap.get(cv2.CAP_PROP_FRAME_WIDTH)),int(Cap.get(cv2.CAP_PROP_FRAME_HEIGHT)))
    baog = int(Cap.get(cv2.CAP_PROP_EXPOSURE))
    # 输出参数
    print('摄像头设置，尺寸:' + str(size))
    print('摄像头设置，曝光:' + str(baog))

    cv2.namedWindow("CamShift Demo", 1)
    cv2.setMouseCallback("CamShift Demo", onMouse)

    grabbed, frame = Cap.read() # 取出视频的第一帧
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)  # 灰度化
    points1 = cv2.goodFeaturesToTrack(gray, mask=None, **feature_params)

    prevGray = None
    needToInit = True

    # 读取图像
    while True:
        grabbed, frame = Cap.read()
        if frame is None:
            continue
        image=frame.copy()
        # 将原图像转换为灰度图像
        gray=cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

        if needToInit == True:
            points2 = cv2.goodFeaturesToTrack(gray, mask=None, **feature_params)
            addRemovePt = False
        elif points1 is not None and points1.size != 0 :
            if prevGray is None:
                prevGray = gray.copy()

            # 计算光流以获取点的新位置
            points2, st, err = cv2.calcOpticalFlowPyrLK(prevGray, gray, points1, None, **lk_params)
            # 绘制跟踪点
            for i, (new) in enumerate(zip(points2[st == 1])):
                frame = cv2.circle(frame, ((int)(new[0][0]),(int)(new[0][1])), 8, (0, 255, 0), -1)
           
        if addRemovePt == True and points2 is None :
            maskroi=np.zeros_like(gray)
            maskroi[yo:yo+5, xo:xo+5]=255
            # 效果看起来像是点追踪，实际上是追踪一个长宽都为5的正方形roi区域
            # CornerSubPix函数的操作方法没找到...
            points2 = cv2.goodFeaturesToTrack(gray,mask=maskroi, **feature_params)
            addRemovePt = False;

        needToInit = False
        cv2.imshow('CamShift Demo', frame)
   
        k = cv2.waitKey(30)  # & 0xff

        # r 等于114
        if k == 114:
            needToInit = True
        # c 等于99
        if k == 99:
            points1 = None
            points2 = None

        points1=points2
        prevGray = gray.copy()
    cv2.destroyAllWindows()
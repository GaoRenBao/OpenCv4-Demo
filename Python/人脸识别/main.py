# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：人脸识别
# 博客：http://www.bilibili996.com/Course?id=4962812000072
# 作者：高仁宝
# 时间：2023.11

import cv2

# 读取xml文件
face_cascade = cv2.CascadeClassifier('Files/haarcascade_frontalface_alt.xml')
eyes_cascade = cv2.CascadeClassifier('Files/haarcascade_eye_tree_eyeglasses.xml')


def detectAndDisplay(frame):
    # 将原图像转换为灰度图像
    frame_gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    # 直方图均衡化, 用于提高图像的质量
    frame_gray = cv2.equalizeHist(frame_gray)

    # 人脸检测
    faces = face_cascade.detectMultiScale(frame_gray, scaleFactor=1.1,
                                          minNeighbors=2, minSize=(20, 20),
                                          flags=(0 | cv2.CASCADE_SCALE_IMAGE))

    for (x, y, w, h) in faces:
        # 绘制脸部区域
        # 这里一定要转换成int，不然ellipse会提示：ellipse() takes at most 5 arguments (9 given)
        center = (int(x + w / 2), int(y + w / 2))  # 中心点位置
        axes = (int(w / 2), int(h / 2))  # 长轴半径、短轴半径
        cv2.ellipse(frame, center, axes, 0, 0, 360, (255, 0, 255), thickness=2, lineType=8)

        # 绘制眼睛区域
        faceROI = frame_gray[y:y + h, x:x + w]
        cv2.imshow('faceROI', faceROI)

        eyes = eyes_cascade.detectMultiScale(faceROI, scaleFactor=1.1,
                                             minNeighbors=2, minSize=(20, 20),
                                             flags=(0 | cv2.CASCADE_SCALE_IMAGE))

        for (x2, y2, w2, h2) in eyes:
            sye_x = x + x2 + w2 / 2
            sye_y = y + y2 + h2 / 2
            radius = round((w2 + h2) * 0.25, 0)
            frame = cv2.circle(frame, (int(sye_x), int(sye_y)), int(radius), (0, 0, 255), 3, 8, 0)
    return frame


if __name__ == "__main__":
    # 打开摄像头
    Cap = cv2.VideoCapture(0)
    # 判断视频是否打开
    if (Cap.isOpened() == False):
        print('Open Camera Error.')
    else:
        Cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)  # 设置图像宽
        Cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)  # 设置图像高
        Cap.set(cv2.CAP_PROP_EXPOSURE, -3)  # 设置曝光值

        # 读取图像
        while True:
            grabbed, frame = Cap.read()
            if frame is None:
                continue

            frame = detectAndDisplay(frame)
            cv2.imshow('frame', frame)
            cv2.waitKey(30)

        cv2.destroyAllWindows()

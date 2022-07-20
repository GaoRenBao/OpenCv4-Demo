import cv2
import numpy as np

video = cv2.VideoCapture('1.avi')
fps = video.get(cv2.CAP_PROP_FPS)

success = True
while success:
    # 读帧
    success, frame = video.read()
    if success == False :
        break
    cv2.imshow('windows', frame)  # 显示
    cv2.waitKey(int(1000 / int(fps)))  # 设置延迟时间

video.release()



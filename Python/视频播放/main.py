# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：视频播放
# 博客：http://www.bilibili996.com/Course?id=4380386000009
# 作者：高仁宝
# 时间：2023.11

import cv2

video = cv2.VideoCapture('../images/lol.avi')
fps = video.get(cv2.CAP_PROP_FPS)

success = True
while success:
    # 读帧
    success, frame = video.read()
    if success == False:
        break
    cv2.imshow('windows', frame)  # 显示
    cv2.waitKey(int(1000 / int(fps)))  # 设置延迟时间

video.release()

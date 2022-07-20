import cv2
import numpy as np

#######################[摄像头图像实时显示]###############################
# 打开摄像头
Cap = cv2.VideoCapture(0)
# 判断视频是否打开
if (Cap.isOpened() == False):
    print('Open Camera Error.')
else:
    Cap.set(cv2.CAP_PROP_FRAME_WIDTH,640)  # 设置图像宽
    Cap.set(cv2.CAP_PROP_FRAME_HEIGHT,480) # 设置图像高
    Cap.set(cv2.CAP_PROP_EXPOSURE, -3)     # 设置曝光值
    # 读取设置的参数
    size = (int(Cap.get(cv2.CAP_PROP_FRAME_WIDTH)),int(Cap.get(cv2.CAP_PROP_FRAME_HEIGHT)))
    baog = int(Cap.get(cv2.CAP_PROP_EXPOSURE))
    # 输出参数
    print('摄像头设置，尺寸:' + str(size))
    print('摄像头设置，曝光:' + str(baog))
    # 读取图像
    while True:
        grabbed, img = Cap.read()
        if img is None:
            continue
        cv2.imshow('windows', img)  # 显示
        cv2.waitKey(30)  # 设置延迟时间

#############################[录像]######################################
## 打开摄像头
#Cap = cv2.VideoCapture(0)
## 判断视频是否打开
#if (Cap.isOpened() == False):
#    print('Open Camera Error.')
#else:
#    Cap.set(cv2.CAP_PROP_FRAME_WIDTH,640)  # 设置图像宽
#    Cap.set(cv2.CAP_PROP_FRAME_HEIGHT,480) # 设置图像高
#    Cap.set(cv2.CAP_PROP_EXPOSURE, -3)     # 设置曝光值
#    # 读取设置的参数
#    size = (int(Cap.get(cv2.CAP_PROP_FRAME_WIDTH)),int(Cap.get(cv2.CAP_PROP_FRAME_HEIGHT)))
#    baog = int(Cap.get(cv2.CAP_PROP_EXPOSURE))
#    # 输出参数
#    print('摄像头设置，尺寸:' + str(size))
#    print('摄像头设置，曝光:' + str(baog))
#    # 设置存储的录像文件格式
#    myavi = cv2.VideoWriter("a.avi",cv2.VideoWriter_fourcc(*'MJPG'),25.0, (640, 480), True)

#    while(Cap.isOpened()):
#        ret, frame = Cap.read()
#        if ret==True:
#            cv2.imshow('frame',frame)
#            myavi.write(frame)
#            if cv2.waitKey(10) & 0xFF == ord('q'):
#                break
#        else:
#            break
#    Cap.release()
#    myavi.release()
#    cv2.destroyAllWindows()

############################[播放录像]####################################
#video = cv2.VideoCapture('a.avi')
#fps = video.get(cv2.CAP_PROP_FPS)
#success = True
#while success:
#    # 读帧
#    success, frame = video.read()
#    if success == False :
#        break
#    cv2.imshow('windows', frame)  # 显示
#    cv2.waitKey(int(1000 / int(fps)))  # 设置延迟时间
#video.release()



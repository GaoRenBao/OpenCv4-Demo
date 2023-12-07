# OpenCv版本：opencv-python 4.6.0.66
# 内容：颜色空间转换与物体追踪
# 博客：http://www.bilibili996.com/Course?id=1618402000255
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

'''
# 最基本的颜色空间转换示例
'''


def demo1():
    # wrong
    # green=np.uint8([0,255,0])
    # print green
    # hsv_green=cv2.cvtColor(green,cv2.COLOR_BGR2HSV)
    # print hsv_green

    # scn (the number of channels of the source),
    # i.e. self.img.channels(), is neither 3 nor 4.
    #
    # depth (of the source),
    # i.e. self.img.depth(), is neither CV_8U nor CV_32F.
    # 所以不能用 [0,255,0] 而 用 [[[0,255,0]]]
    # 的三层括号应 分别对应于 cvArray cvMat IplImage

    green = np.uint8([[[0, 255, 0]]])
    hsv_green = cv2.cvtColor(green, cv2.COLOR_BGR2HSV)
    print(hsv_green)
    # [[[60 255 255]]]

    black = np.uint8([[[0, 0, 0]]])
    hsv_black = cv2.cvtColor(black, cv2.COLOR_BGR2HSV)
    print(hsv_black)
    # [[[0 0 0]]]


def demo2():
    '''
    物体跟踪

    • 从视频中获取每一帧图像
    • 将图像转换到 HSV 空间
    • 设置 HSV 阈值到蓝色范围。
    • 获取蓝色物体 当然我们 可以做其他任何我们想做的事
    比如 在蓝色 物体周围画一个圈。


    当你学习了【轮廓】之后 你就会学到更多 相关知识
    那是你就可以找到物体的重心 并根据重心来跟踪物体
    仅仅在摄像头前挥挥手就可以画出同的图形，或者其他更有趣的事。
    '''

    cap = cv2.VideoCapture(0, cv2.CAP_DSHOW)
    # ret = cap.set(3, 640)
    # ret = cap.set(4, 480)

    # 定蓝色的阈值
    lower = np.array([90, 50, 50])
    upper = np.array([130, 255, 255])

    # 黄色-乒乓球
    # lower = np.array([20, 100, 100])
    # upper = np.array([30, 255, 255])

    # 黑色
    # lower = np.array([0, 0, 0])
    # upper = np.array([180, 255, 30])

    while True:
        # 获取每一帧
        ret, frame = cap.read()
        # 换到 HSV
        hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)

        # 根据阈值构建掩模
        mask = cv2.inRange(hsv, lower, upper)
        # mask = cv2.inRange(hsv, lower_black, upper_black)
        # 对原图像和掩模位运算
        res = cv2.bitwise_and(frame, frame, mask=mask)

        # 显示图像
        cv2.imshow('frame', frame)
        cv2.moveWindow('frame', x=0, y=0)  # 原地
        cv2.imshow('mask', mask)
        cv2.moveWindow('mask', x=frame.shape[1], y=0)  # 右边
        cv2.imshow('res', res)
        cv2.moveWindow('res', y=frame.shape[0], x=0)  # 下边

        k = cv2.waitKey(1)  # & 0xFF
        if k == ord('q'):
            break
    # 关闭窗口
    cap.release()
    cv2.destroyAllWindows()


if __name__ == '__main__':
    demo1()
    # demo2()

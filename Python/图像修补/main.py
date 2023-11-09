# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：分水岭算法
# 博客：http://www.bilibili996.com/Course?id=3482873000211
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np

WINDOW_NAME0="WINDOW_NAME0"
WINDOW_NAME1="WINDOW_NAME1"
WINDOW_NAME2="WINDOW_NAME2"

previousPoint = (-1,-1)

def onMouse(event, x, y, flags, param):
    global srcImage1,inpaintMask,previousPoint

    # 处理鼠标左键相关消息
    if event == cv2.EVENT_LBUTTONUP or flags & cv2.EVENT_FLAG_LBUTTON <= 0:
        previousPoint = (-1, -1)
    elif event == cv2.EVENT_LBUTTONDOWN:
        previousPoint = (x, y)

    # 鼠标左键按下并移动，绘制出白色线条
    elif event == cv2.EVENT_MOUSEMOVE or flags & cv2.EVENT_FLAG_LBUTTON > 0:
        pt = (x,y)
        if previousPoint[0] < 0:
            previousPoint = pt
        cv2.line(inpaintMask, previousPoint, pt, (255, 255, 255), 5, 8, 0)
        cv2.line(srcImage1, previousPoint, pt, (255, 255, 255), 5, 8, 0)
        previousPoint = pt
        cv2.imshow(WINDOW_NAME1, srcImage1)

# 载入原图
srcImage=cv2.imread('../images/Astral2.jpg')
srcImage0=srcImage.copy()
srcImage1=srcImage.copy()
inpaintMask = np.zeros((srcImage1.shape[0], srcImage1.shape[1], 3), np.uint8)
inpaintMask = cv2.cvtColor(inpaintMask, cv2.COLOR_BGR2GRAY)

# 显示原始图参考
cv2.imshow(WINDOW_NAME0, srcImage0);
# 显示原始图
cv2.imshow(WINDOW_NAME1, srcImage1);
# 设置鼠标回调函数
cv2.setMouseCallback("WINDOW_NAME1", onMouse)

# 轮询按键，进行处理
while True:
    c = cv2.waitKey(0)

    # 若按键键值为ESC时，退出
    if c == 27:
        break

    # 键值为2，恢复成原始图像
    if c == 50:
        inpaintMask = np.zeros((srcImage1.shape[0], srcImage1.shape[1], 3), np.uint8)
        inpaintMask = cv2.cvtColor(inpaintMask, cv2.COLOR_BGR2GRAY)
        srcImage1 = srcImage.copy()
        cv2.imshow(WINDOW_NAME1, srcImage1)

    # 键值为1或者空格，进行图像修补操作
    if c == 49 or c == 32:
        inpaintedImage = cv2.inpaint(srcImage1, inpaintMask, 3, cv2.INPAINT_TELEA)
        cv2.imshow(WINDOW_NAME2, inpaintedImage)


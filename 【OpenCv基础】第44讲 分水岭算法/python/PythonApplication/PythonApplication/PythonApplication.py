import cv2
import numpy as np
import random

WINDOW_NAME1="WINDOW_NAME1"
WINDOW_NAME2="WINDOW_NAME2"
prevPt = (-1,-1)

def onMouse(event, x, y, flags, param):
    global g_maskImage,g_srcImage,prevPt

    # 处理鼠标不在窗口中的情况
    if x < 0 or x >= g_srcImage.shape[1] or y < 0 or y >= g_srcImage.shape[0]:
        return

    # 处理鼠标左键相关消息
    if event == cv2.EVENT_LBUTTONUP or flags & cv2.EVENT_FLAG_LBUTTON <= 0:
        prevPt = (-1, -1)
    elif event == cv2.EVENT_LBUTTONDOWN:
        prevPt = (x, y)

    # 鼠标左键按下并移动，绘制出白色线条
    elif event == cv2.EVENT_MOUSEMOVE or flags & cv2.EVENT_FLAG_LBUTTON > 0:
        pt = (x,y)
        if prevPt[0] < 0:
            prevPt = pt
        cv2.line(g_maskImage, prevPt, pt, (255, 255, 255), 5, 8, 0)
        cv2.line(g_srcImage, prevPt, pt, (255, 255, 255), 5, 8, 0)
        prevPt = pt
        cv2.imshow(WINDOW_NAME1, g_srcImage)

def setMat(mat, value):
    for w in range(mat.shape[1]):
        for h in range(mat.shape[0]):
            mat[h, w] = value
    return mat

# 载入原图
g_srcImage=cv2.imread('1.jpg')
cv2.imshow(WINDOW_NAME1, g_srcImage);
srcImage=g_srcImage.copy()
g_maskImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)
grayImage = cv2.cvtColor(g_maskImage, cv2.COLOR_GRAY2BGR)
g_maskImage = setMat(g_maskImage,0)

# 设置鼠标回调函数
cv2.setMouseCallback("WINDOW_NAME1", onMouse)

# 轮询按键，进行处理
while True:
    c = cv2.waitKey(0)

    # 若按键键值为ESC时，退出
    if c == 27:
        needToInit = True

    # 按键键值为2时，恢复源图
    if c == 50:
        g_maskImage = setMat(g_maskImage,0)
        g_srcImage = srcImage.copy()
        cv2.imshow(WINDOW_NAME1, g_srcImage)

    # 若检测到按键值为1，则进行处理
    if c == 49:
        # 寻找图像轮廓
        contours, hierarchy  = cv2.findContours(g_maskImage,cv2.RETR_CCOMP,cv2.CHAIN_APPROX_SIMPLE)
 
        # 轮廓为空时的处理
        if len(contours) == 0:
            continue;

        # 拷贝掩膜
        maskImage = np.ones(g_maskImage.shape, dtype=np.int32)
        maskImage = setMat(maskImage,0)

        # 循环绘制出轮廓
        for index in range(0,len(contours)): 
            # color = (random.randint(0,255), random.randint(0,255), random.randint(0,255)) # 不能采用随机数
            cv2.drawContours(maskImage, contours, index, index+1, -1, 8, hierarchy, cv2.INTER_MAX)

        # 生成随机颜色
        colorTab = []
        for i in range(0,len(contours)): 
            b = random.randint(0,255)
            g = random.randint(0,255)
            r = random.randint(0,255)
            colorTab.append([b, g, r])

        # 分水岭
        maskImage = cv2.watershed(srcImage, maskImage)
    
        # 双层循环，将分水岭图像遍历存入watershedImage中
        compCount = len(contours)
        watershedImage = np.ones((maskImage.shape[0], maskImage.shape[1], 3),np.uint8)
        for i in range(maskImage.shape[1]):
            for j in range(maskImage.shape[0]):
                index = maskImage[j, i]
                if index == -1:
                    watershedImage[j, i] = [255,255,255]
                elif index <= 0 or index > compCount:
                    watershedImage[j, i] = [0,0,0]
                else:
                    watershedImage[j, i] = colorTab[index - 1]

        # cv2.imshow("1", watershedImage);
        # cv2.imshow("2", grayImage);

        # 混合灰度图和分水岭效果图并显示最终的窗口
        watershedImage = cv2.addWeighted(watershedImage, 0.5, grayImage, 0.5, 0)
        cv2.imshow(WINDOW_NAME2, watershedImage);
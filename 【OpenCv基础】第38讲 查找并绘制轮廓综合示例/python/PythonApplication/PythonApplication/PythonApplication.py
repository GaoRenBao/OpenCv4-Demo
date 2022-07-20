import cv2
import numpy as np
import random

def zh_ch(string):    
	return string.encode("gbk").decode(errors="ignore")

g_nThresh = 80

def on_ThreshChange(x):
    global g_nThresh, g_grayImage
    g_nThresh = cv2.getTrackbarPos('g_nThresh', 'dstImage')

    ################################################################

    # 用Canny算子检测边缘
    g_cannyMat_output = edge=cv2.Canny(g_grayImage,g_nThresh,g_nThresh * 2,3)
    # 查找轮廓
    contours, hierarchy  = cv2.findContours(g_cannyMat_output,cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)   
    imgH = g_grayImage.shape[0]
    imgW = g_grayImage.shape[1]
    dstImage = cv2.UMat((imgW, imgH), cv2.CV_8UC3) 
    for i in range(0,len(contours)): 
        cv2.drawContours(dstImage, contours, i, (random.randint(0,255), random.randint(0,255), random.randint(0,255)), 2, 8,  hierarchy)
 
    ################################################################

    #ret,g_cannyMat_output = cv2.threshold(g_grayImage,g_nThresh,255,cv2.THRESH_BINARY)
    ## 查找轮廓
    #contours, hierarchy  = cv2.findContours(g_cannyMat_output,cv2.RETR_CCOMP,cv2.CHAIN_APPROX_SIMPLE)  
    #imgH = g_grayImage.shape[0]
    #imgW = g_grayImage.shape[1]
    #dstImage = cv2.UMat((imgW, imgH), cv2.CV_8UC3) 
    #for i in range(0,len(contours)): 
    #    cv2.drawContours(dstImage, contours, i, (random.randint(0,255), random.randint(0,255), random.randint(0,255)), -1, 8,  hierarchy)
 
    ################################################################

    cv2.imshow("dstImage", dstImage)
    cv2.waitKey(30)

# 【1】载入原始图
g_grayImage = cv2.imread("1.jpg", 0)
cv2.imshow(zh_ch("原始图"), g_grayImage)

# 创建窗口
cv2.namedWindow('dstImage')
cv2.createTrackbar('g_nThresh', 'dstImage', 0, 255, on_ThreshChange)
cv2.setTrackbarPos('g_nThresh', 'dstImage', 80)

while (True):
    if cv2.waitKey(10) == ord('q'):
        break

cv2.destroyAllWindows()




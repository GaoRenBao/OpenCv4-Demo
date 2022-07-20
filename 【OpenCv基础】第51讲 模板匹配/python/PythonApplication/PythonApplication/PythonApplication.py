import cv2
import numpy as np
import random

# 请调整滑动条观察图像效果
# 滑动条对应的方法数值说明
# 方法【0】- 平方差匹配法(SQDIFF)
# 方法【1】- 归一化平方差匹配法(SQDIFF NORMED)
# 方法【2】- 相关匹配法(TM CCORR)
# 方法【3】- 归一化相关匹配法(TM CCORR NORMED)
# 方法【4】- 相关系数匹配法(TM COEFF)
# 方法【5】- 归一化相关系数匹配法(TM COEFF NORMED)

def on_Matching(x):
    global g_srcImage,g_templateImage, g_resultImage

    # 获取滑动条的值
    g_nMatchMethod = cv2.getTrackbarPos('g_nMatchMethod', 'WINDOW_NAME1')

    # 给局部变量初始化
    srcImage = np.copy(g_srcImage)

	# 初始化用于结果输出的矩阵
    resultImage_rows = g_srcImage.shape[1] - g_templateImage.shape[1] + 1
    resultImage_cols = g_srcImage.shape[0] - g_templateImage.shape[0] + 1

    g_resultImage = np.ones((resultImage_cols, resultImage_rows, 3),np.uint8)

    # 进行匹配和标准化
    g_resultImage = cv2.matchTemplate(g_srcImage, g_templateImage, g_nMatchMethod)
    cv2.normalize(g_resultImage,g_resultImage,0,1,cv2.NORM_MINMAX)
    
    # 通过函数 minMaxLoc 定位最匹配的位置
    minValue,maxValue,minLocation,maxLocation=cv2.minMaxLoc(g_resultImage)

	# 对于方法 SQDIFF 和 SQDIFF_NORMED, 越小的数值有着更高的匹配结果. 而其余的方法, 数值越大匹配效果越好
    # SqDiff = 0
    # SqDiffNormed = 1
    # CCorr = 2
    # CCorrNormed = 3
    # CCoeff = 4
    # CCoeffNormed = 5

    if g_nMatchMethod == 0 or g_nMatchMethod == 1:
        matchLocation = minLocation
    else:
        matchLocation = maxLocation

	# 绘制出矩形，并显示最终结果
    cv2.rectangle(srcImage, matchLocation, 
        (matchLocation[0] + g_templateImage.shape[0], matchLocation[1] + g_templateImage.shape[1]),
        (0, 0, 255), 2, 8, 0);
    cv2.rectangle(g_resultImage, matchLocation, 
        (matchLocation[0] + g_templateImage.shape[0], matchLocation[1] + g_templateImage.shape[1]),
        (0, 0, 255), 2, 8, 0);

    cv2.imshow("WINDOW_NAME1", srcImage)
    cv2.imshow("WINDOW_NAME2", g_resultImage)


# 【1】载入原图像和模板块
g_srcImage = cv2.imread("1.jpg")
g_templateImage = cv2.imread("2.jpg")

# 创建窗口
cv2.namedWindow('WINDOW_NAME1')
cv2.namedWindow('WINDOW_NAME2')

cv2.createTrackbar('g_nMatchMethod', 'WINDOW_NAME1', 0, 5, on_Matching)
cv2.setTrackbarPos('g_nMatchMethod', 'WINDOW_NAME1', 0)

# 等待用户按键
cv2.imshow("WINDOW_NAME1", g_srcImage)
cv2.waitKey(0)
cv2.destroyAllWindows()


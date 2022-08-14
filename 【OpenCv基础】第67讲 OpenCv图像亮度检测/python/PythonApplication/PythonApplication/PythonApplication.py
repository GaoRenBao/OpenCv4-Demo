import cv2
import numpy as np
import math

# 环境亮度值计算
def BrightnessDetection(srcImage):
    dstImage = cv2.cvtColor(srcImage,cv2.COLOR_BGR2GRAY)
    # bgr保存了所有0~255个像素值的数量
    bgr =[0]*256
    # a为所有亮度值的和
    a = 0
    # num 为像素点个数
    num = 0

    imgH = dstImage.shape[0]
    imgW = dstImage.shape[1]
    for i in range(imgW):
        for j in range(imgH):
            # 剔除最大和最小值
            if dstImage[j, i] == 0 :
                continue;
            if dstImage[j, i] == 255 :
                continue;
            num = num + 1
            #在计算过程中，考虑128为亮度均值点
            a = a + dstImage[j, i] - 128
            x = dstImage[j, i]
            bgr[x] = bgr[x] + 1

    # 计算像素点平均值
    da = a / num
    D = abs(da)

    # 将平均值乘以每个亮度点的个数，得到整个图像的亮度值，起到均衡亮度值
    # 避免由于某个区域特别亮，导致最终计算结果偏差过大
    Ma = 0;
    for i in range(256):
        Ma = Ma + math.fabs(i - 128 - da) * bgr[i]
    
    # 将整个图像的亮度值除以个数，最终得到每个像素点的亮度值
    Ma = Ma / (imgH * imgW)
    # 输出亮度值
    M = math.fabs(Ma) + 0.0001
    K = D / M
    cast = K if (da > 0) else -K
    return cast

# 打开摄像头
Cap = cv2.VideoCapture(0)
# 判断视频是否打开
if (Cap.isOpened() == False):
    print('Open Camera Error.')
else:
    # 读取图像
    while True:
        grabbed, img = Cap.read()
        if img is None:
            continue

        value = BrightnessDetection(img)
        font = cv2.FONT_HERSHEY_COMPLEX  # 设置字体
        img = cv2.putText(img, "Vlaue:" +  str(value), (10, 30), font, 1, (0, 0, 255), 2)

        cv2.imshow("srcImage", img)
        cv2.waitKey(1)



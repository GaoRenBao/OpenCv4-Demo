import cv2
import numpy as np

img1 = cv2.imread('ml.png')

e1 = cv2.getTickCount()

for i in range(5, 49, 2):
    img1 = cv2.medianBlur(img1, i)

e2 = cv2.getTickCount()
t = (e2 - e1) / cv2.getTickFrequency()  # 时钟频率 或者 每秒钟的时钟数
print(t) 


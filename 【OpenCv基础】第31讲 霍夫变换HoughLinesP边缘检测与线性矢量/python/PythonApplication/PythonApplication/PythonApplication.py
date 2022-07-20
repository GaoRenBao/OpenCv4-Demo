import cv2
import numpy as np

#【1】载入原始图和Mat变量定义   
srcImage = cv2.imread('1.jpg')  #工程目录下应该有一张名为1.jpg的素材图

#【2】进行边缘检测和转化为灰度图
midImage=cv2.Canny(srcImage,50, 200, 3) #进行一此canny边缘检测

# 转化边缘检测后的图为灰度图
dstImage=cv2.cvtColor(midImage, cv2.COLOR_GRAY2BGR)

#【3】进行霍夫线变换
lines = cv2.HoughLinesP(midImage, 1, np.pi/180 ,80, 50, 10)
for line in lines:
    x1, y1, x2, y2 = line[0]
    cv2.line(dstImage, (x1, y1), (x2, y2), (186, 88, 255), 2)

#【5】显示原始图  
cv2.imshow("srcImage", srcImage)

#【6】边缘检测后的图 
cv2.imshow("midImage", midImage)

#【7】显示效果图  
cv2.imshow("Python", dstImage)

cv2.waitKey(0)
cv2.destroyAllWindows()
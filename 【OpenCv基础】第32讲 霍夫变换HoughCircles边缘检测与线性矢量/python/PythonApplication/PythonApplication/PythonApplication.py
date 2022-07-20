import cv2
import numpy as np

#【1】载入原始图和Mat变量定义   
srcImage = cv2.imread('1.jpg')  #工程目录下应该有一张名为1.jpg的素材图

#【2】显示原始图  
cv2.imshow("srcImage", srcImage)

#【3】转化边缘检测后的图为灰度图
midImage=cv2.cvtColor(srcImage,cv2.COLOR_BGR2GRAY)
# 进行高斯滤波操作
midImage = cv2.GaussianBlur(midImage,(9,9), 2)

#【3】进行霍夫线变换
circles = cv2.HoughCircles(midImage,cv2.HOUGH_GRADIENT,1.5,10,
                           param1=200,param2=100,minRadius=0,maxRadius=0)

circles = np.uint16(np.around(circles))
for i in circles[0,:]:
    cv2.circle(srcImage,(i[0],i[1]),5,(0, 255, 0),-1)
    cv2.circle(srcImage,(i[0],i[1]),i[2],(155, 50, 255),5)

#【7】显示效果图  
imgH = srcImage.shape[0]
imgW = srcImage.shape[1]
# srcImage = cv2.resize(srcImage, ((int)(imgW*0.5), (int)(imgH*0.5)), interpolation = cv2.INTER_NEAREST)
cv2.imshow("Python", srcImage)

cv2.waitKey(0)
cv2.destroyAllWindows()
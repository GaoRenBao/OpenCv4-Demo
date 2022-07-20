import cv2
import numpy as np

img = cv2.imread("1.jpg")

# 第一个参数：需要处理的图像；
# 第二个参数：是图像的深度，-1表示采用的是与原图像相同的深度，设定为 CV_16S 避免外溢。目标图像的深度必须大于等于原图像的深度；
# 第三个参数：dx方向求导，0表示这个方向上没有求导，一般为0、1、2。
# 第四个参数：dy方向求导，0表示这个方向上没有求导，一般为0、1、2。
# 第五个参数：dst不用解释了；
# 第六个参数：ksize是Sobel算子的大小，必须为1、3、5、7。
# 第七个参数：scale是缩放导数的比例常数，默认情况下没有伸缩系数；
# 第八个参数：delta是一个可选的增量，将会加到最终的dst中，同样，默认情况下没有额外的值加到dst中；
# 第九个参数：borderType是判断图像边界的模式。这个参数默认值为cv2.BORDER_DEFAULT。
x = cv2.Sobel(img,cv2.CV_16S,1,0,None,3,1,cv2.BORDER_DEFAULT)
y = cv2.Sobel(img,cv2.CV_16S,0,1,None,3,1,cv2.BORDER_DEFAULT)
 
absX = cv2.convertScaleAbs(x)
absY = cv2.convertScaleAbs(y)
 
dst = cv2.addWeighted(absX,0.5,absY,0.5,0)
 
cv2.imshow("absX", absX)
cv2.imshow("absY", absY)
cv2.imshow("Result", dst)
 
cv2.waitKey(0)
cv2.destroyAllWindows() 

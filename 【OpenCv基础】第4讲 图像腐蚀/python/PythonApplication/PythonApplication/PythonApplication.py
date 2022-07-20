import cv2
import numpy as np

# 载入图片
srcImage=cv2.imread('girl.jpg')
# 显示原图
cv2.imshow('原图',srcImage)
#腐蚀图像
element = cv2.getStructuringElement(cv2.MORPH_RECT,(15,15))
srcImage = cv2.erode(srcImage,element)
# 显示效果
cv2.imshow('效果',srcImage)
cv2.waitKey(0)
cv2.destroyAllWindows()


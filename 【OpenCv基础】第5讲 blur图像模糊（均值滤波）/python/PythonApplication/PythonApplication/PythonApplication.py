import cv2
import numpy as np

# 载入图片
srcImage=cv2.imread('girl.jpg')
# 显示原图
cv2.imshow('原图',srcImage)

# 均值滤波
blur2 = cv2.blur(srcImage, (7,7))
# 显示效果图
cv2.imshow('效果',blur2)

# 左右显示原图和效果图
#htich = np.hstack((srcImage, blur2))
#cv2.imshow('效果2',htich)

cv2.waitKey(0)
cv2.destroyAllWindows()


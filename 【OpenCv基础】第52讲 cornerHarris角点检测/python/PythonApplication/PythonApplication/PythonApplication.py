import cv2
import numpy as np
import random

# 以灰度模式载入图像并显示
srcImage = cv2.imread("1.jpg", 0)
# 显示原图
cv2.imshow("srcImage", srcImage)

# 进行Harris角点检测找出角点
cornerStrength = cv2.cornerHarris(srcImage,2, 3, 0.01)

# 对灰度图进行阈值操作，得到二值图并显示  
ret, harrisCorner = cv2.threshold(cornerStrength, 0.00001, 255, cv2.THRESH_BINARY)
cv2.imshow("End", harrisCorner)

cv2.waitKey(0)
cv2.destroyAllWindows()


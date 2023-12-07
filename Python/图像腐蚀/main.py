# OpenCv版本：opencv-python 4.6.0.66
# 内容：图像腐蚀
# 博客：http://www.bilibili996.com/Course?id=4347382000005
# 作者：高仁宝
# 时间：2023.11

import cv2

# 载入图片
srcImage = cv2.imread('../images/girl.jpg')
# 显示原图
cv2.imshow('srcImage', srcImage)
# 腐蚀图像
element = cv2.getStructuringElement(cv2.MORPH_RECT, (15, 15))
srcImage = cv2.erode(srcImage, element)
# 显示效果
cv2.imshow('out', srcImage)
cv2.waitKey(0)
cv2.destroyAllWindows()

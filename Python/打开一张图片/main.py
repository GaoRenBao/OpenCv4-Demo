# OpenCv版本：opencv-python 4.6.0.66
# 内容：打开一张图片
# 博客：http://www.bilibili996.com/Course?id=4316440000002
# 作者：高仁宝
# 时间：2023.11

import cv2

# 读取一张图片
srcImage = cv2.imread("../images/a.jpg")
# 创建一个窗口
cv2.namedWindow("image")
# 显示图片
cv2.imshow("image", srcImage)
# 等待任意输入
cv2.waitKey(0)

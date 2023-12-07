# OpenCv版本：opencv-python 4.6.0.66
# 内容：图像的基础操作
# 博客：http://www.bilibili996.com/Course?id=5178073000253
# 作者：高仁宝
# 时间：2023.11


import cv2

img = cv2.imread('../images/messi5.jpg', 0)  # gray
print(img.shape)

img = cv2.imread('../images/messi5.jpg')
# print(img.shape)
rows, cols, ch = img.shape
print('行/高:', rows, '列/宽:', cols, '通道:', ch)

print(img.size)
print(img.dtype)  # uint8
# 注意 在debug 时 img.dtype非常重要。因为在 OpenCV- Python 代码中经常出现数据类型的不一致。

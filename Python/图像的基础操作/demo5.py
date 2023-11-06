# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：图像的基础操作
# 博客：http://www.bilibili996.com/Course?id=5178073000253
# 作者：高仁宝
# 时间：2023.11

import cv2

'''
例如我们 检测一副图像中 眼睛的位置 我们 先应该在图像中找到脸 再在脸的区域中找眼睛 
而不是 直接在一幅图像中搜索。这样会提高程序的准确性和性能。
'''

img = cv2.imread('../images/messi5.jpg')

ball = img[280:280 + 60, 330:330 + 60]  # 获取ROI区域
img[273:273 + 60, 100:100 + 60] = ball  # 将获取到的ROI区域替换到新的位置

cv2.imshow("messi", img)
cv2.waitKey(0)

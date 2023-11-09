# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：颜色识别
# 博客：http://www.bilibili996.com/Course?id=0177963000226
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np


def ColorFindContours(srcImage, iLowH, iHighH, iLowS, iHighS, iLowV, iHighV):
    # 转为HSV
    imgHSV = cv2.cvtColor(srcImage, cv2.COLOR_BGR2HSV)
    bufImg = cv2.inRange(imgHSV, np.array((iLowH, iLowS, iLowV)), np.array((iHighH, iHighS, iHighV)))
    return bufImg


def ColorFindContours2(srcImage):
    des1 = ColorFindContours(srcImage,
                             350 / 2, 360 / 2,  # 色调最小值~最大值
                             int(255 * 0.70), 255,  # 饱和度最小值~最大值
                             int(255 * 0.60), 255)  # 亮度最小值~最大值

    des2 = ColorFindContours(srcImage,
                             0, int(16 / 2),  # 色调最小值~最大值
                             int(255 * 0.70), 255,  # 饱和度最小值~最大值
                             int(255 * 0.60), 255)  # 亮度最小值~最大值

    return des1 + des2


# 载入色卡
srcImage = cv2.imread("../images/color.jpg")
# 显示原图
cv2.imshow("srcImage", srcImage)

des = ColorFindContours(srcImage,
                        45 / 2, 60 / 2,  # 色调最小值~最大值
                        int(255 * 0.60), 255,  # 饱和度最小值~最大值
                        int(255 * 0.90), 255)  # 亮度最小值~最大值
cv2.imshow("des1", des)

des = ColorFindContours2(srcImage)
cv2.imshow("des2", des)

cv2.waitKey(0)
cv2.destroyAllWindows()

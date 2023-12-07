# OpenCv版本：opencv-python 4.6.0.66
# 内容：7种图像处理形态学（2）
# 博客：http://www.bilibili996.com/Course?id=5411139000128
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np


# 下面是击中击不中操作测试
def demo1():
    array1 = [[0, 0, 0, 0, 0, 0, 0, 0],
              [0, 255, 255, 255, 0, 0, 0, 255],
              [0, 255, 255, 255, 0, 0, 0, 0],
              [0, 255, 255, 255, 0, 255, 0, 0],
              [0, 0, 255, 0, 0, 0, 0, 0],
              [0, 0, 255, 0, 0, 255, 255, 0],
              [0, 255, 0, 255, 0, 0, 255, 0],
              [0, 255, 255, 255, 0, 0, 0, 0]]

    array2 = [[0, 1, 0], [1, -1, 1], [0, 1, 0]]

    def createAlphaMat(mat, array):
        imgH = mat.shape[0]
        imgW = mat.shape[1]
        for w in range(imgW):
            for h in range(imgH):
                mat[h, w] = array[h][w]
        return mat

    # 创建一个2通道的图片
    input_image = np.ones((8, 8), np.uint8)
    input_image = createAlphaMat(input_image, array1)

    kernel = np.ones((3, 3), np.uint8)
    kernel = createAlphaMat(kernel, array2)

    # 进行形态学击中击不中操作
    output_image = cv2.morphologyEx(input_image, cv2.MORPH_HITMISS, kernel)

    # 为便于观察，将输入图像、输出图像、核放大五十倍，显示
    # 一个小方块表示一个像素
    rate = 400
    kernel = (kernel + 1) * 127;

    kernel = cv2.resize(kernel, (rate, rate), interpolation=cv2.INTER_NEAREST)
    cv2.imshow("kernel", kernel);

    input_image = cv2.resize(input_image, (rate, rate), interpolation=cv2.INTER_NEAREST)
    cv2.imshow("Original", input_image);

    output_image = cv2.resize(output_image, (rate, rate), interpolation=cv2.INTER_NEAREST)
    cv2.imshow("Hit or Miss", output_image);
    cv2.waitKey(0)


# 下面是形态学操作
def demo2():
    # 载入原图
    srcImage = cv2.imread('../images/girl4.jpg')

    # 显示原图
    cv2.imshow('image', srcImage)

    # 定义核大小
    element = cv2.getStructuringElement(cv2.MORPH_RECT, (15, 15))

    # 进行形态学腐蚀操作
    out1 = cv2.morphologyEx(srcImage, cv2.MORPH_ERODE, element)

    # 进行形态学膨胀操作
    out2 = cv2.morphologyEx(srcImage, cv2.MORPH_DILATE, element)

    # 进行形态学开运算操作
    out3 = cv2.morphologyEx(srcImage, cv2.MORPH_OPEN, element)

    # 进行形态学闭运算操作
    out4 = cv2.morphologyEx(srcImage, cv2.MORPH_CLOSE, element)

    # 进行形态学梯度操作
    out5 = cv2.morphologyEx(srcImage, cv2.MORPH_GRADIENT, element)

    # 进行形态学顶帽操作
    out6 = cv2.morphologyEx(srcImage, cv2.MORPH_TOPHAT, element)

    # 进行形态学黑帽操作
    out7 = cv2.morphologyEx(srcImage, cv2.MORPH_BLACKHAT, element)

    # 进行形态学击中击不中操作
    des = cv2.cvtColor(srcImage, cv2.COLOR_BGR2GRAY)
    out8 = cv2.morphologyEx(des, cv2.MORPH_HITMISS, element)

    # 显示效果图
    font = cv2.FONT_HERSHEY_COMPLEX  # 设置字体
    out1 = cv2.putText(out1, "Python", (10, 30), font, 1, (0, 0, 255), 2)
    out2 = cv2.putText(out2, "Python", (10, 30), font, 1, (0, 0, 255), 2)
    out3 = cv2.putText(out3, "Python", (10, 30), font, 1, (0, 0, 255), 2)
    out4 = cv2.putText(out4, "Python", (10, 30), font, 1, (0, 0, 255), 2)
    out5 = cv2.putText(out5, "Python", (10, 30), font, 1, (0, 0, 255), 2)
    out6 = cv2.putText(out6, "Python", (10, 30), font, 1, (0, 0, 255), 2)
    out7 = cv2.putText(out7, "Python", (10, 30), font, 1, (0, 0, 255), 2)
    out8 = cv2.putText(out8, "Python", (10, 30), font, 1, (0, 0, 255), 2)

    cv2.imshow('MORPH_ERODE', out1)
    cv2.imshow('MORPH_DILATE', out2)
    cv2.imshow('MORPH_OPEN', out3)
    cv2.imshow('MORPH_CLOSE', out4)
    cv2.imshow('MORPH_GRADIENT', out5)
    cv2.imshow('MORPH_TOPHAT', out6)
    cv2.imshow('MORPH_BLACKHAT', out7)
    cv2.imshow('MORPH_HITMISS', out8)

    cv2.waitKey(0)
    cv2.destroyAllWindows()


if __name__ == '__main__':
    demo1()
    demo2()

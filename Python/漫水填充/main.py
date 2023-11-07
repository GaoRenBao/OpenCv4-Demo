# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：漫水填充
# 博客：http://www.bilibili996.com/Course?id=5744370000163
# 作者：高仁宝
# 时间：2023.11

import cv2
import numpy as np
import yaml
import time
import random

g_maskImage = np.zeros((0, 0), np.uint8)
g_dstImage = np.zeros((0, 0), np.uint8)
# 漫水填充的模式
g_nFillMode = 1
# 负差最大值、正差最大值
g_nLowDifference = 20
g_nUpDifference = 20
# 表示floodFill函数标识符低八位的连通值
g_nConnectivity = 4
# 是否为彩色图的标识符布尔值
g_bIsColor = True
# 是否显示掩膜窗口的布尔值
g_bUseMask = False
# 新的重新绘制的像素值
g_nNewMaskVal = 255


def ShowHelpText():
    # 输出一些帮助信息
    print("\n\n\t欢迎来到漫水填充示例程序~");
    print("\n\n\t本示例根据鼠标选取的点搜索图像中与之颜色相近的点，并用不同颜色标注。");
    print("\n\n\t按键操作说明: \n\n"
          "\t\t鼠标点击图中区域- 进行漫水填充操作\n"
          "\t\t键盘按键【ESC】- 退出程序\n"
          "\t\t键盘按键【1】-  切换彩色图/灰度图模式\n"
          "\t\t键盘按键【2】- 显示/隐藏掩膜窗口\n"
          "\t\t键盘按键【3】- 恢复原始图像\n"
          "\t\t键盘按键【4】- 使用空范围的漫水填充\n"
          "\t\t键盘按键【5】- 使用渐变、固定范围的漫水填充\n"
          "\t\t键盘按键【6】- 使用渐变、浮动范围的漫水填充\n"
          "\t\t键盘按键【7】- 操作标志符的低八位使用4位的连接模式\n"
          "\t\t键盘按键【8】- 操作标志符的低八位使用8位的连接模式\n\n");


# 创建回调函数
def onMouse(event, x, y, flags, param):
    global g_maskImage, g_dstImage

    if event != cv2.EVENT_LBUTTONDOWN:
        return

    # -------------------【<1>调用floodFill函数之前的参数准备部分】---------------
    seed = (x, y)

    # 空范围的漫水填充，此值设为0，否则设为全局的g_nLowDifference
    LowDifference = 0 if g_nFillMode == 0 else g_nLowDifference

    # 空范围的漫水填充，此值设为0，否则设为全局的g_nUpDifference
    UpDifference = 0 if g_nFillMode == 0 else g_nUpDifference

    # 标识符的0~7位为g_nConnectivity，8~15位为g_nNewMaskVal左移8位的值，16~23位为CV_FLOODFILL_FIXED_RANGE或者0。
    m = cv2.FLOODFILL_FIXED_RANGE if g_nFillMode == 1 else 0
    mflags = g_nConnectivity + (g_nNewMaskVal << 8) + m

    # 随机生成bgr值
    b = random.randint(0, 255)  # 随机返回一个0~255之间的值
    g = random.randint(0, 255)  # 随机返回一个0~255之间的值
    r = random.randint(0, 255)  # 随机返回一个0~255之间的值

    # 在重绘区域像素的新值，若是彩色图模式，取Scalar(b, g, r)；
    # 若是灰度图模式，取Scalar(r*0.299 + g*0.587 + b*0.114)
    newVal = (b, g, r) if g_bIsColor == True else (r * 0.299 + g * 0.587 + b * 0.114)

    # 目标图的赋值
    dst = g_dstImage if g_bIsColor == True else g_grayImage

    # --------------------【<2>正式调用floodFill函数】-----------------------------
    ret, g_maskImage = cv2.threshold(g_maskImage, 1, 128, cv2.THRESH_BINARY)
    area = cv2.floodFill(dst, g_maskImage, seed, newVal,
                         (LowDifference, LowDifference, LowDifference),
                         (UpDifference, UpDifference, UpDifference), mflags)

    if g_bUseMask == True:
        cv2.imshow('mask', g_maskImage)

    cv2.imshow('image', dst)
    print("(%d) 个像素被重绘" % (area[0]))


# 负差最大值
def nLowDifference(x):
    global g_nLowDifference
    g_nLowDifference = x


# 正差最大值
def nUpDifference(x):
    global g_nUpDifference
    g_nUpDifference = x


# 载入原图
g_srcImage = cv2.imread('../images/girl3.jpg')

# 显示帮助文字
ShowHelpText()

# 利用image0的尺寸来初始化掩膜mask
rows, cols, channels = g_srcImage.shape

# 拷贝源图到目标图
g_dstImage = np.zeros((rows, cols, 3), np.uint8)
np.copyto(g_dstImage, g_srcImage)

# 转换三通道的image0到灰度图
g_grayImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)

# 不知道是不是版本问题，CreateImage和CreateMat在我这里都用不了
# g_maskImage = cv2.CreateImage((rows + 2, cols + 2), cv2.IPL_DEPTH_8U, 1)
# g_maskImage = cv2.CreateMat(rows + 2, cols + 2, cv2.CV_8UC1)
g_maskImage = np.zeros((rows + 2, cols + 2), np.uint8)

# 创建窗口
cv2.namedWindow('image')
# 显示原图
cv2.imshow('image', g_srcImage)

cv2.setMouseCallback('image', onMouse)
cv2.createTrackbar('nLow', 'image', 0, 255, nLowDifference)
cv2.createTrackbar('nUp', 'image', 0, 255, nUpDifference)
cv2.setTrackbarPos('nLow', 'image', 15)
cv2.setTrackbarPos('nUp', 'image', 15)

while (True):
    # 先显示效果图
    img = g_dstImage if g_bIsColor == True else g_grayImage
    cv2.imshow('image', img)
    c = cv2.waitKey(0)

    # 判断ESC是否按下，若按下便退出
    if c == 27:
        print("程序退出...........")
        break

    # 如果键盘“1”被按下，效果图在在灰度图，彩色图之间互换
    if c == 49:
        if g_bIsColor == True:  # 若原来为彩色，转为灰度图，并且将掩膜mask所有元素设置为0
            print("键盘“1”被按下，切换彩色/灰度模式，当前操作为将【彩色模式】切换为【灰度模式】.")
            g_grayImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)
            # 将mask所有元素设置为0
            g_maskImage = np.zeros((rows + 2, cols + 2), np.uint8)
            # 将标识符置为false，表示当前图像不为彩色，而是灰度
            g_bIsColor = False
        else:  # 若原来为灰度图，便将原来的彩图image0再次拷贝给image，并且将掩膜mask所有元素设置为0
            print("键盘“1”被按下，切换彩色/灰度模式，当前操作为将【彩色模式】切换为【灰度模式】")
            np.copyto(g_dstImage, g_srcImage)
            g_maskImage = np.zeros((rows + 2, cols + 2), np.uint8)
            g_bIsColor = True  # 将标识符置为true，表示当前图像模式为彩色

    # 如果键盘按键“2”被按下，显示/隐藏掩膜窗口
    if c == 50:
        if g_bUseMask:
            cv2.destroyWindow("mask")
            g_bUseMask = False
        else:
            cv2.namedWindow("mask", 0)
            g_maskImage = np.zeros((rows + 2, cols + 2), np.uint8)
            cv2.imshow("mask", g_maskImage)
            g_bUseMask = True;

    # 如果键盘按键“3”被按下，恢复原始图像
    if c == 51:
        print("按键“3”被按下，恢复原始图像")
        np.copyto(g_dstImage, g_srcImage)
        g_grayImage = cv2.cvtColor(g_srcImage, cv2.COLOR_BGR2GRAY)
        g_maskImage = np.zeros((rows + 2, cols + 2), np.uint8)

    # 如果键盘按键“4”被按下，使用空范围的漫水填充
    if c == 52:
        print("按键“4”被按下，使用空范围的漫水填充")
        g_nFillMode = 0

    # 如果键盘按键“5”被按下，使用渐变、固定范围的漫水填充
    if c == 53:
        print("按键“5”被按下，使用渐变、固定范围的漫水填充")
        g_nFillMode = 1

    # 如果键盘按键“6”被按下，使用渐变、浮动范围的漫水填充
    if c == 54:
        print("按键“6”被按下，使用渐变、浮动范围的漫水填充")
        g_nFillMode = 2

    # 如果键盘按键“7”被按下，操作标志符的低八位使用4位的连接模式
    if c == 55:
        print("按键“7”被按下，操作标志符的低八位使用4位的连接模式")
        g_nConnectivity = 4

    # 如果键盘按键“8”被按下，操作标志符的低八位使用8位的连接模式
    if c == 56:
        print("按键“8”被按下，操作标志符的低八位使用8位的连接模式")
        g_nConnectivity = 8

cv2.destroyAllWindows()
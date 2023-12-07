# OpenCv版本：opencv-python 4.6.0.66
# 内容：用OpenCV进行基本绘图
# 博客：http://www.bilibili996.com/Course?id=5121811000097
# 作者：高仁宝
# 时间：2023.11

import numpy as np
import cv2

WINDOW_NAME1 = "image1"  # 为窗口标题定义的宏
WINDOW_NAME2 = "image2"  # 为窗口标题定义的宏
WINDOW_WIDTH = 600  # 定义窗口大小的宏


# 自定义的绘制函数，实现了绘制不同角度、相同尺寸的椭圆
def DrawEllipse(img, angle):
    # 这里一定要转换成int，不然ellipse会提示：ellipse() takes at most 5 arguments (9 given)
    center = (int(WINDOW_WIDTH / 2), int(WINDOW_WIDTH / 2))  # 中心点位置
    axes = (int(WINDOW_WIDTH / 4), int(WINDOW_WIDTH / 16))  # 长轴半径、短轴半径
    cv2.ellipse(img, center, axes, angle, 0, 360, (255, 129, 0), thickness=2, lineType=8)


# 自定义的绘制函数，实现了实心圆的绘制
def DrawFilledCircle(img, center):
    img = cv2.circle(img, (int(center[0]), int(center[1])), int(WINDOW_WIDTH / 32), (0, 0, 255), -1, 8, 0)


# 自定义的绘制函数，实现了凹多边形的绘制
def DrawPolygon(img):
    # 创建一些点
    pts = np.array([
        [WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8],
        [3 * WINDOW_WIDTH / 4, 7 * WINDOW_WIDTH / 8],
        [3 * WINDOW_WIDTH / 4, 13 * WINDOW_WIDTH / 16],
        [11 * WINDOW_WIDTH / 16, 13 * WINDOW_WIDTH / 16],
        [19 * WINDOW_WIDTH / 32, 3 * WINDOW_WIDTH / 8],
        [3 * WINDOW_WIDTH / 4, 3 * WINDOW_WIDTH / 8],
        [3 * WINDOW_WIDTH / 4, WINDOW_WIDTH / 8],
        [26 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8],
        [26 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4],
        [22 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4],
        [22 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8],
        [18 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8],
        [18 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4],
        [14 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 4],
        [14 * WINDOW_WIDTH / 40, WINDOW_WIDTH / 8],
        [WINDOW_WIDTH / 4, WINDOW_WIDTH / 8],
        [WINDOW_WIDTH / 4, 3 * WINDOW_WIDTH / 8],
        [13 * WINDOW_WIDTH / 32, 3 * WINDOW_WIDTH / 8],
        [5 * WINDOW_WIDTH / 16, 13 * WINDOW_WIDTH / 16],
        [WINDOW_WIDTH / 4, 13 * WINDOW_WIDTH / 16]
    ], np.int32)
    cv2.fillPoly(img, [pts], (255, 255, 255), 8)


# 自定义的绘制函数，实现了线的绘制
def DrawLine(img, start, end):
    cv2.line(img, start, end, (0, 0, 0), 2, 8)


# 创建空白的Mat图像
atomImage = np.zeros((WINDOW_WIDTH, WINDOW_WIDTH, 3), np.uint8)
rookImage = np.zeros((WINDOW_WIDTH, WINDOW_WIDTH, 3), np.uint8)

# --------------<1>绘制化学中的原子示例图--------------
# 先绘制出椭圆
DrawEllipse(atomImage, 90)
DrawEllipse(atomImage, 0)
DrawEllipse(atomImage, 45)
DrawEllipse(atomImage, -45)

# 再绘制圆心
DrawFilledCircle(atomImage, (WINDOW_WIDTH / 2, WINDOW_WIDTH / 2))

# --------------<2>绘制组合图--------------
# 先绘制出椭圆
DrawPolygon(rookImage)

# 绘制矩形
cv2.rectangle(rookImage,
              (0, int(7 * WINDOW_WIDTH / 8)), (WINDOW_WIDTH, WINDOW_WIDTH),
              (0, 255, 255), -1, 8)

# 绘制一些线段
DrawLine(rookImage, (0, int(15 * WINDOW_WIDTH / 16)), (WINDOW_WIDTH, int(15 * WINDOW_WIDTH / 16)))
DrawLine(rookImage, (int(WINDOW_WIDTH / 4), int(7 * WINDOW_WIDTH / 8)), (int(WINDOW_WIDTH / 4), WINDOW_WIDTH))
DrawLine(rookImage, (int(WINDOW_WIDTH / 2), int(7 * WINDOW_WIDTH / 8)), (int(WINDOW_WIDTH / 2), WINDOW_WIDTH))
DrawLine(rookImage, (int(3 * WINDOW_WIDTH / 4), int(7 * WINDOW_WIDTH / 8)), (int(3 * WINDOW_WIDTH / 4), WINDOW_WIDTH))

# 显示绘制出的图像
cv2.imshow(WINDOW_NAME1, atomImage)
cv2.moveWindow(WINDOW_NAME1, 0, 200)
cv2.imshow(WINDOW_NAME2, rookImage)
cv2.moveWindow(WINDOW_NAME2, WINDOW_WIDTH, 200)
cv2.waitKey(0);

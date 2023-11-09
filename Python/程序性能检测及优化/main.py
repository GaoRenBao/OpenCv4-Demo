# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：程序性能检测及优化
# 博客：http://www.bilibili996.com/Course?id=2284309000315
# 作者：高仁宝
# 时间：2023.11

import cv2

"""
# 通过使用opencv中的getTickCount方法，来计算程序运行时间
"""


def demo1():
    img1 = cv2.imread('../images/ml.png')

    e1 = cv2.getTickCount()

    for i in range(5, 49, 2):
        img1 = cv2.medianBlur(img1, i)

    e2 = cv2.getTickCount()
    t = (e2 - e1) / cv2.getTickFrequency()  # 时钟频率 或者 每秒钟的时钟数
    print(t)


"""
OpenCV中的默认优化在编译时优化是默认开启的。
因此OpenCV的就是优化后的代码，如果你把优化关闭了，就只能执行低效的代码了。
你可以使用函数cv2.useOptimized() 来查看优化是否开启了，使用函数cv2.setUseOptimized()来开启优化。
"""


def demo2():
    print(cv2.useOptimized())
    cv2.setUseOptimized(False)
    print(cv2.useOptimized())


if __name__ == '__main__':
    demo1()
    demo2()

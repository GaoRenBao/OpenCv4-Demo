# -*- coding: utf-8 -*-
import cv2
import numpy as np
#拆分及合并图像通道

img=cv2.imread('messi5.jpg')

#
b,g,r=cv2.split(img)#比较耗时的操作，请使用numpy 索引
img=cv2.merge((b,g,r))
#获取三通道颜色
b=img[:,:,0]
g=img[:,:,1]
r=img[:,:,2]

#使所有像素的红色通道值都为 0,你不必先拆分再赋值。
# 你可以 直接使用 Numpy 索引,这会更快。
img[:,:,0]=0

#保存到文件，看下效果
cv2.imshow('PythonApplication1.jpg',img)

cv2.waitKey(0)



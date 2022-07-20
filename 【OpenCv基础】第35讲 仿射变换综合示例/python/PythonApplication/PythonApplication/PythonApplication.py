import cv2
import numpy as np

#【1】载入原始图
srcImage = cv2.imread("1.jpg")
cv2.imshow("srcImage", srcImage)

#【2】设置目标图像的大小和类型与源图像一致
dstImage_warp = np.ones(srcImage.shape,srcImage.dtype)

#【3】设置源图像和目标图像上的三组点以计算仿射变换
srcTriangle = np.float32([
    [0,0],
    [srcImage.shape[1] - 1,0],
    [0,srcImage.shape[0] - 1]
    ])

dstTriangle = np.float32([
    [srcImage.shape[1] * 0.0,srcImage.shape[0] * 0.33],
    [srcImage.shape[1] * 0.65,srcImage.shape[0] * 0.35],
    [srcImage.shape[1] * 0.15,srcImage.shape[0] * 0.6]
    ])

#【4】求得仿射变换
warpMat = cv2.getAffineTransform(srcTriangle,dstTriangle)

#【5】对源图像应用刚刚求得的仿射变换
dstImage_warp = cv2.warpAffine(srcImage,warpMat,(srcImage.shape[1],srcImage.shape[0]))

#【6】对图像进行缩放后再旋转
# 计算绕图像中点顺时针旋转50度缩放因子为0.6的旋转矩阵
center = (dstImage_warp.shape[1] / 2, dstImage_warp.shape[0] / 2)
angle = -50.0
scale = 0.6

# 通过上面的旋转细节信息求得旋转矩阵
rotMat = cv2.getRotationMatrix2D(center, angle, scale)
# 旋转已缩放后的图像
dstImage_warp_rotate = cv2.warpAffine(dstImage_warp, rotMat, (dstImage_warp.shape[1],dstImage_warp.shape[0]))

#【7】显示效果图
cv2.imshow('dstImage_warp',dstImage_warp)
cv2.imshow('dstImage_warp_rotate',dstImage_warp_rotate)

cv2.waitKey(0)
cv2.destroyAllWindows()

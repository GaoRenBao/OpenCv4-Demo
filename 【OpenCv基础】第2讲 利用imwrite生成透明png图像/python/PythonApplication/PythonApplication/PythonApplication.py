import cv2
import numpy as np

def createAlphaMat(mat):
    imgH = mat.shape[0]
    imgW = mat.shape[1]
    for w in range(imgW):
        for h in range(imgH):
            b=0xff
            g=(imgH-h)/imgH*0xff
            r=(imgW-w)/imgW*0xff
            a=0.5*(g+r)
            mat[h, w] = [b, g, r, a]
    return mat

if __name__ == "__main__":
    # 创建一个4通道的图片
    mat = np.ones((480, 640, 4),np.uint8) * 255
    # 设置颜色
    mat = createAlphaMat(mat)
    # 显示图片
    cv2.imshow("透明Alpha值图.png", mat)

    # 输出图片
    # cv2.imwrite("透明Alpha值图.png", mat)

    # 解决输出图片中文乱码问题
    cv2.imencode('.png', mat)[1].tofile("透明Alpha值图.png") 

    # 等待任意输入
    cv2.waitKey(0)


# 需要安装 pip install Scikit-Image

# 老版本
#from skimage.measure import compare_ssim as ssim
#from skimage.measure import compare_mse as mse

# 新版本
from skimage.metrics import structural_similarity as ssim
from skimage.metrics import mean_squared_error as mse

import matplotlib.pyplot as plt
import numpy as np
import cv2

# 双摄像头调用
def demo1():
    cap0 = cv2.VideoCapture(0)
    cap1 = cv2.VideoCapture(1)
    ret = cap0.set(3, 320)
    ret = cap0.set(4, 240)
    ret = cap1.set(3, 320)
    ret = cap1.set(4, 240)

    while cap0.isOpened() and cap1.isOpened():
        ret0, frame0 = cap0.read()
        ret1, frame1 = cap1.read()

        if ret0:
            cv2.imshow('frame0', frame0)
            cv2.setWindowTitle('frame0','On Top')
        if ret1:
            cv2.imshow('frame1', frame1)
            # cv2.moveWindow('frame1', x=frame0.shape[1], y=0)
            cv2.moveWindow('frame1', x=320, y=40)

        key = cv2.waitKey(delay=2)
        if key == ord("q"):
            break

    # When everything done, release the capture
    cap0.release()
    cap1.release()
    cv2.destroyAllWindows()

# mse 算法
def my_mse(imageA, imageB):
    # the 'Mean Squared Error' between the two images is the
    # sum of the squared difference between the two images;
    # NOTE: the two images must have the same dimension
    err = np.sum((imageA.astype("float") - imageB.astype("float")) * 2)
    err /= float(imageA.shape[0] * imageA.shape[1])

    # return the MSE, the lower the error, the more "similar"
    # the two images are
    return err

# 图像相似度计算（感觉不咋滴）
def demo2():
    cap = cv2.VideoCapture(0)
    if (cap.isOpened() == False):
        print('Open Camera Error.')
        return

    ret = cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 640)
    ret = cap.set(cv2.CAP_PROP_FRAME_WIDTH, 480)
    title='camera compare'
    plt.ion()

    ret, frame = cap.read()
    if ret==True:
        temp = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        cv2.imshow("temp",temp)
        while cap.isOpened():
            ret, frame = cap.read()
            if ret==True:
                gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
                cv2.imshow("gray",gray)
             
                m = mse(temp, gray)   #均方误差（MSE）
                s = ssim(temp, gray)  #结构相似度指数（SSIM）
                print("MSE: %.2f, SSIM: %.2f" % (m, s))
                temp = gray.copy()
                cv2.waitKey(500)

# 运行demo2，如果想运行其他demo，改一下这个即可
demo2()
# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：使用SVM进行-手写数据OCR
# 博客：http://www.bilibili996.com/Course?id=17494cb1802e403ea25d1096a5659478
# 作者：高仁宝
# 时间：2023.11

# PDF中的代码无法运行。。。
# 参考博客：https://blog.csdn.net/JJSXBL/article/details/127924492

import cv2 as cv
import numpy as np

SZ = 20
bin_n = 16  # Number of bins
affine_flags = cv.WARP_INVERSE_MAP | cv.INTER_LINEAR


# 这个函数用来给手写体做一个变形，变成正体
def deskew(img):
    m = cv.moments(img)
    if abs(m['mu02']) < 1e-2:
        return img.copy()
    skew = m['mu11'] / m['mu02']
    M = np.float32([[1, skew, -0.5 * SZ * skew], [0, 1, 0]])
    img = cv.warpAffine(img, M, (SZ, SZ), flags=affine_flags)
    return img


# 这个函数把图片分成四部分，分别成生成方向直方图，然后返回，用来作为SVM的输入，这点可K近邻算法是不一样的
def hog(img):
    gx = cv.Sobel(img, cv.CV_32F, 1, 0)
    gy = cv.Sobel(img, cv.CV_32F, 0, 1)
    mag, ang = cv.cartToPolar(gx, gy)
    bins = np.int32(bin_n * ang / (2 * np.pi))  # quantizing binvalues in (0...16)
    bin_cells = bins[:10, :10], bins[10:, :10], bins[:10, 10:], bins[10:, 10:]
    mag_cells = mag[:10, :10], mag[10:, :10], mag[:10, 10:], mag[10:, 10:]
    hists = [np.bincount(b.ravel(), m.ravel(), bin_n) for b, m in zip(bin_cells, mag_cells)]
    hist = np.hstack(hists)  # hist is a 64 bit vector
    return hist


# 样本读取
img = cv.imread(cv.samples.findFile('../images/digits.png'), 0)
if img is None:
    raise Exception("we need the digits.png image from samples/data here !")

# 把整张图片分成单独的图片，原数据是50行，100列
cells = [np.hsplit(row, 100) for row in np.vsplit(img, 50)]
# First half is trainData, remaining is testData
train_cells = [i[:50] for i in cells]
test_cells = [i[50:] for i in cells]

######################## Now training ########################

# 使用deskew函数，把倾斜的字体矫正
deskewed = [list(map(deskew, row)) for row in train_cells]
# 生成图像的HOG图
hogdata = [list(map(hog, row)) for row in deskewed]
# 序列化，变成输入数据
trainData = np.float32(hogdata).reshape(-1, 64)
# 把与图片对应的数字生成出来
responses = np.repeat(np.arange(10), 250)[:, np.newaxis]

# 实例化对象
svm = cv.ml.SVM_create()
# 设置核函数
svm.setKernel(cv.ml.SVM_LINEAR)
# 设置训练类型
svm.setType(cv.ml.SVM_C_SVC)
svm.setC(2.67)
svm.setGamma(5.383)

# 输入样本，进行训练
svm.train(trainData, cv.ml.ROW_SAMPLE, responses)
# 存储训练结果
svm.save('svm_data.dat')

######     Now testing      ########################

# 对检验样本的手写体进行矫正
deskewed = [list(map(deskew, row)) for row in test_cells]
# 生成检验样本的HOG图
hogdata = [list(map(hog, row)) for row in deskewed]
# 检验样本序列化
testData = np.float32(hogdata).reshape(-1, bin_n * 4)
# 生成识别结果
result = svm.predict(testData)[1]

#######   Check Accuracy   ########################
mask = result == responses
correct = np.count_nonzero(mask)
print(correct * 100.0 / result.size)
# 输出：93.8

# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：删除图像中的水印
# 博客：http://www.bilibili996.com/Course?id=b608bdd358e041478931886b8b08058c
# 作者：高仁宝
# 时间：2023.11
#
# 来源：https://stackoverflow.com/questions/32125281/removing-watermark-out-of-an-image-using-opencv


# Import the necessary packages
import cv2
import numpy as np


def back_rm(filename):
    # Load the image
    img = cv2.imread(filename)

    # Convert the image to grayscale
    gr = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

    # Make a copy of the grayscale image
    bg = gr.copy()

    # Apply morphological transformations
    for i in range(5):
        kernel2 = cv2.getStructuringElement(cv2.MORPH_ELLIPSE,
                                            (2 * i + 1, 2 * i + 1))
        bg = cv2.morphologyEx(bg, cv2.MORPH_CLOSE, kernel2)
        bg = cv2.morphologyEx(bg, cv2.MORPH_OPEN, kernel2)

    # Subtract the grayscale image from its processed copy
    dif = cv2.subtract(bg, gr)

    # Apply thresholding
    bw = cv2.threshold(dif, 0, 255, cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)[1]
    dark = cv2.threshold(bg, 0, 255, cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)[1]

    # Extract pixels in the dark region
    darkpix = gr[np.where(dark > 0)]

    # Threshold the dark region to get the darker pixels inside it
    darkpix = cv2.threshold(darkpix, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)[1]

    # Paste the extracted darker pixels in the watermark region
    bw[np.where(dark > 0)] = darkpix.T

    bw = cv2.resize(bw, (int(bw.shape[1] * 0.5), int(bw.shape[0] * 0.5)), interpolation=cv2.INTER_NEAREST)
    cv2.imshow('final.jpg', bw)
    cv2.waitKey(0)


back_rm('../images/YZeOg.jpg')

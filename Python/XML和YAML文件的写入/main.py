# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：XML和YAML文件的写入
# 博客：http://www.bilibili996.com/Course?id=5150084000100
# 作者：高仁宝
# 时间：2023.11

# pip install pyyaml -i https://mirror.baidu.com/pypi/simple

import numpy as np
import yaml
import time
import random

def setMat(mat):
    imgH = mat.shape[0]
    imgW = mat.shape[1]
    for w in range(imgW):
        for h in range(imgH):
            b = 0xff
            g = (imgH - h) / imgH * 0xff
            r = (imgW - w) / imgW * 0xff
            a = 0.5 * (g + r)
            mat[h, w] = [b, g, r, a]


cameraMatrix = np.ones((3, 3, 4), np.uint8) * 255
setMat(cameraMatrix)

with open("test.yaml", "w", encoding="utf-8") as f:
    x = random.randint(0, 255) % 640
    y = random.randint(0, 255) % 480
    lbp = random.randint(0, 255) % 256

    aproject = {'frameCount': 5,
                'calibrationDate': time.time(),
                'cameraMatrix': cameraMatrix,
                'features': {'x': x, 'y': y, 'lbp': lbp}
                }
    yaml.dump(aproject, f)

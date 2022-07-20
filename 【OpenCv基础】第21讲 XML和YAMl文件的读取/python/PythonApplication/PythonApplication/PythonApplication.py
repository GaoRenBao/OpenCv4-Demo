import cv2
import numpy as np
import yaml
import time
import random

fs = open('test.yaml','r',encoding='utf-8')
cont = fs.read()
#print(cont)

#load的使用：https://github.com/yaml/pyyaml/wiki/PyYAML-yaml.load(input)-Deprecation
x=yaml.load(cont, Loader=yaml.BaseLoader)

print(x['calibrationDate'])
print(x['cameraMatrix']['args'])
print(x['features']['x'])
print(x['features']['y'])
print(x['features']['lbp'])
print(x['frameCount'])



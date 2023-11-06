# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：XML和YAML文件的读取
# 博客：http://www.bilibili996.com/Course?id=5157334000101
# 作者：高仁宝
# 时间：2023.11

# pip install pyyaml -i https://mirror.baidu.com/pypi/simple

import yaml

fs = open('test.yaml', 'r', encoding='utf-8')
cont = fs.read()
# print(cont)

# load的使用：https://github.com/yaml/pyyaml/wiki/PyYAML-yaml.load(input)-Deprecation
x = yaml.load(cont, Loader=yaml.BaseLoader)
print(x['calibrationDate'])
print(x['cameraMatrix']['args'])
print(x['features']['x'])
print(x['features']['y'])
print(x['features']['lbp'])
print(x['frameCount'])

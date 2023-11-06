# OpenCv版本 OpenCvSharp4.6.0.66
# 内容：绘图函数
# 博客：http://www.bilibili996.com/Course?id=2699750000249
# 作者：高仁宝
# 时间：2023.11


import cv2
import numpy as np
import math
import ft2

"""
利用随机数画不懂尺寸和颜色的圆形
"""


def demo1():
    def click_event(event, x, y, flags, param):
        '''
        用左键点击屏幕，打印坐标
        :param event:
        :param x:
        :param y:
        :param flags:
        :param param:
        :return:
        '''
        if event == cv2.EVENT_LBUTTONDOWN:
            print(x, y, flags, param)

    cv2.namedWindow('Canvas', cv2.WINDOW_GUI_EXPANDED)
    cv2.setMouseCallback("Canvas", click_event)

    canvas = np.zeros((300, 300, 3), dtype="uint8")
    while True:
        try:
            for i in range(0, 25):
                radius = np.random.randint(5, high=200)
                color = np.random.randint(0, high=256, size=(3,)).tolist()
                pt = np.random.randint(0, high=300, size=(2,))
                cv2.circle(canvas, tuple(pt), radius, color, -1)

            cv2.imshow("Canvas", canvas)

            key = cv2.waitKey(1000)  # 等待1秒
            if key == ord('q'):
                break
            else:
                # sleep(1)
                continue
        except KeyboardInterrupt as e:
            print('KeyboardInterrupt', e)
        finally:
            cv2.imwrite('random-circles2.jpg', canvas)


"""
利用freetype-py库显示中文字体
pip install freetype-py ft2 -i https://mirror.baidu.com/pypi/simple
https://fireant.github.io/misc/2017/01/28/ttf-opencv.html
"""


def demo2():
    img = np.zeros((100, 300, 3), dtype=np.uint8)
    line = '你好中文'
    color = (0, 255, 0)  # Green
    pos = (3, 3)
    text_size = 24
    # ft = put_chinese_text('wqy-zenhei.ttc')
    ft = ft2.put_chinese_text('msyh.ttf')
    image = ft.draw_text(img, pos, line, text_size, color)
    name = u'图片展示'
    cv2.imshow(name, image)
    cv2.imwrite("out.jpg", image)
    cv2.waitKey(0)


"""
绘制OpenCv Logo
"""


def demo3():
    r1 = 70
    r2 = 30

    ang = 60

    d = 170
    h = int(d / 2 * math.sqrt(3))

    dot_red = (256, 128)
    dot_green = (int(dot_red[0] - d / 2), dot_red[1] + h)
    dot_blue = (int(dot_red[0] + d / 2), dot_red[1] + h)

    # tan = float(dot_red[0]-dot_green[0])/(dot_green[1]-dot_red[0])
    # ang = math.atan(tan)/math.pi*180

    red = (0, 0, 255)
    green = (0, 255, 0)
    blue = (255, 0, 0)
    black = (0, 0, 0)

    full = -1

    img = np.zeros((512, 512, 3), np.uint8)
    # img = np.ones((512, 512, 3), np.uint8)

    cv2.circle(img, dot_red, r1, red, full)
    cv2.circle(img, dot_green, r1, green, full)
    cv2.circle(img, dot_blue, r1, blue, full)
    cv2.circle(img, dot_red, r2, black, full)
    cv2.circle(img, dot_green, r2, black, full)
    cv2.circle(img, dot_blue, r2, black, full)

    cv2.ellipse(img, dot_red, (r1, r1), ang, 0, ang, black, full)
    cv2.ellipse(img, dot_green, (r1, r1), 360 - ang, 0, ang, black, full)
    cv2.ellipse(img, dot_blue, (r1, r1), 360 - 2 * ang, ang, 0, black, full)

    font = cv2.FONT_HERSHEY_SIMPLEX
    cv2.putText(img, text='OpenCV', org=(15, 450), fontFace=font, fontScale=4, color=(255, 255, 255),
                thickness=10)  # text,

    cv2.imshow("opencv_logo.jpg", img)
    cv2.waitKey(0)


"""
绘制任意图形
"""


def demo4():
    # Create a black image
    img = np.zeros((512, 512, 3), np.uint8)

    # Draw a diagonal blue line with thickness of 5 px
    cv2.line(img, pt1=(0, 0), pt2=(511, 511), color=(255, 0, 0), thickness=5)  # pt1, pt2, color, thickness=
    # cv2.polylines() 可以 用来画很多条线。只需要把想 画的线放在一 个列表中， 将 列表传给函数就可以了。每条线 会被独立绘制。 这会比用 cv2.line() 一条一条的绘制 要快一些。
    # cv2.polylines(img, pts, isClosed, color, thickness=None, lineType=None, shift=None)
    cv2.arrowedLine(img, pt1=(21, 13), pt2=(151, 401), color=(255, 0, 0), thickness=5)

    cv2.rectangle(img, (384, 0), (510, 128), (0, 255, 0), 3)

    cv2.circle(img, center=(447, 63), radius=63, color=(0, 0, 255),
               thickness=-1)  # center, radius, color, thickness=None

    # 一个参数是中心点的位置坐标。 下一个参数是长轴和短轴的长度。椭圆沿逆时针方向旋转的角度。
    # 椭圆弧演顺时针方向起始的角度和结束角度 如果是 0 很 360 就是整个椭圆
    cv2.ellipse(img, center=(256, 256), axes=(100, 50), angle=0, startAngle=0, endAngle=180, color=255,
                thickness=-1)  # center, axes, angle, startAngle, endAngle, color, thickness=

    pts = np.array([[10, 5], [20, 30], [70, 20], [50, 10]], np.int32)
    pts = pts.reshape((-1, 1, 2))
    # 这里 reshape 的第一个参数为-1, 表明这一维的长度是根据后面的维度的计算出来的。
    # 注意 如果第三个参数是 False 我们得到的多边形是不闭合的 ，首 尾不相  连 。

    font = cv2.FONT_HERSHEY_SIMPLEX
    # org :Bottom-left corner of the text string in the image.左下角
    # 或使用 bottomLeftOrigin=True,文字会上下颠倒
    cv2.putText(img, text='bottomLeftOrigin', org=(10, 400), fontFace=font, fontScale=1, color=(255, 255, 255),
                thickness=1, bottomLeftOrigin=True)  # text, org, fontFace, fontScale, color, thickness=
    cv2.putText(img, text='OpenCV', org=(10, 500), fontFace=font, fontScale=4, color=(255, 255, 255),
                thickness=2)  # text, org, fontFace, fontScale, color, thickness=

    # 所有的绘图函数的返回值都是 None ，所以不能使用 img = cv2.line(img,(0,0),(5

    winname = 'example'
    cv2.namedWindow(winname, 0)
    cv2.imshow(winname, img)

    cv2.imwrite("example.jpg", img)

    cv2.waitKey(0)
    cv2.destroyAllWindows()


if __name__ == '__main__':
    # demo1()
    # demo2()
    # demo3()
    demo4()

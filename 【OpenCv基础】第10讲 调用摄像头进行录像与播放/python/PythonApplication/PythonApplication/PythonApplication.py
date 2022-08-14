import cv2
import numpy as np

#####################[demo1 摄像头图像实时显示]#####################
def demo1():
    # 打开摄像头
    # 一般的笔本电脑有内置摄像头。所以参数就是 0。你可以设置成1或者其他的来择别的摄像头
    Cap = cv2.VideoCapture(0)
    # 判断视频是否打开
    if (Cap.isOpened() == False):
        print('Open Camera Error.')
    else:
        '''
        你可以使用函数 cap.get(propId) 来获得 的一些参数信息。   
        propId 可以是 0 到 18 之 的任何整数。
        其中的一些值可以使用 cap.set(propId,value) 来修改value就是你想置成的新值。
        例如 我可以使用 cap.get(3) cv2.CAP_PROP_FRAME_WIDTH和 cap.get(4) cv2.CAP_PROP_FRAME_HEIGHT来查看每一帧的宽和高。   
        默认情况下得到的值是 640X480。但是我可以使用 ret=cap.set(3,320) 和 ret=cap.set(4,240) 来把宽和高改成 320X240。
        '''
        # ret=cap.set(3,320)
        # ret=cap.set(4,240)
        # ret = cap.set(cv2.CAP_PROP_FRAME_WIDTH, 480)#避免计算量过大
        # ret = cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 270)#

        Cap.set(cv2.CAP_PROP_FRAME_WIDTH,640)  # 设置图像宽
        Cap.set(cv2.CAP_PROP_FRAME_HEIGHT,480) # 设置图像高
        Cap.set(cv2.CAP_PROP_EXPOSURE, -3)     # 设置曝光值
        # 读取设置的参数
        size = (int(Cap.get(cv2.CAP_PROP_FRAME_WIDTH)),int(Cap.get(cv2.CAP_PROP_FRAME_HEIGHT)))
        baog = int(Cap.get(cv2.CAP_PROP_EXPOSURE))
        # 输出参数
        print('摄像头设置，尺寸:' + str(size))
        print('摄像头设置，曝光:' + str(baog))

        # 读取图像
        # while (True):
        while Cap.isOpened():  # 检查是否成功初始化，否则就 使用函数 cap.open()
            # Capture frame-by-frame
            ret, frame = Cap.read()  # ret 返回一个布尔值 True/False
            # print('frame shape:',frame.shape)#(720, 1280, 3)

            frame = cv2.flip(frame, flipCode=1)  # 左右翻转,使用笔记本电脑摄像头才有用。
            # flipCode：翻转方向：1：水平翻转；0：垂直翻转；-1：水平垂直翻转

            # Our operations on the frame come here
            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            # Display the resulting frame
            cv2.imshow('frame', gray)
            cv2.setWindowTitle('frame', 'COLOR_BGR2GRAY')

            # Property=cv2.getWindowProperty('frame',0)#无用

            # if cv2.waitKey(1) & 0xFF == ord('q'):#不行
            #     break
            key = cv2.waitKey(delay=10)
            if key == ord("q"):
                break

        # When everything done, release the capture
        cap.release()
        cv2.destroyAllWindows()

#############################[demo2 录像]######################################
def demo2():
    # 打开摄像头
    Cap = cv2.VideoCapture(0)
    # 判断视频是否打开
    if (Cap.isOpened() == False):
        print('Open Camera Error.')
    else:
        Cap.set(cv2.CAP_PROP_FRAME_WIDTH,640)  # 设置图像宽
        Cap.set(cv2.CAP_PROP_FRAME_HEIGHT,480) # 设置图像高
        Cap.set(cv2.CAP_PROP_EXPOSURE, -3)     # 设置曝光值
        # 读取设置的参数
        size = (int(Cap.get(cv2.CAP_PROP_FRAME_WIDTH)),int(Cap.get(cv2.CAP_PROP_FRAME_HEIGHT)))
        baog = int(Cap.get(cv2.CAP_PROP_EXPOSURE))
        # 输出参数
        print('摄像头设置，尺寸:' + str(size))
        print('摄像头设置，曝光:' + str(baog))
        # 设置存储的录像文件格式
        
        # 定义编解码器并创建VideoWriter对象，可以是“MJPG”或者“XVID”
        fourcc = cv2.VideoWriter_fourcc(*'XVID')
        # Error: 'module' object has no attribute 'VideoWriter_fourcc'
        # fourcc=cv2.VideoWriter_fourcc('X', 'V', 'I', 'D')
        #jpeg,h263,'m', 'p', '4', 'v'

        myavi = cv2.VideoWriter("a.avi",fourcc,25.0, (640, 480), True)

        while(Cap.isOpened()):
            ret, frame = Cap.read()
            if ret==True:
                cv2.imshow('frame',frame)
                myavi.write(frame)
                if cv2.waitKey(10) & 0xFF == ord('q'):
                    break
            else:
                break
        Cap.release()
        myavi.release()
        cv2.destroyAllWindows()

############################[播放录像]####################################
def demo3():
    cap = cv2.VideoCapture('a.avi')
    # cap = cv2.VideoCapture('output.avi')
    # cap = cv2.VideoCapture('Minions_banana.mp4')

    # 帧率
    fps = cap.get(cv2.CAP_PROP_FPS)  # 25.0
    print("Frames per second using video.get(cv2.CAP_PROP_FPS) : {0}".format(fps))
    # 总共有多少帧
    num_frames = cap.get(cv2.CAP_PROP_FRAME_COUNT)
    print('共有', num_frames, '帧')
    #
    frame_height = cap.get(cv2.CAP_PROP_FRAME_HEIGHT)
    frame_width = cap.get(cv2.CAP_PROP_FRAME_WIDTH)
    print('高：', frame_height, '宽：', frame_width)

    FRAME_NOW = cap.get(cv2.CAP_PROP_POS_FRAMES)  # 第0帧
    print('当前帧数', FRAME_NOW)  # 当前帧数 0.0

    # 读取指定帧,对视频文件才有效，对摄像头无效？？
    frame_no = 10
    cap.set(1, frame_no)  # Where frame_no is the frame you want
    ret, frame = cap.read()  # Read the frame
    cv2.imshow('frame_no'+str(frame_no), frame)

    FRAME_NOW = cap.get(cv2.CAP_PROP_POS_FRAMES)
    print('当前帧数', FRAME_NOW)  # 当前帧数 122.0

    while cap.isOpened():
        # 读帧
        ret, frame = cap.read()
        if ret == False :
            break

        FRAME_NOW = cap.get(cv2.CAP_PROP_POS_FRAMES)  # 当前帧数
        print('当前帧数', FRAME_NOW)

        cv2.imshow('frame', frame)
        key = cv2.waitKey(int(fps))
        if key == ord("q"):
            break

    cap.release()
    cv2.destroyAllWindows()

#选择当前运行的demo
demo3();


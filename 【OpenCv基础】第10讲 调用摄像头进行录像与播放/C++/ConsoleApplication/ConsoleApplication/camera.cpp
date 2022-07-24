/* 摄像头操作的所有自定义类 */

#include "Camera.h"

/*
摄像头初始化
capid:摄像头id
rows:图像高度
cols:图像宽度
 */
bool Camera::CameraInit(int capid, int rows, int cols)
{
	if (capid >= MAXDEV)
		return false;
	Cap[capid].open(capid);
	if (!Cap[capid].isOpened()) 
		return false;
	Cap[capid].set(CAP_PROP_FRAME_WIDTH, cols);
	Cap[capid].set(CAP_PROP_FRAME_HEIGHT, rows);
	//Cap[capid].set(CAP_PROP_BRIGHTNESS, 1);  //亮度 1
	//Cap[capid].set(CAP_PROP_CONTRAST, 0);   //对比度 40
	//Cap[capid].set(CAP_PROP_SATURATION, 100); //饱和度 50
	//Cap[capid].set(CAP_PROP_HUE, 0);        //色调 50
	//Cap[capid].set(CAP_PROP_EXPOSURE, 0);    //曝光 50
	return true;
}

/*
获取一帧图像
capid:摄像头id
*/
Mat Camera::CameraImg(int capid)
{
	Mat Img;
	if (capid < MAXDEV)
		Cap[capid] >> Img;
	return Img;
}

/*
获取当前可用摄像头数目(仅OpenCv3支持)
*/
int Camera::RefreshCameraNum()
{
	CvCapture *m_Captrue;
	int i = 0;
	//10是和数字VideoCapture Cap[10]对应的，根据实际项目，设置最多可达到多少摄像头数目
	//for (i = 0; i < MAXDEV; i++)
	//{
	//	m_Captrue = cvCreateCameraCapture(i);
	//	if (m_Captrue == NULL) break;
	//	cvReleaseCapture(&m_Captrue);//一定要释放 否则程序进程不能完全退出
	//}
	return i;
}





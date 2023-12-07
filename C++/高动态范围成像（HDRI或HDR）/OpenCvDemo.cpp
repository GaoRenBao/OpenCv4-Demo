/*
OpenCv版本 opencv-4.5.5-vc14_vc15
内容：高动态范围成像（HDRI或HDR）
博客：http://www.bilibili996.com/Course?id=ccb32eb54dd748f9a4fc137544039d51
作者：高仁宝
时间：2023.11
*/

#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/photo.hpp"

using namespace cv;
using namespace std;

int main(int argc, char** argv)
{
    vector<Mat> img_list;
    img_list.push_back(imread("../images/tbs/1tl.jpg"));
    img_list.push_back(imread("../images/tbs/2tr.jpg"));
    img_list.push_back(imread("../images/tbs/3bl.jpg"));
    img_list.push_back(imread("../images/tbs/4br.jpg"));

    vector<float> exposure_times = { 15.0f, 2.5f, 0.25f, 0.0333f };

    Mat hdr_debvec, hdr_robertson;
    Ptr<MergeDebevec> merge_debvec = createMergeDebevec(); 
    merge_debvec->process(img_list, hdr_debvec, exposure_times);
    Ptr<MergeRobertson> merge_robertson = createMergeRobertson();
    merge_robertson->process(img_list, hdr_robertson, exposure_times);

    // Tonemap HDR image
    // 没有 createTonemapDurand 方法，只能用 createTonemapDrago 代替了
    Mat res_debvec;
    Ptr<TonemapDrago> tonemap1 = createTonemapDrago(2.2);
    tonemap1->process(hdr_debvec, res_debvec);

    Mat res_robertson;
    Ptr<TonemapDrago> tonemap2 = createTonemapDrago(1.3);
    tonemap2->process(hdr_robertson, res_robertson);

    // Exposure fusion using Mertens
    Mat res_mertens;
    Ptr<MergeMertens> merge_mertens = createMergeMertens();
    merge_mertens->process(img_list, res_mertens);

    imshow("ldr_debvec.jpg", res_debvec);
    imshow("ldr_robertson.jpg", res_robertson);
    imshow("fusion_mertens.jpg", res_mertens);
    waitKey(0);

    return 0;
}


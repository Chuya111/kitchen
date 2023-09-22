using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DD.Service
{
    public class Capture
    {

        public static void SaveBitmap(Canvas canvas, string filePath = "")
        {
            // 获取当前窗口对象
            //var window = Application.Current.MainWindow;

            // 创建RenderTargetBitmap对象
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            // 渲染画面
            renderTargetBitmap.Render(canvas);

            // 创建BitmapEncoder对象
            BitmapEncoder bitmapEncoder = new PngBitmapEncoder();

            // 添加BitmapFrame对象到BitmapEncoder
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            //var t = bitmapEncoder.Frames[0].GetType();
            //MessageBox.Show(t.ToString());


            // 拼接完整的文件保存路径
            //string filePath = Path.Combine(@"C:\Users\Desktop", fileName);
            if (string.IsNullOrEmpty(filePath))
            {
                var fileName = $"screenshot_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png";
                filePath = Path.Combine(Service.Config.LayoutImageSavePath, fileName);

            }
            // 保存BitmapEncoder到文件
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                bitmapEncoder.Save(fileStream);
            }
        }

        public static void Send()
        {

            string path = "D:\\00_Work\\AI\\04_StableDiffusion\\test\\00011-128127637-Rendering floor plan of the apartment layout,top view,white background,masterpiece, best quality, extremely detailed,best illust2.jpg";
            Bitmap bm = new Bitmap(path);
            var content = ImageProcess.ImgToBase64String(bm);
            DiffusionData data = new DiffusionData(content);
            string input = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            //MessageBox.Show(input);

            //string url = "http://10.88.116.231:7861/controlnet/txt2img";
            //string url = "http://10.88.116.103:7861/controlnet/txt2img";
            //string test_url = "http://127.0.0.1:8000/";
            string url = "http://127.0.0.1:7860/sdapi/v1/txt2img";
            //var response = MyRequest.PostRequest(test_url, input);

            var response = MyRequest.PostRequest(url, input);
            var dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(response);
            var arr = dict["images"] as JArray;
            var ret = arr.Select(x => x.Value<string>()).ToList();

            System.Drawing.Bitmap response_bmp = ImageProcess.Base64StringToImage(ret[0]);
            response_bmp.Save("C:\\Users\\user\\Desktop\\test.png");

        }

    }
}

using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DD.Service
{
    internal class ImageProcess
    {
        public static string ImgToBase64String(System.Drawing.Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] arr = new byte[ms.Length];

                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();

                return Convert.ToBase64String(arr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }


        public static System.Drawing.Bitmap Base64StringToImage(string inputStr)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);

                ms.Close();
                return bmp;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public static BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

    }
}

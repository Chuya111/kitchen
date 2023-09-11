using System.IO;
using System.Net;
using System.Text;

namespace DD.Service
{
    public class MyRequest
    {
        public static string PostRequest(string url, string data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            byte[] dataArray = Encoding.UTF8.GetBytes(data);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = dataArray.Length;
            //request.Headers.Add("data", data);

            #region
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(dataArray, 0, dataArray.Length);
                stream.Close();
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        public static string GetRequest(string url)
        {

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            request.Method = "GET";

            using (WebResponse er = request.GetResponse())
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                string json = reader.ReadToEnd();

                return json;

            }
        }

        //GPT POST
        public static string PostRequest(string url, string data, string Key)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            byte[] dataArray = Encoding.UTF8.GetBytes(data);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Bearer " + Key;

            request.ContentLength = dataArray.Length;
            //request.Headers.Add("data", data);

            #region
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(dataArray, 0, dataArray.Length);
                stream.Close();
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
    }

}

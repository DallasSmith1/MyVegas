using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MyVegas
{
    internal class Screenshot
    {
        #region Custom Vision
        private static string predictionEndpoint = "https://myvegascomputervision-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/c50f50b3-23df-4196-a783-84eeb1816eaa/detect/iterations/Version%207/image";
        private static string predictionKey = "bc98a39197a64edfbb0c4e24b51590c2";
        #endregion

        string ImageDestination;

        public List<Object> objects = new List<Object>();

        public Screenshot()
        {

            Bitmap captureBitmap = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            Rectangle captureRectangle = Screen.PrimaryScreen.Bounds;
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            string fileName = "../../screenshots/" +
                     DateTime.Now.ToString("(dd_MMMM_hh_mm_ss_tt)") + ".png";

            captureBitmap.Save(fileName);

            ImageDestination = fileName;       
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fileStream);
            return reader.ReadBytes((int)fileStream.Length);
        }

        public void ReadImage()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

            string url = predictionEndpoint;

            byte[] data = GetImageAsByteArray(ImageDestination);

            using (var content = new ByteArrayContent(data))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                string str = response.Content.ReadAsStringAsync().Result;

                var strContent = JsonConvert.DeserializeObject<ResponseModel>(str);


                foreach ( var prediction in strContent.Predictions )
                {
                    objects.Add(new Object(prediction.TagName, Double.Parse(prediction.Probability), prediction.CustomBox));
                }
            }

            SortObjects();
        }

        private void SortObjects()
        {
            objects.OrderByDescending(obj => obj.Probability).ToList();
        }
    }
}

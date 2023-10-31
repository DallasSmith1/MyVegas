using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVegas
{
    public class CustomBox
    {
        [JsonProperty("left")]
        public double Left { get; set; }

        [JsonProperty("top")]
        public double Top { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }
    }


    public class Prediction
    {
        [JsonProperty("tagId")]
        public string TagID { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }

        [JsonProperty("probability")]
        public string Probability { get; set; }

        [JsonProperty("boundingBox")]
        public CustomBox CustomBox { get; set; }
    }

    public class ResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("iteration")]
        public string Iteration { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("predictions")]
        public List<Prediction> Predictions { get; set; }
    }
}

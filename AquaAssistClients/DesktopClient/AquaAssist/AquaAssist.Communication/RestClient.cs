using AquaAssist.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AquaAssist.Communication
{
    public static class RestClient
    {
        public static string BaseUrl { get; set; } = "http://127.0.0.1:3000/";

        public static SensorModel Test()
        {
            SensorModel res = new SensorModel();
            try
            {                
                WebRequest request = WebRequest.Create("http://127.0.0.1:3000/SensorDefinition?id=1");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream dataStream = response.GetResponseStream();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SensorModel));
                    StreamReader reader = new StreamReader(dataStream);
                    //string str = reader.ReadToEnd();
                    res = serializer.ReadObject(dataStream) as SensorModel;
                }
            }
            catch(Exception ex)
            {
                return res;
            }
            return res;
        }

        /// <summary>
        /// Generic Get Function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="entity"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static T Get<T>(string root, Dictionary<string, string> arguments) where T : class, new()
        {
            T res = new T();
            try
            {
                string query = null;

                if (arguments != null && arguments.Count() > 0)
                {
                    query = $"?{string.Join("&", arguments.Select(x => $"{x.Key}={x.Value}"))}";
                    query = Uri.EscapeUriString(query);
                }           
                
                WebRequest request = WebRequest.Create($"{BaseUrl}{root}{query}");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream dataStream = response.GetResponseStream();                    
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));                    
                    res = serializer.ReadObject(dataStream) as T;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return res;
        }

        public static List<SensorModel> GetSensorModels()
        {
            return Get<List<SensorModel>>("SensorDefinition", null);
        }

        public static SensorModel GetSensorModelById(int id)
        {
            return Get<SensorModel>("SensorDefinition", 
                new Dictionary<string, string>
                {
                    {"id", id.ToString()}
                });
        }

        public static List<SensorValueModel> GetSensorValuesBySensorId(int sensorId)
        {
            return Get<List<SensorValueModel>>("SensorValues",
                new Dictionary<string, string>
                {
                    { "id", sensorId.ToString() }
                });
        }

        public static List<SensorValueModel> GetSensorValues(int sensorId, DateTime start, DateTime end, int max)
        {
            return Get<List<SensorValueModel>>("SensorValues",
                new Dictionary<string, string>
                {
                    { "id", sensorId.ToString() },
                    { "start", start.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "end", end.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "max", max.ToString() },
                });
        }
    }
}

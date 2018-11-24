﻿using AquaAssist.CrossCutting.Constants;
using AquaAssist.CrossCutting.Enum;
using AquaAssist.CrossCutting.Helpers;
using AquaAssist.CrossCutting.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;

namespace AquaAssist.Communication
{
    public static class RestClient
    {
        public static string BaseUrl { get; set; } = "http://192.168.0.103:3000/";        

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
                    query = Uri.EscapeUriString($"?{string.Join("&", arguments.Select(x => $"{x.Key}={x.Value}"))}");                    
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

        public static SensorModel GetSensorModel(SensorTypes type)
        {
            return GetSensorModelById(Mappings.SensorTypeSensorIdMapping[type]);
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
                    { "start", start.ToAquaAssistDateTimeString() },
                    { "end", end.ToAquaAssistDateTimeString() },
                    { "max", max.ToString() },
                });
        }

        /// <summary>
        /// Returns the Top N SensorValueModel, orderd by TimeStamp (Desc)
        /// </summary>
        /// <param name="sensorId">Represends the Sensor ID in the database</param>
        /// <param name="N">the number of records to be returned</param>
        /// <returns></returns>
        public static List<SensorValueModel> GetSensorValues(int sensorId, int N)
        {
            return Get<List<SensorValueModel>>("SensorValues",
                new Dictionary<string, string>
                {
                    { "id", sensorId.ToString() },                    
                    { "n", N.ToString() }
                });
        }
    }
}

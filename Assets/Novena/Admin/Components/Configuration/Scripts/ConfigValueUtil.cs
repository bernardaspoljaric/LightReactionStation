using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Novena
{
    public static class ConfigValueUtil
    {
        public static T GetValue<T>(string value)
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(string):
                    return (T)(object)value;
                case Type t when t == typeof(int):
                    return (T)GetIntValue(value);
                case Type t when t == typeof(float):
                    return (T)GetFloatValue(value);
                case Type t when t == typeof(bool):
                    return (T)(object)GetBoolValue(value);
                case Type t when t == typeof(Vector2):
                    return (T)(object)GetVectorData(value).ToVector2();
                case Type t when t == typeof(Vector3):
                    return (T)(object)GetVectorData(value).ToVector3();
                default:
                    return (T)(object)value;
            }
        }

        public static string SetValue<T>(T value)
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(string):
                    return (string)(object)value;
                case Type t when t == typeof(int):
                    return ((int)(object)value).ToString();
                case Type t when t == typeof(float):
                    return ((float)(object)value).ToString();
                case Type t when t == typeof(bool):
                    return ((bool)(object)value).ToString();
                case Type t when t == typeof(Vector2):
                    return JsonUtility.ToJson(new VectorData((Vector2)(object)value));
                case Type t when t == typeof(Vector3):
                    return JsonUtility.ToJson(new VectorData((Vector3)(object)value));
                default:
                    return (string)(object)value;
            }
        }

        private static VectorData GetVectorData(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<VectorData>(value);
            }
            catch (Exception ex)
            {
                return new VectorData(); // Return default value for the type T
            }
        }

        private static object GetIntValue(string value)
        {
            return int.TryParse(value, out int result) ? result : 0;
        }

        private static object GetFloatValue(string value)
        {
            return float.TryParse(value, out float result) ? result : 0;
        }

        private static bool GetBoolValue(string value)
        {
            return bool.TryParse(value, out bool result) ? result : false;
        }
    }

    [System.Serializable]
    public class VectorData
    {
        public float x;
        public float y;
        public float z;

        public VectorData() { }

        public VectorData(Vector2 vector2)
        {
            x = vector2.x;
            y = vector2.y;
        }

        public VectorData(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}

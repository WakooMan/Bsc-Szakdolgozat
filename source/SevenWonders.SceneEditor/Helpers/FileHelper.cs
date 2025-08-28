using SevenWonders.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SevenWonders.SceneEditor.Helpers
{
    public static class FileHelper
    {
        public static string TempPath => Path.Combine(Directory.GetCurrentDirectory(), "temp");
        public static string ScenesPath => Path.Combine(Directory.GetCurrentDirectory(), "savedscenes");

        public static void Serialize<T>(T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T? Deserialize<T>(string filePath) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(filePath))
            {
                return serializer.Deserialize(reader) as T;
            }
        }
    }
}

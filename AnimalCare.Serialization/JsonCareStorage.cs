using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AnimalCare.Core;

namespace AnimalCare.Serialization
{
    public static class JsonCareStorage
    {
        private static readonly string FilePath = "care_data.json";

        public static void Save(List<Care> careList)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string json = JsonSerializer.Serialize(careList, options);
            File.WriteAllText(FilePath, json);
        }

        public static List<Care> Load()
        {
            if (!File.Exists(FilePath))
                return new List<Care>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Care>>(json, new JsonSerializerOptions
            {
                IncludeFields = true
            }) ?? new List<Care>();
        }
    }
}

//
// Author: Dominik Philp
// Description: Base Class for all Classes that should be able to be serialized to JSON
//
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JSONObject<T>
{
    public static bool JSONFileExists(string _fileName)
    {
        return File.Exists(Application.persistentDataPath + @"/" + _fileName + @".json");
    }

    public static void DeleteJSONFile(string _fileName)
    {
        File.Delete(Application.persistentDataPath + @"/" + _fileName + @".json");
    }

    public static T CreateFromJSON(string _fileName)
    {
        // using json.net tools
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(Application.persistentDataPath + @"/" + _fileName + @".json"));
    }


    public static bool FileExists(string _fileName)
    {
        return File.Exists(Application.persistentDataPath + @"/" + _fileName + @".json");
    }


    /// <param name="_fileName">name of json file without file extension</param>
    public bool SaveToJson(string _fileName)
    {
        string jsonFile = Application.persistentDataPath + @"/" + _fileName + @".json";

        if (!File.Exists(jsonFile))
        {
            File.WriteAllText(jsonFile, JsonConvert.SerializeObject(this, Formatting.Indented));

            return File.Exists(jsonFile);
        }
        else
        {
            Debug.LogError("File \"" + jsonFile + "\" already exists.");
            return false;
        }
    }

    /// <param name="_fileName">name of json file without file extension</param>
    public bool SaveToJson(string _fileName, bool _allowOverwrite)
    {
        string jsonFile = Application.persistentDataPath + @"/" + _fileName + @".json";

        if (!File.Exists(jsonFile) || _allowOverwrite)
        {
            File.WriteAllText(jsonFile, JsonConvert.SerializeObject(this, Formatting.Indented));

            return File.Exists(jsonFile);
        }
        else
        {
            Debug.LogError("File \"" + jsonFile + "\" already exists, and overwrite flag is unset.");
            return false;
        }
    }
}

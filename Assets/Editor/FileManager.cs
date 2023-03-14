using UnityEngine;
using System.IO;

public class FileManager
{
    /// <summary>
    /// Load File
    /// </summary>
    /// <typeparam name="T">Data Model Type</typeparam>
    /// <param name="filename">File Name</param>
    /// <returns>Instance</returns>
    public static T Load<T>(string filePath) where T : new()
    {
        T output;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            output = JsonUtility.FromJson<T>(dataAsJson);
        }
        else
        {
            output = new T();
        }

        return output;
    }

    /// <summary>
    /// Delete File
    /// </summary>
    /// <typeparam name="T">Data Model Type</typeparam>
    /// <param name="filename">File Name</param>
    /// <returns>Instance</returns>
    public static void Delete<T>(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Save File
    /// </summary>
    /// <typeparam name="T">Model Type</typeparam>
    /// <param name="filename">File Name</param>
    /// <param name="content">Model Content</param>
    public static void Save<T>(string filePath, T content)
    {
        string dataAsJson = JsonUtility.ToJson(content);
        File.WriteAllText(filePath, dataAsJson);
    }
}
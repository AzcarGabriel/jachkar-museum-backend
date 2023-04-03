// JachkarMuseumUtils.cs
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class JachkarMuseumUtils : UnityEngine.MonoBehaviour
{
    static string OBJECT_FROM_FOLDER_PATH = "Assets/RawObjects/";
    static string OBJECT_TO_FOLDER_PATH = "Assets/ProcessedObjects/";
    static string OBJECT_PROCESSED_FOLDER_PATH = "Assets/ProcessedAssets/";
    static string CONFIG_FILE = "Assets/config.json";

    public class Config
    {
        public int actualStoneNumber;
        public int actualThumbNumber;
    }

    static void BuildObjectAssetBundle()
    {
        Console.WriteLine("PROCESS BUILD ASSET BUNDLE BEGINS -------------------------------------------------------");

        int i = 0;
        string log = "Logs/BuildObjectAssetBundle_details.txt";
        string[] assetN;
        int N_Files;
        UnityEditor.AssetBundleBuild[] AssetMap = new UnityEditor.AssetBundleBuild[2];
        AssetMap[0].assetBundleName = "bundle";

        // Adding to path /Models
        string path = OBJECT_FROM_FOLDER_PATH;

        // log
        if (!File.Exists(log))
        {
            File.Create(log);
        }

        File.AppendAllText(log, System.DateTime.Now.ToString() + "\n\n");
        File.AppendAllText(log, path + "\n");

        DirectoryInfo dir = new System.IO.DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles();

        // Number of files in OBJECT_FROM_FOLDER_PATH folder
        N_Files = files.Length;

        // log
        File.AppendAllText(log, "Num assets: " + N_Files + " \n");

        assetN = new string[N_Files];
        foreach (FileInfo file in files)
        {
            if (file.Exists)
            {
                if (!file.Extension.Equals(".meta"))
                {
                    assetN[i] = OBJECT_FROM_FOLDER_PATH + file.Name;
                    File.AppendAllText(log, assetN[i] + " \n");
                    i += 1;
                }
            }
        }
        AssetMap[0].assetNames = assetN;

        UnityEditor.BuildPipeline.BuildAssetBundles(OBJECT_PROCESSED_FOLDER_PATH, AssetMap, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.WebGL);

        // log
        File.AppendAllText(log, "\t----X----\n");
    }

    static void MoveFile(string fromFolder, string destinationFolder, string filename, string destinationName = null)
    {
        try
        {
            string path = fromFolder + filename;
            string metaPath = path + ".meta";
            string newPath = destinationFolder + filename;
            if (destinationName == null)
            {
                destinationName = filename;
            }

            if (!File.Exists(path))
            {
                // This statement ensures that the file is created, but the handle is not kept.  
                using FileStream fs = File.Create(path);
            }

            // Ensure that the target does not exist.  
            if (File.Exists(newPath))
            {
                Console.WriteLine("New path is taken.");
            }

            // Move the file.  
            File.Move(path, newPath);
            Console.WriteLine("{0} was moved to {1}.", path, newPath);

            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
                Console.WriteLine("{0} was deleted.", metaPath);
            }

            // See if the original exists now.  
            if (File.Exists(path))
            {
                Console.WriteLine("The original file still exists, which is unexpected.");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
    }
}
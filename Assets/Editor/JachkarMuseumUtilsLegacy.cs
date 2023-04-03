// JachkarMuseumUtilsLegacy.cs
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class JachkarMuseumUtilsLegacy : UnityEngine.MonoBehaviour
{
    static string OBJECT_FROM_FOLDER_PATH = "Assets/RawObjects/";
    static string OBJECT_TO_FOLDER_PATH = "Assets/ProcessedObjects/";
    static string OBJECT_PROCESSED_FOLDER_PATH = "Assets/ProcessedAssets/";
    static string THUMBS_FROM_FOLDER_PATH = "Assets/RawThumbs/";
    static string THUMBS_PROCESSED_FOLDER_PATH = "Assets/ProcessedThumbs/";
    static string CONFIG_FILE = "Assets/config.json";

    public class Config
    {
        public int actualStoneNumber;
        public int actualThumbNumber;
    }

    /*
     * This function searchs in FROM_FOLDER_PATH files of type obj and create a prefab from them
     */
    static void ProcessStonePrefabs()
    {
        Console.WriteLine("PROCESS PREFABS BEGINS ------------------------------------------------------------------");
        string[] files = Directory.GetFiles(OBJECT_FROM_FOLDER_PATH);
        Config config = FileManager.Load<Config>(CONFIG_FILE);

        for (int i = 0; i < files.Length; i++)
        {
            if (Path.GetExtension(files[i]).Contains(".obj") && !files[i].Contains(".meta"))
            {
                Console.WriteLine("Reading {0}", files[i]);
                string[] filenameParts = files[i].Split('/');
                string[] filenameParts2 = filenameParts[filenameParts.Length - 1].Split('\\');
                string filename = filenameParts2[filenameParts2.Length - 1].Replace(".obj", "");
                CreateStonePrefab("Stone" + config.actualStoneNumber.ToString(), filename);
                config.actualStoneNumber++;
                FileManager.Save<Config>(CONFIG_FILE, config);
            }
        }
    }

    static void CreateStonePrefab(string prefabName, string objName, string mtlName = null, string textureName = null)
    {
        Console.WriteLine("Creating {0}", objName);

        if (mtlName == null)
        {
            mtlName = objName;
        }

        if (textureName == null)
        {
            textureName = objName;
        }

        // Import asset
        var relativePath = OBJECT_FROM_FOLDER_PATH + objName + ".obj";
        GameObject createdObject = AssetDatabase.LoadAssetAtPath(relativePath, typeof(GameObject)) as GameObject;
        // createdObject.AddComponent<ContureRendering>();
        Quaternion rt = Quaternion.Euler(-90, 0, 0);
        Vector3 sp = new Vector3(0.0f, 0.0f, 0.0f);
        var instantiatedObject = Instantiate(createdObject, sp, rt);

        // Set the path as within the Assets folder and name it as the GameObject's name with the .Prefab format
        string localPath = OBJECT_TO_FOLDER_PATH + prefabName + ".prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab.
        PrefabUtility.SaveAsPrefabAssetAndConnect(instantiatedObject, localPath, InteractionMode.UserAction);
        DestroyImmediate(instantiatedObject);

        // Move the files to a processed folder
        string[] filesNames = { objName + ".obj", mtlName + ".mtl", textureName + ".png" };
        for (int i = 0; i < filesNames.Length; i++)
        {
            MoveFileToProcessed(OBJECT_FROM_FOLDER_PATH, OBJECT_PROCESSED_FOLDER_PATH, filesNames[i]);
        }

        // Create the new Asset Bundle
        string log = "BuildThumbsAssetBundleDetailsLog.txt";

        string[] assetN = new string[1];
        assetN[0] = localPath;
        System.IO.File.AppendAllText(log, assetN[0] + " \n");

        UnityEditor.AssetBundleBuild[] AssetMap = new UnityEditor.AssetBundleBuild[2];
        AssetMap[0].assetBundleName = "bundle";
        AssetMap[0].assetNames = assetN;

        UnityEditor.BuildPipeline.BuildAssetBundles("Assets/AssetBundles", AssetMap, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.WebGL);

        // log
        File.AppendAllText(log, "\t----X----\n");
    }

    static void ProcessThumbs()
    {
        Console.WriteLine("PROCESS THUMBS BEGINS ------------------------------------------------------------------");
        string[] files = Directory.GetFiles(THUMBS_FROM_FOLDER_PATH);
        Config config = FileManager.Load<Config>(CONFIG_FILE);

        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].Contains(".meta"))
            {
                string[] filenameParts = files[i].Split('/');
                string filename = filenameParts[filenameParts.Length - 1];
                MoveFileToProcessed(THUMBS_FROM_FOLDER_PATH, THUMBS_PROCESSED_FOLDER_PATH, filename, filename.Replace("Stone", ""));
                config.actualStoneNumber++;
                FileManager.Save<Config>(CONFIG_FILE, config);
            }
        }
    }

    static void BuildStoneAssetBundle()
    {
        Console.WriteLine("PROCESS BUILD ASSET BUNDLE BEGINS -------------------------------------------------------");

        int i = 0;
        string log = "BuildStoneAssetBundleDetailsLog.txt";
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
        System.IO.File.AppendAllText(log, "Num assets: " + N_Files + " \n");

        assetN = new string[N_Files];
        foreach (FileInfo file in files)
        {
            if (file.Exists)
            {
                if (!file.Extension.Equals(".meta"))
                {
                    assetN[i] = OBJECT_FROM_FOLDER_PATH + file.Name;
                    System.IO.File.AppendAllText(log, assetN[i] + " \n");
                    i += 1;
                }
            }
        }
        AssetMap[0].assetNames = assetN;

        UnityEditor.BuildPipeline.BuildAssetBundles("Assets/AssetBundles", AssetMap, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.WebGL);

        // log
        File.AppendAllText(log, "\t----X----\n");
    }

    static void BuildThumbsAssetBundle()
    {
        Console.WriteLine("PROCESS THUMBS ASSET BUNDLE BEGINS -------------------------------------------------------");

        int i = 0;
        string log = "BuildThumbsAssetBundleDetailsLog.txt";
        string[] assetN;
        int N_Files;
        UnityEditor.AssetBundleBuild[] AssetMap = new UnityEditor.AssetBundleBuild[2];
        AssetMap[0].assetBundleName = "stones_thumbs";

        // Adding to path /Models
        string path = UnityEngine.Application.dataPath + "/ProcessedThumbs";

        // log
        //if (!File.Exists(log))
        //{
        //   File.Create(log);
        //}

        //File.AppendAllText(log, DateTime.Now.ToString() + "\n\n");
        //File.AppendAllText(log, path + "\n");

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles();

        // Number of files in "/ProcessedThumbs" folder
        N_Files = files.Length;

        // log
        //File.AppendAllText(log, "Num assets: " + N_Files + " \n");

        assetN = new string[N_Files];
        foreach (FileInfo file in files)
        {
            if (file.Exists)
            {
                if (!file.Extension.Equals(".meta"))
                {
                    assetN[i] = "Assets/ProcessedThumbs/" + file.Name;
                    //File.AppendAllText(log, assetN[i] + " \n");
                    i += 1;
                }
            }
        }
        AssetMap[0].assetNames = assetN;

        UnityEditor.BuildPipeline.BuildAssetBundles("Assets/AssetBundles", AssetMap, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.WebGL);

        // log
        //File.AppendAllText(log, "\t----X----\n");
    }

    static void MoveFileToProcessed(string fromFolder, string destinationFolder, string filename, string destinationName = null)
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
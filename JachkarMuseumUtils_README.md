** Jachkar Museum Asset Bundle Creator **

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -createProject project

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -projectPath . -executeMethod BuildAssets.BuildStoneAssetBundle -logFile Logs/BuildStoneAssetBundle_log.txt

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -projectPath . -executeMethod BuildAssets.BuildThumbsAssetBundle -logFile Logs/BuildThumbsAssetBundle_log.txt

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -projectPath . -executeMethod BuildPrefabs.ProcessStonePrefabs -logFile Logs/ProcessStonePrefabs_log.txt

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -projectPath . -executeMethod BuildPrefabs.ProcessThumbs -logFile Logs/ProcessThumbs_log.txt
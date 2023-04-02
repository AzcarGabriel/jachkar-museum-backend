** Jachkar Museum Asset Bundle Creator **

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -createProject project

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -projectPath . -executeMethod JachkarMuseumUtils.BuildStoneAssetBundle -logFile Logs/BuildStoneAssetBundle_log.txt

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -projectPath . -executeMethod JachkarMuseumUtils.BuildThumbsAssetBundle -logFile Logs/BuildThumbsAssetBundle_log.txt

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -username jachkar.museum -password nbaloianGEOCOLLAB1 -quit -projectPath . -executeMethod JachkarMuseumUtils.ProcessStonePrefabs -logFile Logs/ProcessStonePrefabs_log.txt

C:\'Program Files'\Unity\Hub\Editor\2021.3.8f1\Editor\Unity -batchmode -quit -projectPath . -executeMethod JachkarMuseumUtils.ProcessThumbs -logFile Logs/ProcessThumbs_log.txt
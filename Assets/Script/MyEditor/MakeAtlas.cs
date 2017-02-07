using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MyEditor
{
    public class MakeAtlas : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
		
        }
	
        // Update is called once per frame
        void Update()
        {
		
        }
        [MenuItem("SH/AtlasMaker/Map")]
        static private void MakeAtlasMap(){
            MakeAtlasStart("map");
        }
        [MenuItem("SH/AtlasMaker/Chara")]
        static private void MakeAtlasChara(){
            MakeAtlasStart("chara");
        }

        [MenuItem("SH/AtlasMaker/All")]
        static private void MakeAtlasAll()
        {
            MakeAtlasStart("");
        }

        static private void MakeAtlasStart(string atlasName)
        {
            string spriteDir = Application.dataPath + "/Resources/Sprite";

            if (!Directory.Exists(spriteDir))
            {
                Directory.CreateDirectory(spriteDir);
            }

            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                if (!string.IsNullOrEmpty(atlasName) && dirInfo.FullName.IndexOf("/Atlas/" + atlasName) < 0)
                {
                    continue;
                }
                foreach (FileInfo pngFile in dirInfo.GetFiles("*.png",SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                    Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    GameObject go = new GameObject(sprite.name);
                    go.AddComponent<SpriteRenderer>().sprite = sprite;
                    allPath = spriteDir + "/" + sprite.name + ".prefab";
                    string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                    PrefabUtility.CreatePrefab(prefabPath, go);
                    GameObject.DestroyImmediate(go);
                }
            }	
        }

        [MenuItem("SH/Build Assetbundle/Map")]
        static private void BuildAssetBundleMap()
        {
            BuildAssetBundle("map");
        }
        [MenuItem("SH/Build Assetbundle/Chara")]
        static private void BuildAssetBundleChara()
        {
            BuildAssetBundle("chara");
        }

        [MenuItem("SH/Build Assetbundle/All")]
        static private void BuildAssetBundleAll()
        {
            BuildAssetBundle("");
        }

        static private void BuildAssetBundle(string atlasName)
        {
            string dir = Application.dataPath + "/Editor Default Resources/assetbundle/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                if (!string.IsNullOrEmpty(atlasName) && dirInfo.FullName.IndexOf("/Atlas/" + atlasName) < 0)
                {
                    continue;
                }
                List<Sprite> assets = new List<Sprite>();
                //string path = dir + "/" + dirInfo.Name + ".assetbundle";
                string path = dir + "/" + dirInfo.Name + ".unity3d";
                Debug.Log("path=" + path);
                foreach (FileInfo pngFile in dirInfo.GetFiles("*.png",SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                    assets.Add(AssetDatabase.LoadAssetAtPath<Sprite>(assetPath));
                }
                if (BuildPipeline.BuildAssetBundle(null, assets.ToArray(), path, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.CollectDependencies, GetBuildTarget()))
                {
                }
            }	
        }
        [MenuItem("SH/Build Assetbundle/Master/All")]
        static private void BuildAssetBundleMasterAll()
        {
            BuildAssetBundleTile();
            BuildAssetBundleTopMap();
            BuildAssetBundleBuilding();
            BuildAssetBundlePromptMessage();
        }
        [MenuItem("SH/Build Assetbundle/Master/Avatar")]
        static private void BuildAssetBundleAvatar()
        {
            BuildAssetBundleMaster(App.Model.Avatar.AvatarAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/Tile")]
        static private void BuildAssetBundleTile()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.TileAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/TopMap")]
        static private void BuildAssetBundleTopMap()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.TopMapAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/PromptMessage")]
        static private void BuildAssetBundlePromptMessage()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.PromptMessageAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/Building")]
        static private void BuildAssetBundleBuilding()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.BuildingAsset.Name);
        }
        static private void BuildAssetBundleMaster(string name)
        {
            string assetPath = string.Format("ScriptableObject/{0}.asset", name);
            ScriptableObject asset = EditorGUIUtility.Load(assetPath) as ScriptableObject;
            Debug.LogError("BuildAssetBundleMaster:" + assetPath + ", " + asset);
            if (asset == null)
            {
                return;
            }
            List<ScriptableObject> assets = new List<ScriptableObject>();
            assets.Add(asset);
            string dir = Application.dataPath + "/Editor Default Resources/assetbundle/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = dir + "/" + name + ".unity3d";
            assetPath = string.Format("{0}/Editor Default Resources/ScriptableObject/{1}.asset", Application.dataPath, name);
            //File.Delete(path);
            Debug.LogError("path="+path);
            if (BuildPipeline.BuildAssetBundle(null, assets.ToArray(), path, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.CollectDependencies, GetBuildTarget()))
            {
            }
            /*if (toDelete)
            {
                File.Delete(assetPath);
            }*/
            Debug.LogError("assetPath="+assetPath);
        }

        [MenuItem("SH/Create ScriptableObject/AvatarAsset")]
        static void CreateAvatarAsset()
        {
            var avatarAsset = ScriptableObject.CreateInstance<App.Model.Avatar.AvatarAsset>();

            AssetDatabase.CreateAsset(avatarAsset, "Assets/Data/avatarasset.asset");
            AssetDatabase.Refresh();
        }



        static public BuildTarget GetBuildTarget()
        {
            BuildTarget target = BuildTarget.WebPlayer;
            #if UNITY_STANDALONE
		target = BuildTarget.StandaloneWindows;
            #elif UNITY_IPHONE
            target = BuildTarget.iOS;
            #elif UNITY_ANDROID
		target = BuildTarget.Android;
            #endif
            return target;
        }
    }
}
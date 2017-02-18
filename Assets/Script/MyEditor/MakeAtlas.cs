using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
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
        /*
        [MenuItem("SH/Create ImageAtlas/Map")]
        static private void MakeAtlasMap(){
            MakeAtlasStart("map");
        }
        [MenuItem("SH/Create ImageAtlas/Chara")]
        static private void MakeAtlasChara(){
            MakeAtlasStart("chara");
        }

        [MenuItem("SH/Create ImageAtlas/All")]
        static private void MakeAtlasAll()
        {
            MakeAtlasStart("");
        }
        */
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
                if (!string.IsNullOrEmpty(atlasName) && dirInfo.Name != atlasName)
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
                    allPath = spriteDir + "/" + dirInfo.Name + "/" + sprite.name + ".prefab";
                    string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                    PrefabUtility.CreatePrefab(prefabPath, go);
                    GameObject.DestroyImmediate(go);
                }
            }	
        }

        [MenuItem("SH/Build Assetbundle/Image/Map")]
        static private void BuildAssetBundleMap()
        {
            BuildAssetBundle("map");
        }
        [MenuItem("SH/Build Assetbundle/Image/Chara")]
        static private void BuildAssetBundleChara()
        {
            BuildAssetBundle("chara");
        }
        [MenuItem("SH/Build Assetbundle/Image/Clothes")]
        static private void BuildAssetBundleClothes()
        {
            BuildAssetBundle("clothes");
        }
        [MenuItem("SH/Build Assetbundle/Image/Hat")]
        static private void BuildAssetBundleHat()
        {
            BuildAssetBundle("hat");
        }
        [MenuItem("SH/Build Assetbundle/Image/Horse")]
        static private void BuildAssetBundleHorse()
        {
            BuildAssetBundle("horse");
        }
        [MenuItem("SH/Build Assetbundle/Image/Weapon")]
        static private void BuildAssetBundleWeapon()
        {
            BuildAssetBundle("weapon");
        }
        [MenuItem("SH/Build Assetbundle/Image/All")]
        static private void BuildAssetBundleAll()
        {
            BuildAssetBundle("");
        }

        [MenuItem("SH/Build Assetbundle/Master/All")]
        static private void BuildAssetBundleMasterAll()
        {
            BuildAssetBundleAvatar();
            BuildAssetBundleConstant();
            BuildAssetBundleTile();
            BuildAssetBundleBaseMap();
            BuildAssetBundleBuilding();
            BuildAssetBundlePromptMessage();
            BuildAssetBundleWorld();
            BuildAssetBundleArea();
        }
        [MenuItem("SH/Build Assetbundle/Master/Avatar")]
        static private void BuildAssetBundleAvatar()
        {
            BuildAssetBundleMaster(App.Model.Avatar.AvatarAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/Constant")]
        static private void BuildAssetBundleConstant()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.ConstantAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/Tile")]
        static private void BuildAssetBundleTile()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.TileAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/World")]
        static private void BuildAssetBundleWorld()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.WorldAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/Area")]
        static private void BuildAssetBundleArea()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.AreaAsset.Name);
        }
        [MenuItem("SH/Build Assetbundle/Master/BaseMap")]
        static private void BuildAssetBundleBaseMap()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.BaseMapAsset.Name);
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
        [MenuItem("SH/Build Assetbundle/Master/Character")]
        static private void BuildAssetBundleCharacter()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.CharacterAsset.Name);
        }
        static private void BuildAssetBundleMaster(string name)
        {
            string assetPath = string.Format("ScriptableObject/{0}.asset", name);
            ScriptableObject asset = EditorGUIUtility.Load(assetPath) as ScriptableObject;
            if (asset == null)
            {
                return;
            }
            /*List<ScriptableObject> assets = new List<ScriptableObject>();
            assets.Add(asset);*/
            string dir = "Editor Default Resources/assetbundle/";
            /*
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }*/
            string path = "Assets/" + dir;
            assetPath = string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", name);
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            builds[0].assetBundleName = name + ".unity3d";
            string[] enemyAssets = new string[1];
            enemyAssets[0] = assetPath;
            builds[0].assetNames = enemyAssets;
            BuildPipeline.BuildAssetBundles(path,builds,
                BuildAssetBundleOptions.ChunkBasedCompression
                ,GetBuildTarget()
            );
            Debug.LogError("BuildAssetBundleMaster success : "+name);
        }
        static private void BuildAssetBundle(string atlasName)
        {
            /*string dir = Application.dataPath + "/Editor Default Resources/assetbundle/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }*/
            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                if (!string.IsNullOrEmpty(atlasName) && dirInfo.Name != atlasName)
                {
                    continue;
                }
                //List<Sprite> assets = new List<Sprite>();
                List<string> paths = new List<string>();
                //string path = dir + dirInfo.Name + ".unity3d";
                foreach (FileInfo pngFile in dirInfo.GetFiles("*.png",SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                    //assets.Add(AssetDatabase.LoadAssetAtPath<Sprite>(assetPath));
                    paths.Add(assetPath);
                }
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = dirInfo.Name + "image.unity3d";
                build.assetNames = paths.ToArray();
                builds.Add(build);
                /*if (BuildPipeline.BuildAssetBundle(null, assets.ToArray(), path, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.CollectDependencies, GetBuildTarget()))
                {
                }*/
                Debug.LogError("BuildAssetBundle success : "+dirInfo.Name);
            } 
            string path = "Assets/Editor Default Resources/assetbundle/";
            BuildPipeline.BuildAssetBundles(path,builds.ToArray(),
                BuildAssetBundleOptions.ChunkBasedCompression
                ,GetBuildTarget()
            );
            Debug.LogError("BuildAssetBundle over ");  
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
            #if UNITY_STANDALONE
            return BuildTarget.StandaloneWindows;
            #elif UNITY_IPHONE
            return BuildTarget.iOS;
            #elif UNITY_ANDROID
            return BuildTarget.Android;
            #else
            return BuildTarget.WebPlayer;
            #endif
        }
    }
}

#endif
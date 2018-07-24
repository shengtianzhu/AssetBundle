using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager{

    private static ResourceManager m_aResourceManager;
    public static ResourceManager Instance
    {
        get{
            if (null == m_aResourceManager)
                m_aResourceManager = new ResourceManager();
            return m_aResourceManager;
        }
    }
    AssetBundleManifest aAssetBundleManifest;
    public string StreamingAssetsPath = "";
    public string PersistentDataPath = "";
    public string DataPath = "";
    public string RunAssetsPath = "";

    public Dictionary<string, AssetBundle> mapAssetBundle = new Dictionary<string, AssetBundle>();
    public void LoadResource<T>(string strAssetBundleName, string strName, System.Action<T> CallBack) where T : UnityEngine.Object
    {
        if (mapAssetBundle.ContainsKey(strAssetBundleName))
        {
            Debug.Log("cccccc");
            T _aObj = mapAssetBundle[strAssetBundleName].LoadAsset<T>(strName);

            if (null != CallBack)
                CallBack(_aObj);
            return;
        }

        //获取依赖
        string[] dependence = aAssetBundleManifest.GetAllDependencies(strAssetBundleName);
        for(int i = 0; i < dependence.Length; ++i)
        {
            if (mapAssetBundle.ContainsKey(dependence[i]))
                continue;

            string _Path = System.IO.Path.Combine(RunAssetsPath, dependence[i]);
            Debug.Log("_Path = " + _Path);
            AssetBundle _a = AssetBundle.LoadFromFile(_Path);

            if(null != _a)
            {
                mapAssetBundle.Add(dependence[i], _a);
                Debug.Log(_a.name);
            }
            else
            {
                Debug.Log(_Path + " Is Null ");
            }
        }
        string aPath = System.IO.Path.Combine(RunAssetsPath, strAssetBundleName);
        AssetBundle a = AssetBundle.LoadFromFile(aPath);
        mapAssetBundle.Add(strAssetBundleName, a);
        T aObj = a.LoadAsset<T>(strName);
        Debug.Log("bbbbbbbbb");
        if (null != CallBack)
            CallBack(aObj);
    }


    public void LoadScene(string strAssetBundleName, System.Action CallBack)
    {
        //获取依赖
        string[] dependence = aAssetBundleManifest.GetAllDependencies(strAssetBundleName);
        for (int i = 0; i < dependence.Length; ++i)
        {
            if (mapAssetBundle.ContainsKey(dependence[i]))
                continue;

            string _Path = System.IO.Path.Combine(RunAssetsPath, dependence[i]);
            Debug.Log("_Path = " + _Path);
            AssetBundle _a = AssetBundle.LoadFromFile(_Path);

            if (null != _a)
            {
                mapAssetBundle.Add(dependence[i], _a);
                Debug.Log(_a.name);
            }
            else
            {
                Debug.Log(_Path + " Is Null ");
            }
        }
        string aPath = System.IO.Path.Combine(RunAssetsPath, strAssetBundleName);
        AssetBundle a = AssetBundle.LoadFromFile(aPath);
        
        Debug.Log("bbbbbbbbb");
        if (null != CallBack)
            CallBack();
    }

    public void LoadAssetBundleManifest()
    {
        if (null != aAssetBundleManifest)
            return;

        AssetBundle b = AssetBundle.LoadFromFile(RunAssetsPath + "/StandaloneWindows");
        aAssetBundleManifest = b.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        if(null != aAssetBundleManifest)
        {
            Debug.Log(aAssetBundleManifest.name);
        }
        
    }

    

}

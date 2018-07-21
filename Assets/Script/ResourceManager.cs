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
    public void LoadResource<T>(string strAssetBundleName, string strName, System.Action<T> CallBack) where T : UnityEngine.Object
    {
        //获取依赖
        string[] dependence = aAssetBundleManifest.GetAllDependencies(strAssetBundleName);
        for(int i = 0; i < dependence.Length; ++i)
        {
            string _Path = System.IO.Path.Combine(StreamingAssetsPath, dependence[i]);
            Debug.Log("_Path = " + _Path);
            AssetBundle _a = AssetBundle.LoadFromFile(_Path);
            if(null != _a)
            {
                Debug.Log(_a.name);
            }
            else
            {
                Debug.Log(_Path + " Is Null ");
            }
        }
        string aPath = System.IO.Path.Combine(StreamingAssetsPath, strAssetBundleName);
        AssetBundle a = AssetBundle.LoadFromFile(aPath);
        T aObj = a.LoadAsset<T>(strName);
        if (null != CallBack)
            CallBack(aObj);
    }

    public void LoadAssetBundleManifest()
    {
        if (null != aAssetBundleManifest)
            return;

        AssetBundle b = AssetBundle.LoadFromFile(StreamingAssetsPath + "/Android");
        aAssetBundleManifest = b.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        if(null != aAssetBundleManifest)
        {
            Debug.Log(aAssetBundleManifest.name);
        }
    }

    

}

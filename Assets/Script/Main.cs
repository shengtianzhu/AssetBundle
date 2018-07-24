using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour {

    public Text Text1;
    public Text Text2;
    public Text Text3;

    HttpDownLoad m_aHttpDownLoad;

    UILogic1 m_aUI;
    // Use this for initialization
    void Start () {


        ResourceManager.Instance.StreamingAssetsPath = Application.streamingAssetsPath;
        ResourceManager.Instance.PersistentDataPath = Application.persistentDataPath;
        ResourceManager.Instance.DataPath = Application.dataPath;
        ResourceManager.Instance.RunAssetsPath = ResourceManager.Instance.StreamingAssetsPath;

        Text1.text = ResourceManager.Instance.StreamingAssetsPath;
        Text2.text = ResourceManager.Instance.PersistentDataPath;
        Text3.text = ResourceManager.Instance.DataPath;
        Debug.Log("PersistentDataPath = " + ResourceManager.Instance.PersistentDataPath);
        //StartCoroutine(CopyToPersistentDataPath());

        ResourceManager.Instance.LoadAssetBundleManifest();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick1()
    {
        if (null != m_aUI)
            return;

        //m_aUI = new UILogic1();
        //m_aUI.Init();
        //m_aUI.Open();

        //m_aHttpDownLoad = new HttpDownLoad();
        //m_aHttpDownLoad.DownLoad("http://192.168.17.99:280/GDC%20Unreal%20Engine.mp4", Application.persistentDataPath, null);
        //StartCoroutine(DownLoadRes());

        ResourceManager.Instance.LoadScene("testscene", () => 
        {
            SceneManager.LoadScene("Test");
        });
    }

    public void OnClick2()
    {
        if (null != m_aUI)
            return;

        ResourceManager.Instance.LoadAssetBundleManifest();

        m_aUI = new UILogic1();
        m_aUI.Init();
        m_aUI.Open();
        
    }

    public void OnClick3()
    {
        if (null != m_aUI)
            return;

        if (null != m_aHttpDownLoad)
            m_aHttpDownLoad.Close();
    }

    string getStreamingPath_for_www()
    {
        string pre = "file://";
#if UNITY_EDITOR
        pre = "file://";
#elif UNITY_ANDROID
        pre = "";
#elif UNITY_IPHONE
	    pre = "file://";
#endif
        string path = pre + Application.streamingAssetsPath + "/";
        return path;
    }

    string getPersistentPath_for_www()
    {
        string pre = "file://";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        pre = "file:///";
#elif UNITY_ANDROID
        pre = "file://";
#elif UNITY_IPHONE
        pre = "file://";
#endif
        string path = pre + Application.persistentDataPath + "/";
        return path;
    }

    List<string> ltNeedDownFiles = new List<string>();
    public IEnumerator DownLoadRes()
    {
        
        string strResInfo = "ResInfo.txt";
        string src = getStreamingPath_for_www() + strResInfo;
        Debug.Log("src = " + src);
        WWW www = new WWW(src);
        yield return www;
        if (www.isDone)
        {
            Debug.Log("www ResInfo " + www.bytes.Length);
        }
        else
        {
            Debug.Log("www ResInfo " + www.error);
        }

        MemoryStream stream = null;
        StreamReader reader = null;

        stream = new MemoryStream(www.bytes);
        reader = new StreamReader(stream);
        

        ReadIndexStream(reader, ref ltNeedDownFiles);
        _DownLoad();


    }
    private void _DownLoad()
    {
        Debug.Log("_DownLoad");
        string _strUrl = "http://192.168.17.99:280/";
        if (ltNeedDownFiles.Count > 0)
        {
            string aPath = ltNeedDownFiles[0];
            ltNeedDownFiles.RemoveAt(0);
            DoDownLoad(Path.Combine(_strUrl, aPath), _DownLoad);
        }
        else
        {
            Debug.Log("_DownLoad  Over");
        }
    }
    private void DoDownLoad(string strUrl, System.Action callBack)
    {
        Debug.Log("_DownLoad  " + strUrl);
        HttpDownLoad _aHttpDownLoad = new HttpDownLoad();
        _aHttpDownLoad.DownLoad(strUrl, ResourceManager.Instance.PersistentDataPath, callBack);
    }
    public IEnumerator CopyToPersistentDataPath()
    {
        string strResInfo = "ResInfo.txt";
        string src = getStreamingPath_for_www() + strResInfo;
        Debug.Log("src = " + src);
        WWW www = new WWW(src);
        yield return www;
        if(www.isDone)
        {
            Debug.Log("www ResInfo " + www.bytes.Length);
        }
        else
        {
            Debug.Log("www ResInfo " + www.error);
        }
        
        MemoryStream stream = null;
        StreamReader reader = null;

        stream = new MemoryStream(www.bytes);
        reader = new StreamReader(stream);
        List<string> ltFiles = new List<string>();

        ReadIndexStream(reader, ref ltFiles);
        
        for (int i = 0; i < ltFiles.Count; ++i)
        {
            Debug.Log("111ltFiles[i] = " + ltFiles[i]);
            StartCoroutine(copy(ltFiles[i]));
        }
        Debug.Log("Cope Over !!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    private void ReadIndexStream(StreamReader iReader, ref List<string> ltFiles)
    {
        while (true)
        {
            string line = iReader.ReadLine();
            if (line == null || line.Length == 0)
                break;

            ltFiles.Add(line);
        }
    }

    IEnumerator copy(string fileName)
    {
        string src = getStreamingPath_for_www() + fileName;
        string des = Application.persistentDataPath + "/" + fileName;
        Debug.Log("des:" + des);
        Debug.Log("src:" + src);
        WWW www = new WWW(src);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("www.error:" + www.error);
        }
        else
        {
            //des = Application.persistentDataPath + "/" + fileName;
            if (File.Exists(des))
            {
                File.Delete(des);
            }
            FileStream fsDes = File.Create(des);
            fsDes.Write(www.bytes, 0, www.bytes.Length);
            fsDes.Flush();
            fsDes.Close();
        }
        www.Dispose();
    }

}

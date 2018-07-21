using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class Main : MonoBehaviour {

    public Text Text1;
    public Text Text2;
    public Text Text3;

    UILogic1 m_aUI;
    // Use this for initialization
    void Start () {

        ResourceManager.Instance.StreamingAssetsPath = Application.streamingAssetsPath;
        ResourceManager.Instance.PersistentDataPath = Application.persistentDataPath;
        ResourceManager.Instance.DataPath = Application.dataPath;

        Text1.text = ResourceManager.Instance.StreamingAssetsPath;
        Text2.text = ResourceManager.Instance.PersistentDataPath;
        Text3.text = ResourceManager.Instance.DataPath;

        //ResourceManager.Instance.LoadAssetBundleManifest();
        StartCoroutine(CopyToPersistentDataPath());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        if (null != m_aUI)
            return;

        m_aUI = new UILogic1();
        m_aUI.Init();
        m_aUI.Open();
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
            Debug.Log("ltFiles[i] = " + ltFiles[i]);
            //StartCoroutine(copy(ltFiles[i]));
        }

        for (int i = 0; i < ltFiles.Count; ++i)
        {
            Debug.Log("111ltFiles[i] = " + ltFiles[i]);
            StartCoroutine(copy(ltFiles[i]));
        }
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

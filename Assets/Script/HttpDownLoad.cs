using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class Tool
{
    ///
    /// 创建目录
    ///
    ///需要创建的目录路径
    public static void CreateDirectory(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            string dirName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }
    }
    ///
    /// 创建文件
    ///
    ///文件路径
    ///文件内容
    public static void CreatFile(string filePath, byte[] bytes)
    {
        FileInfo file = new FileInfo(filePath);
        Stream stream = file.Create();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        stream.Dispose();
    }
}

public class HttpDownLoad
{
    //进度
    public float progrees { get; private set; }
    //是否停止，这是独立的线程。Unity关闭时线程不会关闭。
    private bool IsStop;
    //下载的线程
    private Thread m_aThread;
    //是否下载完成
    public bool IsDown { get; private set; }

    // 网络资源的Url
    private string m_strUrl;
    /// 资源下载存放路径，不包含文件名
    private string m_strSavePath;
    /// 文件名,不包含后缀
    private string m_fileNameWithoutExt;
    /// 文件后缀
    private string m_fileExt;

    //临时文件的后缀
    private string m_tempFileExt = ".temp";

    /// 下载文件全路径，路径+文件名+后缀
    private string m_saveFilePath;

    /// 下载临时文件全路径，路径+文件名+后缀
    private string m_tempFilePath;
    /// 原文件大小
    ///
    private long m_fileLength;
    ///
    /// 当前下载好了的大小
    ///
    private long m_currentLength;

    public void Close()
    {
        IsStop = true;
    }
    public void DownLoad(string _url, string _savePath, System.Action callBack)
    {
        Debug.Log("DownLoad111111111 " + _url);
        m_strUrl = _url;
        m_strSavePath = _savePath;
        m_fileNameWithoutExt = Path.GetFileNameWithoutExtension(m_strUrl);
        m_fileExt = Path.GetExtension(m_strUrl);

        m_saveFilePath = string.Format("{0}/{1}{2}", m_strSavePath, m_fileNameWithoutExt, m_fileExt);
        m_tempFilePath = string.Format("{0}/{1}{2}", m_strSavePath, m_fileNameWithoutExt, m_tempFileExt);

        if((File.Exists(m_saveFilePath)))
        {
            Debug.Log("DownLoad File.Exists " + m_saveFilePath);
            if (null != callBack)
                callBack();
            return;

        }
        IsStop = false;
        m_aThread = new Thread(delegate () {

            IsDown = false;

            FileStream fileSteam;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(m_strUrl);

            if (!Directory.Exists(m_strSavePath))
            {
                Directory.CreateDirectory(m_strSavePath);
            }

            if(File.Exists(m_tempFilePath))
            {
                fileSteam = File.OpenWrite(m_tempFilePath);
                m_currentLength = fileSteam.Length;
                fileSteam.Seek(m_currentLength, SeekOrigin.Current);
                Debug.Log("m_tempFilePath Exists " + m_currentLength);
            }
            else
            {
                fileSteam = new FileStream(m_tempFilePath, FileMode.Create, FileAccess.Write);
                m_currentLength = 0;
            }

            m_fileLength = GetLength(m_strUrl);

            if(m_currentLength < m_fileLength)
            {
                request.AddRange((int)m_currentLength);
                Stream stream = request.GetResponse().GetResponseStream();

                byte[] buffer = new byte[1024];
                int length = stream.Read(buffer, 0, buffer.Length);
                while(length > 0)
                {
                    if (IsStop)
                        break;
                    fileSteam.Write(buffer, 0, length);
                    m_currentLength += length;
                    Debug.Log("GetProcess =  " + GetProcess());
                    length = stream.Read(buffer, 0, buffer.Length);
                }
                stream.Close();
                stream.Dispose();
                request.GetResponse().Close();

            }
            
            fileSteam.Close();
            IsDown = true;
            if(GetProcess() == 1)
            {
                Debug.Log("Move");
                File.Move(m_tempFilePath, m_saveFilePath);
            }
            
            Debug.Log("Over");
            if (null != callBack)
                callBack();
        });

        m_aThread.IsBackground = true;
        m_aThread.Start();
    }

    long GetLength(string url)
    {
        HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
        requet.Method = "HEAD";
        HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
        return response.ContentLength;
    }

    public float GetProcess()
    {
        if (m_fileLength > 0)
        {
            return Mathf.Clamp((float)m_currentLength / m_fileLength, 0, 1);
        }
        return 0;
    }
    public long GetCurrentLength()
    {
        return m_currentLength;
    }
    public long GetLength()
    {
        return m_fileLength;
    }
}
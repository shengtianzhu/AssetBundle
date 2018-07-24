using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogicBase{

    public bool IsInit = false;
    protected string UI_PATH;
    protected string UI_Name;
    protected bool bLoadObj = false;
    private bool m_bOpen = false;

    protected bool bOpen
    {
        get { return m_bOpen; }
        set
        {
            m_bOpen = value;
        }
    }

    protected GameObject m_UIObj { get; set; }

    public virtual void Init()
    {
        IsInit = true;

    }

    public virtual void Open()
    {
        if (bOpen)
        {
            return;
        }

        bOpen = true;
        
        if (!bLoadObj)
        {
            LoadResource();
        }
        else
        {
            m_UIObj.transform.SetAsLastSibling();
            m_UIObj.SetActive(true);
            OnUIOpen();
        }
        UIManager.Instance.AddUI(this);
    }

    public virtual void OnUIOpen()
    {

    }

    public virtual void InitLoadSucceeObj()
    {
        bLoadObj = true;
        if (m_UIObj == null)
            return;
    }

    private void LoadResource()
    {
        ResourceManager.Instance.LoadResource<GameObject>(UI_PATH, UI_Name, 
            (GameObject a) => {
                Debug.Log("LoadResource qqqqqqqq");
                m_UIObj = GameObject.Instantiate(a);
                Debug.Log("LoadResource ccccccccc");
                m_UIObj.transform.SetParent(UIManager.Instance.m_aUIRoot, false);
                m_UIObj.transform.localPosition = Vector3.zero;
                m_UIObj.transform.localScale = Vector3.one;

                InitLoadSucceeObj();

                OnUIOpen();
            });
    }

    public virtual void Release()
    {
        Close();
        if (null != m_UIObj)
        {
            //删除UI
            m_UIObj = null;
        }
    }

    public virtual void Close()
    {
        bOpen = false;
        UIManager.Instance.RemoveUI(this);
        
        if (null != m_UIObj)
        {
            m_UIObj.SetActive(false);
        }
    }

}

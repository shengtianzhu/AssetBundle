using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class UIManager
{
    private static UIManager m_aInstance;
    public static UIManager Instance
    {
        get
        {
            if (null == m_aInstance)
                m_aInstance = new UIManager();
            return m_aInstance;
        }
    }

    public Transform m_aUIRoot;
    List<UILogicBase> m_ltUIBase = new List<UILogicBase>();

    private UIManager()
    {
        m_aUIRoot = GameObject.FindGameObjectWithTag("UIRoot").transform;
    }

    public int AddUI(UILogicBase _ui)
    {
        m_ltUIBase.Add(_ui);
        return m_ltUIBase.Count;
    }
    public void RemoveUI(UILogicBase _ui)
    {
        m_ltUIBase.Remove(_ui);
    }


}

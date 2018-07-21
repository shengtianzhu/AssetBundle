using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public class UILogic1 : UILogicBase
{

    private UIDesigner1 m_UIDev;

    public override void Init()
    {
        base.Init();
        UI_PATH = "ui1";
        UI_Name = "UI1";
    }

    public override void OnUIOpen()
    {
        base.OnUIOpen();
    }

    public override void InitLoadSucceeObj()
    {
        base.InitLoadSucceeObj();
        m_UIDev = m_UIObj.GetComponent<UIDesigner1>();

        m_UIDev.OnButton1Click = OnButton1Click;
        m_UIDev.OnButton2Click = OnButton2Click;
    }

    private void OnButton2Click()
    {
        Debug.Log("Logic OnButton2Click " + Time.realtimeSinceStartup);

    }

    private void OnButton1Click()
    {
        Debug.Log("Logic OnButton1Click " + Time.realtimeSinceStartup);

        ResourceManager.Instance.LoadResource<SpriteAtlas>("altasb", "AltasB",
            (SpriteAtlas a) =>
            {
                m_UIDev.SetImage(a.GetSprite("UI_zb_txtb7"));
            });
    }
}

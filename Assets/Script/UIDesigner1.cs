using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIDesigner1 : UIDesignerBase
{
    public System.Action OnButton1Click = null;
    public System.Action OnButton2Click = null;

    public Button Button1;
    public Button Button2;
    public Image Image1;

    public void Button1Click()
    {
        Debug.Log("Button1Click " + Time.realtimeSinceStartup);

        if (null != OnButton1Click)
            OnButton1Click();
    }

    public void Button2Click()
    {
        Debug.Log("Button2Click " + Time.realtimeSinceStartup);

        if (null != OnButton2Click)
            OnButton2Click();
    }

    public void SetImage(Sprite aSprite)
    {
        Image1.sprite = aSprite;
    }

}

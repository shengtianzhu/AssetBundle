using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MySpriteAtlasManager : MonoBehaviour {

    private void Awake()
    {
        SpriteAtlasManager.atlasRequested += SpriteAtlasManager_atlasRequested;
    }

    private void SpriteAtlasManager_atlasRequested(string tag, System.Action<SpriteAtlas> action)
    {
        Debug.Log("tag = " + tag);
        //if(tag == "AltasA")
        //{
        //    ResourceManager.Instance.LoadResource<SpriteAtlas>("altasa", "AltasA",
        //    (SpriteAtlas a) =>
        //    {
        //        Debug.Log("aaaaaaaaaa");
        //        action(a);
        //    });
            
        //}
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

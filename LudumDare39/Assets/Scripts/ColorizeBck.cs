using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorizeBck : MonoBehaviour
{
    public List<SpriteRenderer> MainColor;
    public List<SpriteRenderer> SecondaryColor;
    public List<SpriteRenderer> ThirdColor;

    public List<SpriteRenderer> DefaultBck;

    public static Color[] yellowColors = {
        new Color(255.0f/255.0f,231.0f/255.0f,166.0f/255.0f),
        new Color(255.0f/255.0f,247.0f/255.0f,94.0f/255.0f),
        new Color(255.0f/255.0f,126.0f/255.0f,75.0f/255.0f)
    };

    public static Color[] blueColors = {
        new Color(182.0f/255.0f,200.0f/255.0f,223.0f/255.0f),
        new Color(255.0f/255.0f,139.0f/255.0f,180.0f/255.0f),
        new Color(12.0f/255.0f,255.0f/255.0f,243.0f/255.0f)
    };

    public static Color[] greenColors = {
        new Color(196/255.0f,212/255.0f,148/255.0f),
        new Color(0/255.0f,221/255.0f,57/255.0f),
        new Color(255.0f/255.0f,65/255.0f,58/255.0f)
    };

    public void SetColor(int idx)
    {
        foreach(var itm in MainColor)
        {
            itm.gameObject.SetActive(true);
            if (idx == 0) itm.color = yellowColors[0];
            else if (idx == 1) itm.color = blueColors[0];
            else itm.color = greenColors[0];
        }

        foreach (var itm in SecondaryColor)
        {
            itm.gameObject.SetActive(true);
            if (idx == 0) itm.color = yellowColors[1];
            else if (idx == 1) itm.color = blueColors[1];
            else itm.color = greenColors[1];
        }

        foreach (var itm in ThirdColor)
        {
            itm.gameObject.SetActive(true);
            if (idx == 0) itm.color = yellowColors[2];
            else if (idx == 1) itm.color = blueColors[2];
            else itm.color = greenColors[2];
        }

        foreach (var itm in DefaultBck)
            itm.gameObject.SetActive(false);
    }

    public void SetUncolor()
    {
        foreach (var itm in MainColor)
            itm.gameObject.SetActive(false);

        foreach (var itm in SecondaryColor)
            itm.gameObject.SetActive(false);

        foreach (var itm in ThirdColor)
            itm.gameObject.SetActive(false);
        
        foreach (var itm in DefaultBck)
            itm.gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

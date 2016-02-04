using UnityEngine;
using System.Collections;

public class menuButton : MonoBehaviour {

    GameObject shopPane;

	// Use this for initialization
	void Start () {
        shopPane = GameObject.Find("MenuPanel");
        shopPane.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if(shopPane.activeInHierarchy)
        {
            shopPane.SetActive(false);
        }
        else
        {
            shopPane.SetActive(true);
        }
    }
}

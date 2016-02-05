using UnityEngine;
using System.Collections;

public class AddBet : MonoBehaviour {

    // Use this for initialization
    pullLever script;

    void Start () {
        script = GameObject.Find("lever").GetComponent<pullLever>();
	}
	
	// Update is called once per frame
	void Update () { 
	}

    public void BetPlus()
    {

    }

    void OnMouseDown()
    {
        script.curBet += 1;
    }
}

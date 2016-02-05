using UnityEngine;
using System.Collections;

public class MinusBet : MonoBehaviour {

    private pullLever script;

	// Use this for initialization
	void Start () {
        script = GameObject.Find("lever").GetComponent<pullLever>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BetMinus()
    {
        script.curBet -= 1;
    }
}

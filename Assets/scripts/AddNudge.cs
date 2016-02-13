using UnityEngine;
using System.Collections;

public class AddNudge : MonoBehaviour {

    pullLever script;

	// Use this for initialization
	void Start () {
        script = GameObject.Find("lever").GetComponent<pullLever>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NudgePlus()
    {
        if((script.nudgePrice <= script.coins && script.nudges < script.maxNudges && script.curState == pullLever.stateTypes.SPINNING) || script.cheatsOn)
        {
            script.nudges += 1;
            if (!script.cheatsOn) script.coins -= script.nudgePrice;
        }
    }
}

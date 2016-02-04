using UnityEngine;
using System.Collections;

public class reelScript : MonoBehaviour {

    Transform myTransform;

	// Use this for initialization
	void Start () {
        myTransform = gameObject.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnMouseDown()
    {
        HingeJoint myHinge = gameObject.GetComponent<HingeJoint>();
        if(myHinge.velocity < 0.5)
        {
            GameObject lever = GameObject.Find("lever");
            pullLever script = lever.GetComponent<pullLever>();
            if((script.nudges > 0 && script.nudgesUsed != script.maxNudges) || script.cheatsOn)
            {
                myTransform.Rotate(0, (360f / script.slotDivisions), 0);
                int curReel = 0;
                for(int i = 0; i < 3; ++i)
                {
                    if (gameObject == script.reels[i]) curReel = i;
                }
                script.results[curReel] = (script.results[curReel] + 1) % script.slotDivisions;
                if(!script.cheatsOn) script.nudges -= 1;
                script.source.PlayOneShot(script.stop);
                script.CheckSlots();
            }
        }
    }
}

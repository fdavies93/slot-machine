using UnityEngine;
using System.Collections;

public class reelScript : MonoBehaviour {

    Transform myTransform;
    bool spinning = false;
    public float velocity = 1;//how many degrees to rotate per tick when spinning


	// Use this for initialization
	void Start () {
        myTransform = gameObject.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (spinning) MoveReel();
	}

    public void SetVelocity(float newV)
    {
        velocity = newV;
    }

    public void StartReel()
    {
        spinning = true;
    }

    public void StopReel()
    {
        spinning = false;
    }

    void MoveReel()
    {
        myTransform.Rotate(0,velocity,0);
    }

    void OnMouseDown()
    {
        HingeJoint myHinge = gameObject.GetComponent<HingeJoint>();
        if(myHinge.velocity < 0.5)
        {
            GameObject lever = GameObject.Find("lever");
            pullLever script = lever.GetComponent<pullLever>();
            //if(script.nudges > 0 && script.nudgesUsed != script.maxNudges)
            //{
                myTransform.Rotate(0, (360f / script.slotDivisions), 0);
                int curReel = 0;
                for(int i = 0; i < 3; ++i)
                {
                    if (gameObject == script.reels[i]) curReel = i;
                }
                script.results[curReel] = (script.results[curReel] + 1) % script.slotDivisions;
                script.nudges -= 1;
                script.CheckSlots();
            //}
        }
    }
}

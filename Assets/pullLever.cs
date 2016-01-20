using UnityEngine;
using System.Collections;

public class pullLever : MonoBehaviour {
    private GameObject[] reels;
    private int maxForce = 50;
    private int minForce = 50;
    private int maxVelocity = 500;
    private int minVelocity = 500;
    //all the above control randomness & feel of slot results
    private int coins = 10;
    private UnityEngine.UI.Text coinText;
    private UnityEngine.UI.Text winText;
    private int slotDivisions = 2;
    private stateTypes curState = stateTypes.READY; 
    private resultTypes[] slotList;//array of results beginning from zero and having number of entries equal to slotDivisions
    private float[] results;//gives the sectors of each reel for calculating results

    enum resultTypes
    {
        CHERRY,
        SEVEN,
        GOLD,
        MELON
    }

    enum stateTypes
    {
        READY,
        PAUSE,
        SPINNING,
        FIRSTSTOPPED,
        SECONDSTOPPED,
        ALLSTOPPED
    }
    // Use this for initialization
    void Start () {
        reels = new GameObject[3];
        reels[0] = GameObject.Find("topReel");
        reels[1] = GameObject.Find("centerReel");
        reels[2] = GameObject.Find("bottomReel");
        slotList = new resultTypes[4];
        slotList[0] = resultTypes.CHERRY;
        slotList[1] = resultTypes.GOLD;
        slotList[2] = resultTypes.SEVEN;
        slotList[3] = resultTypes.MELON;
        results = new float[3];
        GameObject coinDisplay = GameObject.Find("cashCounter");
        coinText = coinDisplay.GetComponent<UnityEngine.UI.Text>();
        GameObject winDisplay = GameObject.Find("victoryText");
        winText = winDisplay.GetComponent<UnityEngine.UI.Text>();
        coinText.text = "Coins: " + coins;
        winText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        coinText.text = "Coins: " + coins;
        HingeJoint curJoint;
        bool hasStopped = true;
        if(curState == stateTypes.ALLSTOPPED)
        {
            for(int i = 0; i < 3; i++)
            {
                curJoint = reels[i].GetComponent<HingeJoint>();

                if (curJoint.velocity > 0) hasStopped = false;
            }
            if (hasStopped)
            {
                //and check for a win
                CheckSlots();
                curState = stateTypes.READY;
            }
        }
    }

    void CheckSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            print("Reel " + i + " in sector " + results[i]);

            //sector determined, we can work out the sectors on either side with a simple +/- 1 % sectors & test for < 0

        }
        if(results[0] == results[1] && results[1] == results[2])
        {
            coins += 10;
            winText.text = "WIN!";
            StartCoroutine("wipeText");
            //print("WIN!");
        }
        else
        {
            //print("lose...");
        }
        //bring up win screen on match-3
    }

    IEnumerator wipeText()
    {
        yield return new WaitForSeconds(1);
        winText.text = "";
    }

    void stopMotor(GameObject toStop)
    {
        HingeJoint curHinge = toStop.GetComponent<HingeJoint>();
        JointSpring curSpring = curHinge.spring;
        curHinge.useMotor = false;
        curSpring.spring = 0;
        curHinge.spring = curSpring;
        curState = stateTypes.PAUSE;
        StartCoroutine("snapOnStop", toStop);
    }

    IEnumerator snapOnStop(GameObject toStop)
    {
        HingeJoint joint = toStop.GetComponent<HingeJoint>();
        Transform transform = toStop.GetComponent<Transform>();
        while (joint.velocity > 0.5) yield return null;//wait until stop
        int springSector = Mathf.FloorToInt((transform.localEulerAngles.y / 360) * slotDivisions);
        float targetPosition = (springSector * (360 / slotDivisions)) + (180 / slotDivisions);//centre on reel
        //snap to targetPosition
        transform.Rotate(new Vector3(0,targetPosition - transform.localEulerAngles.y, 0));
        yield return new WaitForFixedUpdate();//wait one tick for degree to auto-adjust
        if (joint == reels[0].GetComponent<HingeJoint>())
        {
            curState = stateTypes.FIRSTSTOPPED;
            results[0] = springSector;
        }
        else if (joint == reels[1].GetComponent<HingeJoint>())
        {
            curState = stateTypes.SECONDSTOPPED;
            results[1] = springSector;
        }
        else if (joint == reels[2].GetComponent<HingeJoint>())
        {
            curState = stateTypes.ALLSTOPPED;
            results[2] = springSector;
        }
    }

    void OnMouseDown()
    {
        if (curState == stateTypes.READY)
        {
            coins -= 1;
            StartCoroutine("spinReels");
        }
        else if (curState == stateTypes.SPINNING)
        {
            stopMotor(reels[0]);
        }
        else if (curState == stateTypes.FIRSTSTOPPED)
        {
            stopMotor(reels[1]);
        }
        else if (curState == stateTypes.SECONDSTOPPED)
        {
            stopMotor(reels[2]);
        }
    }

    IEnumerator spinReels()
    {
        curState = stateTypes.PAUSE;//i.e. getting ready to spin
        winText.text = "";
        HingeJoint curHinge;
        JointMotor curMotor;

        for(int i = 0; i < 3; i++)//get reels up to speed
        {
            curHinge = reels[i].GetComponent<HingeJoint>();
            curMotor = curHinge.motor;
            curMotor.force = minForce + (Random.value * (maxForce - minForce));
            curMotor.targetVelocity = minVelocity + (Random.value * (maxVelocity - minVelocity));
            curHinge.motor = curMotor;
            //randomness ensures different rates of rotation, creating a more interesting slot machine
            curHinge.useMotor = true;
        }
        yield return new WaitForSeconds(0.5f);//let the motor get up to speed
        curState = stateTypes.SPINNING;
    }
}

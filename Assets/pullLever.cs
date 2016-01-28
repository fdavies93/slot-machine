using UnityEngine;
using System.Collections;

public class pullLever : MonoBehaviour {
    public GameObject[] reels;
    private int maxForce = 50;
    private int minForce = 50;
    private int maxVelocity = 500;
    private int minVelocity = 500;
    //all the above control randomness & feel of slot results
    public int coins = 10;
    public int nudges = 100;
    public int nudgesUsed = 0;
    public int maxNudges = 3;
    private UnityEngine.UI.Text coinText;
    private UnityEngine.UI.Text nudgeText;
    private UnityEngine.UI.Text winText;
    public int slotDivisions = 16;
    private stateTypes curState = stateTypes.READY;
    private int[] resultWinnings;//how many coins you win for each resultType
    private resultTypes[,] slotList;//array of results beginning from zero and having number of entries equal to slotDivisions
    public int[] results;//gives the sectors of each reel for calculating results

    enum resultTypes
    {
        KARATE,
        DARTHVADER,
        STORMTROOPER,
        NINJA,
        BATMAN,
        COWBOY,
        PIRATE,
        ROBIN,
        TYPES
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
        GameObject menu = GameObject.Find("menuPanel");
        menu.SetActive(false);

        reels = new GameObject[3];
        reels[0] = GameObject.Find("topReel");
        reels[1] = GameObject.Find("centerReel");
        reels[2] = GameObject.Find("bottomReel");
        slotList = new resultTypes[3, slotDivisions];

        slotList[0,15] = resultTypes.PIRATE;
        slotList[0,0] = resultTypes.ROBIN;
        slotList[0,1] = resultTypes.PIRATE;
        slotList[0,2] = resultTypes.STORMTROOPER;
        slotList[0, 3] = resultTypes.KARATE;
        slotList[0, 4] = resultTypes.NINJA;
        slotList[0, 5] = resultTypes.ROBIN;
        slotList[0, 6] = resultTypes.PIRATE;
        slotList[0, 7] = resultTypes.ROBIN;
        slotList[0, 8] = resultTypes.DARTHVADER;
        slotList[0, 9] = resultTypes.BATMAN;
        slotList[0, 10] = resultTypes.COWBOY;
        slotList[0, 11] = resultTypes.STORMTROOPER;
        slotList[0, 12] = resultTypes.NINJA;
        slotList[0, 13] = resultTypes.BATMAN;
        slotList[0, 14] = resultTypes.COWBOY;

        slotList[1, 15] = resultTypes.PIRATE;
        slotList[1, 0] = resultTypes.PIRATE;
        slotList[1, 1] = resultTypes.COWBOY;
        slotList[1, 2] = resultTypes.NINJA;
        slotList[1, 3] = resultTypes.ROBIN;
        slotList[1, 4] = resultTypes.COWBOY;
        slotList[1, 5] = resultTypes.ROBIN;
        slotList[1, 6] = resultTypes.BATMAN;
        slotList[1, 7] = resultTypes.STORMTROOPER;
        slotList[1, 8] = resultTypes.PIRATE;
        slotList[1, 9] = resultTypes.ROBIN;
        slotList[1, 10] = resultTypes.NINJA;
        slotList[1, 11] = resultTypes.STORMTROOPER;
        slotList[1, 12] = resultTypes.BATMAN;
        slotList[1, 13] = resultTypes.KARATE;
        slotList[1, 14] = resultTypes.DARTHVADER;

        slotList[2, 15] = resultTypes.COWBOY;
        slotList[2, 0] = resultTypes.BATMAN;
        slotList[2, 1] = resultTypes.KARATE;
        slotList[2, 2] = resultTypes.COWBOY;
        slotList[2, 3] = resultTypes.BATMAN;
        slotList[2, 4] = resultTypes.ROBIN;
        slotList[2, 5] = resultTypes.PIRATE;
        slotList[2, 6] = resultTypes.NINJA;
        slotList[2, 7] = resultTypes.STORMTROOPER;
        slotList[2, 8] = resultTypes.PIRATE;
        slotList[2, 9] = resultTypes.ROBIN;
        slotList[2, 10] = resultTypes.NINJA;
        slotList[2, 11] = resultTypes.STORMTROOPER;
        slotList[2, 12] = resultTypes.PIRATE;
        slotList[2, 13] = resultTypes.DARTHVADER;
        slotList[2, 14] = resultTypes.ROBIN;

        resultWinnings = new int[(int)resultTypes.TYPES];

        resultWinnings[(int)resultTypes.KARATE] = 5;
        resultWinnings[(int)resultTypes.DARTHVADER] = 20;
        resultWinnings[(int)resultTypes.STORMTROOPER] = 10;
        resultWinnings[(int)resultTypes.NINJA] = 2;
        resultWinnings[(int)resultTypes.BATMAN] = 2;
        resultWinnings[(int)resultTypes.COWBOY] = 2;
        resultWinnings[(int)resultTypes.PIRATE] = 2;
        resultWinnings[(int)resultTypes.ROBIN] = 2;

        results = new int[3];
        GameObject coinDisplay = GameObject.Find("cashCounter");
        coinText = coinDisplay.GetComponent<UnityEngine.UI.Text>();
        GameObject winDisplay = GameObject.Find("victoryText");
        winText = winDisplay.GetComponent<UnityEngine.UI.Text>();
        GameObject nudgeDisplay = GameObject.Find("nudgeCounter");
        nudgeText = nudgeDisplay.GetComponent<UnityEngine.UI.Text>();
        coinText.text = "Coins: " + coins;
        winText.text = "";
        nudgeText.text = "Nudges: " + nudges;
    }
	
	// Update is called once per frame
	void Update () {
        coinText.text = "Coins: " + coins;
        nudgeText.text = "Nudges: " + nudges;
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

    public void CheckSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            //print("Reel " + i + " in sector " + results[i]);
            //print("Reel " + i + " has enum value " + slotList[i, results[i]]);
            //sector determined, we can work out the sectors on either side with a simple +/- 1 % sectors & test for < 0

        }
        if(slotList[0,results[0]] == slotList[1, results[1]] && slotList[1, results[1]] == slotList[2, results[2]])
        {
            coins += resultWinnings[(int)slotList[0,results[0]]];
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

    public IEnumerator snapOnStop(GameObject toStop)
    {
        HingeJoint joint = toStop.GetComponent<HingeJoint>();
        Transform transform = toStop.GetComponent<Transform>();
        while (joint.velocity > 0.5) yield return null;//wait until stop
        int springSector = Mathf.FloorToInt((transform.eulerAngles.y / 360) * slotDivisions);
        float targetPosition = (springSector * (360 / slotDivisions)) + (180 / slotDivisions);//centre on reel
        //snap to targetPosition
        transform.Rotate(new Vector3(0,targetPosition - transform.eulerAngles.y, 0));
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
            nudgesUsed = 0;
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

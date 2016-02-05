using UnityEngine;
using System.Collections;

public class pullLever : MonoBehaviour {
    //because of serialisation, basically all public variables are static
    public GameObject[] reels;
    public float hasteFactor = 2;
    public float slowFactor = 2;
    public float crazyFactor = 2;
    public float maximumForce = 100;
    public float minimumForce = 100;
    public float maximumVelocity = 500;
    public float minimumVelocity = 500;

    //these are for actually manipulating in run time
    private float maxForce;
    private float minForce;
    private float maxVelocity;
    private float minVelocity;
    //all the above control randomness & feel of slot results
    public int coins = 10;
    public int nudges = 100;
    public int nudgesUsed = 0;
    public int maxNudges = 3;
    public int curBet = 4;
    private UnityEngine.UI.Text coinText;
    private UnityEngine.UI.Text nudgeText;
    private UnityEngine.UI.Text winText;
    private UnityEngine.UI.Text betText;
    public int slotDivisions = 16;
    private stateTypes curState = stateTypes.READY;
    private int[] resultWinnings;//how many coins you win for each resultType
    private resultTypes[,] slotList;//array of results beginning from zero and having number of entries equal to slotDivisions
    public int[] results;//gives the sectors of each reel for calculating results
    private bool[] activeFlags;

    public bool cheatsOn = true;//gives unlimited nudges and coins

    [System.NonSerialized]
    public AudioSource source;
    [System.NonSerialized]
    public AudioClip lever;
    [System.NonSerialized]
    public AudioClip stop;

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

    enum modifierTypes
    {
        SLOWREELS,//reels go slower; some cost
        FASTREELS,//reels go faster; no cost
        GOLDREELS,//only need 2 reels to win
        THREEROWS,//the rows to either side of the center also count; probably 2 coin cost
        CRAZYREELS,//multiplies range of force + velocity; no cost
        TYPES
    }

    // Use this for initialization
    void Start () {
        reels = new GameObject[3];
        reels[0] = GameObject.Find("topReel");
        reels[1] = GameObject.Find("centerReel");
        reels[2] = GameObject.Find("bottomReel");
        slotList = new resultTypes[3, slotDivisions];

        //this is bad, but it seemed pointless to put this in data when unity
        //doesn't have a firm division of code + data anyway
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
        GameObject betDisplay = GameObject.Find("BetText");
        betText = betDisplay.GetComponent<UnityEngine.UI.Text>();
        coinText.text = "Coins: " + coins;
        winText.text = "";
        nudgeText.text = "Nudges: " + nudges;
        activeFlags = new bool[(int)modifierTypes.TYPES];
        //activeFlags[(int)modifierTypes.FASTREELS] = true;
        //activeFlags[(int)modifierTypes.SLOWREELS] = true;
        //activeFlags[(int)modifierTypes.THREEROWS] = true;
        //activeFlags[(int)modifierTypes.GOLDREELS] = true;
        //gold and three rows exclusive
        //activeFlags[(int)modifierTypes.CRAZYREELS] = true;
        maxForce = maximumForce;
        minForce = minimumForce;
        maxVelocity = maximumVelocity;
        minVelocity = minimumVelocity;

        source = gameObject.GetComponent<AudioSource>();
        lever = Resources.Load<AudioClip>("sound/SE/lever");//change this to a dict / something more elegant
        stop = Resources.Load<AudioClip>("sound/SE/stop");
    }
	
	// Update is called once per frame
	void Update () {
        coinText.text = "Coins: " + coins;
        nudgeText.text = "Nudges: " + nudges;
        betText.text = "Bet: " + curBet;
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
    
    void checkRow(int[] resultArray)
    {
        if (activeFlags[(int)modifierTypes.GOLDREELS])
        {
            bool stop = false;
            for(int i = 0; i < 2; ++i)
            {
                for(int i2 = i+1; i2 < 3; ++i2)
                {
                    if((slotList[i, resultArray[i]] == slotList[i2, resultArray[i2]]))
                    {
                        print("win on " +i+ " and " +i2);
                        coins += resultWinnings[(int)slotList[i, resultArray[i]]] * curBet;
                        winText.text = "WIN!";
                        nudges = 0;
                        StartCoroutine("wipeText");
                        stop = true;
                        break;
                    }
                }
                if (stop) break;
            }
        }
        else
        {
            if (slotList[0, resultArray[0]] == slotList[1, resultArray[1]] && slotList[1, resultArray[1]] == slotList[2, resultArray[2]])
            {
                coins += resultWinnings[(int)slotList[0, resultArray[0]]] * curBet;
                winText.text = "WIN!";
                nudges = 0;
                StartCoroutine("wipeText");
            }
        }
    }

    public void CheckSlots()
    {
        checkRow(results);
        if(activeFlags[(int)modifierTypes.THREEROWS])
        {
            print("three rows");
            int[] tempResults = new int[3];
            for(int i = 0; i < 3; ++i)
            {
                tempResults[i] = results[i] - 1;
                if (tempResults[i] < 0) tempResults[i] = slotDivisions + tempResults[i];
            }
            checkRow(tempResults);
            for(int i = 0; i < 3; ++i) tempResults[i] = (results[i] + 1) % slotDivisions;
            checkRow(tempResults);
        }
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
        //curHinge.spring = curSpring;
        curState = stateTypes.PAUSE;
        StartCoroutine("snapOnStop", toStop);
    }

    public IEnumerator snapOnStop(GameObject toStop)
    {
        yield return new WaitForFixedUpdate();
        HingeJoint joint = toStop.GetComponent<HingeJoint>();
        Transform transform = toStop.GetComponent<Transform>();
        while (joint.velocity > 0.5) yield return null;//wait until stop
        int springSector = Mathf.FloorToInt((transform.eulerAngles.y / 360f) * slotDivisions);
        float targetPosition = (springSector * (360f / slotDivisions)) + (180f / slotDivisions);//centre on reel
        transform.Rotate(new Vector3(0,targetPosition - transform.eulerAngles.y, 0));
        source.PlayOneShot(stop);
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
            maxForce = maximumForce;
            maxVelocity = maximumVelocity;
            minForce = minimumForce;
            minVelocity = minimumVelocity;
            results[2] = springSector;
        }
    }

    void OnMouseDown()
    {
        if (curState == stateTypes.READY)
        {
            if(!cheatsOn) coins -= 1;
            nudges = 0;
            source.PlayOneShot(lever);
            for (int i = 0; i < (int)modifierTypes.TYPES; ++i)
            {
                if(!cheatsOn) activeFlags[i] = false;
            }
            StartCoroutine("spinReels");
            
        }
        else if (curState == stateTypes.SPINNING)
        {
            source.PlayOneShot(stop);
            stopMotor(reels[0]);
        }
        else if (curState == stateTypes.FIRSTSTOPPED)
        {
            source.PlayOneShot(stop);
            stopMotor(reels[1]);
        }
        else if (curState == stateTypes.SECONDSTOPPED)
        {
            source.PlayOneShot(stop);
            stopMotor(reels[2]);
        }
    }

    IEnumerator spinReels()
    {
        curState = stateTypes.PAUSE;//i.e. getting ready to spin
        winText.text = "";
        reelScript curScript;

        HingeJoint curHinge;
        JointMotor curMotor;
        if(activeFlags[(int)modifierTypes.SLOWREELS])
        {
            minForce /= slowFactor;
            maxForce /= slowFactor;
            minVelocity /= slowFactor;
            maxVelocity /= slowFactor;
        }
        if(activeFlags[(int)modifierTypes.FASTREELS])
        {
            minForce *= hasteFactor;
            maxForce *= hasteFactor;
            minVelocity *= hasteFactor;
            maxVelocity *= hasteFactor;
        }
        if(activeFlags[(int)modifierTypes.CRAZYREELS])
        {
            minForce /= crazyFactor;
            maxForce *= crazyFactor;
            minVelocity /= crazyFactor;
            maxVelocity *= crazyFactor;
        }
        //only this part activates reels
        
        //TRANSLATE METHOD
        /*for (int i = 0; i < 3; ++i)
        {
            curScript = reels[i].GetComponent<reelScript>();
            curScript.SetVelocity(minVelocity + (Random.value * (maxVelocity - minVelocity)));
            curScript.StartReel();
        }*/
        //PHYSICS METHOD
        for (int i = 0; i < 3; i++)//get reels up to speed
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

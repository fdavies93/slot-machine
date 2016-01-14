using UnityEngine;
using System.Collections;

public class pullLever : MonoBehaviour {

    private GameObject[] reels;
    private int maxForce = 100;
    private int minForce = 20;
    private int maxVelocity = 800;
    private int minVelocity = 200;
    //all the above control randomness & feel of slot results
    private int slotDivisions = 2;
    private resultTypes[] slotList;//array of results beginning from zero and having number of entries equal to slotDivisions

    private bool justSpun;

    enum resultTypes
    {
        CHERRY,
        SEVEN,
        GOLD,
        MELON
    }
    // Use this for initialization
    void Start () {
        reels = new GameObject[3];
        reels[0] = GameObject.Find("leftReel");
        reels[1] = GameObject.Find("centerReel");
        reels[2] = GameObject.Find("rightReel");
        justSpun = false;
        slotList = new resultTypes[2];
        slotList[0] = resultTypes.CHERRY;
        slotList[1] = resultTypes.GOLD;
        slotList[2] = resultTypes.SEVEN;
        slotList[3] = resultTypes.MELON;
    }
	
	// Update is called once per frame
	void Update () {
        Rigidbody curBody;
        bool hasStopped = true;
        if(justSpun)
        {
            for(int i = 0; i < 3; i++)
            {
                curBody = reels[i].GetComponent<Rigidbody>();
                if (curBody.angularVelocity.y > 0) hasStopped = false;
            }
            if (hasStopped)
            {
                justSpun = false;
                //and check for a win
                CheckSlots();
            }
            //test current speed of objects
        }
    }

    void CheckSlots()
    {
        Transform curTransform;
        Vector3 curEuler;
        float theta;
        int curSector = 0;
        for (int i = 0; i < 3; i++)
        {
            curTransform = reels[i].GetComponent<Transform>();
            //curTransform.rotation.ToAngleAxis();
            curEuler = curTransform.right;
            theta = Mathf.Atan2(curEuler.y, curEuler.z);
            if (theta < 0) theta = (Mathf.PI * 2) + theta;
            curSector = Mathf.FloorToInt(slotDivisions * (theta / (Mathf.PI * 2)));
            //print("Reel " + i + " at " + theta + " radians (" + (theta / (Mathf.PI * 2)) * 360 + " degrees) relative to x axis.");
            print("Reel " + i + " in sector " + curSector);
        }
    }

    void OnMouseDown()
    {
        StartCoroutine("spinReels");
    }

    IEnumerator spinReels()
    {
        HingeJoint curHinge;
        JointMotor curMotor;

        for(int i = 0; i < 3; i++)//get reels up to speed
        {
            curHinge = reels[i].GetComponent<HingeJoint>();
            curMotor = curHinge.motor;
            curMotor.force = minForce + (Random.value * (maxForce - minForce));
            curMotor.targetVelocity = minVelocity + (Random.value * (maxVelocity - minVelocity));
            curHinge.motor = curMotor;
            //ensures different rates of rotation, creating a more interesting slot machine
            curHinge.useMotor = true;
        }
        yield return new WaitForSeconds(2);//let the motor get up to speed
        justSpun = true;
        for(int i = 0; i < 3; i++)//slow reels to halt
        {
            curHinge = reels[i].GetComponent<HingeJoint>();
            curHinge.useMotor = false;
        }
    }
}

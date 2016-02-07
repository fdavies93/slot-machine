using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0, 2, 0);    
	}
}

﻿using UnityEngine;
using System.Collections;

public class HighScoreController : MonoBehaviour {

    PlayerCollider playerScript;
    public int keepScore;
    void Awake ()
{
    DontDestroyOnLoad(this);
    GameObject player = GameObject.FindWithTag("Player"); //create reference for Player gameobject, and assign the variable via FindWithTag at start
    if (player != null) // if the playerObject gameObject-reference is not null - assigning the reference via FindWithTag at first frame -
    {
        playerScript = player.GetComponent<PlayerCollider>();// - set the PlayerController-reference (called playerControllerScript) to the <script component> of the Player gameobject (via the gameObject-reference) to have access the instance of the PlayerController script
    }
    if (player == null) //for exception handling - to have the console debug the absense of a player controller script in order for this entire code, the code in the GameController to work
    {
        Debug.Log("Cannot find ScoreController script for final score referencing to GameOver - finalAcquired Label");
    }
}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        keepScore = playerScript.scoreValue;
	}
}

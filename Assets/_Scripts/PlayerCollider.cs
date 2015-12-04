/* Author: Arunan Shan */
/* File: PlayerCollider.cs */
/* Creation Date: Oct 19, 2015 */
/* Description: Controls the score & collider with object*/
/* Last Modified by: Monday October 25, 2015 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerCollider : MonoBehaviour {

	//PUBLIC INSTANCE VARIABLES
	public Text scoreLabel;
	public Text livesLabel;
	public Text gameOverLabel;
    public Text winLabel;
	public Text finalScoreLabel;
	public Text restartLabel;
	public int  scoreValue = 0;
	public int  livesValue = 3;

    private bool restart;
    //private Animator _animator;

	// Use this for initialization
	void Start () {
        restart = false;
		this._SetScore ();
		this.gameOverLabel.enabled = false; // Hides end game text 
		this.finalScoreLabel.enabled = false;
		this.restartLabel.enabled = false;
        this.winLabel.enabled = false;
        //this._animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	if(restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
            Application.LoadLevel(Application.loadedLevel);
            }
		}
	}

	void OnTriggerEnter2D(Collider2D otherGameObject) {
		if (otherGameObject.tag == "GoldCoin") {
			this.scoreValue += 10; // add 10 points
		}


		if (otherGameObject.tag == "Heart") {
			this.livesValue += 1; 
		}


		if (otherGameObject.tag == "Death") {
			this._EndGame();
		}
        if(otherGameObject.tag == "Portal")
        {
            this._WinGame();
        }
		this._SetScore ();
	}

	void OnCollisionEnter2D(Collision2D otherGameObject)
	{

		if (otherGameObject.gameObject.CompareTag ("Snake")) {
			this.livesValue--; // remove one life
			if(this.livesValue <= 0) {
				
				this._EndGame();
			}
		}

		this._SetScore ();
	}

	// PRIVATE METHODS
	private void _SetScore() {
		this.scoreLabel.text = "Score: " + this.scoreValue;
		this.livesLabel.text = "Lives: " + this.livesValue;
	}
	//ends game displays game over text
	private void _EndGame() {
		Destroy(gameObject);

		this.scoreLabel.enabled = false;
		this.livesLabel.enabled = false;
		this.gameOverLabel.enabled = true; // Makes game over, final score, restart text appear when game ends 
		this.finalScoreLabel.enabled = true;
		this.restartLabel.enabled = true;
		this.finalScoreLabel.text = "Final Score: " + this.scoreValue;

	}

    private void _WinGame()
    {
        Destroy(gameObject);

        this.scoreLabel.enabled = false;
        this.livesLabel.enabled = false;
        this.winLabel.enabled = true; // Makes game over, final score, restart text appear when game ends 
        this.finalScoreLabel.enabled = true;
        this.restartLabel.enabled = true;
        this.finalScoreLabel.text = "Final Score: " + this.scoreValue;

    }


}

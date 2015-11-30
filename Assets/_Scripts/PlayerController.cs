/* Author: Arunan Shan */
/* File: PlayerController.cs */
/* Creation Date: Oct 19, 2015 */
/* Description: Controls player movement, audio and animation*/
/* Last Modified by: Monday October 25, 2015 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//VELOCITY RANGE
[System.Serializable]
public class VelocityRange {
	public float vMin, vMax;

	public VelocityRange(float vMin, float vMax)
	{
		this.vMin = vMin;
		this.vMax = vMax;
	}
	}

public class PlayerController : MonoBehaviour {
	//PUBLIC INSTANCE VARIABLES
	public float speed = 50f;
	public float jump = 500f;


	public VelocityRange velocityRange = new VelocityRange (300f, 1000f);



	//PRIVATE INSTANCE VARIABLES
	private Rigidbody2D _rigidBody2D;
	private Transform _transform;
	private Animator _animator;
	private AudioSource[] _audioSources;
	private AudioSource _goldCoinSound;
	private AudioSource _jumpSound;
	private AudioSource _hitSound;
	private AudioSource _lifeSound;
	private AudioSource _gameOverSound;
	
	private float _movingValue = 0;
	private bool _isFacingRight = true;
	private bool _isGrounded =true;





	// Use this for initialization
	void Start () {
		this._rigidBody2D = gameObject.GetComponent<Rigidbody2D> ();
		this._transform = gameObject.GetComponent<Transform> ();
		this._animator = gameObject.GetComponent<Animator> ();

		//Audio Array
		this._audioSources = gameObject.GetComponents<AudioSource> ();
		this._goldCoinSound = this._audioSources [0];
		this._jumpSound = this._audioSources [1];
		this._hitSound = this._audioSources [2];
		this._lifeSound = this._audioSources [3];
		this._gameOverSound = this._audioSources [4];




	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float forceX = 0f;
		float forceY = 0f;

		float absVelX = Mathf.Abs (this._rigidBody2D.velocity.x);
		float absVelY = Mathf.Abs (this._rigidBody2D.velocity.y);

		this._movingValue = Input.GetAxis ("Horizontal"); //value is between -1 and 1

		//check is player is moving

		if (this._movingValue != 0) {
			//we're moving
			this._animator.SetInteger("AnimState", 1); //play walk clip
			if(this._movingValue > 0)
			{
				//moving right
				if (absVelX < this.velocityRange.vMax){
					 forceX = this.speed;
					this._isFacingRight = true;
					this._flip();
				}
			}
			else if (this._movingValue < 0)
			{
				//moving left 
				if (absVelX < this.velocityRange.vMax){
					forceX = -this.speed;
					this._isFacingRight = false;
					this._flip();
				}
			}
		} else if (this._movingValue == 0) {
			//we're idle
			this._animator.SetInteger("AnimState", 0); 
		}

		//Check if player is jumping

		if (Input.GetKey ("up") || Input.GetKey (KeyCode.W)) {
			//check if player is grounded
			if(this._isGrounded){
			//player is jumping
				this._animator.SetInteger("AnimState", 2);
			if (absVelY < this.velocityRange.vMax) {
				forceY = this.jump;
					this._jumpSound.Play ();
					this._isGrounded = false;
			}
			}
		}
		
		this._rigidBody2D.AddForce (new Vector2 (forceX, forceY));


	}
	void OnTriggerEnter2D(Collider2D otherGameObject) 
	{
		if (otherGameObject.tag == "GoldCoin") {
			this._goldCoinSound.Play (); //play coin sound

		}
////		if (otherGameObject.tag == "Snake") {
////			this._animator.SetInteger("AnimState", 3);
////			this._hitSound.Play (); //play hit sound
////
////			
//		}
		if (otherGameObject.tag == "Heart") {
			this._lifeSound.Play (); //play life sound
			
		}
		if (otherGameObject.tag == "Death") {
			this._gameOverSound.Play ();
		}
	}

	void OnCollisionEnter2D(Collision2D otherGameObject)
	{
		if (otherGameObject.gameObject.CompareTag ("Snake")) {
			this._animator.SetInteger("AnimState", 3);
			this._hitSound.Play (); //play hit sound
			
			
		}
	}


	void OnCollisionStay2D(Collision2D otherCollider)
	{
		if (otherCollider.gameObject.CompareTag ("Platform")) {
			this._isGrounded = true;
		}

	}

	//flips player sprite
	private void _flip(){
		if(this._isFacingRight){
			this._transform.localScale = new Vector3(1f, 1f, 1f);// flip back to normal
		}
		else{
			this.transform.localScale = new Vector3(-1f, 1f, 1f);//flip other way
			}
	}


}

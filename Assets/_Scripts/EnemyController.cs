using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    // PUBLIC INSTANCE VARIABLES
    public float speed = 100f;
    public int hit;

    //public Transform sightStart;
    public Transform sightEnd;

    //public GameObject shot;
    //public Transform shotSpawn; //this variable is a refernece of the game object Shot Spawn but the variable type only references its transform component

    // PRIVATE INSTANCE VARIABLES
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private Animator _animator;
    private PolygonCollider2D _enemyCollider;
    private bool _isGrounded = false;
    private bool _isGroundAhead = true;

    private AudioSource[] _audioSources;
    private AudioSource _hit;
    private AudioSource _death;

    public bool blackWolf;
    public Transform target;

    private float _distanceFromTarget;
    void Awake()
    {
        if(blackWolf)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }
    // Use this for initialization
    void Start()
    {
        this._rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        this._transform = gameObject.GetComponent<Transform>();
        this._animator = gameObject.GetComponent<Animator>();
        this._enemyCollider = gameObject.GetComponent<PolygonCollider2D>();

        this._audioSources = gameObject.GetComponents<AudioSource>();
        this._hit = this._audioSources[0];
        this._death = this._audioSources[1];
    }

    void Update()
    {/*
        if (Time.time > nextFire) //shoot every 0.5 sec by - game time > nextFire = 0
        {
            nextFire = Time.time + fireRate; // then update nextFire = gametime (now 0.2) + fireRate (0.25) --> then when game time is 0.27 > nextFire (0.26) and fire button is held = shoot Bolt prefab via the reference shot gameObject 
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);//instantiate the game object shot per frame at a held key press, set at a vector3 position, at a set quaternion euler (rotation)
            shot.GetComponent<AudioSource>().playOnAwake = true;//upon instantiating the (shot), if the audio isn't playing on awake (on the very first frame), play this audio clip
        }*/
        if(blackWolf)
        {
            this._distanceFromTarget = Vector3.Distance(this._transform.position, this.target.position);
            Debug.Log(this._distanceFromTarget);
            if (this._distanceFromTarget < 275)
            {
                this.speed = 400f;
            /*// move towards the target
            this._transform.position = Vector3.MoveTowards(this._transform.position, target.position, this.speed);

            // look at the target
            Vector3 targetDir = this.target.position - this._transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, this.speed, 0.0F);
            this._transform.rotation = Quaternion.LookRotation(newDir);*/
            }
            else 
            {
                this.speed = 200f;
            }
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // check if enemy is grounded
        if (this._isGrounded)
        {
            this._animator.SetInteger("AnimState", 0); // play walking animation -
            this._rigidbody2D.velocity = new Vector2(this._transform.localScale.x, 0) * -this.speed; // and have enemy's velocity go forward by speed

            this._isGroundAhead = Physics2D.Linecast(_transform.position, this.sightEnd.position, 1 << LayerMask.NameToLayer("Ground")); // linecast between enemy's transform and the ground's - returns a boolean value
            Debug.DrawLine(_transform.position, this.sightEnd.position);

            if (this._isGroundAhead == false) // when the line cast is past the end position of the ground layer == false
            {
                this._flip(); // flip enemy local scale (invert sprite and gameobject's direction)
            }

        }
        else //if enemy is not grounded -
        {
            //this._animator.SetInteger("AnimState", 1); // play idle animation
        }
    }

    void OnCollisionEnter2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Snake"))
        {
            this._flip();
        }

        if (otherCollider.gameObject.CompareTag("Wolf"))
        {
            this._flip();
        }

        if (otherCollider.gameObject.CompareTag("Black Wolf"))
        {
            this._flip();
        }

        if (otherCollider.gameObject.CompareTag("Turok"))
        {
            this._flip();
        }

        if (otherCollider.gameObject.CompareTag("Arrow"))
        {
            Destroy(otherCollider.gameObject);
            hit--;
            //enemy hit sound
            if (hit <= 0)
            {
                this._death.Play();
                this._enemyCollider.isTrigger = true; //so it doesn't check a collision with collision2d - colliding with the player after being hit, let the death animatoin play-out before destroying enemy object
                this.speed = 0f;
                this._animator.SetInteger("AnimState", 1); // play death animation
                Invoke("_blackWolfDeathAnimationTransition", 0.22f);
                Destroy(gameObject, 0.8f);
            }
            else
            {
                this._hit.Play();
                this._animator.SetInteger("AnimState", 2); // play hit animation
            }
        }

    }

    private void _blackWolfDeathAnimationTransition()
    {
        if (this.gameObject.CompareTag("Black Wolf"))
        {
            this.gameObject.transform.position = new Vector2(_transform.position.x, (_transform.position.y - 30));
        }
    }

    void OnCollisionStay2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Platform"))
        {
            this._isGrounded = true;
        }
    }

    void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Platform"))
        {
            this._isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Platform"))
        {
            this._isGrounded = false;
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Platform"))
        {
            this._isGrounded = false;
        }
    }
    // PRIVATE METHODS
    private void _flip()
    {
        if (this._transform.localScale.x == 1)
        {
            this._transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            this._transform.localScale = new Vector3(1f, 1f, 1f); // reset to normal scale
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	private Animator anim;
	public GameObject explosion;
	public float speed = 1f;
	private bool canMove = true;

     
	public ParticleSystem rightMuzzle, leftMuzzle, rightFire, leftFire;

	private ParticleSystem.EmissionModule right_Muzzle_Emission, left_Muzzle_Emission,
	   right_Fire_Emission, left_Fire_Emission;


    private AudioSource audioManager;
    private bool canShoot;

     
	// Start is called before the first frame update
	void Start()
	{
        anim = GetComponentInChildren<Animator>();
        anim.Play("Walk");
        audioManager = GetComponent<AudioSource>();


        right_Muzzle_Emission = rightMuzzle.emission;
        right_Fire_Emission = rightFire.emission;
        right_Muzzle_Emission.rateOverTime = 0f;
        right_Fire_Emission.rateOverTime = 0f;

        left_Muzzle_Emission = leftMuzzle.emission;
        left_Fire_Emission = leftFire.emission;
        left_Muzzle_Emission.rateOverTime = 0f;
        left_Fire_Emission.rateOverTime = 0f;

    }

	// Update is called once per frame
	void Update()
	{
        Move();
        CheckToShoot();
	}
    void Move()
    {
        if (canMove)
        {
            transform.Translate(Vector3.forward * speed *Time.deltaTime);

            if (!isGrounded() || checkFront ())
            {
                anim.Play("Idle");
                canMove = false;
                LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f,0.5f).setOnComplete(CompleteMove);
            }

        }
    }
    bool isGrounded()
    {
        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f,
            Vector3.down, 0.1f);
    }

    bool checkFront()
    {
        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f,
            transform.forward, 0.1f);
    }

    void CompleteMove() {

        anim.Play("Walk");
        canMove = true;
    }
    void CheckToShoot()
    {
        if (canShoot)
        {
            if (!audioManager.isPlaying)
            {
                audioManager.Play();
            }
            right_Muzzle_Emission.rateOverTime = left_Muzzle_Emission.rateOverTime = 10f;
            right_Fire_Emission.rateOverTime = left_Fire_Emission.rateOverTime = 30f;
        }
        else
        {    
            audioManager.Stop();

            right_Muzzle_Emission.rateOverTime = left_Muzzle_Emission.rateOverTime = 0f;
            right_Fire_Emission.rateOverTime = left_Fire_Emission.rateOverTime = 0f;
        }
    }

    void Damage()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnParticleCollision(GameObject target)
    {
        if (target.name == "Muzzle")
        {
            Damage(); 
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.name == "Robot")
        {
            canShoot = true;
        }
    }
    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.name == "Robot")
        {
            canShoot = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update

    void LaunchMissile(Vector3 tarPos) {
        Invoke("setActive", 0.9f);

        LeanTween.move(gameObject, tarPos, 1.6f).setEase(LeanTweenType.easeInBack).setOnComplete(Explode);


    }
    void setActive()
    {
        GetComponent<Collider>().enabled = true;
    }

    void Explode() {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (target.tag == "Box")
            {
                target.GetComponent<Rigidbody>().AddExplosionForce(2000f, transform.position, 6f);
            }
            else if (target.tag == "Enemy")
            {
                target.SendMessage("Damage");
            }
            Explode();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

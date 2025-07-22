using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatientController : MonoBehaviour
{
    [SerializeField] Rigidbody mainRB;
    [SerializeField] Collider mainCollider;
    [SerializeField] int patientLives = 5;
    [SerializeField] List<GameObject> limbs;
    [SerializeField] GameObject head;
    [SerializeField] GameObject spine1;
    [SerializeField] GameObject spine2;

    public UnityEvent OnDeath;


    private void Start()
    {
        //Invoke("SpawnLevel", 3);
        // mainCollider.enabled = false;
        // mainRB.isKinematic = true;
    }

    public void Explode()
    {
        Debug.Log("Explode");
        foreach(GameObject limb in limbs)
        {
            limb.SetActive(false);
        }
        head.SetActive(false);
        spine1.SetActive(false);
        spine2.SetActive(false);
        mainCollider.enabled = true;
        mainRB.isKinematic = false;
        mainRB.AddExplosionForce(1000f, transform.position, 10f);
        AudioManager.instance.PlaySound(Sound.Explode);

        // foreach(GameObject level in easyLevels)
        // {
        //     level.SetActive(true);
        // }
        // head.SetActive(true);
    }


    public void TakeDamage()
    {
        patientLives -= 1;        

        if(patientLives <= 0)
        {
            Debug.Log("Patient is dead.");
            if(head.GetComponent<PatientJoint>() != null)
            {
                head.GetComponent<PatientJoint>().TearOffLimb();
            }
            OnDeath?.Invoke();
            GameStateManager.instance.PatientDeath();
            return;
            //END GAME
        }

        PopoffLimb();
    }


    void PopoffLimb()
    {
        //popoff random limb.
        int randomNumber = Random.Range(0, limbs.Count);
        GameObject limb = limbs[randomNumber];

        if(limb.GetComponent<PatientJoint>() != null)
        {
            limb.GetComponent<PatientJoint>().TearOffLimb();
        }
        //limb.SetActive(false);
        limbs.RemoveAt(randomNumber);
        AudioManager.instance.PlaySound(Sound.LimbPopOff);
    }

}

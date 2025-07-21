using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatientController : MonoBehaviour
{
    [SerializeField] AimController aimController;
    [SerializeField] Rigidbody mainRB;
    [SerializeField] Collider mainCollider;
    [SerializeField] int patientLives = 5;
    [SerializeField] List<GameObject> limbs;
    [SerializeField] Transform levelSpawnPos;
    [SerializeField] List<IndividualLevelController> easyLevels;
    [SerializeField] List<GameObject> mediumLevels;
    [SerializeField] List<GameObject> hardLevels;
    [SerializeField] GameObject head;
        [SerializeField] GameObject spine1;
    [SerializeField] GameObject spine2;

    public UnityEvent OnDeath;


    private void Start()
    {
        Invoke("SpawnLevel", 3);
        mainCollider.enabled = false;
        mainRB.isKinematic = true;
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

        // foreach(GameObject level in easyLevels)
        // {
        //     level.SetActive(true);
        // }
        // head.SetActive(true);
    }

    void SpawnLevel()
    {
        IndividualLevelController level = easyLevels[0];
        if(level == null) return;

        level.gameObject.SetActive(true);
        if(aimController != null && level.GetInteractable() != null)
        {
            aimController.SetCurrentInteractable(level.GetInteractable().transform);
        }



        // if(GameStateManager.instance != null)
        // {
        //     int patientsSaved = GameStateManager.instance.GetPatientsSaved();

        //     if(patientsSaved < 3)
        //     {
        //         int randomNumber = Random.Range(0, easyLevels.Count);
        //         easyLevels[randomNumber].SetActive(true);
        //     }
        //     else if(patientsSaved < 6)
        //     {
        //         int randomNumber = Random.Range(0, mediumLevels.Count);
        //         mediumLevels[randomNumber].SetActive(true);
        //     }
        //     else
        //     {
        //         int randomNumber = Random.Range(0, hardLevels.Count);
        //         hardLevels[randomNumber].SetActive(true);
        //     }
        // }
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
    }

}

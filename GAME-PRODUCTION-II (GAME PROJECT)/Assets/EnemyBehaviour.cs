using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public Transform target;  // Assign this in the Inspector (e.g., Player)
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < -5)
        {
            gameObject.transform.position = new Vector3(-2.1f,2.75f, -194.6f);
            Debug.Log("Respawn Enemy, they fell off the map");
        }

        //Basic seek

    
        if (target != null)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Rotate to face the target
            transform.LookAt(target);
        }

    }

   

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(5.71f,0f,0f);
        Debug.Log("RRRAAAAAAAAAAAH");

    }

}

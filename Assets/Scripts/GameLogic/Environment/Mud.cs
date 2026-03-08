using System.Collections;
using UnityEngine;

public class Mud : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player stepped on mud!");
            StartCoroutine(StartSelfDestruct());
        }
    }
    IEnumerator StartSelfDestruct()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        
    }
}

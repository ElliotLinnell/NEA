using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public GameObject jumpscare;
    public Canvas canvas;
    public AudioSource jumpscareSound;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            jumpscare.SetActive(true);
            canvas.gameObject.SetActive(true);
            jumpscareSound.Play();
            WaitForSeconds wait = new WaitForSeconds(5);
            StartCoroutine(DisableJumpscare(wait));
        }
    }
    IEnumerator DisableJumpscare(WaitForSeconds wait)
    {
        yield return wait;
        jumpscare.SetActive(false);
        canvas.gameObject.SetActive(false);
    }
}

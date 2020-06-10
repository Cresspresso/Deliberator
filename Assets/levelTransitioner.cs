using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp /// </author>
public class levelTransitioner : MonoBehaviour
{
    public int levelIndex;

    public Animator fadeAnim;

    // Start is called before the first frame update
    void Start()
    {
        fadeAnim = GameObject.FindGameObjectWithTag("SceneFader").GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            FadeToLevel();
        }
    }

    public void FadeToLevel ()
    {
        fadeAnim.SetTrigger("Fade");
        StartCoroutine(WaitToTransition());
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator WaitToTransition()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(levelIndex);
    }
}

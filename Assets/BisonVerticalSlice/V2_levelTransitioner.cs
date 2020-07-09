using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp /// </author>
public class V2_levelTransitioner : MonoBehaviour
{
    public string sceneName = "Ang_MenuScene";

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

    //for QuitGameButton
    public void QuitGame()
    {
        fadeAnim.SetTrigger("Fade");
        StartCoroutine(WaitToQuit());
    }

    IEnumerator WaitToTransition()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator WaitToQuit()
    {
        yield return new WaitForSeconds(1.0f);
        V2_Quitter.StaticQuit();
    }
}

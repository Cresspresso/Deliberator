using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class V2_pressItemSpawner : MonoBehaviour
{
    public GameObject blueItem;
    public GameObject redItem;
    public GameObject greenItem;

    public GameObject CratePress;

    void OnTriggerEnter(Collider col)
    {
        //CratePress.transform.parent.GetComponent<Animator>().SetTrigger("PressSlam");

        //CratePress.GetComponent<Animator>().SetTrigger("PressSlam");
        CratePress.GetComponent<V2_PressSound>().SoundEffect();

        if (col.tag == "BoxBlue")
        {
            Destroy(col.gameObject);
            Instantiate(blueItem, gameObject.transform);
        }
        else if (col.tag == "BoxGreen")
        {
            Destroy(col.gameObject);
            Instantiate(greenItem, gameObject.transform);
        }
        else if (col.tag == "BoxRed")
        {
            Destroy(col.gameObject);
            Instantiate(redItem, gameObject.transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author> Lorenzo Sae-Phoo Zemp </author>
public class V2_pressItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blueItem;
    [SerializeField] private GameObject redItem;
    [SerializeField] private GameObject greenItem;

    [SerializeField] private GameObject CratePress;

    /// <summary>
    /// This function gets triggered when a crate/box object enters its trigger area
    /// and in turn spawns an object specified in the inspector
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        //CratePress.transform.parent.GetComponent<Animator>().SetTrigger("PressSlam");

        //CratePress.GetComponent<Animator>().SetTrigger("PressSlam");
        CratePress.GetComponent<V2_PressSound>().SoundEffect();

        if (col.tag == "BoxBlue")
        {
            Destroy(col.gameObject);
            if (blueItem != null)
            {
                Instantiate(blueItem, gameObject.transform);
            }
        }
        else if (col.tag == "BoxGreen")
        {
            Destroy(col.gameObject);
            if (greenItem != null)
            {
                Instantiate(greenItem, gameObject.transform);
            }
        }
        else if (col.tag == "BoxRed")
        {
            Destroy(col.gameObject);
            if (redItem != null)
            {
                Instantiate(redItem, gameObject.transform);
            }
        }
    }
}

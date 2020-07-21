using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V3_FingerPrintSpawner : MonoBehaviour
{
    private V2_ButtonHandle[] Buttons;
    private GameObject gameManager;

    public GameObject[] FingerPrints;
    public float zOffset = -1.0f;

    class RandomInfo
    {
        public int[] buttonsPos;
    }

    static Dictionary<int, RandomInfo> persistentAcrossSceneReset;
    static HashSet<int> aliveCurrently;

    private int ID;
    private RandomInfo randomInfo;

    private void Awake()
    {
        ID = Mathf.RoundToInt(transform.position.sqrMagnitude);
        aliveCurrently.Add(ID);
        // TODO only check if exsists if scene was not a transition

        if (persistentAcrossSceneReset.ContainsKey(ID))
        {
            randomInfo = persistentAcrossSceneReset[ID];
        }
        else
        {
            randomInfo = new RandomInfo {
                buttonsPos = new int[2] {
                Random.Range(0, 10),
                Random.Range(0, 10)
            }
            };
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        persistentAcrossSceneReset = new Dictionary<int, RandomInfo>(persistentAcrossSceneReset.Where(Filter).ToDictionary(p => p.Key, p => p.Value));

        Buttons = gameObject.GetComponent<V2_NumPad>().numkeys;

        PlacePrints(FingerPrints[0], randomInfo.buttonsPos[0]);
        PlacePrints(FingerPrints[1], randomInfo.buttonsPos[1]);
    }

    void PlacePrints(GameObject prefab, int buttonNumber)
    {
        Transform buttonTransform = Buttons[buttonNumber].transform;

        GameObject.Instantiate(prefab, buttonTransform.position + (buttonTransform.forward * zOffset), buttonTransform.rotation, buttonTransform);
    }

    bool Filter(KeyValuePair<int, RandomInfo> keyValuePair)
    {
        if (aliveCurrently.Contains(keyValuePair.Key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

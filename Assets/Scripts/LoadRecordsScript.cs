using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadRecordsScript : MonoBehaviour
{
    [SerializeField] Text textSimpleRecord;
    [SerializeField] Text textNormalRecord;
    [SerializeField] Text textHardRecord;

    void Start()
    {
        if (PlayerPrefs.HasKey("SimpleRecord"))
        {
            textSimpleRecord.text = PlayerPrefs.GetString("SimpleRecord") + " сек";
        }

        if (PlayerPrefs.HasKey("NormalRecord"))
        {
            textSimpleRecord.text = PlayerPrefs.GetString("NormalRecord") + " сек";
        }

        if (PlayerPrefs.HasKey("HardRecord"))
        {
            textSimpleRecord.text = PlayerPrefs.GetString("HardRecord") + " сек";
        }
    }
}

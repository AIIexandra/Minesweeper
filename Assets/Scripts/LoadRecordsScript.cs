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

    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("SimpleRecord"))
        {
            textSimpleRecord.text = PlayerPrefs.GetFloat("SimpleRecord").ToString("F2") + " сек";
        }

        if (PlayerPrefs.HasKey("NormalRecord"))
        {
            textSimpleRecord.text = PlayerPrefs.GetFloat("NormalRecord").ToString("F2") + " сек";
        }

        if (PlayerPrefs.HasKey("HardRecord"))
        {
            textSimpleRecord.text = PlayerPrefs.GetFloat("HardRecord").ToString("F2") + " сек";
        }
    }
}

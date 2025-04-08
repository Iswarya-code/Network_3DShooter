using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNameScript : MonoBehaviour
{
    public Text[] names;
    public Image[] healthBars;

    private void Start()
    {
        for(int i=0; i<names.Length;i++)
        {
            names[i].gameObject.SetActive(false);
            healthBars[i].gameObject.SetActive(false);
        }
    }
}

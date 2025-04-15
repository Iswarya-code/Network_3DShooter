using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TeamKillCount : MonoBehaviour
{
    public List<Kills> highestKills = new List<Kills>();
    public Text[] killamts;
    GameObject killCountPanel;
    GameObject namesObject;
    bool killCountOn = false;
    public bool countDown = true;
    public GameObject winnerPanel;
    public Text winnerText;

    int redTeamKills;
    int greenTeamKills;
    // Start is called before the first frame update
    void Start()
    {
        killCountPanel = GameObject.Find("KillCountPanel");
        namesObject = GameObject.Find("NamesBG");
        killCountPanel.SetActive(false);
        winnerPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && countDown == true)
        {
            if (killCountOn == false)
            {
                killCountPanel.SetActive(true);
                killCountOn = true;
                highestKills.Clear();
                for (int i = 0; i < 6; i++)
                {
                    highestKills.Add(new Kills(namesObject.GetComponent<NickNameScript>().names[i].text, namesObject.GetComponent<NickNameScript>().kills[i]));
                }
                redTeamKills = highestKills[0].playerKills + highestKills[1].playerKills + highestKills[2].playerKills;
                greenTeamKills = highestKills[3].playerKills + highestKills[4].playerKills + highestKills[5].playerKills;
                killamts[0].text = redTeamKills.ToString();
                killamts[1].text = greenTeamKills.ToString();


            }
            else if (killCountOn == true)
            {
                killCountPanel.SetActive(false);
                killCountOn = false;
            }
        }
    }

    public void TimeOver()
    {
        killCountPanel.SetActive(true);
        winnerPanel.SetActive(true);
        killCountOn = true;
        highestKills.Clear();
        for (int i = 0; i < 6; i++)
        {
            highestKills.Add(new Kills(namesObject.GetComponent<NickNameScript>().names[i].text, namesObject.GetComponent<NickNameScript>().kills[i]));
        }
        redTeamKills = highestKills[0].playerKills + highestKills[1].playerKills + highestKills[2].playerKills;
        greenTeamKills = highestKills[3].playerKills + highestKills[4].playerKills + highestKills[5].playerKills;
        killamts[0].text = redTeamKills.ToString();
        killamts[1].text = greenTeamKills.ToString();
        if(redTeamKills > greenTeamKills)
          {
            winnerText.text = "RED TEAM WINS";
          }
        if (redTeamKills < greenTeamKills)
        {
            winnerText.text = "GREEN TEAM WINS";
        }
    }
 }

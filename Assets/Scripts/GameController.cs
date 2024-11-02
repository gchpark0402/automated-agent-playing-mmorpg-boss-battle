using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using System.IO;


public class GameController : MonoBehaviour
{
    private static GameController instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static GameController Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public GameObject agent;
    public GameObject boss;
    public GameObject agent2;
    public GameObject boss2;
    public GameObject mainCam;

    public GameObject bossHP;
    public GameObject HP;
    public GameObject Trial;
    public GameObject RoundBossHP;
    public GameObject DPS;
    public GameObject MinBossHP;
    //public GameObject RoundDPS;


    public int trial;
    public int remainingBossHP;
    public float roundBossHP;
    public float dps;
    public float remainingDps;
    public float roundDps;
    public float time;
    public int minBossHP;
    public float roundTime;

    public int trial2;
    public int remainingBossHP2;
    public float roundBossHP2;
    public float dps2;
    public float remainingDps2;
    public float roundDps2;
    public float time2;
    public int minBossHP2;
    public float roundTime2;

    public int timer;
    public int timer2;

    public Slider BossHPSlider;
    public Slider PlayerHPSlider;

    public bool cameraSwitch;
    public int[] damageCount = new int[101];
    public int[] damageCount2 = new int[101];

    private void Start()
    {
        mainCam = Camera.main.gameObject;
        trial = 0; trial2 = 0;
        remainingBossHP = 0; remainingBossHP2 = 0;
        remainingDps= 0; remainingDps2 = 0;
        roundBossHP = 0; roundBossHP2 = 0; 
        roundDps= 0; roundDps2 = 0;
        dps = 0; dps2 = 0;
        minBossHP= 100; minBossHP2 = 100;
        cameraSwitch = true;
    }


    private void Update()
    {
        Time.timeScale = 1;

        bossHP.GetComponent<TextMeshProUGUI>().text = boss.GetComponent<EnemyScript1>().HP.ToString() + " : " + boss2.GetComponent<EnemyScript1>().HP.ToString();
       
        HP.GetComponent<TextMeshProUGUI>().text = "HP : " + agent.GetComponent<AgentScript>().HP.ToString() + " : " +
            agent2.GetComponent<AgentScript>().HP.ToString();
        
        Trial.GetComponent<TextMeshProUGUI>().text = "Trial : " + trial.ToString() + " : " + trial2.ToString();
        RoundBossHP.GetComponent<TextMeshProUGUI>().text = "Average Boss Hp : " + roundBossHP.ToString() + " : " + roundBossHP2.ToString();
        DPS.GetComponent<TextMeshProUGUI>().text = "DPS : " + dps.ToString() + " : " + dps2.ToString();

        MinBossHP.GetComponent<TextMeshProUGUI>().text = "MinBossHP : " + minBossHP.ToString() + " : " + minBossHP2.ToString();
        //RoundDPS.GetComponent<TextMeshProUGUI>().text = "RoundDPS : " + roundDps.ToString() + " : " + roundDps2.ToString();

        time += Time.deltaTime;
        time2 += Time.deltaTime;
        timer = Mathf.FloorToInt(time);
        timer2 = Mathf.FloorToInt(time2);
        Debug.Log(timer);
        Debug.Log(timer2);
        dps = (100 - boss.GetComponent<EnemyScript1>().HP) / Mathf.Max(timer, 0.0001f);
        dps2 = (100 - boss2.GetComponent<EnemyScript1>().HP) / Mathf.Max(timer2, 0.0001f);

        //SetCamPos();

        if(BossHPSlider != null)
            BossHPSlider.value = boss.GetComponent<EnemyScript1>().HP;
        if (PlayerHPSlider != null)
        {
            PlayerHPSlider.value = agent.GetComponent<AgentScript>().HP;
            PlayerHPSlider.transform.position = Camera.main.WorldToScreenPoint(agent.transform.position + new Vector3(0, 2f, 0));
        }
           
    }

    public void UpdateData(int checknum)
    {
        if(checknum == 0)
        {
            trial++;
            damageCount[100 - boss.GetComponent<EnemyScript1>().HP] += 1;
            remainingBossHP += boss.GetComponent<EnemyScript1>().HP;
            roundBossHP = remainingBossHP / trial;
            remainingDps += dps;
            roundDps= remainingDps / trial;
            roundTime += timer;
            time = 0;
            timer = 0;

            if (minBossHP > boss.GetComponent<EnemyScript1>().HP)
            {
                minBossHP = boss.GetComponent<EnemyScript1>().HP;
            }

        }
        else if (checknum == 1)
        {
            trial2++;
            damageCount2[100 - boss2.GetComponent<EnemyScript1>().HP] += 1;
            remainingBossHP2 += boss2.GetComponent<EnemyScript1>().HP;
            roundBossHP2 = remainingBossHP2 / trial2;
            remainingDps2 += dps2;
            roundDps2 = remainingDps2 / trial2;
            roundTime2+= timer2;
            time2 = 0;
            timer2 = 0;


            if (minBossHP2 > boss2.GetComponent<EnemyScript1>().HP)
            {
                minBossHP2 = boss2.GetComponent<EnemyScript1>().HP;
            }

        }

        if(trial % 500 == 0) { SavetxtFile(); }

       
    }

    void SavetxtFile()
    {
        string fulltxtPath = "Assets/Config/test_1_" + trial;

        if(false == File.Exists(fulltxtPath + ".txt"))
        {
            var file = File.CreateText(fulltxtPath + ".txt");
            file.Close();
        }

        StreamWriter sw = new StreamWriter(fulltxtPath + ".txt");
        sw.WriteLine("Trial : " + trial);
        sw.WriteLine("Round Boss HP : " + roundBossHP);
        sw.WriteLine("Round DPS : " + roundDps);
        sw.WriteLine("Round Time : " + (roundTime / trial));
        sw.WriteLine("Damage Count");
        for (int i = 0; i < 100; i++)
        {
            sw.WriteLine(damageCount[i]);
        }

        sw.Flush();
        sw.Close();

        fulltxtPath = "Assets/Config/test_2_" + trial2;

        if (false == File.Exists(fulltxtPath + ".txt"))
        {
            var file = File.CreateText(fulltxtPath + ".txt");
            file.Close();
        }

        sw = new StreamWriter(fulltxtPath + ".txt");
        sw.WriteLine("Trial : " + trial2);
        sw.WriteLine("Round Boss HP : " + roundBossHP2);
        sw.WriteLine("Round DPS : " + roundDps2);
        sw.WriteLine("Round Time : " + (roundTime2 / trial2));
        sw.WriteLine("Damage Count");
        for (int i = 0; i < 100; i++)
        {
            sw.WriteLine(damageCount2[i]);
        }

        sw.Flush();
        sw.Close();
    }


    void SetCamPos()
    {
        Vector3 tempVec = (agent.transform.position - boss.transform.position).normalized;


        if (cameraSwitch)
        {
            Vector3 campos = agent.transform.position + tempVec * 10;
            campos.y = 10;
            mainCam.transform.localPosition = campos;
            mainCam.transform.LookAt(boss.transform.position);
        }
        else
        {
            Vector3 campos = agent.transform.position + new Vector3(0, 0, 10);
            campos.y = 40;
            mainCam.transform.localPosition =
                Vector3.Lerp(mainCam.transform.localPosition, campos, 3);
            mainCam.transform.rotation = Quaternion.Euler(new Vector3(100, 0, 180));
        }
        
    }
    
}

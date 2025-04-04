using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playHUD : MonoBehaviour
{
    
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI GameTimeText;
    float CurrentTime;
    public PlayerBaseScript player;
    float LiveHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime += Time.deltaTime;
        GameTimeText.text = "Time: " + CurrentTime.ToString();

        LiveHealth = player.getPlayerHealth();
        HealthText.text = "HP: " + LiveHealth.ToString();
    }
}

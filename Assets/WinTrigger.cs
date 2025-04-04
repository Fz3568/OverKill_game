using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject WinScreen;
    public GameObject[] turnoff;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WinScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            WinScreen.SetActive(true);
            turnOff();
        }
    }
    public void turnOff()
    {
        foreach (var VARIABLE in turnoff)
        {
            VARIABLE.SetActive(false);
        }
    }
}

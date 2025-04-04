using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    void Start()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

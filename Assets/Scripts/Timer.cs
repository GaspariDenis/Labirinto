using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float currentTime = 0;

    private float min;
    private float sec;

    public bool InBox = false;

    [SerializeField] private TextMeshProUGUI minuto1;
    [SerializeField] private TextMeshProUGUI minuto2;
    [SerializeField] private TextMeshProUGUI secondo1;
    [SerializeField] private TextMeshProUGUI secondo2;

    public float Tempo
    {
        get { return currentTime; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InBox) {
            currentTime += Time.deltaTime;
        }
        min = Mathf.FloorToInt(currentTime / 60);
        sec = Mathf.FloorToInt(currentTime % 60);

        string current = string.Format("{00:00}{1:00}", min, sec);
        minuto1.text = current[0].ToString();
        minuto2.text = current[1].ToString();
        secondo1.text = current[2].ToString();
        secondo2.text = current[3].ToString();
    }
}

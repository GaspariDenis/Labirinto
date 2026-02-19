using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float SensX;
    public float SensY;

    public Transform Orientation;

    public GameObject[] Torcia;

    bool LigthActive = false;

    float xRotazione;
    float yRotazione;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Torcia.Length; i++) {
            Torcia[i].SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < Torcia.Length; i++)
            {
                Torcia[i].SetActive(!LigthActive);
            }
            LigthActive = !LigthActive;
        }

        //Posizione x e y del cursore
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensY;

        //La X del mouse e la x della telecamera sono rovesce, quindi x è y
        yRotazione += mouseX;
        xRotazione -= mouseY;

        xRotazione = Mathf.Clamp(xRotazione, -90f, 90f);

        //Rotazione
        transform.rotation = Quaternion.Euler(xRotazione, yRotazione, 0);
        Orientation.rotation = Quaternion.Euler(0,yRotazione, 0);
    }
}

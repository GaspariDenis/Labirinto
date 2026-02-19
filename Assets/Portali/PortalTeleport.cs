using DoorScript;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PortalTeleport : MonoBehaviour
{
    public PortalTeleport Portale_Opposto;
    public Door Propria_Porta;

    public bool Invert = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!Portale_Opposto.Propria_Porta.open)
            Portale_Opposto.Propria_Porta.OpenDoor();
    }

    private void OnTriggerStay(Collider other)
    {
        //Portale_Opposto.Propria_Porta.open = Propria_Porta.open;

        //è la capsula che rigghera l'evento quindi devo teletrasportare tutto il punto
        Transform padre = other.GetComponentInParent<Transform>();
        
        float zPosizione = transform.worldToLocalMatrix.MultiplyPoint3x4(padre.position).z;

        if(zPosizione < 0 && Propria_Porta.open)
        {
            Teletrasporta(other.transform);
        }
    }

    private void Teletrasporta(Transform OggettoDaTeletrasportare)
    {
        Vector3 PosizioneLocale = transform.worldToLocalMatrix.MultiplyPoint3x4(OggettoDaTeletrasportare.position);
        PosizioneLocale = new Vector3(-PosizioneLocale.x, PosizioneLocale.y, -PosizioneLocale.z);
        OggettoDaTeletrasportare.position = Portale_Opposto.transform.localToWorldMatrix.MultiplyPoint3x4(PosizioneLocale);

        Quaternion diff = Portale_Opposto.transform.rotation * Quaternion.Inverse(transform.rotation * Quaternion.Euler(0, 180, 0));
        OggettoDaTeletrasportare.rotation = diff * OggettoDaTeletrasportare.rotation;

        if (Invert)
        {
            Rigidbody rg = OggettoDaTeletrasportare.gameObject.GetComponent<Rigidbody>();
            Vector3 i = -rg.GetAccumulatedForce();
            rg.AddForce(i);
        }

        // Chiusura delle porte
        Door porta = Portale_Opposto.GetComponentInChildren<Door>();
        porta.OpenDoor();
       
    }
}

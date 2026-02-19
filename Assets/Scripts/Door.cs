using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoorScript
{
	[RequireComponent(typeof(AudioSource))]


    public class Door : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        public Corridoio DaInizializzare;

        private bool PlayerOnArea = false;

        // Use this for initialization
        void Start()
        {
            asource = GetComponent<AudioSource>();
        }

        public void OnTriggerEnter(Collider other)
        {
            PlayerOnArea = true;
            if (DaInizializzare != null) {
                DaInizializzare.CreationTrigger = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            PlayerOnArea = false;
        }


        // Update is called once per frame
        void Update()
        {
            if (PlayerOnArea)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenDoor();
                }
            }

            Quaternion target = Quaternion.Euler(0, (open)? DoorOpenAngle : DoorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
        }

        public void OpenDoor()
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
        }
    }
}
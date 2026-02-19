using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed; //Velocita
    public float RunningSpeed; //Velocita quando corre
    public Transform orientation;

    //Pos nel pavimento
    public float altezza; //altezza personaggio
    public float attrito; // resistenza del pavimento
    public LayerMask pavimento; //il pavimento
    bool aTerra; 

    //Salto
    public float jumpForce; // forza del salto
    public float jumpCooldown; // timer per quando può saltare
    public float airMul; //cosa in più (resistenza dell'aria)
    public int numberOfJumps;
    private int currentNumberOfJumps;
    private bool readyToJump = true;

    //Bind dei tasti
    private KeyCode jumpKey = KeyCode.Space;

    float InputOriz;
    float InputVert;

    Vector3 Direzione;

    Rigidbody rg;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        rg .freezeRotation = true;
        ResetJump();
    }
    private void Jump()
    {
        rg.velocity = new Vector3(rg.velocity.x, 0f, rg.velocity.z);
        rg.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void MyInput()
    {
        InputOriz = Input.GetAxisRaw("Horizontal");
        InputVert = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(jumpKey) && readyToJump && aTerra)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    //Limita velocità
    public void SpeedControl()
    {
        Vector3 limitVel = new Vector3(rg.velocity.x, 0, rg.velocity.z);
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (limitVel.magnitude > movementSpeed)
            {
                Vector3 newVel = limitVel.normalized * movementSpeed;
                rg.velocity = new Vector3(newVel.x, rg.velocity.y, newVel.z);
            }
        }
        else
        {
            if(limitVel.magnitude > RunningSpeed)
            {
                Vector3 newVel = limitVel.normalized * RunningSpeed;
                rg.velocity = new Vector3(newVel.x, rg.velocity.y, newVel.z);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        aTerra = Physics.Raycast(transform.position, Vector3.down, altezza * 0.7f, pavimento);

        MyInput();
        SpeedControl();

        if (aTerra)
        {
            rg.drag = attrito;
        }
        else
        {
            rg.drag = 0;
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        //Calcola Direzione
        Direzione = orientation.forward * InputVert + orientation.right * InputOriz;

        if (aTerra)
        {
            rg.AddForce(Direzione.normalized * movementSpeed, ForceMode.Impulse);
        }
        else
        {
            rg.AddForce(Direzione.normalized * movementSpeed * airMul, ForceMode.Force);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}

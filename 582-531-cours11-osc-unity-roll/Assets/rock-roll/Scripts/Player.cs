using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class Player : MonoBehaviour
{
    private int etatEnMemoire = 1;
    public extOSC.OSCReceiver oscReceiver;
    public float torqueForce = 1f;
    public float jumpForce = 5f;

    public float checkRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        oscReceiver.Bind("/change", TraiterOscAngle);
        oscReceiver.Bind("/bouton", TraiterOscBouton);
        rb = GetComponent<Rigidbody2D>();
    }



    void TraiterOscAngle(OSCMessage message)
    {
        // Si le message n'a pas d'argument ou l'argument n'est pas un Int on l'ignore
        if (message.Values.Count == 0)
        {
            Debug.Log("No value in OSC message");
            return;
        }

        if (message.Values[0].Type != OSCValueType.Int)
        {
            Debug.Log("Value in message is not an Int");
            return;
        }

        // Récupérer la valeur de l’angle depuis le message OSC
        int value = message.Values[0].IntValue;


        if (value > 0)
        {
            rb.AddTorque(-torqueForce * value); // clockwise
        }
        else if (value < 0)
        {
            rb.AddTorque(torqueForce * -value); // counter-clockwise
        }
    }





    void TraiterOscBouton(OSCMessage message)
    {
        // Si le message n'a pas d'argument ou l'argument n'est pas un Int on l'ignore
        if (message.Values.Count == 0)
        {
            Debug.Log("No value in OSC message");
            return;
        }

        if (message.Values[0].Type != OSCValueType.Int)
        {
            Debug.Log("Value in message is not an Int");
            return;
        }

        // Récupérer la valeur de l’angle depuis le message OSC

        // EXEMPLE : utiliser la valeur pour appliquer une rotation
        // Adapter proportionnellement la valeur reçue
        //float angle = Proportion(value, 0, 1, 0, 1);
        // Appliquer la rotation à l’objet
        //transform.rotation = Quaternion.Euler(0, angle, 0);
        int nouveauEtat = message.Values[0].IntValue; // REMPLACER ici les ... par le code qui permet de récuérer la nouvelle donnée du flux
        if (etatEnMemoire != nouveauEtat)
        { // Le code compare le nouvel etat avec l'etat en mémoire
            etatEnMemoire = nouveauEtat; // Le code met à jour l'état mémorisé
            if (nouveauEtat == 0)
            {
                if (IsGrounded())
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                // METTRE ici le code pour lorsque le bouton est relaché
            }
        }

    }




    void FixedUpdate()
    {
        // Roll left/right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddTorque(-torqueForce); // clockwise
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddTorque(torqueForce); // counter-clockwise
        }

       
    }

    void Update()
    {
        // Jump
        // GetKeyDown() does not work in FixedUpdate()
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    public bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            GetComponent<CircleCollider2D>().radius + extraHeight,
            groundLayer
        );
        return hit.collider != null;
    }
}

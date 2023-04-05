using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
public class ArduinoConnector : MonoBehaviour
{
    // Start is called before the first frame update

    public bool useArduino;
    public string IOPort = "/dev/cu.HC05-SPPDev";
    public int baudeRate = 9600;
    public Vector2 inputValues = Vector2.zero;
    public float WheelRaduis;
    public SerialPort sp;

    private string recievedValue;
    private bool hasPassed;
    private float timeBtwPassings;

    public Rigidbody m_Rigidbody;

    void Start()
    {
        if (useArduino) ActivateSP();
        
    }
    void ActivateSP()
    {
        sp = new SerialPort(IOPort, baudeRate, Parity.None, 8, StopBits.One);

        sp.Open();
        sp.ReadTimeout = 35;
    }
    // Update is called once per frame
    void Update()
    {
        if (useArduino && sp.IsOpen)
        {
            try
            {
                recievedValue = sp.ReadLine(); //reads the serial input
                //Debug.Log(recievedValue);
                SetDirection(recievedValue); //translates the string into a Vector
            }
            catch (System.Exception)
            {

            }
        }
    }

    void SetDirection(string message)
    {
        float input = float.Parse(message);
        timeBtwPassings += Time.deltaTime;
        if (input == 0 && hasPassed == false)
        {
            hasPassed = true;
            inputValues.y = WheelRaduis * 2 * Mathf.PI / timeBtwPassings;
            timeBtwPassings = 0f;
           /* if (inputValues.y<50)
            {
                Debug.Log(inputValues.y);
                //m_Rigidbody.AddForce(transform.right * inputValues.y/10, ForceMode.Impulse );
                transform.position += transform.forward * inputValues.y * Time.deltaTime;

            }*/

        }
        else if (input == 1)
        {
            hasPassed = false;
        }

        if (inputValues.y < 50)
        {
            Debug.Log(inputValues.y);
            //m_Rigidbody.AddForce(transform.right * inputValues.y/10, ForceMode.Impulse );
            transform.position += transform.forward * inputValues.y/50 * Time.deltaTime;

        }

        if (timeBtwPassings>3)
        {
            inputValues.y = 0f;
        }
        
        //Debug.Log((int)timeBtwPassings);
    }
}

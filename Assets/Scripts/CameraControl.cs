using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float cSpeed;
    public float highCSpeed;
    public float rotateCamSpeed;
    public float clampAngle;

    private CharacterController myCC;
    private PhotonView PV;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        PV = GetComponent<PhotonView>();
        myCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicMovement();
        BasicRotation();
    }

    void BasicMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if(Input.GetKey(KeyCode.LeftShift))
                myCC.Move(transform.forward * Time.deltaTime * highCSpeed);
            else
                myCC.Move(transform.forward * Time.deltaTime * cSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                myCC.Move(-transform.right * Time.deltaTime * highCSpeed);
            else
                myCC.Move(-transform.right * Time.deltaTime * cSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                myCC.Move(-transform.forward * Time.deltaTime * highCSpeed);
            else
                myCC.Move(-transform.forward * Time.deltaTime * cSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                myCC.Move(transform.right * Time.deltaTime * highCSpeed);
            else
                myCC.Move(transform.right * Time.deltaTime * cSpeed);
        }
    }

    void BasicRotation()
    {
        if (Input.GetMouseButton(1))
        {

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            rotY += mouseX * rotateCamSpeed * Time.deltaTime;
            rotX += mouseY * rotateCamSpeed * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);


            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);

            transform.rotation = localRotation;
            //transform.rotation = localRotation;
            /*
            //pitch
            if (transform.rotation.x >= -180 || transform.rotation.x <= 180)
            {
                transform.rotation *= Quaternion.AngleAxis(
                        -Input.GetAxis("Mouse Y") * rotateCamSpeed,
                        Vector3.right
                    );

                // Paw
                transform.rotation = Quaternion.Euler(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y + Input.GetAxis("Mouse X") * rotateCamSpeed,
                    transform.eulerAngles.z
                );
            }*/
        }
    }
}

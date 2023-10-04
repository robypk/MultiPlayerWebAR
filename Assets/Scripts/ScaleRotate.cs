using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleRotate : MonoBehaviour
{


    [SerializeField] TMP_Text NameText;
    [SerializeField] PhotonView self_photonView;

    private float initialDistance; // Store initial distance for scaling
    private Vector2 initialTouchPosition; // Store initial touch position for rotation

    private void Start()
    {
        setPlayerNickName(self_photonView.Owner.NickName);
    }
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float scaleFactor = currentDistance / initialDistance;

                // Scale the object
                transform.localScale *= scaleFactor;

                initialDistance = currentDistance;
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPosition = touch.position;
                Vector2 delta = currentTouchPosition - initialTouchPosition;

                float rotationSpeed = 0.1f; // Adjust this to control rotation speed

                // Rotate the object
                transform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
                transform.Rotate(Vector3.right, delta.y * rotationSpeed, Space.World);

                initialTouchPosition = currentTouchPosition;
            }
        }


    }


    public void setPlayerNickName(string NickName)
    {
        NameText.text = NickName;
    }
}

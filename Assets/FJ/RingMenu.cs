using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu : MonoBehaviour
{
    public WeaponsData[] weapons;
    public RingPiece ringPiecePrefab;


    private RingPiece[] ringPieces;
    private float degreesPerPiece;
    private float gapDegrees = 1f;
    private void Start()
    {
        degreesPerPiece = 360f / weapons.Length;
        float distanceToIcon = Vector3.Distance(ringPiecePrefab.icon.transform.position, ringPiecePrefab.backGround.transform.position);

        ringPieces = new RingPiece[weapons.Length];

        for(int i = 0; i < weapons.Length; i++ )
        {
            ringPieces[i] = Instantiate(ringPiecePrefab, transform);

            ringPieces[i].backGround.fillAmount = (1f / weapons.Length) - (gapDegrees / 360f);
            ringPieces[i].backGround.transform.localRotation = Quaternion.Euler(0, 0, degreesPerPiece / 2f + gapDegrees / 2f + i * degreesPerPiece);

            ringPieces[i].icon.sprite = weapons[i].icon;

            Vector3 directionVector = Quaternion.AngleAxis(i * degreesPerPiece, Vector3.forward) * Vector3.up;
            Vector3 movementVector = directionVector * distanceToIcon;
            ringPieces[i].icon.transform.localPosition = ringPieces[i].backGround.transform.localPosition + movementVector;
        }
    }
    private void Update()
    {
        int activeElement = GetActiveElement();
        HighLightActiveElement(activeElement);
        RespondToMouseInput();
    }

    private void RespondToMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            
        }
    }

    

    private int GetActiveElement()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        Vector3 cursorVector = Input.mousePosition - screenCenter;

        float mouseAngle = Vector3.SignedAngle(Vector3.up, cursorVector, Vector3.forward) + degreesPerPiece / 2f;
        float normalizedMouseAngle = NormalizeAngle(mouseAngle);
        return (int)(normalizedMouseAngle / degreesPerPiece);
    }


    private void HighLightActiveElement(int activeElement)
    {
        for(int i = 0; i < ringPieces.Length; i++)
        {
            if(i==activeElement)
            {
                ringPieces[i].backGround.color = new Color(1f, 1f, 0.75f);
            }
            else
            {
                ringPieces[i].backGround.color = new Color(1f, 1f, 0.5f);
            }
        }
    }
    private float NormalizeAngle(float a) => (a + 360f) % 360f;
    
}

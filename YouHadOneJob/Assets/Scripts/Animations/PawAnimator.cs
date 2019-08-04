using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawAnimator : MonoBehaviour
{
    [SerializeField]
    private Transform keyboardPaw;

    [SerializeField]
    private Transform mousePaw;

    private Vector3 keyboardOriginal;

    private Vector3 mouseOriginal;

    [SerializeField]
    private float movingSpeed;

    private void Start()
    {
        keyboardOriginal = keyboardPaw.localPosition;
        mouseOriginal = mousePaw.localPosition;
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(PressKeyboard());
            keyboardPaw.localPosition = keyboardOriginal;
            StartCoroutine(PressKeyboard());
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine(PressMouse());
            mousePaw.localPosition = mouseOriginal;
            StartCoroutine(PressMouse());
        }
    }

    private IEnumerator PressKeyboard()
    {
        Vector3 keyboardUp = keyboardPaw.localPosition + new Vector3(0, 0.03f, 0);
        while (keyboardPaw.position != keyboardUp)
        {
            keyboardPaw.localPosition = Vector3.MoveTowards(keyboardPaw.localPosition, keyboardUp, Time.fixedDeltaTime * movingSpeed);
            yield return new WaitForFixedUpdate();
        }
        while (keyboardPaw.position != keyboardOriginal)
        {
            keyboardPaw.position = Vector3.MoveTowards(keyboardPaw.localPosition, keyboardOriginal, Time.fixedDeltaTime * movingSpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator PressMouse()
    {
        Vector3 mouseUp = mousePaw.localPosition + new Vector3(0, 0.03f, 0);
        while (mousePaw.position != mouseUp)
        {
            mousePaw.localPosition = Vector3.MoveTowards(mousePaw.localPosition, mouseUp, Time.fixedDeltaTime * movingSpeed);
            yield return new WaitForFixedUpdate();
        }
        while (mousePaw.position != mouseOriginal)
        {
            mousePaw.position = Vector3.MoveTowards(mousePaw.localPosition, mouseOriginal, Time.fixedDeltaTime * movingSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}

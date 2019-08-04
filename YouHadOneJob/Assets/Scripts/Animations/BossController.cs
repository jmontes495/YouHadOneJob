using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum CurrentDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
    }

    [SerializeField]
    private Transform leftBoss;

    [SerializeField]
    private Transform rightBoss;

    [SerializeField]
    private Transform rightHead;

    [SerializeField]
    private Transform leftHead;

    [SerializeField]
    private Transform lookingPosition;

    [SerializeField]
    private float walkingSpeed;

    [SerializeField]
    private float delayBeforeStare;

    [SerializeField]
    private float turningHeadSpeed;

    [SerializeField]
    private float stareDurration;

    [SerializeField]
    private float delayBetweenDoggos;

    private bool hasLooked = false;

    private CurrentDirection currentDirection;

    private Vector3 originalLeftPosition;

    private Vector3 originalRightPosition;

    private Transform currentBoss;

    private Transform currentHead;

    private bool isStaring;

    private void Start()
    {
        originalLeftPosition = leftBoss.position;
        originalRightPosition = rightBoss.position;
        currentDirection = CurrentDirection.None;
        StartCoroutine(StartDoggos());
    }

    private IEnumerator StartDoggos()
    {
        yield return new WaitForSeconds(delayBetweenDoggos);
        ComeFromDirection(CurrentDirection.Right);
    }

    public bool IsWalking()
    {
        return currentDirection != CurrentDirection.None;
    }

    public CurrentDirection GetCurrentDirection()
    {
        return currentDirection;
    }

    public bool IsStaring()
    {
        return isStaring;
    }

    public bool HasLookedOnHisWay()
    {
        return hasLooked;
    }

    public void ComeFromDirection(CurrentDirection direction)
    {
        hasLooked = false;
        currentDirection = direction;
        currentBoss = currentDirection == CurrentDirection.Left ? leftBoss : rightBoss;
        currentHead = currentDirection == CurrentDirection.Left ? leftHead : rightHead;
        StartCoroutine(WalkToStarePoint());
    }

    private IEnumerator WalkToStarePoint()
    {
        while (currentBoss.position != lookingPosition.position)
        {
            currentBoss.position = Vector3.MoveTowards(currentBoss.position, lookingPosition.position, Time.fixedDeltaTime * walkingSpeed);
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(WaitToStare());
    }

    private IEnumerator WaitToStare()
    {
        yield return new WaitForSeconds(delayBeforeStare);
        StartCoroutine(Stare());
    }

    private IEnumerator Stare()
    {
        Quaternion originalPosition = currentHead.localRotation;
        Vector3 target = currentDirection == CurrentDirection.Left? new Vector3(0,0,1) : new Vector3(0, 0, -1);
        //currentHead.Rotate(target);
        for (int i = 0; i < 18; i++)
        {
            isStaring = true;
            currentHead.Rotate(target*4);
            yield return new WaitForSeconds(turningHeadSpeed);
        }
        isStaring = false;
        hasLooked = true;
        yield return new WaitForSeconds(stareDurration);
        currentHead.localRotation = originalPosition;
        StartCoroutine(CompleteJourney());
    }

    private IEnumerator CompleteJourney()
    {
        Vector3 otherExtreme = currentDirection == CurrentDirection.Left ? rightBoss.position : leftBoss.position;
        while (currentBoss.position != otherExtreme)
        {
            currentBoss.position = Vector3.MoveTowards(currentBoss.position, otherExtreme, Time.fixedDeltaTime * walkingSpeed);
            yield return new WaitForFixedUpdate();
        }

        currentBoss.position = currentDirection == CurrentDirection.Left ? originalLeftPosition : originalRightPosition;
        ComeFromDirection(currentDirection == CurrentDirection.Left ? CurrentDirection.Right : CurrentDirection.Left);
    }




}

using UnityEngine;
using System.Collections;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    public float moveDelay = 5f; 
    private Vector3 startPos;
    private bool isPlayerInside = false;
    private bool canMove = false; 
    private bool coroutineStarted = false; 
    private float lastDelta;

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    private void OnEnable()
    {
        EventCallBack.ResetBg += ResetPosition;
    }

    private void OnDisable()
    {
        EventCallBack.ResetBg -= ResetPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (!coroutineStarted)
            {
                StartCoroutine(DelayBeforeMove());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            canMove = false;
            coroutineStarted = false;
        }
    }

    private IEnumerator DelayBeforeMove()
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(moveDelay);
        if (isPlayerInside)
        {
            canMove = true;
        }
    }

    public void Move(float delta)
    {
        if (isPlayerInside && canMove)
        {
            Vector3 newPos = transform.localPosition;
            newPos.x -= delta * parallaxFactor;
            transform.localPosition = newPos;
            lastDelta = delta;
        }
    }

    private void ResetPosition()
    {
        isPlayerInside = false;
        canMove = false;
        coroutineStarted = false;

        Vector3 pos = transform.localPosition;
        pos.x = startPos.x;
        transform.localPosition = pos;

        lastDelta = 0;
    }

    //private void isFlse()
    //{
    //    isPlayerInside = false;
    //    canMove = false;
    //    coroutineStarted = false;
    //}
}

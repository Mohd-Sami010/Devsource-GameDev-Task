using UnityEngine;
using System.Collections;
public class MovingPlatform :MonoBehaviour {

    [SerializeField] private bool useRangeMovement = true;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector2 moveRange = new Vector2(3f, 0f);
    [SerializeField] private float waitTimeAtEnds = 1f;

    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 endPosA;
    private Vector2 endPosB;
    private Vector2 targetPos;

    private bool movingToA = true;
    private bool isWaiting = false;
    private Vector2 lastPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        startPos = transform.position;

        endPosA = startPos - moveRange;
        endPosB = startPos + moveRange;

        targetPos = endPosB;
    }

    private void FixedUpdate()
    {
        if (!useRangeMovement || isWaiting) return;

        Vector2 currentPos = rb.position;
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        lastPos = (newPos - currentPos) / Time.fixedDeltaTime;

        if (Vector2.Distance(newPos, targetPos) < 0.05f)
        {
            StartCoroutine(WaitBeforeSwitch());
        }
    }

    private IEnumerator WaitBeforeSwitch()
    {
        isWaiting = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTimeAtEnds);

        movingToA = !movingToA;
        targetPos = movingToA ? endPosA : endPosB;

        isWaiting = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var passengerRb))
        {
            if (!isWaiting) passengerRb.position += lastPos * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Vector2 sp = transform.position;
            Gizmos.DrawLine(sp - moveRange, sp + moveRange);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(endPosA, endPosB);
        }
    }
}
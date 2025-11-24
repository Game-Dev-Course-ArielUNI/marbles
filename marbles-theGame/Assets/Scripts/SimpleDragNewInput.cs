using UnityEngine;

public class SimpleDragNewInput : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rb;

    private bool dragging = false;
    private Vector3 startPos;

    // Drag settings
    public float maxDrag = 2f;
    public float launchPower = 8f;

    // Player index (0 = player1, 1 = player2)
    public int playerIndex;

    // Shot results
    private bool enteredHole = false;
    private bool insideHole = false;       // is marble currently inside hole?
    private bool touchedHoleThisShot = false; // did the marble enter hole at any moment?

    private bool hitEnemy = false;

    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Do not allow dragging unless it's this player's turn
        if (GameManager.Instance == null ||
            GameManager.Instance.phase == GamePhase.GameOver ||
            GameManager.Instance.currentPlayerIndex != playerIndex)
            return;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // START DRAG
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                dragging = true;
                startPos = transform.position;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.isKinematic = true;

                enteredHole = false;
                hitEnemy = false;
            }
        }

        // DRAGGING
        if (dragging && Input.GetMouseButton(0))
        {
            Vector3 offset = mouseWorldPos - startPos;

            // Limit drag distance
            if (offset.magnitude > maxDrag)
                offset = offset.normalized * maxDrag;

            transform.position = startPos + offset;
        }

        // RELEASE
        if (dragging && Input.GetMouseButtonUp(0))
        {
            touchedHoleThisShot = false;
            insideHole = false;

            dragging = false;
            rb.isKinematic = false;

            Vector3 releasePos = transform.position;
            Vector2 launchVector = (startPos - releasePos) * launchPower;

            rb.AddForce(launchVector, ForceMode2D.Impulse);

            // Start watching for when marble stops
            CancelInvoke(nameof(CheckStopped));
            InvokeRepeating(nameof(CheckStopped), 0.1f, 0.1f);
        }
    }

    private void CheckStopped()
    {
        if (rb.linearVelocity.magnitude < 0.05f)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            CancelInvoke(nameof(CheckStopped));

            // Notify GameManager that shot is fully finished
            //GameManager.Instance.OnShotFinished(enteredHole, hitEnemy);
            bool finalHoleShot = (touchedHoleThisShot && insideHole);

            GameManager.Instance.OnShotFinished(finalHoleShot, hitEnemy);

        }
    }

    //// HOLE detection
    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.CompareTag("Hole"))
    //    {
    //        enteredHole = true;
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Hole"))
        {
            touchedHoleThisShot = true;
            insideHole = true;
            Debug.Log("Entered Hole");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Hole"))
        {
            insideHole = false;
            Debug.Log("Exited Hole");
        }
    }


    // HIT ENEMY detection
    private void OnCollisionEnter2D(Collision2D col)
    {
        SimpleDragNewInput other = col.collider.GetComponent<SimpleDragNewInput>();
        if (other != null && other.playerIndex != this.playerIndex)
        {
            hitEnemy = true;
            Debug.Log("Hit enemy marble!");
        }
    }
}

















//using UnityEngine;

//public class SimpleDragNewInput : MonoBehaviour
//{
//    private Camera cam;
//    private Rigidbody2D rb;
//    private bool dragging = false;
//    private Vector3 startPos;
//    private float maxDrag = 2f;
//    private float launchPower = 7f;

//    private void Awake()
//    {
//        cam = Camera.main;
//        rb = GetComponent<Rigidbody2D>();
//    }

//    void Update()
//    {
//        // MOUSE PRESSED
//        if (Input.GetMouseButtonDown(0))
//        {
//            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
//            mouse.z = 0;

//            // Raycast to detect marble under the mouse
//            RaycastHit2D hit = Physics2D.Raycast(mouse, Vector2.zero);

//            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
//            {
//                dragging = true;
//                startPos = transform.position;
//                rb.isKinematic = true;
//            }
//        }

//        // MOUSE HELD (DRAGGING)
//        if (dragging && Input.GetMouseButton(0))
//        {
//            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
//            mouse.z = 0;

//            Vector3 offset = mouse - startPos;
//            if (offset.magnitude > maxDrag)
//                offset = offset.normalized * maxDrag;

//            transform.position = startPos + offset;
//        }

//        // MOUSE RELEASED
//        if (dragging && Input.GetMouseButtonUp(0))
//        {
//            dragging = false;
//            rb.isKinematic = false;

//            Vector3 currentPos = transform.position;
//            Vector2 force = (startPos - currentPos) * launchPower;

//            rb.AddForce(force, ForceMode2D.Impulse);
//        }
//    }
//}

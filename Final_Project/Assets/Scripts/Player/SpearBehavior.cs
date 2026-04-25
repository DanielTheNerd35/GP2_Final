using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehavior : MonoBehaviour
{
    public PlayerMovement player;
    public Transform playerPos;
    public Rigidbody2D rb;
    public Camera cam;
    private Vector3 mousePos;
    public float speed;
    public int damage;
    private float travelTime = 3f;
    private bool hasLaunched;
    
    // private void OnEnable()
    // {
    //     GameManager.OnPlayerSpawned += SetupReferences;
    // }

    // private void OnDisable()
    // {
    //     GameManager.OnPlayerSpawned -= SetupReferences;
    // }

    private void SetupReferences(GameObject spawnedPlayer)
    {
        player = spawnedPlayer.GetComponent<PlayerMovement>();
        playerPos = spawnedPlayer.transform;
        cam = Camera.main;

        // Start attached to player
        transform.SetParent(playerPos, false);
    }

    void Update()
    {
        if (player.hasthrown && !hasLaunched)
        {
            StartCoroutine(SpearTravel());
        }
        
        beyondScreen();
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Health health = hitInfo.GetComponent<Health>();
        if (health != null)
        {
            health.ChangeHealth(-damage, transform.position);
        }
        ReturnPlayer();
        
        if(hitInfo.CompareTag("Wall"))
        {
            ReturnPlayer();
        }
    }

    private void beyondScreen()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(this.gameObject.transform.position);

        if (viewPos.x >= 1.5F)
        {
            ReturnPlayer();
        }
    }

    private void ReturnPlayer()
    {
        hasLaunched = false;
        player.hasthrown = false;
        player.anim.SetBool("SpearThrow", false);
        this.transform.SetParent(playerPos, false);
        transform.position = new Vector3(playerPos.position.x + 5, playerPos.position.y + 0.3f, -1);
        player.spearSR.enabled = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void TeleportPlayer()
    {
        if (!hasLaunched) return;

        player.transform.position = transform.position;

        ReturnPlayer();
    }

    private IEnumerator SpearTravel()
    {
        hasLaunched = true;
        FindObjectOfType<AudioManager>().Play("SpearThrow");
        yield return new WaitForSeconds(travelTime);
    }
}
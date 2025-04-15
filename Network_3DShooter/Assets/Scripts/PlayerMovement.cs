using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    //Components
    Rigidbody rb;
    Animator anim;
    //Player 
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 100.0f;
    bool canJump = true;
    public bool isDead = false;
    Vector3 startPos;
    bool reSpawned = false;
    GameObject respawnPanel;
    public bool gameOver = false;
    public bool noRespawn;
    //last one standing
    bool startChecking = false;
    GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
        respawnPanel = GameObject.Find("RespawnPanel");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead == false)
        {
            respawnPanel.SetActive(false);
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            Vector3 rotateY = new Vector3(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);
            if (movement != Vector3.zero)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateY));
            }
            rb.MovePosition(rb.position + transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime + transform.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime);

            anim.SetFloat("BlendH", Input.GetAxis("Horizontal"));
            anim.SetFloat("BlendV", Input.GetAxis("Vertical"));
        }

    }

    private void Update()
    {
        if (isDead == false)
        {
            if (Input.GetButtonDown("Jump") && canJump == true)
            {
                canJump = false;
                rb.AddForce(Vector3.up * 800 * Time.deltaTime, ForceMode.VelocityChange);
                StartCoroutine(JumpAgain());
            }
        }

        if (isDead == true && reSpawned == false && gameOver == false && noRespawn == false)
        {
            reSpawned = true;
            respawnPanel.SetActive(true);
            respawnPanel.GetComponent<RespawnTimer>().enabled = true;
            StartCoroutine(RespawnWait());
        }
        if (isDead == true && reSpawned == false && gameOver == false && noRespawn == true)
        {
            reSpawned = true;
            GetComponent<DisplayColor>().NoRespawnExit();
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && startChecking == false)
        {
            startChecking = true;
            InvokeRepeating("CheckForWinner", 3, 3);
        }
    }

    void CheckForWinner()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount ==1 && noRespawn == true)
        {
            canvas.GetComponent<KillCount>().NoRespawnWinner(GetComponent<PhotonView>().Owner.NickName);
        }
    }

    IEnumerator JumpAgain()
    {
        yield return new WaitForSeconds(1);
        canJump = true;
    }

    IEnumerator RespawnWait()
    {
        yield return new WaitForSeconds(3);
        isDead = false;
        reSpawned = false;
        transform.position = startPos;
        GetComponent<DisplayColor>().Respawn(GetComponent<PhotonView>().Owner.NickName);
    }
}

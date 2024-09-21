using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;

    public float gravity;
    public Vector2 sensitivity;

    public Transform orientation;

    public float hp = 20f;

    public int shieldLives = 3;

    public float meleeDamage = 1f;
    public float projectileDamage = 2f;

    public float shieldCooldown = 2f;

    public float projectileCooldown = 2f;

    public GameObject projectile;

    Rigidbody rb;

    public GameObject cameraHandler;
    Camera cam;

    Vector3 moveDir;

    Vector3 camOffset;

    Vector2 input;

    float xRot;
    float yRot;

    Vector3 camPos;

    Text interactionText;

    InteractionHandler interactionHandler; 

    bool interacting = false;

    Interactable activeInteractable;

    GameObject weapon;

    bool attacking = false;

    bool firing = false;

    bool usingShield = false;

    float zRot = 0f;

    int shieldBlocks = 0;

    bool shieldAvailable = true;

    public Image shieldStatusImage;

    public Image projectileStatusImage;

    public Text hpText;


    public RectTransform hpBar;

    public bool tutorialActive = true;

    private Tutorial tutorialManager;

    public GameObject DeathUI;

    private float maxHp;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        maxHp = hp;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cam = cameraHandler.GetComponentInChildren<Camera>();
        camOffset = new Vector3(Mathf.Abs(gameObject.transform.position.x - cam.transform.position.x), Mathf.Abs(gameObject.transform.position.y - cam.transform.position.y), Mathf.Abs(gameObject.transform.position.z - cam.transform.position.z));
        interactionHandler = FindObjectOfType<InteractionHandler>();
        interactionText = GameObject.Find("InteractionText").GetComponent<Text>();
        hpText = GameObject.Find("HealthText").GetComponent<Text>();
        shieldStatusImage = GameObject.Find("ShieldStatusSymbol").GetComponent<Image>();
        projectileStatusImage = GameObject.Find("ProjectileStatusSymbol").GetComponent<Image>();
        hpBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        tutorialManager = FindObjectOfType<Tutorial>();

        /*
        sensitivity.x = Settings.getSensitivity();
        sensitivity.y = Settings.getSensitivity();
        */
        
        if (Settings.getTutorialComplete()){
            tutorialManager.active = false;
            tutorialActive = false;

        }else{
            
            GameObject.Find("PlayerObject").transform.position = new Vector3(-225.3f, -101.53f, -78.8f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP: " + (hp/20 * 100).ToString() + "%";
        hpBar.sizeDelta = new Vector2((hp/20 * 100), 100);

        if (hp <= 0){
            enableDeathUI();
        }else{
            weapon = cam.gameObject.GetComponentInChildren<Sword>().gameObject;
            camPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z) + camOffset;
            cameraHandler.transform.position = camPos;
            

            float mouseX = Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;

            yRot += mouseX;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            cam.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
            orientation.rotation = Quaternion.Euler(0f, yRot, 0f);


            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            moveDir = (orientation.forward * input.y) + (orientation.right * input.x);
            moveDir += new Vector3(0, gravity, 0);
            rb.velocity = moveDir.normalized * speed * 10f;
            
            if (shieldBlocks >= shieldLives){
                shieldBlocks = 0;
                usingShield = false;
                StartCoroutine(shieldAvailableCooldown());
            }

            if (Input.GetKeyDown(KeyCode.E)){
                if (interacting){
                    interactionHandler.activateTriggerable(activeInteractable.id);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
                if (!attacking){
                    attacking = true;
                    StartCoroutine(attack());
                }
            }

            if (Input.GetMouseButtonDown(1)){
                if (!usingShield && shieldAvailable){
                    StartCoroutine(useShield());
                }
            }

            if (Input.GetKeyDown(KeyCode.E)){
                if (!firing){
                    firing = true;
                    StartCoroutine(fireProjectile());
                }
            }

            if (Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.T)){
                SceneManager.LoadScene("MainMenu");
            }

            if (tutorialActive){
                if (Input.GetKeyDown(KeyCode.R)){
                    tutorialManager.pressedR();
                }
            }
        }

        
        
        

    }

    public void enableInteractionGui(Interactable interactable){
        interacting = true;
        activeInteractable = interactable;

        Debug.Log("Interacting with: " + interactable.interactableName);
        interactionText.text = interactable.interactableText;
    }

    public void disableInteractionGui(){
        interacting = false;
        activeInteractable = null;
        Debug.Log("Ending Interaction GUI");
        interactionText.text = "";

    }

    public IEnumerator attack() {
        weapon = cam.gameObject.GetComponentInChildren<Sword>().gameObject;
        Transform origTransform = weapon.transform;
        gameObject.GetComponentInChildren<PlayerAttackBox>().active = true;
        gameObject.GetComponentInChildren<PlayerAttackBox>().damage = meleeDamage;
            
        weapon.transform.eulerAngles = (orientation.eulerAngles + new Vector3(40, 0, 0));
        yield return new WaitForSeconds(0.1f);
        weapon.transform.eulerAngles = (orientation.eulerAngles + new Vector3(40, -15, 0));
        yield return new WaitForSeconds(0.1f);
        weapon.transform.eulerAngles = (orientation.eulerAngles + new Vector3(40, -15, 43));
        yield return new WaitForSeconds(0.1f);
        weapon.transform.eulerAngles = (orientation.eulerAngles);
        attacking = false;

        gameObject.GetComponentInChildren<PlayerAttackBox>().active = false;
    }

    public IEnumerator fireProjectile(){
        GameObject fired = Instantiate(projectile, cam.transform.position, cam.transform.rotation);
        fired.GetComponent<Projectile>().m_Damage = projectileDamage;
        
        for (int i = 0; i < 10; i++){
            yield return new WaitForSeconds(projectileCooldown/10);
            projectileStatusImage.color = new Color(shieldStatusImage.color.r, shieldStatusImage.color.g, shieldStatusImage.color.b, 0.1f * i);
        }
        firing = false;
    }

    public IEnumerator useShield(){
        usingShield = true;
        cam.gameObject.GetComponentInChildren<ShieldTool>().transform.position += new Vector3(0, 0.1f, 0);
        yield return new WaitForSeconds(0.2f);
        cam.gameObject.GetComponentInChildren<ShieldTool>().transform.position += new Vector3(0, 0.2f, 0);
        yield return new WaitForSeconds(1.4f);

        cam.gameObject.GetComponentInChildren<ShieldTool>().transform.position -= new Vector3(0, 0.3f, 0);
        usingShield = false;
    }

    public IEnumerator shieldAvailableCooldown(){
        cam.gameObject.GetComponentInChildren<ShieldTool>().gameObject.transform.position -= new Vector3(0, 0.3f, 0);
        shieldAvailable = false;
        for (int i = 0; i < 10; i++){
            yield return new WaitForSeconds(shieldCooldown/10);
            shieldStatusImage.color = new Color(shieldStatusImage.color.r, shieldStatusImage.color.g, shieldStatusImage.color.b, 0.1f * i);
        }
        shieldAvailable = true;
        cam.gameObject.GetComponentInChildren<ShieldTool>().gameObject.transform.position += new Vector3(0, 0.3f, 0);
    }

    public IEnumerator hitAnim(){
        if (!usingShield){
            zRot = 10;
            cam.gameObject.GetComponentInChildren<Sword>().gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            cam.gameObject.GetComponentInChildren<ShieldTool>().gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            cam.gameObject.GetComponentInChildren<Sword>().gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            cam.gameObject.GetComponentInChildren<ShieldTool>().gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            zRot = 0;
        }else{
            cam.gameObject.GetComponentInChildren<ShieldTool>().gameObject.transform.eulerAngles += new Vector3(0, 0, 15);
            yield return new WaitForSeconds(0.1f);
            cam.gameObject.GetComponentInChildren<ShieldTool>().gameObject.transform.eulerAngles -= new Vector3(0, 0, 15);
        }

    }

    public void takeDamage(float damage){
        if (!usingShield){
            StopCoroutine(hitAnim());
            hp -= damage;
            StartCoroutine(hitAnim());
            Debug.Log("Player took " + damage + " damage. HP: " + hp);
        }else{
            if (tutorialActive){
                Debug.Log("Player Blocked Attack");
                tutorialManager.dummyHit();
            }else{
                shieldBlocks++;
                StopCoroutine(hitAnim());
                StartCoroutine(hitAnim());
            }

        }
    }

    public void enableDeathUI(){
        Time.timeScale = 0;
        DeathUI.SetActive(true);
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        DeathUI.GetComponent<Canvas>().enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void heal(float amt){
        hp += amt;
        if (hp > maxHp){
            hp = maxHp;
        }
    }
}

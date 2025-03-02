using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerAnimate pa;
    public PlayerWeapon pw;

    public HealthBar healthBar;

    public int lockedToDim;
    public SpriteFlip dashSpriteFlip;
    [Header("XZ Input")]
    public bool TUTORIAL_MODE = false;
    public bool DUCK_MODE = false;
    public float speed;
    public float sprintMod, dashSpeed, dashTime, dashCooldown, dashGhostFreq, daggerDashSpeed, daggerDashTime;
    private float currentSprintMod, dashTimer, daggerDashTimer, dashCooldownTimer, dashGhostTimer;
    [HideInInspector] public float daggerDashMult;
    public bool dashing, daggerDashing;
    private Vector2 dashDirection, daggerDashDirection;
    Vector3 adjustedInput;
    public GameObject dashGhost;
    public MeshRenderer[] renderers;
    public GameObject step;
    public float stepSpawnFreq;
    private float stepSpawnTimer;
    Rigidbody rb;
    Vector2 input, lastNonzeroInput;

    [Header("Rotation")]
    public Camera cam;
    public Transform mousePoint, screenPoint, weaponBase, weapon, gyro;
    [HideInInspector] public Vector2 startScreenPos;
    float defaultWeaponOffset;

    public bool disableInput;

    public LayerMask ground;
    public Transform foot;
    bool grounded;

    public Animator hitOverlay;
    public CameraShake cs;

    public Health health;

    public bool overrideCheckpoint;

    public GameObject extraSongThing, backgroundMusic, duckCounter, deathAnim;

    public bool finalAnim;
    public GameObject[] splits;
    public GameObject particles1, particles2;
    public AudioSource tingaling;
    public Texture2D finalTexture;
    public SceneTransitioner st;

    float defaultFov;


    private void OnEnable()
    {
    }

    private void Awake()
    {
        //ReCheckpoint();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScreenPos = cam.WorldToScreenPoint(transform.position);
        defaultWeaponOffset = weapon.localPosition.z;
        health = GetComponent<Health>();

        deathAnim = GameObject.Find("Death");
        deathAnim.SetActive(false);

        defaultFov = 57;

        SwitchDim();
        Invoke("SwitchDim", 0.3f);

        Scene scene = SceneManager.GetActiveScene();
        int thing = scene.buildIndex;
        PlayerPrefs.SetInt("Scene", thing);

        if (PlayerPrefs.GetFloat("CheckpointX" + thing) != 0 && PlayerPrefs.GetFloat("CheckpointZ" + thing) != 0 && !overrideCheckpoint)
        {
            rb.MovePosition(new Vector3(PlayerPrefs.GetFloat("CheckpointX" + thing), transform.position.y, PlayerPrefs.GetFloat("CheckpointZ" + thing)));
            Debug.Log(transform.position);
        }

        //Invoke("ReCheckpoint", 0.2f);
        //Invoke("ReCheckpoint", 0.3f);
        //Invoke("ReCheckpoint", 0.4f);

        currentSprintMod = 1;

        health.OnDeath += OnDeath;
        health.OnHit += OnHit;
        health.OnHeal += OnHeal;

        healthBar.SetMaxHealth(health.GetMaxHealth());

        // StartFinalAnim();
    }

    private void ReCheckpoint()
    {
        Debug.Log(PlayerPrefs.GetFloat("CheckpointX") + ", " + PlayerPrefs.GetFloat("CheckpointZ"));
        if (PlayerPrefs.GetFloat("CheckpointX") != 0 && PlayerPrefs.GetFloat("CheckpointZ") != 0)
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("CheckpointX"), transform.position.y, PlayerPrefs.GetFloat("CheckpointZ"));
            Debug.Log(transform.position);
        }
    }

    private void SwitchDim() { if (lockedToDim != -1) GameManager.Inst.SwitchDimension((Dimension)lockedToDim); }

    private void Update()
    {
        // XZ input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { input.x = -1; }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { input.x = 1; }
        else { input.x = 0; }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { input.y = 1; }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { input.y = -1; }
        else { input.y = 0; }

        if (disableInput) input = Vector2.zero;

        if (input != Vector2.zero)
            lastNonzeroInput = input;

        adjustedInput = transform.right * input.x + transform.forward * input.y;

        pa.walking = input != Vector2.zero;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { currentSprintMod = sprintMod; cs.customFov = defaultFov + 10; }
        else { currentSprintMod = 1; cs.customFov = defaultFov; }

        dashCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && !dashing && dashCooldownTimer <= 0 && !disableInput && !GameManager.Inst.paused)
        {
            GetComponent<AudioSource>().Play();
            dashTimer = dashTime;
            dashing = true;
            if (input != Vector2.zero)
                dashDirection = new(adjustedInput.x, adjustedInput.z);
            else
                dashDirection = lastNonzeroInput;

            if (!TUTORIAL_MODE && lockedToDim == -1)
            {
                Dimension nextDimension = GameManager.Inst.dimension + 1;
                if ((int)nextDimension > 4)
                    nextDimension = 0;
                GameManager.Inst.SwitchDimension(nextDimension);
            }
            else {
                Dimension nextDimension = GameManager.Inst.dimension + 1;
                if ((int)nextDimension > lockedToDim)
                    nextDimension = 0;
                GameManager.Inst.SwitchDimension(nextDimension);
            }
        }
    }

    private void FixedUpdate()
    {
        grounded = Physics.OverlapSphere(foot.position, 0.1f, ground).Length > 0;
        if (grounded) { transform.position = new(transform.position.x, 0.5f, transform.position.z); }

        if (finalAnim) { health.Heal(10); dashSpriteFlip.transform.localPosition = Vector3.MoveTowards(dashSpriteFlip.transform.localPosition, new(0f, 1.7f, 0f), Time.fixedDeltaTime * 2); }

        daggerDashTimer -= Time.fixedDeltaTime;
        dashTimer -= Time.fixedDeltaTime;
        stepSpawnTimer -= Time.fixedDeltaTime * input.magnitude * currentSprintMod;
        if (stepSpawnTimer < 0)
        {
            stepSpawnTimer = stepSpawnFreq;
            GameObject go = Instantiate(step, new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z), Quaternion.identity);
            Destroy(go, 1);
        }

        if (dashing || (daggerDashing && daggerDashMult > 2.9f))
        {
            // Spawns dash ghosts
            dashGhostTimer -= Time.fixedDeltaTime;
            if (dashGhostTimer <= 0)
            {
                dashGhostTimer = dashGhostFreq;
                GameObject ghost = Instantiate(dashGhost, transform.position, Quaternion.identity);
                ghost.transform.GetChild(0).localEulerAngles = weaponBase.localEulerAngles;
                DashGhost dg = ghost.GetComponent<DashGhost>();
                for (int i = 0; i < dg.renderers.Length; i++)
                {
                    //dg.renderers[i].material.mainTexture = renderers[i].material.mainTexture;
                    dg.renderers[i].material.color = new Color(0, 1, 1);
                }
                Destroy(ghost, 0.25f);
            }
        }

        // Sets rb velocity
        if (dashing)
        {
            if (dashTimer < 0)
            {
                dashing = false;
                dashCooldownTimer = dashCooldown;
            }
            else
            {
                Vector2 adjustedVelocity = dashDirection.normalized * dashSpeed;
                rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
            }
        }

        if (!dashing && daggerDashing)
        {
            if (daggerDashTimer < 0)
            {
                daggerDashing = false;
            }
            else
            {
                Vector2 adjustedVelocity = daggerDashDirection.normalized * daggerDashSpeed * daggerDashMult;
                rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
            }
        }

        if (!dashing && !daggerDashing)
        {
            Vector2 adjustedVelocity = new Vector2(adjustedInput.x, adjustedInput.z).normalized * speed * currentSprintMod;
            rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);
        }
        // Sets rotation
        weaponBase.localEulerAngles = new(weaponBase.eulerAngles.x, RotationFromMouse() + 90 + pw.offset, 0);
        gyro.localEulerAngles = new(0, RotationFromMouse() + 90, 0);
        // Offsets weapon localpos to avoid clipping through torso when weapon faces side to side
        weapon.localPosition = new(0, weapon.localPosition.y, defaultWeaponOffset - Mathf.Abs(Mathf.Sin(weaponBase.eulerAngles.y * Mathf.Deg2Rad)) / 4);
    }

    public void StartDaggerDash(Vector2 direction, float mult = 1)
    {
        daggerDashDirection = direction;
        daggerDashTimer = daggerDashTime;
        daggerDashing = true;
        daggerDashMult = mult;
    }

    private float RotationFromMouse()
    {
        // return gyro.localRotation.eulerAngles.y;

        // ROTATES TOWARDS MOUSE by projecting player onto screen and finding angle between that and mouse (its messy)
        mousePoint.position = Input.mousePosition;
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        screenPoint.position = new Vector3(screenPos.x + Screen.width / 2 - startScreenPos.x, screenPos.y + Screen.height / 2 - startScreenPos.y, screenPos.z);
        float rot = Mathf.Rad2Deg * Mathf.Atan2((mousePoint.position.y - screenPoint.position.y), (-mousePoint.position.x + screenPoint.position.x));
        if (rot < 0) { rot += 360; }
        if (rot > 360) { rot -= 360; }
        return rot;
    }

    protected void OnDeath()
    {
        deathAnim.SetActive(true);
        Invoke("RestartScene", 2);
        healthBar.SetHealth(health.GetHealth());
        GameObject.Find("Background Music Manager").SetActive(false);
    }
    private void RestartScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    protected void OnHit(Damage damage)
    {
        hitOverlay.Play("Hit");
        healthBar.SetHealth(health.GetHealth());
        cs.Shake(0.51f, 1);
    }
    protected void OnHeal()
    {
        healthBar.SetHealth(health.GetHealth());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Set Camera"))
        {
            cs.introAnim = true;
            cs.standardFov = cam.fieldOfView;
            cs.standardPos = other.transform.GetChild(0).localPosition;
            cs.standardRot = other.transform.GetChild(0).localEulerAngles;
            cs.realLerpSpeed = 3;

            if (extraSongThing) extraSongThing.SetActive(true);
            if (backgroundMusic) backgroundMusic.SetActive(false);
            if (duckCounter) { duckCounter.SetActive(true); GameManager.Inst.SwitchDimension(Dimension.Conscientiousness); }
        }
    }

    public void StartFinalAnim()
    {
        if (finalAnim) return;
        weaponBase.gameObject.SetActive(false);
        disableInput = true;
        pw.disableInput = true;
        finalAnim = true;
        particles1.SetActive(true);
        Invoke("Split", 3);
    }
    private void Split()
    {
        foreach(GameObject go in splits) { go.SetActive(true); }
        Invoke("Merge", 3);
    }
    private void Merge()
    {
        foreach (GameObject go in splits)
        {
            go.GetComponent<BasicShake>().goTowards = particles2.transform;
            Invoke("ChangeSprite", 1f);
        }
    }
    private void ChangeSprite()
    {
        tingaling.Play();
        pa.enabled = false;
        pa.myMat.mainTexture = finalTexture;
        foreach (GameObject go in splits) { go.SetActive(false); }
        Invoke("Blast", 1);
    }
    private void Blast() { particles2.SetActive(true); Invoke("End", 3); }
    private void End()
    {
        st.NextScene();
    }
}

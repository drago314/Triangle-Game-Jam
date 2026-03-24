using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerAnimate pa;
    public PlayerWeapon pw;

    public HealthBar healthBar;

    public bool TUTORIAL_MODE = false;
    public bool DUCK_MODE = false;
    public bool USE_FREECAM = false;

    public int lockedToDim; // Start dim
    public SpriteFlip dashSpriteFlip; // Sprite that is flipped when dashing

    public float gravity;

    [Header("XZ Input")]
    public float speed;
    public float sprintMod, dashSpeed, dashTime, dashCooldown, dashGhostFreq, daggerDashSpeed, daggerDashTime;
    private float currentSprintMod, dashTimer, daggerDashTimer, dashCooldownTimer, dashGhostTimer;
    [HideInInspector] public float daggerDashMult;
    public Transform faceCamera;
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

    public Vector3 startPos;

    [Header("Rotation")]
    public Camera cam;
    public Transform mousePoint, screenPoint, weaponBase, weapon, gyro;
    public float weaponRotateSpeed;
    [HideInInspector] public Vector2 startScreenPos;
    float defaultWeaponOffset;

    public int water;

    public int maxWater = 9999;

    public TextMeshProUGUI waterText;

    public bool disableInput;

    public LayerMask ground;
    public Transform foot;
    bool grounded;

    public Animator hitOverlay;
    public CameraShake cs;

    public Health health;
    public int thorns;

    public bool overrideCheckpoint; // used in development to not automatically spawn at checkpoint

    public GameObject extraSongThing, backgroundMusic, duckCounter, deathAnim;

    [Header("Evolution")]
    public Animator hopAnimator;
    public bool[] evolutions;
    public GameObject[] evolutionComponents;
    public EvolutionMenu em;
    public Transform body;

    float defaultFov;

    private void Start()
    {
        evolutions = new bool[100];

        rb = GetComponent<Rigidbody>();
        startScreenPos = cam.WorldToScreenPoint(transform.position);
        defaultWeaponOffset = weapon.localPosition.z;
        health = GetComponent<Health>();

        deathAnim = GameObject.Find("Death");
        deathAnim.SetActive(false);

        defaultFov = 57;

        SwitchDim();
        Invoke("SwitchDim", 0.3f);

        // Loads in checkpoint data for current scene and sets player pos to that checkpoint
        Scene scene = SceneManager.GetActiveScene();
        int currentSceneIndex = scene.buildIndex;
        PlayerPrefs.SetInt("Scene", currentSceneIndex);

        if (PlayerPrefs.GetFloat("CheckpointX" + currentSceneIndex) != 0 && PlayerPrefs.GetFloat("CheckpointZ" + currentSceneIndex) != 0 && !overrideCheckpoint)
        {
            rb.MovePosition(new Vector3(PlayerPrefs.GetFloat("CheckpointX" + currentSceneIndex), transform.position.y, PlayerPrefs.GetFloat("CheckpointZ" + currentSceneIndex)));
        }

        currentSprintMod = 1;

        health.OnDeath += OnDeath;
        health.OnHit += OnHit;
        health.OnHeal += OnHeal;

        healthBar.SetMaxHealth(health.GetMaxHealth());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SwitchDim() { if (lockedToDim != -1) GameManager.Inst.SwitchDimension((Dimension)lockedToDim); }

    private void Update()
    {
        if (!deathAnim) {
            deathAnim = GameObject.Find("Death");
            deathAnim.SetActive(false);
        }

        // Timers
        dashCooldownTimer -= Time.deltaTime;

        // XZ input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { input.x = -1; }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { input.x = 1; }
        else { input.x = 0; }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { input.y = 1; }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { input.y = -1; }
        else { input.y = 0; }

        if (disableInput) input = Vector2.zero;

        // Adjusts input based on camera direction
        adjustedInput = -faceCamera.right * input.x + -faceCamera.forward * input.y;
        if (adjustedInput != Vector3.zero) lastNonzeroInput = new Vector2(adjustedInput.x, adjustedInput.z);

        pa.walking = input != Vector2.zero;
        
        // Sprint input
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { currentSprintMod = sprintMod; cs.customFov = defaultFov + 10; }
        else { currentSprintMod = 1; cs.customFov = defaultFov; }

        // Dash Input
        if (Input.GetKeyDown(KeyCode.Space) && !dashing && dashCooldownTimer <= 0 && !disableInput && !GameManager.Inst.paused)
        {
            GetComponent<AudioSource>().Play();
            dashTimer = dashTime;
            dashing = true;
            if (input != Vector2.zero)
                dashDirection = new(adjustedInput.x, adjustedInput.z);
            else
                dashDirection = lastNonzeroInput;

            // Cycles dimension based on start dimension
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

        hopAnimator.SetBool("Walking", input != Vector2.zero && !evolutions[0]);

        if (Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.U)) { SceneManager.LoadScene("Menu 1"); }
        if (Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    }

    private void FixedUpdate()
    {
        // Timers ----------------------------------------------------
        daggerDashTimer -= Time.fixedDeltaTime;
        dashTimer -= Time.fixedDeltaTime;
        stepSpawnTimer -= Time.fixedDeltaTime * input.magnitude * currentSprintMod;
        if (stepSpawnTimer < 0)
        {
            stepSpawnTimer = stepSpawnFreq;
            GameObject go = Instantiate(step, new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z), Quaternion.identity);
            Destroy(go, 1);
        }
        // ------------------------------------------------------------

        // TEMP - This locks the player to a specific y-level to avoid weird collision across tiles. Needs to be fixed for vertical movement
        grounded = Physics.OverlapSphere(foot.position, 0.1f, ground).Length > 0;
        //if (grounded) { transform.position = new(transform.position.x, 0.5f, transform.position.z); }
        if (!grounded) { rb.velocity = new(rb.velocity.x, rb.velocity.y - Time.fixedDeltaTime * gravity, rb.velocity.z); }

        // Spawns dash ghosts
        if (dashing || (daggerDashing && daggerDashMult > 2.9f))
        {
            dashGhostTimer -= Time.fixedDeltaTime;
            if (dashGhostTimer <= 0)
            {
                // Instantiates ghost prefab
                dashGhostTimer = dashGhostFreq;
                GameObject ghost = Instantiate(dashGhost, transform.position, Quaternion.identity);
                ghost.transform.GetChild(0).localEulerAngles = weaponBase.localEulerAngles;
                DashGhost dg = ghost.GetComponent<DashGhost>();
                for (int i = 0; i < dg.renderers.Length; i++)
                {
                    dg.renderers[i].material.color = new Color(0, 1, 1);
                }
                Destroy(ghost, 0.25f);
            }
        }

        // Default velocity
        Vector2 adjustedVelocity = new Vector2(adjustedInput.x, adjustedInput.z).normalized * speed * currentSprintMod;

        // Dashing
        if (dashing)
        {
            if (dashTimer < 0)
            {
                dashing = false;
                dashCooldownTimer = dashCooldown;
            }
            else
            {
                adjustedVelocity = dashDirection.normalized * dashSpeed;
            }
        }

        // Dagger dashing
        if (!dashing && daggerDashing)
        {
            if (daggerDashTimer < 0)
            {
                daggerDashing = false;
            }
            else
            {
                adjustedVelocity = daggerDashDirection.normalized * daggerDashSpeed * daggerDashMult;
            }
        }

        // Actually sets rb.velocity based on camera rotation
        rb.velocity = new Vector3(adjustedVelocity.x, rb.velocity.y, adjustedVelocity.y);

        // Sets rotation
        float rot = Mathf.LerpAngle(weaponBase.eulerAngles.y, RotationFromMouse() + 90 + pw.offset, weaponRotateSpeed * Time.fixedDeltaTime);
        weaponBase.localEulerAngles = new(weaponBase.eulerAngles.x, rot, 0);
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
        // ROTATES TOWARDS MOUSE by projecting player onto screen and finding angle between that and mouse (its messy)
        mousePoint.position = Input.mousePosition;
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        screenPoint.position = new Vector3(screenPos.x + Screen.width / 2 - startScreenPos.x, screenPos.y + Screen.height / 2 - startScreenPos.y, screenPos.z);
        float rot = Mathf.Rad2Deg * Mathf.Atan2((mousePoint.position.y - screenPoint.position.y), (-mousePoint.position.x + screenPoint.position.x));
        if (rot < 0) { rot += 360; }
        if (rot > 360) { rot -= 360; }

        if (USE_FREECAM) { rot = Mathf.Atan2(-lastNonzeroInput.y, lastNonzeroInput.x) * Mathf.Rad2Deg + 180; }

        return rot;
    }

    protected void OnDeath()
    {
        deathAnim.SetActive(true);
        Invoke("RestartScene", 2);
        healthBar.SetHealth(health.GetHealth());
        GameObject.Find("Background Music Manager").SetActive(false);
    }
    private void RestartScene() { SceneManager.LoadScene("Menu 1"); }
    protected void OnHit(Damage damage)
    {
        hitOverlay.Play("Hit");
        healthBar.SetHealth(health.GetHealth());
        //UpdateWater(-10);
        cs.Shake(0.51f, 1);
    }
    protected void OnHeal()
    {
        healthBar.SetHealth(health.GetHealth());
    }

    public void UpdateWater(int change)
    {
        water = Mathf.Clamp(water + change, 0, maxWater);
        waterText.text = water + "";
    }

    public void Evolve(Evolution evolution)
    {
        UpdateWater(-evolution.price);

        if (evolution.id >= 0) evolutions[evolution.id] = true;

        if (evolution.enableComponent) evolutionComponents[evolution.id].SetActive(true);
        body.localPosition += evolution.bodyOffset;
        speed += evolution.addSpeed;
        health.SetMaxHealth(health.GetMaxHealth() + evolution.addHealth);
        healthBar.SetMaxHealth(health.GetMaxHealth());

        if (evolution.enableProj >= 1) { pw.weaponsEnabled[evolution.enableProj - 1] = true; }

        if (evolution.id == 4) { thorns++; }
    }

    public Vector2 GetInput() { return input; }

    private void OnTriggerEnter(Collider other)
    {
        // Sets camera data to lock to a specific position/angle
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

    public IEnumerator LoadNewLevel(string level)
    {
        SceneManager.LoadScene(level);

        yield return new WaitForSeconds(1f);

        deathAnim.SetActive(false);
        rb.position = startPos;
        Debug.Log(transform.position);
    }
}

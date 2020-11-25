using System.Collections;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{    
    [SerializeField] float runSpeed=10f;
    [SerializeField] float penaltyTime = 2f;
    [SerializeField] float waitForWeaponLeave = 1f;
    public float weaponSpeed = 2f;

    [SerializeField] Joystick joystickDynamic1;
    [SerializeField] Joystick joystickDynamic2;
    [SerializeField] Transform sprite = null;

    WeaponControl weapon;

    CharacterController controller;
    PlayerDummyWeapon dummyWeapon;
    Rigidbody weaponRb;
    Animator anim;
    WeaponIndicator weaponIndicator;

    public bool isWeaponThrown = false;

    public float timer;
    
    bool restoringWeapon = false;

    Quaternion lastCharRotation;

    private void Awake()
    {
        weapon = FindObjectOfType<WeaponControl>();
        weaponIndicator = FindObjectOfType<WeaponIndicator>();
    }

    void Start()
    {
        weapon.gameObject.SetActive(false);
        dummyWeapon = GetComponentInChildren<PlayerDummyWeapon>();
        controller = GetComponent<CharacterController>();
        weaponRb = weapon.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        joystickDynamic1 = FindObjectOfType<DynamicJoystick>();
        joystickDynamic2 = FindObjectOfType<DynamicJoystick2>();

        weaponIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        MoveCharacter();
        CatchWeaponAfterThrow();
        AnimationControl();
    }

    void AnimationControl()
    {
        if(joystickDynamic1.Direction.magnitude > 0.2f)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    void MoveCharacter()
    {
        sprite.position = new Vector3(joystickDynamic1.Horizontal + transform.position.x, 0.01f, joystickDynamic1.Vertical + transform.position.z);
        if(joystickDynamic1.Direction.magnitude != 0f)
        {
            transform.LookAt(new Vector3(sprite.position.x, 0f, sprite.position.z));
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            lastCharRotation = transform.rotation;
        }
        else
        {
            transform.rotation = lastCharRotation;
        }

        

        var v = joystickDynamic1.Vertical; 
        var h = joystickDynamic1.Horizontal;


        if (h >= 0.2f || h <= -0.2f || v >= 0.2f || v <= -0.2f)
        {
            controller.Move(transform.forward*Time.deltaTime*runSpeed);
        }
        
    }
    
    public void Attack()
    {
        if (!isWeaponThrown)
        {
            CheckForWalls();
            timer = 0f;
            GetWeaponIntoPosition();
            weapon.gameObject.SetActive(true);
            Vector3 joystickDir3 = new Vector3(joystickDynamic2.Horizontal, 0f, joystickDynamic2.Vertical);
            weaponRb.AddForce(joystickDir3 * -weaponSpeed, ForceMode.Impulse);
            LoseWeapon();
            joystickDynamic2.gameObject.SetActive(false);
            weaponIndicator.gameObject.SetActive(true);
        }
    }

    void GetWeaponIntoPosition()
    {
        weapon.transform.position = transform.position;        
        weapon.transform.rotation = transform.rotation;
    }

    void CheckForWalls()
    {
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, weapon.GetComponent<CapsuleCollider>().radius, hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag("Wall"))
            {
                Invoke("CatchWeapon", 0.1f);
            }
        }
    }

    private void LoseWeapon()
    {        
        dummyWeapon.gameObject.SetActive(false);
        isWeaponThrown = true;
    }

    public void CatchWeapon()
    {
        isWeaponThrown = false;
        weaponRb.velocity = Vector3.zero;
        weapon.gameObject.SetActive(false);
        dummyWeapon.gameObject.SetActive(true);
        joystickDynamic2.gameObject.SetActive(true);
        weaponIndicator.gameObject.SetActive(false);
    }

    void CatchWeaponAfterThrow()
    {
        if(timer >= waitForWeaponLeave && Vector3.Distance(weapon.transform.position, transform.position) < 1.5f && !restoringWeapon)
        {
            CatchWeapon();
        }
    }

    public void FailedToCatchWeapon()
    {
        if (isWeaponThrown) 
        {
            StartCoroutine(RestoreWeapon());
            weaponIndicator.gameObject.SetActive(false);
        }
    }

    IEnumerator RestoreWeapon()
    {
        restoringWeapon = true;
        yield return new WaitForSeconds(penaltyTime);
        CatchWeapon();
        restoringWeapon = false;
        StopCoroutine(RestoreWeapon());        
    }
}
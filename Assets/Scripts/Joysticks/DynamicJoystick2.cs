using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick2 : Joystick
{
    public float MoveThreshold2 { get { return moveThreshold2; } set { moveThreshold2 = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold2 = 1;

    CharacterControl controller;
    TimeManager timeManager;
    Animator anim;

    public Vector2 orginalPos;

    protected override void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        controller = FindObjectOfType<CharacterControl>();
        orginalPos = background.anchoredPosition;
        anim = controller.GetComponent<Animator>();

        MoveThreshold2 = moveThreshold2;
        base.Start();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {       
        timeManager.SlowDown();
        

        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);     
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        anim.SetTrigger("Throw");
        Time.timeScale = 1f;
        controller.Attack();

        background.anchoredPosition = orginalPos;
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {

        //if (magnitude > moveThreshold2)
        //{
        //    Vector2 difference = normalised * (magnitude - moveThreshold2) * radius;
        //    background.anchoredPosition += difference;
        //}
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}
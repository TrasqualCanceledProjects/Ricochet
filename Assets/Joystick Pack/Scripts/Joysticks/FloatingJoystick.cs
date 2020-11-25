using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    CharacterControl controller;
    TimeManager timeManager;

    protected override void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();

        base.Start();
        background.gameObject.SetActive(false);
        controller = FindObjectOfType<CharacterControl>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        timeManager.SlowDown();
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Time.timeScale = 1f;
        controller.Attack();
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}
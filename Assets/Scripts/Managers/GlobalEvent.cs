using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalEvent : MonoBehaviour
{
    public static GlobalEvent Instance;

    public GlobalEventsManagerScriptableObject GlobalEventData;

    public GameObject PanelGlobalEvent = null;
    public float SliderGlobalEventTimer = 10.0f;

    public Image SliderCurrentEvent = null;
    public Image ImageCurrentEvent = null;
    private int CurrentEventIndex = 0;

    public List<Image> ListEventSlider = new List<Image>();
    public List<Image> ListEventImage = new List<Image>();

    private bool IsLastEventTriggered = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetAllEventImages();
        ResetAllEventSliders();
        ResetCurrentEventSlider();
        ResetCurrentEventImage(CurrentEventIndex);
    }

    private void Update()
    {
        if(IsLastEventTriggered ==false)
        {
            IncreaseCurrentTimer();
        }
    }

    private void SetAllEventImages()
    {
        for (int i = 0; i < ListEventImage.Count; i++)
        {
            ListEventImage[i].sprite = GlobalEventData.GlobalEventsSprites[i];
        }
    }

    private void ResetCurrentEventImage(int index)
    {
        ImageCurrentEvent.sprite = GlobalEventData.GlobalEventsSprites[index];
    }

    private void ResetAllEventSliders()
    {
        foreach (Image slider_image in ListEventSlider)
        {
            slider_image.fillAmount = 0.0f;
        }        
    }

    private void ChangeSliderColour()
    {
        ListEventSlider[CurrentEventIndex].color = Color.red;
    }

    private void ChangeCurrentSliderColour()
    {
        SliderCurrentEvent.color = Color.red;
    }

    private void ResetCurrentEventSlider()
    {
        SliderCurrentEvent.fillAmount = 0.0f;
    }

    private void IncreaseCurrentTimer()
    {
        SliderCurrentEvent.fillAmount += Time.deltaTime / SliderGlobalEventTimer;
        ListEventSlider[CurrentEventIndex].fillAmount += Time.deltaTime / SliderGlobalEventTimer;

        if(SliderCurrentEvent.fillAmount >= 1.0f)
        {
            ChangeSliderColour();
            SwitchCurrentEventIndex(); // this needs to be triggered between the Reset functions
            TriggerEvent();

            if (IsLastEventTriggered == false)
            {
                ResetCurrentEventSlider();
                ResetCurrentEventImage(CurrentEventIndex);
            }
        }
    }

    private void TriggerEvent()
    {
        GameManager.Instance.TriggerEnemySpawnEvent();
    }

    private void SwitchCurrentEventIndex()
    {
        if(CurrentEventIndex == ListEventSlider.Count-1)
        {
            ChangeCurrentSliderColour();
            IsLastEventTriggered = true;
        }

        else
        {
            CurrentEventIndex += 1;
        }
    }
}

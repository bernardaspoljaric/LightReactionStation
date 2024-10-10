using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimeLineController : MonoBehaviour
{
  [SerializeField] private TMP_Text _timeTextTMP;

  private Slider _sliderTime = null;
  private NVideoPlugin _nVideoPlugin = null;

  private bool _wasPlayingBeforeTimelineDrag;
  private bool _isHoveringOverTimeline;
  private bool _isDraggingSeekBar = false;

  private void Awake()
  {
    Init();
    CreateTimelineDragEvents();
  }

  private void OnEnable()
  {
    Init();
  }

  private void Init()
  {
    GetComponents();
  }

  private void GetComponents()
  {
    _sliderTime = GetComponent<Slider>();
    _nVideoPlugin = GetComponentInParent<NVideoPlayerControls>().NVideoPlayer.Player;
  }

  private void CreateTimelineDragEvents()
  {
    EventTrigger trigger = GetComponent<EventTrigger>();
    if (trigger != null)
    {
      EventTrigger.Entry entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerDown;
      entry.callback.AddListener((data) => { OnTimeSliderBeginDrag(); });
      trigger.triggers.Add(entry);

      entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.Drag;
      entry.callback.AddListener((data) => { OnTimeSliderDrag(); });
      trigger.triggers.Add(entry);

      entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerUp;
      entry.callback.AddListener((data) => { OnTimeSliderEndDrag(); });
      trigger.triggers.Add(entry);

      entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerEnter;
      entry.callback.AddListener((data) => { OnTimelineBeginHover((PointerEventData)data); });
      trigger.triggers.Add(entry);

      entry = new EventTrigger.Entry();
      entry.eventID = EventTriggerType.PointerExit;
      entry.callback.AddListener((data) => { OnTimelineEndHover((PointerEventData)data); });
      trigger.triggers.Add(entry);
    }
  }

  private void OnTimeSliderBeginDrag()
  {
    //Debug.Log("OnTimeSliderBeginDrag");
    _isDraggingSeekBar = true; 
  }

  private void OnTimeSliderDrag()
  {
    if (_nVideoPlugin != null)
    {
      _isDraggingSeekBar = true;
      _isHoveringOverTimeline = true;
    }
  }

  private void OnTimeSliderEndDrag()
  {
    //Debug.Log("OnTimeSliderEndDrag");
    _isDraggingSeekBar = false;
    _nVideoPlugin.SetTime((long)((double)_nVideoPlugin.Duration * _sliderTime.value));
  }

  private void OnTimelineBeginHover(PointerEventData eventData)
  {
    if (eventData.pointerCurrentRaycast.gameObject != null)
    {
      _isHoveringOverTimeline = true;
      //Set scale of slider if necessary
      _sliderTime.transform.localScale = new Vector3(1f, 1f, 1f);
    }
  }

  private void OnTimelineEndHover(PointerEventData eventData)
  {
    _isHoveringOverTimeline = false;
    //Set scale of slider if necessary
    _sliderTime.transform.localScale = new Vector3(1f, 1f, 1f);
  }

  private void UpdateTimeText()
  {
    if (_timeTextTMP)
    {
      string currentTime = GetTimeString(_nVideoPlugin.Time);
      string totalTime = GetTimeString(_nVideoPlugin.Duration);
  
      _timeTextTMP.text = string.Format("{0} / {1}", currentTime, totalTime);
    }
  }

  //Update the position of the Seek slider to the match the VLC Player
  void UpdateSeekBar()
  {
    var duration = _nVideoPlugin.Duration;
    if (duration > 0)
      _sliderTime.value = (float)((double)_nVideoPlugin.Time / duration);
  }

  private void Update()
  {
    if (_nVideoPlugin != null && _isDraggingSeekBar == false)
    {
      UpdateSeekBar(); 
      UpdateTimeText();
    }
  }

  #region internal

  private string GetTimeString(long timeMilliseconds)
  {
    float totalSeconds = timeMilliseconds / 1000;
    float totalMilliseconds = timeMilliseconds;
    int hours = Mathf.FloorToInt(totalMilliseconds / 3600000);
    float usedSeconds = hours * 60f * 60f;

    int minutes = Mathf.FloorToInt((totalSeconds - usedSeconds) / 60f);
    usedSeconds += minutes * 60f;

    int seconds = Mathf.FloorToInt(totalSeconds - usedSeconds);

    string result;
    if (hours <= 0)
    {
      result = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    else
    {
      result = string.Format("{2}:{0:00}:{1:00}", minutes, seconds, hours);
    }

    return result;
  }

  #endregion
}


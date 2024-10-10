using System;
using Novena.Settings;
using UnityEngine;

namespace Novena.Components.Idle
{
  public class IdleController : MonoBehaviour
  {
    /// <summary>
    /// When timer ends and idle is enabled.
    /// </summary>
    public static Action OnIdleEnabled;
    
    /// <summary>
    /// Singletone instance.
    /// </summary>
    public static IdleController Instance { get; set; }

    [Tooltip("How long until idle invokes")]
    
    [SerializeField]
    private float _resetTime;
    
    [Tooltip("Current timer time (for editor only)")]
    [SerializeField] private float _currentTime;

    [Tooltip("Name of node in noody that is designated for idle.")]
    [SerializeField] private string _idleNodeName;
    

    #region Timer

    private float _timeRemaining;
    private bool _timerIsRunning = true;

    #endregion

    private void Awake()
    {
      Instance = this;
      Settings.Settings.OnSettingsUpdate += OnSettingsUpdate;
    }

    private void OnSettingsUpdate()
    {
      _resetTime = _resetTime.GetSettingsValue<float>("IdleTimer");
      _timeRemaining = _resetTime;
    }

    /// <summary>
    /// Reset idle timer to defined reset time.
    /// </summary>
    public void ResetIdleTimer()
    {
      ResetTimer();
    }

    /// <summary>
    /// Reset's time.
    /// </summary>
    private void ResetTimer()
    {
      _timeRemaining = _resetTime;
      _timerIsRunning = true;
    }

    private void EnableIdle()
    {
      OnIdleEnabled?.Invoke();
      IdleHelper.GoToIdleNode(_idleNodeName);
    }

    /// <summary>
    /// Detects input and resets timer.
    /// </summary>
    private void CheckInput()
    {
      if (Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
      {
        ResetTimer();
      }
    }

    private void Update()
    {
      CheckInput();
      
      if (_timerIsRunning)
      {
        if (_timeRemaining > 0)
        {
          _timeRemaining -= Time.deltaTime;
        }
        else
        {
          _timerIsRunning = false;
          _timeRemaining = _resetTime;
          EnableIdle();
        }
      }

      _currentTime = _timeRemaining;
    }
  }
}
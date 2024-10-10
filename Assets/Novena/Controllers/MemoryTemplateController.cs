using Doozy.Engine;
using Novena.Games.MemoryGame;
using Novena.UiUtility.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryTemplateController : UiController
{
  public GameManager GameManager;
  public override void OnShowViewStart()
  {
    base.OnShowViewStart();
    GameManager.Setup();
  }

  public void Back()
  {
    GameEventMessage.SendEvent("Back");
  }
}

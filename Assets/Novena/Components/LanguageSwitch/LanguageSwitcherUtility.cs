using System;
using System.Linq;
using DG.Tweening;
using Doozy.Engine.Nody;
using UnityEngine;

namespace Novena.Components.LanguageSwitch
{
  public class LanguageSwitcherUtility
  {
    public static bool UseLanguageSwitchNode { get; private set; }

    private static GraphController _graphController;

    public LanguageSwitcherUtility(bool useLanguageSwitchNode)
    {
      UseLanguageSwitchNode = useLanguageSwitchNode;
    }

    /// <summary>
    /// Invoke node switch to language node.
    /// </summary>
    /// <param name="nodeName">Name of language splash node</param>
    /// <param name="timeToStay">How long to stay on Node</param>
    public static void GoToLanguageNode(string nodeName = "LanguageSplash", float timeToStay = 1f)
    {
      try
      {
        GetAndSetController();

        if (_graphController is { })
        {
          //If node is all ready active dont invoke it again
          if (_graphController.Graph.ActiveNode.Name == nodeName) return;

          //Go to Language node
          _graphController.Graph.SetActiveNodeByName(nodeName);

          //Return to previous node
          DOVirtual.DelayedCall(timeToStay, () =>
          {
            _graphController.Graph.SetActiveNodeByName(_graphController.Graph.PreviousActiveNode.Name);
          });
        }
      }
      catch (Exception e)
      {
        Debug.Log(e);
        throw;
      }
    }

    private static void GetAndSetController()
    {
      if (_graphController != null) return;

      _graphController = GraphController.Database
        .FirstOrDefault(d => d.ControllerName == "UiNavigation");
    }
  }
}
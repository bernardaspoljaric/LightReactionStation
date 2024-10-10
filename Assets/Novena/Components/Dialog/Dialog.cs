using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Novena.Components.Dialog
{
  public class Dialog
  {
    private string Title { get; set; }
    private string Text { get; set; }
    private List<DialogButton> Buttons { get; set; }


    #region Private fields

    private DialogUi _dialogUi;

    #endregion
  
    public Dialog(string title, string text, List<DialogButton> buttons = null)
    {
      try
      {
        Title = title;
        Text = text;
        Buttons = buttons;
        _generateAndShowDialog();
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }
    }

    /// <summary>
    /// Hide dialog.
    /// </summary>
    public void Close()
    {
      GameObject.Destroy(_dialogUi);
    }

    public void Show()
    {
      _dialogUi.gameObject.SetActive(true);
    }

    private void _generateAndShowDialog()
    {
      var overrlayTransform = GameObject.FindObjectOfType<UiOverlayView>();
      var dialogUiPrefab = overrlayTransform.Overrlays.FirstOrDefault((d) => d.name == "DialogUi").GetComponent<DialogUi>();
      _dialogUi = GameObject.Instantiate(dialogUiPrefab, overrlayTransform.transform);
      _dialogUi.Setup(Title, Text, Buttons);
    }
  }

  public class DialogButton
  {
    public string Label { get; set; }
    public Action Action { get; set; }
  }
}
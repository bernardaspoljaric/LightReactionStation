using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novena
{
    public class NudpExample : MonoBehaviour
    {
        public TMP_Text text;
        public NUDP nudp;

        public Slider slider;

        void Awake()
        {
            //Set the remote IP and ports in awake or before NUDP object is enabled if needed to get data from settings 
           /*  nudp.remoteIP = "127.0.0.1";
            nudp.remotePort = 60747;
            nudp.localPort = 5005; */

            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            int intValue = Convert.ToInt32(sliderValue);

            string formattedString = string.Format("B2_{0:D3}<CR>", intValue);

            nudp.SendData(formattedString);
        }

        public void OnUdpDataReceived(string data)
        {
            text.text = data;
        }
    }
}
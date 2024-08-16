using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class Resource : ResourceValue
    {
        public int MaximumValue
        {
            get
            {
                return maximumValue;
            }
            set
            {
                maximumValue = value;
                if (maximumValue > 0 && this.Value > maximumValue)
                {
                    this.Value = maximumValue;
                }
            }
        }
        [SerializeField] private int maximumValue = 9999;

        public override int Value
        {
            set
            {
                if (MaximumValue > 0 && value > MaximumValue)
                {
                    value = MaximumValue;
                }
                if (Value == value)
                {
                    return;
                }
                base.Value = value;
                if (Display != null)
                {
                    Display.SetText(Value.ToString());
                }
            }
        }

        public ResourceDisplay Display
        {
            get
            {
                return display;
            }
            private set
            {
                display = value;
            }
        }
        [SerializeField] private ResourceDisplay display;


        public void OnValidate()
        {
            if (Display != null)
            {
                Display.SetData(Value.ToString(), Data.Icon);
            }
        }
    }
}
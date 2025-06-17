using HexTecGames.Basics;
using HexTecGames.Basics.UI;
using HexTecGames.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/Grid/PlacementData")]
    public class PlacementData : DisplayableObject
    {
        public GridObjectData Data
        {
            get
            {
                return this.data;
            }
            private set
            {
                this.data = value;
            }
        }
        [SerializeField] private GridObjectData data;
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            private set
            {
                displayName = value;
            }
        }
        [SerializeField] private string displayName;
        
        public ColorType ColorType
        {
            get
            {
                return colorType;
            }
            set
            {
                colorType = value;
            }
        }
        [Space, SerializeField] private ColorType colorType;
        protected Color IconColor
        {
            get
            {
                return iconColor;
            }
            set
            {
                iconColor = value;
            }
        }
        [DrawIf(nameof(colorType), ColorType.Custom), SerializeField] private Color iconColor = Color.white;

        public bool IsDraggable
        {
            get
            {
                return isDraggable;
            }
            set
            {
                isDraggable = value;
            }
        }
        [Space, SerializeField] private bool isDraggable;
        public virtual bool IsReplaceable
        {
            get
            {
                return isReplaceable;
            }
            private set
            {
                isReplaceable = value;
            }
        }
        [SerializeField] private bool isReplaceable;

        public SoundClipBase PlacementSound
        {
            get
            {
                return placementSound;
            }
        }
        [Space, SerializeField] private SoundClipBase placementSound = default;

        public KeyCode Hotkey
        {
            get
            {
                return this.hotkey;
            }
            private set
            {
                this.hotkey = value;
            }
        }
        private KeyCode hotkey;

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(DisplayName) && Data != null)
            {
                DisplayName = Utility.CovertToDisplayName(Data.name);
            }
        }

        public Color GetColor()
        {
            if (ColorType == ColorType.Custom)
            {
                return IconColor;
            }
            else if (Data != null)
            {
                return Data.Color;
            }
            return Color.white;
        }
    }
}
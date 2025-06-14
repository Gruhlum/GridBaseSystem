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
        public Color IconColor
        {
            get
            {
                return iconColor;
            }
            private set
            {
                iconColor = value;
            }
        }
        [SerializeField] private Color iconColor = Color.white;

        public GridObjectDataBase Data
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
        [SerializeField] private GridObjectDataBase data;

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
        [SerializeField] private bool isDraggable;
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
        [SerializeField] private SoundClipBase placementSound = default;

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
    }
}
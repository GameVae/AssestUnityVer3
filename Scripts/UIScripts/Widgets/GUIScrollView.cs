﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(ScrollRect))]
    public class GUIScrollView : CustomGUI
    {
        [SerializeField] private Sprite backgroundSprite;
        [SerializeField, HideInInspector] private ScrollRect scrollRect;
        [SerializeField, HideInInspector] private Image background;

        public RectTransform Content
        {
            get { return ScrollRect?.content; }
        }

        public ScrollRect ScrollRect
        {
            get { return scrollRect ?? (scrollRect = GetComponent<ScrollRect>()); }
            private set { scrollRect = value; }
        }

        public override Image MaskImage
        {
            get { return maskImage ?? (maskImage = GetComponent<Image>()); }
            protected set { maskImage = value; }
        }

        public Image Background
        {
            get { return background ?? (background = transform.GetChild(0).GetComponent<Image>()); }
            protected set { background = value; }
        }

        public override void InteractableChange(bool value) { }

        public override void SetChildrenDependence() { }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Background)
                Background.sprite = backgroundSprite;
        }
#endif
    }
}
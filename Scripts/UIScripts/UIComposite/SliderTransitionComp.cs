﻿using UnityEngine;
using static UnityEngine.UI.Selectable;
using UnityEngine.UI;

namespace UI.Composites
{
    [RequireComponent(typeof(Slider))]
    public class SliderTransitionComp : TransitionComp
    {
        [SerializeField, HideInInspector] private Slider slider;

        public Slider Slider
        {
            get { return slider ?? (slider = GetComponent<Slider>()); }
        }

        public override Transition Transition
        {
            get { return Slider.transition; }
            set
            {
                Slider.transition = value;
            }
        }

        public override Object TargetDirty
        {
            get { return Slider; }
        }

        public override bool ConfirmOffset()
        {
            bool isChanged = false;
            if (transitionObject != null)
            {
                if (Slider.colors != transitionObject.Colors)
                {
                    Slider.colors = transitionObject.Colors;
                    isChanged = true;
                }

                if (!Slider.spriteState.Equal(transitionObject.SpriteState))
                {
                    Slider.spriteState = transitionObject.SpriteState;
                    isChanged = true;
                }
            }
            return isChanged;
        }

        public override void Refresh()
        {
            base.Refresh();
            FindSlider();
        }

        private void FindSlider()
        {
            slider = GetComponent<Slider>();
        }
    }
}
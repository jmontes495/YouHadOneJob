using System;
using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public class Codeline : UIButtonSetter
    {
        [SerializeField]
        private Text label;
        [SerializeField]
        private Image bugHighlight;

        private Action<Codeline> onFixBug;

        protected override void Awake ()
        {
            base.Awake ();
            SetFixed ();
        }

        public void Setup (string text)
        {
            label.text = text;
        }

        public void SetBugged (Action<Codeline> onFixBug)
        {
            bugHighlight.gameObject.SetActive (true);
            this.onFixBug = onFixBug;
            SetInteractable (true);
        }

        public void SetFixed ()
        {
            bugHighlight.gameObject.SetActive (false);
            SetInteractable (false);
        }

        protected override void HandleClick ()
        {
            SetFixed ();
            onFixBug (this);
            onFixBug = null;
        }
    }
}
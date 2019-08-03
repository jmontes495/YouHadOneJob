using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    [RequireComponent (typeof (Button))]
    public abstract class UIButtonSetter : MonoBehaviour
    {
        private Button button;

        protected virtual void Awake ()
        {
            button = GetComponent<Button> ();
            button.onClick.AddListener (HandleClick);
        }

        protected abstract void HandleClick ();
    }

}
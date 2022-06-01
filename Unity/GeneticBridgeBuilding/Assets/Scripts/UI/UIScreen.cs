using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class UIScreen:MonoBehaviour
    {
        [SerializeField]protected bool showing = true;
        public Screens screen;
        protected List<GameObject> subObjects= new List<GameObject>();
        
        
        public void Start()
        {
            foreach (var transform in GetComponentsInChildren<RectTransform>())
            {
                subObjects.Add(transform.gameObject);
            }
            Hide();
        }

        public void  Hide()
        {
            if (!showing) return;
            foreach (var o in subObjects)
            {
                o.SetActive(false);
            }

            showing = false;

        }

        public void Show()
        {
            if (showing) return;

            foreach (var o in subObjects)
            {

                o.SetActive(true);
            }

            showing = true;
        }

       
    }
}
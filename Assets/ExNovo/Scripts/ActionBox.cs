using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExNovo
{
	public class ActionBox : MonoBehaviour
	{
        private Image Image;
        private TMPro.TMP_Text Text;

        private void Start()
		{
            Image = GetComponent<Image>();
            if (Image == null)
            {
                throw new MissingComponentException("Requires Image");
            }

            Text = GetComponentInChildren<TMPro.TMP_Text>();
            if (Text == null)
            {
                throw new MissingComponentException("Requires child with TextMesh Pro Text");
            }
		}

        public void ChangeVisuals(string text, Color color)
        {
            gameObject.SetActive(true);
            Text.text = text;
            Image.color = color;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
	}
}

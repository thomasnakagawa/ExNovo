using UnityEngine;
using UnityEngine.UI;

namespace ExNovo
{
    public class ExNovoBoxUI : MonoBehaviour
    {
        [SerializeField] private ExNovoBoxUI Next1 = default;
        [SerializeField] private ExNovoBoxUI Next2 = default;
        [SerializeField] private ExNovoBoxUI Next3 = default;

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

        public void OnChangeActionTreePosition(ExNovoActionTreeNode actionTreeNode)
        {
            if (actionTreeNode == null)
            {
                Hide(true/*recursive*/);
                return;
            }
            if (actionTreeNode.IsRoot)
            {
                Hide(false/*not recursive*/);
            }
            else
            {
                ChangeVisuals(actionTreeNode.ActionName, actionTreeNode.Color);
            }

            if (Next1 != null)
            {
                Next1.OnChangeActionTreePosition(actionTreeNode.Child(1));
            }
            if (Next2 != null)
            {
                Next2.OnChangeActionTreePosition(actionTreeNode.Child(2));
            }
            if (Next3 != null)
            {
                Next3.OnChangeActionTreePosition(actionTreeNode.Child(3));
            }
        }

        private void ChangeVisuals(string text, Color color)
        {
            gameObject.SetActive(true);
            Text.text = text;
            Image.color = color;
        }

        public void Hide(bool recursive)
        {
            gameObject.SetActive(false);
            if (recursive)
            {
                if (Next1 != null)
                {
                    Next1.Hide(true);
                }
                if (Next2 != null)
                {
                    Next2.Hide(true);
                }
                if (Next3 != null)
                {
                    Next3.Hide(true);
                }
            }
        }

    }
}

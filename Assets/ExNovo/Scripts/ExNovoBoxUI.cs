using UnityEngine;
using UnityEngine.UI;

namespace ExNovo
{
    /// <summary>
    /// Controls and show the boxes that make up the visuals of ExNovo.
    /// </summary>
    public class ExNovoBoxUI : MonoBehaviour
    {
        [SerializeField] private ExNovoBoxUI Next1 = default;
        [SerializeField] private ExNovoBoxUI Next2 = default;
        [SerializeField] private ExNovoBoxUI Next3 = default;

        private Image Image;
        private TMPro.TMP_Text Text;

        private void Awake()
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

        /// <summary>
        /// Updates the visuals of this BoxUI node and any higher up BoxUI nodes based on what the currently selected actionTreeNode is
        /// </summary>
        /// <param name="actionTreeNode"></param>
        public void OnChangeActionTreePosition(ExNovoActionTreeNode actionTreeNode)
        {
            if (actionTreeNode == null)
            {
                Hide(true/*hide recursively*/);
                return;
            }
            if (actionTreeNode.IsRoot)
            {
                Hide(false/*not recursive, show the rest of the tree*/);
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

        /// <summary>
        /// Hides this BoxUI. If recursive=true, then also hide any higher up BoxUI nodes
        /// </summary>
        /// <param name="recursive">When true, also hides higher up BoxUI nodes</param>
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

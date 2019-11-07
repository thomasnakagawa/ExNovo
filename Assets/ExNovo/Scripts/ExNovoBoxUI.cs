using UnityEngine;
using UnityEngine.UI;

namespace ExNovo
{
    /// <summary>
    /// Controls and show the boxes that make up the visuals of ExNovo.
    /// </summary>
    public class ExNovoBoxUI : MonoBehaviour
    {
        [Header("Next branch BoxUIs")]
        [SerializeField] private ExNovoBoxUI Next1 = default;
        [SerializeField] private ExNovoBoxUI Next2 = default;
        [SerializeField] private ExNovoBoxUI Next3 = default;

        [Header("Visuals")]
        [SerializeField] private Sprite HoverSprite = default;
        [SerializeField] private Color HoverTint = Color.white;
        [SerializeField] private float HoverTintLerp = 0.5f;

        private Sprite UnHoverSprite;
        private Color MainBoxColor;

        private Image Image;
        private TMPro.TMP_Text Text;

        private void Awake()
        {
            Image = GetComponent<Image>();
            if (Image == null)
            {
                throw new MissingComponentException("Requires Image");
            }
            UnHoverSprite = Image.sprite;

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
            MainBoxColor = color;
            Image.color = MainBoxColor;
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

        public void SetHover(bool hover)
        {
            if (hover && HoverSprite != null)
            {
                Image.color = Color.Lerp(MainBoxColor, HoverTint, HoverTintLerp);
                Image.sprite = HoverSprite;
            }
            else
            {
                Image.color = MainBoxColor;
                Image.sprite = UnHoverSprite;
            }
        }

        /// <summary>
        /// Sets a child box to be hovered, and unhovers other child boxes
        /// </summary>
        /// <param name="childNumber">Which child box to hover (1, 2 or 3)</param>
        public void SetHoverChild(int childNumber)
        {
            if (childNumber < 1 || childNumber > 3)
            {
                throw new System.ArgumentOutOfRangeException(nameof(childNumber), "Child number must be 1, 2 or 3");
            }
            if (Next1 != null)
            {
                Next1.SetHover(childNumber == 1);
            }
            if (Next2 != null)
            {
                Next2.SetHover(childNumber == 2);
            }
            if (Next3 != null)
            {
                Next3.SetHover(childNumber == 3);
            }
        }

        /// <summary>
        /// Sets all child boxes to be not hovered
        /// </summary>
        public void  UnHoverChildren()
        {
            if (Next1 != null)
            {
                Next1.SetHover(false);
            }
            if (Next2 != null)
            {
                Next2.SetHover(false);
            }
            if (Next3 != null)
            {
                Next3.SetHover(false);
            }
        }
    }
}

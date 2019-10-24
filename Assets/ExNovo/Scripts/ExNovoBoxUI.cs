using UnityEngine;

namespace ExNovo
{
    public class ExNovoBoxUI : MonoBehaviour
    {
        [SerializeField] private ActionBox SelectedActionBox = default;
        [SerializeField] private ActionBox Next1 = default;
        [SerializeField] private ActionBox Next2 = default;
        [SerializeField] private ActionBox Next3 = default;

        private void Start()
        {

        }

        public void OnChangeActionTreePosition(ExNovoActionTreeNode actionTreeNode)
        {
            if (actionTreeNode.IsRoot)
            {
                SelectedActionBox.Hide();
            }
            else
            {
                SelectedActionBox.ChangeVisuals(actionTreeNode.ActionName, actionTreeNode.Color);
            }

            if (actionTreeNode.HasChild(1))
            {
                Next1.ChangeVisuals(actionTreeNode.Child(1).ActionName, actionTreeNode.Child(1).Color);
            }
            else
            {
                Next1.Hide();
            }
            if (actionTreeNode.HasChild(2))
            {
                Next2.ChangeVisuals(actionTreeNode.Child(2).ActionName, actionTreeNode.Child(2).Color);
            }
            else
            {
                Next2.Hide();
            }
            if (actionTreeNode.HasChild(3))
            {
                Next3.ChangeVisuals(actionTreeNode.Child(3).ActionName, actionTreeNode.Child(3).Color);
            }
            else
            {
                Next3.Hide();
            }
        }
    }
}

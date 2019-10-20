using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExNovo
{
    public class ExNovoController : MonoBehaviour
    {
        [SerializeField] private TextAsset JSONActionTree = default;

        private ExNovoSoundPlayer ExNovoSoundPlayer;

        private ExNovoActionTreeNode ActionTreeRoot;
        private ExNovoActionTreeNode CurrentTreeNode;

        private void Start()
        {
            if (JSONActionTree == null)
            {
                throw new MissingReferenceException("ExNovo controller requires an action tree json file");
            }
            ActionTreeRoot = ExNovoActionTreeJSONReader.ReadTreeFromJSON(JSONActionTree.text);
            ActionTreeRoot.DEBUG_print_tree();
            CurrentTreeNode = ActionTreeRoot;

            ExNovoSoundPlayer = GetComponent<ExNovoSoundPlayer>();
            if (ExNovoSoundPlayer == null)
            {
                throw new MissingComponentException();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                OnInputSelect(1);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnInputSelect(2);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                OnInputSelect(3);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnInputConfirm();
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                OnInputCancel();
            }
        }

        public void OnInputSelect(int selectNumber)
        {
            if (CurrentTreeNode.HasChild(selectNumber))
            {
                CurrentTreeNode = CurrentTreeNode.Child(selectNumber);
                ExNovoSoundPlayer.PlaySelectSound(selectNumber);
            }
            else
            {
                Debug.Log("Cannot traverse that way");
                ExNovoSoundPlayer.PlayErrorSound();
            }
        }

        public void OnInputConfirm()
        {
            if (CurrentTreeNode.HasCommandToRun)
            {
                Debug.Log("Running command " + CurrentTreeNode.CommandText);
                CurrentTreeNode = ActionTreeRoot;
                ExNovoSoundPlayer.PlayConfirmSound();
            }
            else
            {
                Debug.Log("Cannot run command here");
                ExNovoSoundPlayer.PlayErrorSound();
            }
        }

        public void OnInputCancel()
        {
            if (CurrentTreeNode.IsRoot == false)
            {
                CurrentTreeNode = ActionTreeRoot;
                ExNovoSoundPlayer.PlayCancelSound();
            }
            else
            {
                Debug.Log("Cannot cancel here");
                ExNovoSoundPlayer.PlayErrorSound();
            }
        }
    }
}

using System.Linq;
using UnityEngine;

namespace ExNovo
{
    public class ExNovoController : MonoBehaviour
    {
        [SerializeField] private TextAsset JSONActionTree = default;

        private ExNovoSoundPlayer ExNovoSoundPlayer;
        private ExNovoBoxUI ExNovoBoxUI;

        private ExNovoActionTreeNode ActionTreeRoot;
        private ExNovoActionTreeNode CurrentTreeNode;

        private CommandRunner CommandRunner;

        private void Start()
        {
            // process json action tree
            if (JSONActionTree == null)
            {
                throw new MissingReferenceException("ExNovo controller requires an action tree json file");
            }
            ActionTreeRoot = ExNovoActionTreeJSONReader.ReadTreeFromJSON(JSONActionTree.text);
            ActionTreeRoot.DEBUG_print_tree();
            CurrentTreeNode = ActionTreeRoot;

            // initialize boxUI
            ExNovoBoxUI = GetComponentInChildren<ExNovoBoxUI>();
            if (ExNovoBoxUI == null)
            {
                throw new MissingComponentException("Requies ExNovoBoxUI in a child object");
            }
            ExNovoBoxUI.OnChangeActionTreePosition(CurrentTreeNode);

            // get reference to command runner
            CommandRunner = FindObjectOfType<CommandRunner>();
            if (CommandRunner == null)
            {
                throw new MissingComponentException("No CommandRunner was found. Make sure there is one in the scene. ExNovo cannot run commands without it");
            }

            // Get sound player
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
                ExNovoBoxUI.OnChangeActionTreePosition(CurrentTreeNode);
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
            try
            {
                CommandRunner.RunMethodCallText(CurrentTreeNode.MethodCallText);
                ExNovoSoundPlayer.PlayConfirmSound();
            }
            catch (System.Exception e)
            {
                ExNovoSoundPlayer.PlayErrorSound();
                throw e;
            }
            finally
            {
                CurrentTreeNode = ActionTreeRoot;
                ExNovoBoxUI.OnChangeActionTreePosition(CurrentTreeNode);
            }
        }

        public void OnInputCancel()
        {
            if (CurrentTreeNode.IsRoot == false)
            {
                CurrentTreeNode = ActionTreeRoot;
                ExNovoBoxUI.OnChangeActionTreePosition(CurrentTreeNode);
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

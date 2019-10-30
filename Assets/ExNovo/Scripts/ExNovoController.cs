using System.Linq;
using UnityEngine;

namespace ExNovo
{
    public class ExNovoController : MonoBehaviour
    {
        [SerializeField] private TextAsset JSONActionTree = default;
        [SerializeField] private ExNovoBoxUI ExNovoBoxUI = default;

        private ExNovoSoundPlayer ExNovoSoundPlayer;

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
            // ActionTreeRoot.DEBUG_print_tree();
            CurrentTreeNode = ActionTreeRoot;

            // initialize boxUI
            if (ExNovoBoxUI == null)
            {
                throw new MissingReferenceException("Requies ExNovoBoxUI");
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
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.A))
            {
                OnInputSelect(1);
            }
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.S))
            {
                OnInputSelect(2);
            }
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.D))
            {
                OnInputSelect(3);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnInputConfirm();
            }
            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
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
                if (CurrentTreeNode.IsRoot)
                {
                    throw new System.InvalidOperationException("Cannot confirm at root");
                }
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

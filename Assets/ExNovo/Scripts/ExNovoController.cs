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
            if (CurrentTreeNode.HasCommandToRun)
            {
                Debug.Log("Running command " + CurrentTreeNode.CommandText);
                (string methodName, string[] arguments) = ParseCommandText(CurrentTreeNode.CommandText);
                try
                {
                    CommandRunner.RunActionForCommand(methodName, arguments);
                    ExNovoSoundPlayer.PlayConfirmSound();
                }
                finally
                {
                    CurrentTreeNode = ActionTreeRoot;
                    ExNovoBoxUI.OnChangeActionTreePosition(CurrentTreeNode);
                }
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
                ExNovoBoxUI.OnChangeActionTreePosition(CurrentTreeNode);
                ExNovoSoundPlayer.PlayCancelSound();
            }
            else
            {
                Debug.Log("Cannot cancel here");
                ExNovoSoundPlayer.PlayErrorSound();
            }
        }

        public (string methodName, string[] arguments) ParseCommandText(string commandText)
        {
            string[] splitCommand = commandText.Split(new char[] { '(', ')', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (splitCommand.Length == 0)
            {
                return (null, null);
            }
            else if (splitCommand.Length == 1)
            {
                return (splitCommand[0], null);
            }
            else
            {
                // return the method name and the arguments seperately
                return (
                    splitCommand[0],
                    splitCommand
                        .Skip(1)
                        .Select(arg => arg.Trim())
                        .ToArray()
                );
            }
        }
    }
}

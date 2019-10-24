using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExNovo
{
    public class ExNovoController : MonoBehaviour
    {
        [SerializeField] private TextAsset JSONActionTree = default;

        private ExNovoSoundPlayer ExNovoSoundPlayer;

        private ExNovoActionTreeNode ActionTreeRoot;
        private ExNovoActionTreeNode CurrentTreeNode;

        private CommandRunner CommandRunner;

        private void Start()
        {
            // check for json action tree
            if (JSONActionTree == null)
            {
                throw new MissingReferenceException("ExNovo controller requires an action tree json file");
            }
            ActionTreeRoot = ExNovoActionTreeJSONReader.ReadTreeFromJSON(JSONActionTree.text);
            ActionTreeRoot.DEBUG_print_tree();
            CurrentTreeNode = ActionTreeRoot;

            // check for command runner
            CommandRunner = FindObjectOfType<CommandRunner>();
            if (CommandRunner == null)
            {
                throw new MissingComponentException("No CommandRunner was found. Make sure there is one in the scene. ExNovo cannot run commands without it");
            }

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
                (string methodName, string[] arguments) = ParseCommandText(CurrentTreeNode.CommandText);
                Debug.Log("Method: " + methodName);
                Debug.Log("num args: " + arguments.Length);
                CommandRunner.RunActionForCommand(methodName, arguments);
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

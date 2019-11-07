using UnityEngine;

namespace ExNovo
{
    /// <summary>
    /// The main ExNovo class. Reads the action tree from json file, listens for inputs, calls to UI to update and runs actions
    /// </summary>
    [RequireComponent(typeof(ExNovoSoundPlayer))]
    public class ExNovoController : MonoBehaviour
    {
        [SerializeField] private TextAsset JSONActionTree = default;
        [SerializeField] private ExNovoBoxUI ExNovoBoxUIRoot = default;

        private ExNovoSoundPlayer ExNovoSoundPlayer;

        private ExNovoActionTreeNode ActionTreeRoot;
        private ExNovoActionTreeNode CurrentTreeNode;

        private ExNovoActionRunner ActionRunner;

        private void Start()
        {
            // process json action tree
            if (JSONActionTree == null)
            {
                throw new MissingReferenceException("ExNovo controller requires an action tree json file");
            }
            ActionTreeRoot = ExNovoActionTreeJSONReader.ReadTreeFromJSON(JSONActionTree.text);
            CurrentTreeNode = ActionTreeRoot;

            // initialize boxUI
            if (ExNovoBoxUIRoot == null)
            {
                throw new MissingReferenceException("Requies reference to root of ExNovoBoxUI");
            }
            ExNovoBoxUIRoot.OnChangeActionTreePosition(CurrentTreeNode);

            // get reference to command runner
            ActionRunner = FindObjectOfType<ExNovoActionRunner>();
            if (ActionRunner == null)
            {
                throw new MissingComponentException("No ActionRunner was found. Make sure there is one in the scene. ExNovo cannot run commands without it");
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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnInputHover(1);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnInputHover(2);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInputHover(3);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnInputUnHover();
            }
        }

        public void OnInputSelect(int selectNumber)
        {
            if (CurrentTreeNode.HasChild(selectNumber))
            {
                CurrentTreeNode = CurrentTreeNode.Child(selectNumber);
                ExNovoBoxUIRoot.OnChangeActionTreePosition(CurrentTreeNode);
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
                ActionRunner.RunMethodCallText(CurrentTreeNode.MethodCallText);
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
                ExNovoBoxUIRoot.OnChangeActionTreePosition(CurrentTreeNode);
            }
        }

        public void OnInputCancel()
        {
            if (CurrentTreeNode.IsRoot == false)
            {
                CurrentTreeNode = ActionTreeRoot;
                ExNovoBoxUIRoot.OnChangeActionTreePosition(CurrentTreeNode);
                ExNovoSoundPlayer.PlayCancelSound();
            }
            else
            {
                Debug.Log("Cannot cancel here");
                ExNovoSoundPlayer.PlayErrorSound();
            }
        }

        public void OnInputHover(int selectNumber)
        {
            ExNovoBoxUIRoot.SetHoverChild(selectNumber);
        }

        public void OnInputUnHover()
        {
            ExNovoBoxUIRoot.UnHoverChildren();
        }
    }
}

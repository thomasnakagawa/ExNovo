using UnityEngine;

namespace ExNovo.Example
{
    public class ExampleActionRunner : ExNovoActionRunner
    {
        [SerializeField] private TMPro.TMP_Text MessageText = default;

        private void Start()
        {
            if (MessageText == null)
            {
                throw new MissingReferenceException("Text element required");
            }
        }

        public void Cut()
        {
            ShowCommandMessageInUI("Cut");
        }

        public void Copy()
        {
            ShowCommandMessageInUI("Copy");
        }

        public void Paste()
        {
            ShowCommandMessageInUI("Paste");
        }

        public void Ok(string arg)
        {
            MessageText.text = "Alright: " + arg;
        }

        public void Hey()
        {
            ShowCommandMessageInUI("Hey");
        }

        public void Insert()
        {
            ShowCommandMessageInUI("Insert");
        }

        public void ResetArm()
        {
            GameObject.Find("Arm").transform.eulerAngles = Vector3.zero;
            ShowCommandMessageInUI("Arm rotation reset");
        }

        public void ChangeSkyColor(string color)
        {
            ColorUtility.TryParseHtmlString(color, out Color col);
            Camera.main.backgroundColor = col;
        }

        private void ShowCommandMessageInUI(string commandMessage)
        {
            MessageText.text = "Command \"" + commandMessage + "\" was run";
        }
    }
}

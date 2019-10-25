using UnityEngine;

namespace ExNovo.Example
{
    public class ExampleCommandRunner : CommandRunner
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

        private void ShowCommandMessageInUI(string commandMessage)
        {
            MessageText.text = "Command \"" + commandMessage + "\" was run";
        }

        public void Ok(string arg)
        {
            MessageText.text = "Alright: " + arg;
        }
    }
}

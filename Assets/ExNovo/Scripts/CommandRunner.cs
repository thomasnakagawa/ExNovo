using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ExNovo
{
    /// <summary>
    /// Class that runs commands for ExNovo to call. Works by using reflection to match command names from the ActionTree.json to method names in the class.
    /// To define commands, create a class that inherits from this class. Create methods whose names match the names in ActionTree.json. Put your new class on an object in the Unity scene.
    /// </summary>
    public abstract class CommandRunner : MonoBehaviour
    {
        public void RunMethodCallText(string methodCallText)
        {
            if (methodCallText == null)
            {
                throw new System.ArgumentNullException(nameof(methodCallText), "Method call text cannot be null");
            }

            (string methodName, string[] arguments) = ParseCommandText(methodCallText);
            if (methodName == null)
            {
                throw new System.ArgumentException("Cannot run action because method call text has no method name");
            }

            MethodInfo methodInfo = GetType().GetMethod(methodName);
            if (methodInfo == null)
            {
                throw new System.ArgumentException("Command runner has no method with the name \"" + methodName + "\". Make sure there is a public method with this name in the class " + this.GetType().Name);
            }
            methodInfo.Invoke(this, arguments);
        }

        private (string methodName, string[] arguments) ParseCommandText(string commandText)
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

using System.Reflection;
using System.Text;
using UnityEngine;

namespace ExNovo
{
    /// <summary>
    /// Class that runs commands for ExNovo to call. Works by using reflection to match command names from the ActionTree.json to method names in the class.
    /// To define commands, create a class that inherits from this class. Create methods whose names match the names in ActionTree.json. Put your new class on an object in the Unity scene.
    /// </summary>
    public abstract class CommandRunner : MonoBehaviour
    {
        public void RunActionForCommand(string command, string[] arguments)
        {
            MethodInfo methodInfo = GetType().GetMethod(command);
            if (methodInfo == null)
            {
                throw new System.ArgumentException("Command runner has no method with the name \"" + command + "\". Make sure you create a method with this name in your class that derives from CommandRunner");
            }
            methodInfo.Invoke(this, arguments);
        }

        public void DebugMessage()
        {
            Debug.Log("CommandRunner debug command was run");
        }

        public void DebugMessageWithArg(string arg)
        {
            Debug.Log("CommandRunner debug command was run with argument " + arg);
        }

        public void DebugMessageWithArgs(string arg1, string arg2)
        {
            Debug.Log("CommandRunner debug command was run with arguments " + arg1 + ", " + arg2);
        }
    }
}

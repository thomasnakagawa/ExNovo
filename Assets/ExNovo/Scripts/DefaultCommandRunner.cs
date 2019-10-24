using UnityEngine;
using ExNovo;

public class DefaultCommandRunner : CommandRunner
{
    public void Cut()
    {
        Debug.Log("Cut command was run");
    }

    public void Copy()
    {
        Debug.Log("Copy command was run");
    }

    public void Paste()
    {
        Debug.Log("Paste command was run");
    }
}

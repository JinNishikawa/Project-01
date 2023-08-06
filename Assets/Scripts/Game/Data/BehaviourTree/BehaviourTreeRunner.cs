using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    [HideInInspector]
    public BehaviourTree _tree;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _tree?.Update();
    }


    public void SetTree(BehaviourTree tree)
    {
        _tree = tree.Clone();
        _tree.Bind();
    }
}
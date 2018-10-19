using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//definition of our spell tree, it is more like a forest in reality
public class SpellTree{

    List<SpellTreeNode> children;
    //is use as iterator, yes it's moche
    SpellTreeNode actualNode;


    public SpellTree()
    {
        children = new List<SpellTreeNode>();
        actualNode = null;
    }

    public SpellTree(List<SpellColliderType> spellDef, string spellName)
    {
        if(children == null)
            children = new List<SpellTreeNode>();
        children.Add(new SpellTreeNode(spellDef, spellName));
        actualNode = null;
    }

    //use to move the actualNode (where we are)
    public bool advance(SpellColliderType type)
    {
        bool isFind = false;

        if (actualNode == null)
        {
            foreach (SpellTreeNode child in children)
            {
                if (child.isColliderType(type))
                {
                    isFind = true;
                    actualNode = child;
                    //Debug.Log("actual is " + actualNode.spellColliderType);
                    break;
                }
            }
            //if(!isFind)
                //Debug.Log("can't Advance to " + type);

            return isFind;
        }
        else
        {
            SpellTreeNode newNode = actualNode.getChildOfType(type);
            if(newNode == null)
            {
                //Debug.Log("can't Advance to " + type);
                return false;
            }
            else
            {
                actualNode = newNode;
                //Debug.Log("actual is " + actualNode.spellColliderType);
                return true;
            }
            
        }
    }

    //use to reset the actualNode at the start, i.e. no collision register
    public void resetActualNode()
    {
        //Debug.Log("actual is reset");

        actualNode = null;
    }

    //true if the actualNode is a leaf (we have the name of the spell)
    public string isSpell()
    {
        if(actualNode.spellName != null)
        {
            return actualNode.spellName;
        }
        else
        {
            return null;
        }
    }

    //use to add a spell, a spell is an order of ColliderType and a name
    public void addSpell(List<SpellColliderType> spellDef, string spellName)
    {
        SpellColliderType type = spellDef[0];
        bool isFind = false;

        foreach (SpellTreeNode child in children)
        {
            if(child.isColliderType(type))
            {
                isFind = true;
                spellDef.RemoveAt(0);
                child.addChild(spellDef, spellName);
                break;
            }
        }

        if (!isFind)
        {
            children.Add(new SpellTreeNode(spellDef, spellName));
        }
    }

    //use to debug the tree
    public void DebugTree()
    {
        foreach (SpellTreeNode child in children)
        {
            child.DebugTreeNode("");
        }
    }

    //a node of the tree
    public class SpellTreeNode
    {
        //is equal to null exept for the last node of a spell
        public string spellName { get; private set; }
        public SpellColliderType spellColliderType { get; private set; }

        private List<SpellTreeNode> children;

        public SpellTreeNode(List<SpellColliderType> spellDefinition, string spellName)
        {
            SpellColliderType _spellColliderType = spellDefinition[0];
            spellDefinition.RemoveAt(0);
            this.spellColliderType = _spellColliderType;
            
            if (spellDefinition.Count != 0)
            {
                children = new List<SpellTreeNode>();
                this.spellName = null;
                children.Add(new SpellTreeNode(spellDefinition, spellName));
            }
            else
            {
                children = null;
                this.spellName = spellName;
            }
        }

        //true if on children is of type _spellColliderType
        public bool hasColliderChildOfType(SpellColliderType _spellColliderType)
        {
            foreach(SpellTreeNode child in children)
            {
                if (child.spellColliderType == _spellColliderType)
                    return true;
            }
            return false;
        }

        //return the child of type _spellColliderType or null if he doesn't exist
        public SpellTreeNode getChildOfType(SpellColliderType _spellColliderType)
        {
            foreach (SpellTreeNode child in children)
            {
                if (child.spellColliderType == _spellColliderType)
                    return child;
            }
            return null;
        }

        //return true of the node is of type _spellColliderType
        public bool isColliderType(SpellColliderType _spellColliderType)
        {
            return this.spellColliderType == _spellColliderType;
        }

        //use to add more spell
        public void addChild(List<SpellColliderType> spellDefinition, string spellName)
        {
            if (spellDefinition.Count != 0) {

                SpellColliderType newChildType = spellDefinition[0];
                bool isFind = false;

                foreach (SpellTreeNode child in children)
                {
                    if (child.spellColliderType == newChildType)
                    {
                        child.addChild(spellDefinition, spellName);
                        spellDefinition.RemoveAt(0);
                        isFind = true;
                        break;
                    }
                }
                if(!isFind)
                    children.Add(new SpellTreeNode(spellDefinition,spellName));
            }
        }

        //use to debug
        public void DebugTreeNode(string str)
        {
            if(children == null)
            {
                str = str + " + " + this.spellColliderType + " = " + this.spellName;
                Debug.Log(str);
            }
            else
            {
                if(str == "")
                    str += this.spellColliderType;
                else
                    str += " + " + this.spellColliderType;
                foreach (SpellTreeNode child in children)
                {
                    child.DebugTreeNode(str);
                }
            }
        }
    }
}

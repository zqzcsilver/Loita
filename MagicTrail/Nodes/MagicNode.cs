using System.Collections.Generic;

namespace Loita.MagicTrail.Nodes
{
    internal abstract class MagicNode
    {
        public MagicNode Parent;
        public List<MagicNode> Children = [];
        public virtual bool AutoLoad => true;

        public virtual void Apply(MagicParameters parameters)
        {
        }

        public virtual bool Link(MagicNode node)
        {
            if (Parent != null)
                return LinkTo(node);
            else
                return node.LinkTo(this);
        }

        public virtual bool LinkTo(MagicNode node)
        {
            if (node.Parent != null)
                return false;
            node.Parent = this;
            Children.Add(node);
            return true;
        }

        public virtual void GetInfo(MagicParameters parameters)
        {
        }
    }
}
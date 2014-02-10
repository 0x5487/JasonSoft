using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Atlantis
{
    public class FileSystem
    {
        public String Name { get; private set; }

        public Boolean EnableACL { get; private set;}


        internal FileSystem()
        {

        }

        public Node[] GetFirstLevelNodes()
        {
            throw new NotImplementedException();
        }

        public Node[] GetFirstLevelNodes(Int32[] permissionItems)
        {
            throw new NotImplementedException();
        }

        public Node CreateNode(Node node)
        {
            throw new NotImplementedException();
        }

        public Node GetNode(Int32 nodeID)
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodes()
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodes(int take, int skip)
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodes(Int32 parentNodeID, Int32[] permissionItems)
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodes(Int32 parentNodeID, Int32[] permissionItems, int take, int skip)
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodesByParentNodeID(Int32 parentNodeID)
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodesByParentNodeID(Int32 parentNodeID, int take, int skip)
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodesByParentNodeID(Int32 parentNodeID, Int32[] permissionItems)
        {
            throw new NotImplementedException();
        }

        public Node[] GetNodesByParentNodeID(Int32 parentNodeID, Int32[] permissionItems, int take, int skip)
        {
            throw new NotImplementedException();
        }

        public void UpdateNode(Node node)
        {
            throw new NotImplementedException();
        }

        public void DeleteNode(Int32 nodeID)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public List<AccessControl> GetACL(Int32 nodeID)
        {
            throw new NotImplementedException();
        }

        public Permission[] GetPermissionsForAdd()
        {
            throw new NotImplementedException();
        }

        public void UpdateACL(List<AccessControl> acl)
        {
            throw new NotImplementedException();
        }

        public Boolean HasPermission(Int32 nodeID, Char code)
        {
            throw new NotImplementedException();
        }

        public void UpdateInheritStatus(Int32 nodeID)
        {
            throw new NotImplementedException();
        }

        public void UpdateDisplayName(Int32 userID, String newDisplayName)
        {
            throw new NotImplementedException();
        }
    }
}

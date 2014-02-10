using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonSoft.Components.Atlantis;
using Xunit;


namespace JasonSoft.Tests.Components.Atlantis
{

    public class FileSystemTestCase
    {
        private AtlantisUserProfile _adminProfile = null;
        private AtlantisUserProfile _systemProfile = null;
        private AtlantisUserProfile _jasonProfile = null;
        
        public FileSystemTestCase()
        {
            _adminProfile = new AtlantisUserProfile(AtlantisUserProfileEnum.Administrator);
            _systemProfile = new AtlantisUserProfile(AtlantisUserProfileEnum.System);
            
            Permission readItem = new Permission();
            readItem.Code = 'R';
            readItem.Name = "讀取";
            readItem.AllowStatus = PermissionRightStatus.Native;
            FileSystemManager.CreatePermission(readItem);

            Permission writeItem = new Permission();
            writeItem.Code = 'W';
            writeItem.Name = "寫入";
            writeItem.AllowStatus = PermissionRightStatus.Native;
            FileSystemManager.CreatePermission(writeItem);

            Permission editItem = new Permission();
            editItem.Code = 'E';
            editItem.Name = "修改";
            editItem.AllowStatus = PermissionRightStatus.Native;
            FileSystemManager.CreatePermission(editItem);
            Assert.True( editItem.ID > 0);

            Permission deleteItem = new Permission();
            deleteItem.Code = 'D';
            deleteItem.Name = "刪除";
            deleteItem.AllowStatus = PermissionRightStatus.Native;
            FileSystemManager.CreatePermission(deleteItem);

            Permission fullItem = new Permission();
            fullItem.Code = 'F';
            fullItem.Name = "完全控制";
            fullItem.AllowStatus = PermissionRightStatus.Native;
            FileSystemManager.CreatePermission(fullItem);
        }


        ~ FileSystemTestCase()
        {
            FileSystemManager.ClearPermissionGroups();
            FileSystemManager.ClearPermissions();
        }


        public void CreateFileSystem()
        {
            FileSystemManager fsm = new FileSystemManager(_adminProfile);
            FileSystem fs = fsm.GetFileSystem("ActiveDirectory");
            Assert.Null(fs);

            FileSystemManager.CreateFileSystem("ActiveDirectory", "根目錄");
            fs = fsm.GetFileSystem("ActiveDirectory");
            Assert.NotNull(fs);
            Assert.Same("ActiveDirectory", fs.Name);

            //delete filesystem
            FileSystemManager.DeleteFileSystem("ActiveDirectory");
            fs = fsm.GetFileSystem("ActiveDirectory");
            Assert.Null(fs);
        }


        public void SimpleOperation1()
        {
            FileSystemManager.CreateFileSystem("ActiveDirectory", "根目錄");

            FileSystemManager fsm = new FileSystemManager(_adminProfile);
            FileSystem fs = fsm.GetFileSystem("ActiveDirectory");

            //get rootnode
            Node rootNode = fs.GetNode(0);
            Assert.Same("根目錄", rootNode.Name);
            Assert.Same(-1, rootNode.ParentID);
            Assert.Same(0, rootNode.CreatorID);

            //create nodes in first level
            Node node1 = new Node();
            node1.ParentID = 0;
            node1.Name = "Node1";
            node1.Attribute = "HelloWorld1";
            fs.CreateNode(node1);
            Assert.True(node1.ID > 0);
            Assert.Same(0, node1.ParentID);
            Assert.True(node1.IsInheritance);
            Assert.Same("Node1", node1.Name);

            //do update
            node1.Name = "Jason";
            node1.Attribute = "Jason Attribute";
            fs.UpdateNode(node1);
            node1 = fs.GetNode(node1.ID);
            Assert.Same("Jason", node1.Name);
            Assert.Same("Jason Attribute", node1.Attribute);

            Node node2 = new Node();
            node2.ParentID = 0;
            node2.Name = "Node2";
            node2.Attribute = "HelloWorld2";
            node2 = fs.CreateNode(node2);
            Assert.True(node2.ID > 0);
            Assert.False(node1.HasChild);
            Assert.Same("HelloWorld1", node1.Attribute);

            Node[] nodes = fs.GetNodesByParentNodeID(0);
            Assert.Same(2, nodes.Length);
            Assert.True(nodes[1].ID > 0);

            //create nodes in second level
            Node node21 = new Node();
            node21.ParentID = node2.ID;
            node21.Name = "Node 21";
            node21.Attribute = "Hello World 21";
            fs.CreateNode(node21);
            Assert.Same(node2.ID, node21.ParentID);
            node2 = fs.GetNode(node2.ID);
            Assert.True(node2.HasChild);


            //do delete action
            fs.DeleteNode(node2.ID);
            node2 = fs.GetNode(node2.ID);
            Assert.Null(node2);
            Assert.Null(fs.GetNode(node21.ID));

            //try to delete rootNode
            //expect exception
            Boolean doTest = false;
            try
            {
                fs.DeleteNode(0);
            }
            catch (FileSystemException)
            {
                doTest = true;
            }
            Assert.True(doTest);
           

            //end 
            FileSystemManager.DeleteFileSystem("ActiveDirectory");
        }

        public void PermissionsAndGroupTest()
        {
            FileSystemManager.ClearPermissionGroups();
            FileSystemManager.ClearPermissions();
            Permission[] permissions = FileSystemManager.GetPermissions();
            Assert.Null(permissions);
            String[] permissionGroup = FileSystemManager.GetPermissionGroups();
            Assert.Null(permissionGroup);

            CreateDefaultPermisssionGroup();

            permissions = FileSystemManager.GetPermissionsFromGroup("Default");
            Assert.Same(5, permissions.Length);

            Permission permission = FileSystemManager.GetPermission('E');
            FileSystemManager.RemovePermissionFromGroup(permission.ID, "Default");
            permissions = FileSystemManager.GetPermissionsFromGroup("Default");
            Assert.Same(4, permissions.Length);

            //create buildin permission


            //create custom permission
            Permission custom = new Permission();
            custom.Code = 'Z';
            custom.Name = "客制";
            custom.AllowStatus = PermissionRightStatus.Native;
            FileSystemManager.CreatePermission(custom);

            //end
            FileSystemManager.DeletePermissionGroup("Default");
        }

        public void AdvancedOperationWithACL()
        {
            CreateDefaultPermisssionGroup();

            FileSystemManager.CreateFileSystem("ActiveDirectory", "根目錄", "default");

            FileSystemManager fsm = new FileSystemManager(_adminProfile);
            FileSystem fs = fsm.GetFileSystem("ActiveDirectory");

            List<AccessControl> acl = fs.GetACL(0);
            Assert.Same(1, acl.Count);
            Assert.Same(UserGroupType.Group, acl[0].UserGroupType);
            Assert.Same(new AtlantisUserGroup(AtlantisUserGroupEnum.Users).ID, acl[0].UserOrGroupID);
            Assert.Same(new Permission(PermissionEnum.FullControl).ID, acl[0].Permissions[0].ID);

            //create nodes in first level
            Node node1 = new Node();
            node1.ParentID = 0;
            node1.Name = "Node1";
            node1.Attribute = "HelloWorld1";

            Node node2 = new Node();
            node2.ParentID = 0;
            node2.Name = "Node2";
            node2.Attribute = "HelloWorld2";

            fs.CreateNode(node1);
            fs.CreateNode(node2);
            Assert.Same(false, node2.HasChild);
            Assert.Same(true, node2.IsInheritance);

            acl = fs.GetACL(node2.ID);
            Assert.Same(1, acl.Count);
            Assert.Same(UserGroupType.Group, acl[0].UserGroupType);
            Assert.Same(new AtlantisUserGroup(AtlantisUserGroupEnum.Users).ID, acl[0].UserOrGroupID);
            Assert.Same(new Permission(PermissionEnum.FullControl).ID, acl[0].Permissions[0].ID);

            AccessControl accessControl = new AccessControl();
            accessControl.NodeID = node1.ID;
            accessControl.UserOrGroupID = 10;
            accessControl.UserOrGroupName = "Jason";
            accessControl.Permissions = fs.GetPermissionsForAdd();
            acl.Clear();
            acl.Add(accessControl);
            fs.UpdateACL(acl);

            acl = fs.GetACL(node1.ID);
            Assert.Same(2, acl.Count);
            Assert.Same(UserGroupType.User, acl[0].UserGroupType);

            //change user
            fs = new FileSystemManager(new AtlantisUserProfile() {UserID = 10}).GetFileSystem("ActiveDirectory");
            //fs.HasPermission(node1.ID, '').

            //delete filesystem
            FileSystemManager.DeleteFileSystem("ActiveDirectory");
            FileSystemManager.DeletePermissionGroup("Default");
        }

        private void CreateDefaultPermisssionGroup()
        {
            Permission readItem = FileSystemManager.GetPermission('R');
            Permission writeItem = FileSystemManager.GetPermission('W');
            Permission editItem = FileSystemManager.GetPermission('E');
            Permission deleteItem = FileSystemManager.GetPermission('D');
            Permission fullItem = FileSystemManager.GetPermission('F');
            

            FileSystemManager.CreatePermissionGroup("Default");
            FileSystemManager.AddPermissionToGroup(readItem.ID, "Default");
            FileSystemManager.AddPermissionToGroup(writeItem.ID, "Default");
            FileSystemManager.AddPermissionToGroup(editItem.ID, "Default");
            FileSystemManager.AddPermissionToGroup(deleteItem.ID, "Default");
            FileSystemManager.AddPermissionToGroup(fullItem.ID, "Default");
        }
    }
}

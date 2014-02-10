using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Atlantis
{
    public class IdentityFacade
    {
        public IdentityFacade(AtlantisUserProfile executer)
        {

        }




        public AtlantisUserProfile CreateUserProfile(AtlantisUserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public AtlantisUserProfile GetUserProfile(Guid membershipID)
        {
            throw new NotImplementedException();
        }

        public AtlantisUserProfile GetUserProfile(Int32 userProfileID)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserProfile(Int32 userProfileID)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserProfile(Int32 userProfileID)
        {
            throw new NotImplementedException();
        }

        public AtlantisUserGroup GetUserGroup(Int32 userGroupID)
        {
            throw new NotImplementedException();
        }

        public AtlantisUserGroup CreateUserGroup(AtlantisUserGroup userGroup)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserGroup(Int32 userGroupID)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserGroup(AtlantisUserGroup userGroup)
        {
            throw new NotImplementedException();
        }

        public void AddUserToGroup(Int32 userID, Int32 userGroupID)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromGroup(Int32 userID, Int32 userGroupID)
        {
            throw new NotImplementedException();
        }

        public AtlantisUserProfile[] GetUsersFromGroup(Int32 userGorup)
        {
            throw new NotImplementedException();
        }
    }
}

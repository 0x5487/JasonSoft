namespace JasonSoft.Services.Storage
{
    public class StorageFacade
    {
        public StorageFacade(string storageHostUrl, string storagePhysicalPath)
        {
            this._storageHostUrl = storageHostUrl;
            this._storagePhysicalPath = storagePhysicalPath;
        }

        private string _storageHostUrl;

        public string storageHostUrl
        {
            get { return _storageHostUrl; }
        }

        private string _storagePhysicalPath;

        public string storagePhysicalPath
        {
            get { return _storagePhysicalPath; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>return something looks like "~/storage/users/jason/{sessionid}/"</returns>
        public string GetTempDirByUserID(int userID)
        {
            throw new System.NotImplementedException();
        }

        public void ClearAllTempDirs(int howManyDaysBeforeToday_IncludeToday)
        {
            throw new System.NotImplementedException();
        }

        //protected string GetPhysicalPathByApplication(IApplication application)
        //{
        //    throw new System.NotImplementedException();
        //}

        //protected string GetHostUrlByApplication(IApplication application)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
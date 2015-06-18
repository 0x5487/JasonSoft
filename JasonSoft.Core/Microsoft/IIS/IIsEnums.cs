namespace JasonSoft.Microsoft.IIS
{
    public enum AuthenticationFlags
    {
        Anonymous = 1,
        Basic = 2,
        Integrated = 4,
        Digest = 16,
        Passport = 64
    }

    public enum AccessPermissionFlags
    {
        Read = 1,
        Write = 2,
        Execute = 4,
        Source = 16,
        Script = 512,
        NoRemoteRead = 4096,
        NoRemoteWrite = 1024,
        NoRemoteExecute = 8192,
        NoRemoteScript = 16384,
        NoPhysicalDir = 32768
    }

    public enum DirectoryBrowseFlags: long 
    {
        EnableDirBrowsing = 2147483648,
        EnableDefaultDoc = 1073741824,
        ShowDate = 2,
        ShowTime = 4,
        ShowSize = 8,
        ShowExtension = 16,
        ShowLongDate = 32
    }

    public enum AppPoolStates
    {
        Starting = 1,
        Started = 2,
        Stopping = 3,
        Stopped = 4
    }

    public enum AppPoolIdentityTypes
    {
        LocalSystem = 0,
        LocalService = 1,
        NetworkService = 2,
        UserDefined = 3
    }

    public enum SiteStates
    {
        Starting = 1,
        Started = 2,
        Stopping = 3,
        Stopped = 4,
        Pausing = 5,
        Paused = 6,
        Continuing = 7
    }

    public enum ASPNETVersions
    {
        v1_1_4322 = 1,
        v2_0_50727 = 2
    }

}

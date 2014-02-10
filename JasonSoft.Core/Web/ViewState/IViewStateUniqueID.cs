// Copyright 2008 JasonSoft - http://www.jasonsoft.net/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System;

namespace JasonSoft.Web.ViewState
{
    /// <summary>
    /// Implements a way to get a UniqueID
    /// </summary>
    public interface IViewStateUniqueID
    {
        string GetUniqueID();
    }


    /// <summary>
    /// Uses the HttpContext SessionID of a user as a UniqueID
    /// </summary>
    public class ViewStateHttpContextSessionID : IViewStateUniqueID
    {
        #region IUserUniqueID Members

        public string GetUniqueID()
        {
            return System.Web.HttpContext.Current.Session.SessionID;
        }

        #endregion
    }

    /// <summary>
    /// Uses a Application GUID as the UniqueID
    /// </summary>
    public class ViewStateGUID : IViewStateUniqueID
    {
        #region IUserUniqueID Members

        public string GetUniqueID()
        {
            return System.Guid.NewGuid().ToString();
        }

        #endregion
    }

    /// <summary>
    /// Uses a combination of Ip Address, page being accessed and a timestamp.
    /// </summary>
    public class ViewStateIpPageTimeStamp : IViewStateUniqueID
    {
        #region IUserUniqueID Members

        public string GetUniqueID()
        {
            return
                System.Web.HttpContext.Current.Request.UserHostAddress.ToString() + "@" +
                System.Web.HttpContext.Current.Request.RawUrl + "@" + DateTime.Now;
        }

        #endregion
    }
}
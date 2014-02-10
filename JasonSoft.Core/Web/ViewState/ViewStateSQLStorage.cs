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


namespace JasonSoft.Web.ViewState
{
    /// <summary>
    /// Provides the ability to save viewstate into a database table. 
    /// Uses EnterpriseLibrary to connect to the database and call stored procedures.
    /// </summary>
    //public class ViewStateSQLStorage : IViewstateStorage
    //{
    //    //Database _db;
    //    //DbCommand _cmd;
    //    //public ViewStateSQLStorage()
    //    //{
    //    //    ViewStateConfiguration config = (ViewStateConfiguration)System.Configuration.ConfigurationManager.GetSection("ViewStateConfiguration");
    //    //    if (config.defaultDatabase.Trim().Length == 0)
    //    //        _db = DatabaseFactory.CreateDatabase();
    //    //    else
    //    //        _db = DatabaseFactory.CreateDatabase(System.Configuration.ConfigurationManager.ConnectionStrings[config.defaultDatabase].Name);
    //    //}
    //    //#region IViewstateStorage Members
    //    //public void Add(KeyValuePair<object, string> viewstateItem)
    //    //{
    //    //    _cmd = _db.GetStoredProcCommand("ViewStateSource_ADD");
    //    //    _db.AddInParameter(_cmd, "viewstateKey", System.Data.DbType.String, viewstateItem.Key.ToString());
    //    //    _db.AddInParameter(_cmd, "viewstate", System.Data.DbType.String, viewstateItem.Value.ToString());
    //    //    _db.AddInParameter(_cmd, "ip", System.Data.DbType.String, System.Web.HttpContext.Current.Request.UserHostAddress.ToString());

    //    //    _db.ExecuteNonQuery(_cmd);
    //    //}
    //    //public KeyValuePair<object, string> Get(object key)
    //    //{
    //    //    _cmd = _db.GetStoredProcCommand("ViewStateSource_GET");
    //    //    _db.AddInParameter(_cmd, "viewstateKey", System.Data.DbType.String, key.ToString());
    //    //    KeyValuePair<object, string> retItem = new KeyValuePair<object, string>(((object)key), ((string)_db.ExecuteScalar(_cmd)));
    //    //    return retItem;
    //    //}
    //    //public void Remove(object key)
    //    //{

    //    //}
    //    //#endregion
    //}
}
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
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Web.UI;
using JasonSoft.IO.Compression;


namespace JasonSoft.Web.ViewState
{
    //Reference: 
    //http://www.codeproject.com/KB/viewstate/ServerViewState.aspx
    //http://www.codeproject.com/KB/viewstate/ViewStateCompression.aspx
    public class NoViewStateBasePage : System.Web.UI.Page
    {
        private IViewstateStorage _viewstateStorage;
        private ViewStateConfiguration _config;

        protected override object LoadPageStateFromPersistenceMedium()
        {
            object retVal;
            string id = Request.Form["__VIEWSTATE_UniqueID"];
            byte[] bytes = null;
            _config = (ViewStateConfiguration) ConfigurationManager.GetSection("ViewStateConfiguration");
            this._viewstateStorage = ViewstateStorageFactory.GetStorageMedium();
            bytes = Convert.FromBase64String(this._viewstateStorage.Get(id).Value);
            if (_config.Compression)
                bytes = bytes.Decompress();
            LosFormatter formatter = new LosFormatter();
            retVal = formatter.Deserialize(Convert.ToBase64String(bytes));
            return retVal;
        }

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            this._viewstateStorage = ViewstateStorageFactory.GetStorageMedium();
            _config = (ViewStateConfiguration) ConfigurationManager.GetSection("ViewStateConfiguration");
            LosFormatter formatter = new LosFormatter();
            StringWriter writer = new StringWriter();
            formatter.Serialize(writer, viewState);
            string viewStateString = writer.ToString();
            byte[] bytes = Convert.FromBase64String(viewStateString);
            if (_config.Compression)
                bytes = bytes.Compress();
            string viewstateBase64 = Convert.ToBase64String(bytes);
            string id = ViewstateStorageFactory.GetUserUniqueID().GetUniqueID();
            this._viewstateStorage.Add(new KeyValuePair<object, string>(id, viewstateBase64));
            ScriptManager.RegisterHiddenField(this, "__VIEWSTATE_UniqueID", id);
        }
    }


}
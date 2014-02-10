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
    /// Uses Enterprise Librarys Caching block to store viewstate in the application cache.
    /// </summary>
    //class ViewStateApplicationCacheStorage: ViewStateEliminator.Data.IViewstateStorage
    //{
    //    //private CacheManager _cache;
    //    //#region IViewstateStorage Members
    //    //public ViewStateApplicationCacheStorage()
    //    //{
    //    //    _cache = CacheFactory.GetCacheManager("ViewStateApplicationCacheStorage");
    //    //}
    //    //public void Add(KeyValuePair<object, string> viewstateItem)
    //    //{         
    //    //    _cache.Add(viewstateItem.Key.ToString(), viewstateItem.Value);
    //    //}
    //    //public KeyValuePair<object, string> Get(object key)
    //    //{
    //    //    return new KeyValuePair<object, string>(key, ((string) _cache.GetData(key.ToString())));
    //    //}
    //    //public void Remove(object key)
    //    //{
    //    //    _cache.Remove(key.ToString());
    //    //}
    //    //#endregion
    //}
}
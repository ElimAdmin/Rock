﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

using Rock.Model;
using Rock.Rest.Filters;
using Rock.Web.UI.Controls;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PagesController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "PagesGetChildren",
                routeTemplate: "api/Pages/GetChildren/{id}",
                defaults: new
                {
                    controller = "Pages",
                    action = "GetChildren"
                } );
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="hidePageIds">List of pages that should not be included in results</param>
        /// <returns></returns>
        [Authenticate, Secured]
        public IQueryable<TreeViewItem> GetChildren( int id, string hidePageIds = null)
        {
            IQueryable<Page> qry;
            if ( id == 0 )
            {
                qry = Get().Where( a => a.ParentPageId == null );
            }
            else
            {
                qry = Get().Where( a => a.ParentPageId == id );
            }

            List<int> hidePageIdList = ( hidePageIds ?? string.Empty ).Split( ',' ).Select( s => s.AsInteger()).ToList();

            List<Page> pageList = qry.Where( a => !hidePageIdList.Contains(a.Id) ).OrderBy( a => a.Order ).ThenBy( a => a.InternalName ).ToList();
            List<TreeViewItem> pageItemList = new List<TreeViewItem>();
            foreach ( var page in pageList )
            {
                var pageItem = new TreeViewItem();
                pageItem.Id = page.Id.ToString();
                pageItem.Name = page.InternalName;

                pageItemList.Add( pageItem );
            }

            // try to quickly figure out which items have Children
            List<int> resultIds = pageList.Select( a => a.Id ).ToList();

            var qryHasChildren = from x in Get().Select( a => a.ParentPageId )
                                 where resultIds.Contains( x.Value )
                                 select x.Value;

            var qryHasChildrenList = qryHasChildren.ToList();

            foreach ( var g in pageItemList )
            {
                int pageId = int.Parse( g.Id );
                g.HasChildren = qryHasChildrenList.Any( a => a == pageId );
                g.IconCssClass = "fa fa-file-o";
            }

            return pageItemList.AsQueryable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Artex.Models.DAL.DTO.General
{
    public class MenuDashboardDTO
    {
        /// <summary>
        /// Name of the menu item
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// The category the menu is contained
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The url to which this menu will lead to
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// The javascript that the menu should have
        /// </summary>
        public string JSAction { get; set; }

        /// <summary>
        /// Name of the parent menu item
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Tells whether this menu item is root or not
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// List of all the menu child items the menu has
        /// </summary>
        public List<MenuDashboardDTO> MenuChilds { get; set; }

        /// <summary>
        /// List of categories the main menu has
        /// </summary>
        public List<string> Categories { get; set; }

        public string Icon { get; set; }
    }
}
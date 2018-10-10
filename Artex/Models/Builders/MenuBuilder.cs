using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Artex.Models.DAL.DAO;
using Artex.DB;
using Artex.Models.DAL.DTO.General;


namespace Artex.Models.Builders
{
    public class MenuBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        public MenuDashboardDTO GetMenuModel(int rolId)
        {
            return null;
        }

        /// <summary>
        /// Logic that builds all the navigation menu of the site
        /// </summary>
        /// <param name="user">The username</param>
        /// <param name="companyId">The id of the company the user is related to</param>
        /// <returns>Return the MenuModel list that the view should parse to display the navigation menu bar</returns>
        public static List<MenuDashboardDTO> GetMenuModel()
        {

            List<MenuDashboardDTO> menuModelList = new List<MenuDashboardDTO>();
            using (ArtexConnection dorantesContext = new ArtexConnection())
            {
                AspNetUsers userEntity = UsuarioDAO.GetUserLogged(dorantesContext);
                /*
                if (userEntity != null)
                {
                    var coockie = HttpContext.Current.Response.Cookies.Get("userName");
                    if (coockie != null && HttpContext.Current.Response.Cookies.AllKeys.Contains("userName") && coockie.Value != userEntity.UserName)
                    {
                        DIRECCION direction = userEntity.PERSONAL.PERSONA.DIRECCION;
                        HttpContext.Current.Response.Cookies["userDirection"].Value = direction.ESTADO1.DESCRIPCION + ", " + direction.PAIS1.DESCRIPCION;
                        HttpContext.Current.Response.Cookies["userDirection"].Expires = DateTime.Now.AddYears(4);

                    }

                    HttpContext.Current.Response.Cookies["userName"].Value = userEntity.UserName;
                    HttpContext.Current.Response.Cookies["userName"].Expires = DateTime.Now.AddYears(4);


                    HttpContext.Current.Response.Cookies["userImage"].Value = userEntity.UserImage;
                    HttpContext.Current.Response.Cookies["userImage"].Expires = DateTime.Now.AddYears(4);

                }
                else
                {
                    var coockie = HttpContext.Current.Response.Cookies.Get("userName");
                    if (coockie == null)
                    {
                        HttpContext.Current.Response.Cookies["userDirection"].Value = "";
                        HttpContext.Current.Response.Cookies["userName"].Value = "";
                        HttpContext.Current.Response.Cookies["userImage"].Value = "";
                    }
                }

                */

                // Get user rol and build modules access with the rol
                if (userEntity != null && userEntity.rol != null)
                {
                    rol role = userEntity.rol;
                    menuModelList = GetMenuModelByRol(role);
                }

                dorantesContext.Dispose();
            }
            return menuModelList;
        }

        /// <summary>
        /// Logic that builds all the navigation menu of the site with the role
        /// </summary>
        /// <param name="user">The username</param>
        /// <param name="companyId">The id of the company the user is related to</param>
        /// <returns>Return the MenuModel list that the view should parse to display the navigation menu bar</returns>
        public static List<MenuDashboardDTO> GetMenuModelByRol(rol role)
        {
            List<MenuDashboardDTO> menuModelList = new List<MenuDashboardDTO>();

            // Get the modules this role has acces to
            List<modulo> modules = role.modulo.ToList();

            // Get the main menu items
            List<modulo> parent = (modules.Where(m => m.NIVEL == 0 && m.VERSION == 1)).ToList();

            // For each main menu item, retrieve its childs
            foreach (modulo parentModule in parent)
            {
                // Add parent menu item to the MenuModel
                MenuDashboardDTO menuModel = new MenuDashboardDTO();
                menuModel.IsParent = true;
                menuModel.Name = parentModule.NOMBRE;
                menuModel.Icon = parentModule.ICONO;
               // menuModel.JSAction = parentModule.ACCION;

                // Retrieve and add child menu items and add them to the MenuModel
                List<modulo> childMenuItems = (modules.Where(m => m.ID_PADRE == parentModule.ID)).ToList<modulo>();
                if (childMenuItems != null && childMenuItems.Count() > 0)
                {
                    // Get number of categories
                   // List<string> categoriesList = childMenuItems.SelectMany(m => new[] { m.CATEGORIA }).Distinct().ToList<string>();

                    // Se the categories this menu item has
                    //menuModel.Categories = categoriesList;
                    List<MenuDashboardDTO> childMenuModelList = new List<MenuDashboardDTO>();
                    foreach (modulo childModule in childMenuItems)
                    {
                        MenuDashboardDTO childMenuModel = new MenuDashboardDTO();
                        childMenuModel.Name = childModule.NOMBRE;
                      //  childMenuModel.JSAction = childModule.ACCION;
                        childMenuModel.URL = childModule.URL;
                       // childMenuModel.Category = childModule.CATEGORIA;
                        childMenuModel.ParentName = menuModel.Name;
                        childMenuModel.IsParent = false;

                        // Add the child menu item to the main menu item
                        childMenuModelList.Add(childMenuModel);
                    }
                    menuModel.MenuChilds = childMenuModelList;
                }
                menuModelList.Add(menuModel);
            }
            return menuModelList;
        }

    }
}
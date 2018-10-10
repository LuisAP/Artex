using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Artex.DB;
using Artex.Util.Sistema;

namespace Artex.Controllers.Catalogos
{
    public class settsController : Controller
    {
        private ArtexConnection db = new ArtexConnection();


        public ActionResult Index()
        {
            var MODEL = PermisosModulo.ObtenerPermisos(Modulo.SETT);

            return View(db.sett.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sett sett = db.sett.Find(id);
            if (sett == null)
            {
                return HttpNotFound();
            }
            return View(sett);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NOMBRE,DESCRIPCION,ACTIVO")] sett sett)
        {
            if (ModelState.IsValid)
            {
                db.sett.Add(sett);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sett);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sett sett = db.sett.Find(id);
            if (sett == null)
            {
                return HttpNotFound();
            }
            return View(sett);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NOMBRE,DESCRIPCION,ACTIVO")] sett sett)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sett).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sett);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sett sett = db.sett.Find(id);
            if (sett == null)
            {
                return HttpNotFound();
            }
            return View(sett);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sett sett = db.sett.Find(id);
            db.sett.Remove(sett);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

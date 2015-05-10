using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ITGame.DBConnector;
using ITGame.Infrastructure.Data;
using ITGame.Models.Entities;

namespace ITGame.WebApp.DataManager.Controllers
{
    public class WeaponsController : Controller
    {
        private readonly IEntityDbRepository _repository;
        private readonly IEntityProjector<Weapon> _dbContext;

        public WeaponsController(IEntityDbRepository repository)
        {
            _repository = repository;
            _dbContext = _repository.GetInstance<Weapon>();
        }

        // GET: Weapons
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.GetAllAsync());
        }

        // GET: Weapons/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Weapon weapon = await _dbContext.LoadAsync(id.Value);
            if (weapon == null)
            {
                return HttpNotFound();
            }
            return View(weapon);
        }

        // GET: Weapons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Weapons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,WeaponType,PhysicalAttack,MagicalAttack,Name,Weight,Equipped")] Weapon weapon)
        {
            if (ModelState.IsValid)
            {
                weapon.Id = Guid.NewGuid();
                _dbContext.Add(weapon);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(weapon);
        }

        // GET: Weapons/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Weapon weapon = await _dbContext.LoadAsync(id.Value);
            if (weapon == null)
            {
                return HttpNotFound();
            }
            return View(weapon);
        }

        // POST: Weapons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,WeaponType,PhysicalAttack,MagicalAttack,Name,Weight,Equipped")] Weapon weapon)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Update(weapon);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(weapon);
        }

        // GET: Weapons/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Weapon weapon = await _dbContext.LoadAsync(id.Value);
            if (weapon == null)
            {
                return HttpNotFound();
            }
            return View(weapon);
        }

        // POST: Weapons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Weapon weapon = await _dbContext.LoadAsync(id);
            _dbContext.Delete(weapon);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

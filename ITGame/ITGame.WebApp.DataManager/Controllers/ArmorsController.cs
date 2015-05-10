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
    public class ArmorsController : Controller
    {
        private readonly IEntityDbRepository _repository;
        private readonly IEntityProjector<Armor> _dbContext;

        public ArmorsController(IEntityDbRepository repository)
        {
            _repository = repository;
            _dbContext = _repository.GetInstance<Armor>();
        }

        // GET: Armors
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.GetAllAsync());
        }

        // GET: Armors/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Armor armor = await _dbContext.LoadAsync(id.Value);
            if (armor == null)
            {
                return HttpNotFound();
            }
            return View(armor);
        }

        // GET: Armors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Armors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ArmorType,PhysicalDef,MagicalDef,Name,Weight,Equipped")] Armor armor)
        {
            if (ModelState.IsValid)
            {
                armor.Id = Guid.NewGuid();
                _dbContext.Add(armor);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(armor);
        }

        // GET: Armors/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Armor armor = await _dbContext.LoadAsync(id.Value);
            if (armor == null)
            {
                return HttpNotFound();
            }
            return View(armor);
        }

        // POST: Armors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ArmorType,PhysicalDef,MagicalDef,Name,Weight,Equipped")] Armor armor)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Update(armor);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(armor);
        }

        // GET: Armors/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Armor armor = await _dbContext.LoadAsync(id.Value);
            if (armor == null)
            {
                return HttpNotFound();
            }
            return View(armor);
        }

        // POST: Armors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Armor armor = await _dbContext.LoadAsync(id);
            _dbContext.Delete(armor);
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

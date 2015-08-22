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
using ITGame.Models.Entities;
using ITGame.Models.Entities.Mapping;

namespace ITGame.WebApp.DataManager.Controllers
{
    public class SpellsController : Controller
    {
        private ITGameDBContext db = new ITGameDBContext();

        // GET: Spells
        public async Task<ActionResult> Index()
        {
            return View(await db.Spell.ToListAsync());
        }

        // GET: Spells/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spell spell = await db.Spell.FindAsync(id);
            if (spell == null)
            {
                return HttpNotFound();
            }
            return View(spell);
        }

        // GET: Spells/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Spells/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SpellType,SchoolSpell,MagicalPower,ManaCost,TotalDuration,Name,Equipped")] Spell spell)
        {
            if (ModelState.IsValid)
            {
                spell.Id = Guid.NewGuid();
                db.Spell.Add(spell);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(spell);
        }

        // GET: Spells/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spell spell = await db.Spell.FindAsync(id);
            if (spell == null)
            {
                return HttpNotFound();
            }
            return View(spell);
        }

        // POST: Spells/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SpellType,SchoolSpell,MagicalPower,ManaCost,TotalDuration,Name,Equipped")] Spell spell)
        {
            if (ModelState.IsValid)
            {
                db.Entry(spell).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(spell);
        }

        // GET: Spells/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spell spell = await db.Spell.FindAsync(id);
            if (spell == null)
            {
                return HttpNotFound();
            }
            return View(spell);
        }

        // POST: Spells/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Spell spell = await db.Spell.FindAsync(id);
            db.Spell.Remove(spell);
            await db.SaveChangesAsync();
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

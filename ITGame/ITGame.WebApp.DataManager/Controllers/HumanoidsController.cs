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
    public class HumanoidsController : Controller
    {
        private readonly IITGameDbRepository _repository;
        private readonly HumanoidsDbProjector _dbHumanoidsContext;
        private readonly IEntityProjector<Character> _dbCharactersContext;

        public HumanoidsController(IITGameDbRepository repository)
        {
            _repository = repository;
            _dbHumanoidsContext = _repository.GetHumanoidsProjector();
            _dbCharactersContext = _repository.GetInstance<Character>();
        }

        // GET: Humanoids
        public async Task<ActionResult> Index()
        {
            return View(await _dbHumanoidsContext.GetHumanoidsWithCharacterAsync());
        }

        // GET: Humanoids/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Humanoid humanoid = await _dbHumanoidsContext.LoadAsync(id.Value);
            if (humanoid == null)
            {
                return HttpNotFound();
            }
            return View(humanoid);
        }

        // GET: Humanoids/Create
        public ActionResult Create()
        {
            ViewBag.CharacterId = new SelectList(_dbCharactersContext.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Humanoids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CharacterId,HumanoidRaceType,HP,MP,Strength,Agility,Wisdom,Constitution,Name,Level")] Humanoid humanoid)
        {
            if (ModelState.IsValid)
            {
                humanoid.Id = Guid.NewGuid();
                _dbHumanoidsContext.Add(humanoid);
                await _dbHumanoidsContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CharacterId = new SelectList(_dbCharactersContext.GetAll(), "Id", "Name", humanoid.CharacterId);
            return View(humanoid);
        }

        // GET: Humanoids/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Humanoid humanoid = await _dbHumanoidsContext.LoadAsync(id.Value);
            if (humanoid == null)
            {
                return HttpNotFound();
            }
            ViewBag.CharacterId = new SelectList(_dbCharactersContext.GetAll(), "Id", "Name", humanoid.CharacterId);
            return View(humanoid);
        }

        // POST: Humanoids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CharacterId,HumanoidRaceType,HP,MP,Strength,Agility,Wisdom,Constitution,Name,Level")] Humanoid humanoid)
        {
            if (ModelState.IsValid)
            {
                _dbHumanoidsContext.Update(humanoid);
                await _dbHumanoidsContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CharacterId = new SelectList(_dbCharactersContext.GetAll(), "Id", "Name", humanoid.CharacterId);
            return View(humanoid);
        }

        // GET: Humanoids/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Humanoid humanoid = await _dbHumanoidsContext.LoadAsync(id.Value);
            if (humanoid == null)
            {
                return HttpNotFound();
            }
            return View(humanoid);
        }

        // POST: Humanoids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await _dbHumanoidsContext.DeleteAsync(id);
            await _dbHumanoidsContext.SaveChangesAsync();
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

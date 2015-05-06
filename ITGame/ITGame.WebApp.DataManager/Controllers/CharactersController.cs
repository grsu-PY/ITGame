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
using ITGame.DBConnector.ITGameDBModels;
using ITGame.Infrastructure.Data;
using ITGame.Models.Entities;
using Character = ITGame.Models.Entities.Character;

namespace ITGame.WebApp.DataManager.Controllers
{
    public class CharactersController : Controller
    {
        private readonly IEntityRepository _repository;
        private readonly IEntityProjector<Character> _dbContext;

        public CharactersController()
        {
            _repository = new DBRepository();
            _dbContext = _repository.GetInstance<Character>();
        }
        public CharactersController(IEntityRepository repository)
        {
            _repository = repository;
        }

        // GET: Characters
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.GetAllAsync());
        }

        // GET: Characters/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = await _dbContext.LoadAsync(id.Value);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // GET: Characters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Password,Name,Role")] Character character)
        {
            if (ModelState.IsValid)
            {
                character.Id = Guid.NewGuid();
                _dbContext.Add(character);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(character);
        }

        // GET: Characters/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = await _dbContext.LoadAsync(id.Value);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Password,Name,Role")] Character character)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Update(character);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(character);
        }

        // GET: Characters/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = await _dbContext.LoadAsync(id.Value);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Character character = await _dbContext.LoadAsync(id);
            _dbContext.Delete(character);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

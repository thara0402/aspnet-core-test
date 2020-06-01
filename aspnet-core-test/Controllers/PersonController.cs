using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet_core_test.Infrastructure;
using aspnet_core_test.Infrastructure.Models;
using aspnet_core_test.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_test.Controllers
{
    public class PersonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _repository;

        public PersonController(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: PersonController
        public ActionResult Index()
        {
            var entity = _repository.Get();
            if (entity == null)
            {
                entity = new List<PersonEntity>();
            }
            
            var result = _mapper.Map<List<Person>>(entity);
            return View(result);
        }

        // GET: PersonController/Details/5
        public ActionResult Details(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Person>(entity);
            return View(result);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id, Code, Name")]Person model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<PersonEntity>(model);
                _repository.Create(entity);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(ModelState);
        }

        // GET: PersonController/Edit/5
        public ActionResult Edit(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Person>(entity);
            return View(result);
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id, Code, Name")] Person model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<PersonEntity>(model);
                _repository.Update(id, entity);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(ModelState);
        }

        // GET: PersonController/Delete/5
        public ActionResult Delete(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Person>(entity);
            return View(result);
        }

        // POST: PersonController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

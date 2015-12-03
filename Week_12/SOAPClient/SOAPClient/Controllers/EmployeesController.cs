﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// new...
using SOAPClient.SenecaES;

namespace SOAPClient.Controllers
{
    public class EmployeesController : Controller
    {
        // Manager reference
        private Manager m = new Manager();

        // Attention - Notice that these controller methods are standard and familiar

        // GET: Employees
        public ActionResult Index()
        {
            return View(m.AllEmployees());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            // Get one item
            var fetchedObject = m.EmployeeById(id.GetValueOrDefault());

            if (fetchedObject == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(fetchedObject);
            }
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult Create(EmployeeAdd newItem)
        {
            // Validate the input
            if (!ModelState.IsValid) { return View(); }

            // Process the input
            var addedItem = m.AddEmployee(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }

        // Not used in this controller

        /*
        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        */

    }

}
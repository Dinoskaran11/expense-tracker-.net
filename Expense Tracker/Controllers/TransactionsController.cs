﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transactions.Include(t => t.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/AddOrEdit
        public IActionResult AddOrEdit(int id=0)
        {
            Populatecategories();
            if(id == 0) 
                return View(new Transaction());
            else
                return View(_context.Transactions.Find(id));
        }

        // POST: Transactions/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if(transaction.TransactionId == 0)
                    _context.Add(transaction);
                else
                    _context.Update(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Populatecategories();
            return View(transaction);
        }


        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [NonAction]
        public void Populatecategories()
        {
            var CategoryCollection = _context.Categories.ToList();
            Category DefaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            CategoryCollection.Insert(0, DefaultCategory);
            ViewBag.Categories = CategoryCollection;
        }
    }
  
}

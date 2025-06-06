﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Enums;
using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly LibrarySystemContext _context;

        public RolesController(LibrarySystemContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            ViewBag.RoleOptions = await GetAvailableRoleOptionsAsync();
            return View(await _context.Role.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Role
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        private async Task<List<SelectListItem>> GetAvailableRoleOptionsAsync()
        {
            var existingRoles = await _context.Role.Select(r => r.UserRole).ToListAsync();

            return Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Where(r => !existingRoles.Contains(r))
                .Select(r => new SelectListItem
                {
                    Value = ((int)r).ToString(),
                    Text = r.ToString()
                }).ToList();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.RoleOptions = await GetAvailableRoleOptionsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("UserRole")] Role role)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.Role.AnyAsync(r => r.UserRole == role.UserRole);
                if (exists)
                {
                    ModelState.AddModelError("", "This role already exists.");
                }
                else
                {
                    _context.Add(role);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.RoleOptions = await GetAvailableRoleOptionsAsync();
            return View(role);
        }

        // GET: Roles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserRole")] Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Role
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role != null)
            {
                _context.Role.Remove(role);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.Id == id);
        }
    }
}

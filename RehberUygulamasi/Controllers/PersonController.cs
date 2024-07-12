using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RehberUygulamasi.Data;
using RehberUygulamasi.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RehberUygulamasi.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<PersonController> _logger;

        public PersonController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<PersonController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persons.ToListAsync());
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: id is null");
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                _logger.LogWarning("Details: Person not found for id {id}", id);
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Person person, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];

                    if (file.Length > 0)
                    {


                        // GUID kullanarak benzersiz bir dosya adı oluşturun
                        var fileExtension = Path.GetExtension(file.FileName); // Dosya uzantısını alın
                        var fileName = $"{Guid.NewGuid()}{fileExtension}"; // GUID ve uzantıyı birleştirin
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        // Dosyayı kaydet
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);

                        }

                        // Dosya yolunu veritabanına kaydedin
                        person.PhotoPath = "/images/"+fileName;
                    }
                }

                // Person nesnesini veritabanına kaydetme işlemleri
                _context.Add(person);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }


        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit: id is null");
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                _logger.LogWarning("Edit: Person not found for id {id}", id);
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,Address")] Person person, IFormFile photo)
        {
            if (id != person.Id)
            {
                _logger.LogWarning("Edit: id mismatch");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null && photo.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(fileStream);
                        }

                        person.PhotoPath = uniqueFileName;
                    }

                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        _logger.LogWarning("Edit: Person not found for id {id}", person.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Edit: Concurrency error");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Edit: Error while updating person");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete: id is null");
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                _logger.LogWarning("Delete: Person not found for id {id}", id);
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                try
                {
                    _context.Persons.Remove(person);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Delete: Error while deleting person");
                }
            }
            else
            {
                _logger.LogWarning("Delete: Person not found for id {id}", id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}

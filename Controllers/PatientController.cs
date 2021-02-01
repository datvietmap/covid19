using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Covid19App.Data;
using Covid19App.Services;
using Covid19App.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Covid19App.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly Dictionary<int, string> _levels = new Dictionary<int, string>()
        {
            { 1,"F0"},
            { 2,"F1"},
            { 3,"F2"},
        };
        private readonly Dictionary<int, string> _status = new Dictionary<int, string>()
        {
            { 1, "Đã khỏi bệnh" },
            { 2, "Đang điều trị" },
            { 3, "Đang cách ly" },
            { 4, "Hết thời gian cách ly" },
        };

        public PatientController(ApplicationDbContext context, IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _context = context;
            _mapper = mapper;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {

            var parents = _context.Patients.ToList().Where(a => a.Level != LevelModel.F2)
                .ToDictionary(a => a.Id, b => b);

            var list = _context.Patients.ToList();

            var data = new List<PatientAdminViewModel>();

            var patients = await _context.Patients.ToArrayAsync();

            foreach (var item in patients)
            {
                var viewModel = _mapper.Map<PatientAdminViewModel>(item);
                if (parents.TryGetValue(item.ParentId ?? 0, out var patient))
                {
                    viewModel.Parent = patient.Name;
                }
                if(_status.TryGetValue((int)item.Status, out var status))
                {
                    viewModel.Status = status;
                }
                data.Add(viewModel);
            }

            return View(data);
        }
        public async Task<JsonResult> GetPatients()
        {
            var parents = _context.Patients.ToList().Where(a => a.Level != LevelModel.F2)
                   .ToDictionary(a => a.Id, b => b);

            var list = _context.Patients.ToList();

            var data = new List<PatientAdminViewModel>();

            var patients = await _context.Patients.ToArrayAsync();

            foreach (var item in patients)
            {
                var viewModel = _mapper.Map<PatientAdminViewModel>(item);
                if (parents.TryGetValue(item.ParentId ?? 0, out var patient))
                {
                    viewModel.Parent = patient.Name;
                }
                data.Add(viewModel);
            }
            return Json(data);
        }

        private string GetParentName(int? parentId, Dictionary<int,Patient> parents)
        {
            if (parentId == 0 || parentId == null)
                return "";
            if (parents.ContainsKey((int)parentId))
                return parents[(int)parentId].Name;
            return "";
        }

        public async Task<IActionResult> CreateWithParent(int? id)
        {
            ViewBag.Levels = _levels;
            ViewBag.Status = _status;

            var patient = await _context.Patients.FindAsync(id);

            ViewBag.Parent = patient;
            var level = (int)patient.Level + 1;
            ViewBag.Level = level;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateWithParent(PatientPostModel patient)
        {
            if (ModelState.IsValid)
            {
                var parentId = patient.ParentId;
                var access = await _patientService.CreateAsync(patient);
                if (access)
                    return RedirectToAction(nameof(Details), new {id = parentId });
                else
                {
                    TempData["Notifi"] = "Thêm không thành công";
                    return View(patient);
                }
            }
            return View(patient);
        }
        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactPerson = await _patientService.GetContactPersonAsync(id);

            var patient = _context.Patients.Find(id);
            ViewBag.PatientName = patient.Name;
            ViewBag.PatientId = patient.Id;

            return View(contactPerson);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            ViewBag.Levels = _levels;
            ViewBag.Status = _status;
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientPostModel patient)
        {
            if (ModelState.IsValid)
            {
                var access = await _patientService.CreateAsync(patient);
                if (access)
                    return RedirectToAction(nameof(Index));
                else
                {
                    TempData["Notifi"] = "Thêm không thành công";
                    return View();
                }
            }
            return View(patient);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Levels = _levels;
            ViewBag.Status = _status;

            var patient = await _context.Patients.FindAsync(id);

            var viewModel = _mapper.Map<PatientAdminViewModel>(patient);
            if(patient.ParentId != null && patient.ParentId != 0) 
                viewModel.Parent = _context.Patients.Find(patient.ParentId).Name;
            if (_status.TryGetValue((int)patient.Status, out var status))
            {
                viewModel.Status = status;
            }
            if (patient == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Patient patient)
        {
            if(patient.ParentId == 0) patient.ParentId = id;
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<JsonResult> GetListParents(int level)
        {
            var data = await _patientService.GetListParents(level);
            return Json(data);
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}

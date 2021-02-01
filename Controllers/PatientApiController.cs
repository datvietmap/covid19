using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Covid19App.Data;
using Covid19App.Models;
using Covid19App.Models.UserModels;
using Covid19App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Covid19App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PatientApiController(ApplicationDbContext dbContext, IPatientService patientService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _patientService = patientService;
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("List")]
        public async Task<Result<IEnumerable<PatientViewModel>>> GetListPatient()
        {
           var data =  await _patientService.GetListPatientsAsync();
            return data;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("ListByDate")]
        public async Task<Result<IEnumerable<PatientViewModel>>> GetListPatientByDate(DateTime fromDate,DateTime toDate)
        {
            var data = await _patientService.GetListPatientByDate(fromDate, toDate);
            return data;
        }


    }
}
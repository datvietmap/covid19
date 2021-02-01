using AutoMapper;
using Covid19App.Data;
using Covid19App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19App.Services
{
    public interface IPatientService
    {
        Task<Result<IEnumerable<PatientViewModel>>> GetListPatientsAsync();
        Task<bool> CreateAsync(PatientPostModel patientPost);
        Task<IEnumerable<Patient>> GetListParents(int level);
        Task<IEnumerable<PatientAdminViewModel>> GetContactPersonAsync(int? id);
        Task<Result<IEnumerable<PatientViewModel>>> GetListPatientByDate(DateTime fromDate, DateTime toDate);
    }
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public PatientService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<Result<IEnumerable<PatientViewModel>>> GetListPatientsAsync()
        {
            try
            {
                var patients = await _dbContext.Patients.Where(a => a.Status != Status.Cured &&  a.Status != Status.OutIsolation)
                .Select(a => _mapper.Map<PatientViewModel>(a))
                .ToListAsync();
                return new Result<IEnumerable<PatientViewModel>>(patients);
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<PatientViewModel>>(CODE.ERROR, ex.Message, null);
            }
        }
        public async Task<bool> CreateAsync(PatientPostModel patientPost)
        {
            var patient = _mapper.Map<Patient>(patientPost);

            _dbContext.Patients.Add(patient);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Patient>> GetListParents(int level)
        {
            try
            {
                if (level == (int)LevelModel.F0)
                    return new List<Patient>();

                level -= 1;

                var list = await _dbContext.Patients.ToListAsync();
                list = list.Where(a => (int)a.Level == level).ToList();

                return list;
            }
            catch (Exception ex)
            {

            }
            return null;

        }

        public async Task<IEnumerable<PatientAdminViewModel>> GetContactPersonAsync(int? id)
        {
            var data =  await _dbContext.Patients.Where(a => a.ParentId == id).ToListAsync();

            var parents = _dbContext.Patients.Where(a => a.Level != LevelModel.F2)
                .ToDictionary(a => a.Id, b => b);

            var personcontact = new List<PatientAdminViewModel>();
;

            foreach (var item in data)
            {
                var viewModel = _mapper.Map<PatientAdminViewModel>(item);
                viewModel.Parent = GetParentName(item.ParentId, parents);
                personcontact.Add(viewModel);
            }

            return personcontact;
        }
        public string GetParentName(int? parentId, Dictionary<int, Patient> parents)
        {
            if (parentId == 0 || parentId == null) 
                return "";
            if (parents.ContainsKey((int)parentId))
                return parents[(int)parentId].Name;
            return "";
        }

        public async Task<Result<IEnumerable<PatientViewModel>>> GetListPatientByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var patients = await _dbContext.Patients
                .Where(a => (a.Status != Status.Cured && a.Status != Status.OutIsolation) && (a.VerifyDate >= fromDate && a.VerifyDate <= toDate))
                .Select(a => _mapper.Map<PatientViewModel>(a))
                .ToListAsync();
                return new Result<IEnumerable<PatientViewModel>>(patients);
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<PatientViewModel>>(CODE.ERROR, ex.Message, null);
            }
        }
    }
}

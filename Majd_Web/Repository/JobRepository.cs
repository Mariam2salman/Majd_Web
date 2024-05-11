using Majd_Web.Data;
using Majd_Web.Models;
using Majd_Web.Models.enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Majd_Web.Repository
{
    public class JobRepository : IJobRepository
    {   
        private readonly AppDBContext _db;

        public JobRepository(AppDBContext db)
        {
            _db  = db;
        }
        public async Task CreateAsync( Job job)
        {

            await _db.Jobs.AddAsync(job);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var result = await  _db.Jobs.FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _db.Jobs.Remove(result);
                await _db.SaveChangesAsync();
            }
        }

        

        public async Task<Job> GetByIdAsync(string id)
        {
        var job = await _db.Jobs.Include(j => j.User) .FirstOrDefaultAsync(j => j.Id == id);
            return job == null ? throw new Exception() : job;
        }

        public IQueryable<Job> GetAllAsync()
        {
            var jobs = _db.Jobs
             .Select(e => new Job
             {
                 Id = e.Id,
                 UserId = e.UserId,
                 Title = e.Title,
                 Description = e.Description,
                 Location = e.Location,
                 CreatedDate = e.CreatedDate,
                 Duties = e.Duties,
                 Qualification = e.Qualification,
                 JobType = e.JobType,
                 WorkLocation = e.WorkLocation,
                 JobIndustry = e.JobIndustry,
                 Requirements = e.Requirements,
                 ReasonsToJoin = e.ReasonsToJoin,
                 Responsibilities=e.Responsibilities,
                 RoleDescription= e.RoleDescription,
                 ApplicationsCount=e.ApplicationsCount,
             });
             return jobs;
         }
    }
}

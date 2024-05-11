using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Majd_Web.Repository;
using Majd_Web.Models.enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Majd_Web.ViewModels;
using Majd_Web.Data;
using Majd_Web.Models;
using Majd_Web.Repository;
using Majd_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace Majd_Web.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly AppDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobRepository _IJobRepository;

        public JobController(AppDBContext db, UserManager<ApplicationUser> userManager, IJobRepository jobRepository)
        {
            _db = db;
            _userManager = userManager;
            _IJobRepository = jobRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        //GET Job/CreateJobOffer
        public IActionResult CreateJobOffer()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        //Post Job/CreateJobOffer
        public async Task<IActionResult> CreateJobOffer(Job job)
        {
            if (!ModelState.IsValid)
            {
                //for testing only, error handling is required here!
                var errors = ModelState.Values.SelectMany(v => v.Errors);

            }

            job.UserId = _userManager.GetUserId(User);

            await _IJobRepository.CreateAsync(job);

            return RedirectToAction("JobList", new { id = job.UserId });
        }

        [Authorize]

        public async Task<IActionResult> JobOffers(JobIndustry? selectedIndustry)
        {
            var jobs = _IJobRepository.GetAllAsync();
            if (selectedIndustry is not null)
            {
                jobs = jobs.Where(i => i.JobIndustry == selectedIndustry);
            }
            var adminUserId = (await _userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault()?.Id;
            ViewBag.news = _db.Posts.Where(p => p.UserId == adminUserId).ToList();
            return View(jobs.ToList());
        }

        [Authorize]
        public async Task<IActionResult> JobListAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            List<Job> jobs = _IJobRepository.GetAllAsync().Where(a => a.UserId == id).ToList();
            var ViewModel = new JobListViewModel
            {
                User = user,
                Jobs = jobs

            };
            return View(ViewModel);
        }

        [Authorize]
        public async Task<IActionResult> JobDetailsAsync(string id)
        {
            var job = await _IJobRepository.GetByIdAsync(id);
            var jobApplications = _db.JobApplications.Where(a => a.JobId == id);

            if (job == null)
            {
                return NotFound();
            }

            var Model = new JobApplicationViewModel
            {
                Job = job,
                JobApplications = jobApplications
            };
            ViewBag.news = _db.Posts.ToList();
            return View(Model);
        }

        [HttpPost]
        [Authorize(Roles = "Alumni")]
        public async Task<IActionResult> Apply(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var job = await _IJobRepository.GetByIdAsync(id);

            var userHasApplied = _db.JobApplications.Any(a => a.UserId == user.Id && a.JobId == id);

            if (job == null)
            {
                return NotFound();
            }

            if (!userHasApplied)
            {
                JobApplication jobApplication = new JobApplication
                {
                    UserId = user.Id,
                    JobId = id
                };

                job.JobApplications.Add(jobApplication);
                job.ApplicationsCount++;
                jobApplication.hasApplied = true;
                _db.SaveChanges();
            }


            return RedirectToAction("JobDetails", new { id = id });
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ApplicantsAsync(string id)
        {
            var job = await _IJobRepository.GetByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (job == null || job.UserId != user.Id)
            {
                return NotFound();
            }

            var ApplicationList = _db.JobApplications.Include(u => u.User).Where(j => j.JobId == id);

            var Applicants = ApplicationList.Select(a => a.User).ToList();

            return View(Applicants);
        }



        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteJobAsync(string jobId, string userId)
        {


            var jobApplications = _db.JobApplications.Where(j => j.JobId == jobId).ToList();
            _db.JobApplications.RemoveRange(jobApplications);

            await _IJobRepository.DeleteAsync(jobId);

            return RedirectToAction("JobList", new { id = userId });
        }
    }
}
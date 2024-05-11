using Majd_Web.Data;
using Majd_Web.Models;
using Majd_Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Majd_Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IExperienceRepository _IExperienceRepository;
        private readonly AppDBContext _db;


        public ProfileController(UserManager<ApplicationUser> userManager, IExperienceRepository IExperienceRepository, AppDBContext db)
        {
            _db = db;
            _userManager = userManager;
            _IExperienceRepository = IExperienceRepository;
        }

        //GET Profile/UserProfile
        public async Task<IActionResult> AlumniInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {  //get alumni exper
                await _db.Entry(user)
                  .Collection(u => u.Experiences)
                  .LoadAsync();
                //get alumni major
                var major = await _db.Majors.FirstOrDefaultAsync(m => m.Students.Any(s => s.Id == user.Id));
                if (major != null)
                {
                    user.Major = major;
                }
                return View(user);
            }
            else
            {
                return NotFound("User not found.");

            }
        }

        public async Task <IActionResult> EmployerInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return View(user);
            }
            var adminUserId = (await _userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault()?.Id;
            ViewBag.news = _db.Posts.Where(p => p.UserId == adminUserId).ToList();
            return BadRequest(" user not found");


        }
        [Authorize(Roles ="Alumni")]
        public IActionResult AddExperience()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Alumni")]
        //Post Job/CreateJobOffer

        public async Task<IActionResult> AddExperience(AlumniExperience Experience)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest("User not found");
            }

            Experience.UserId = currentUser.Id;

            await _IExperienceRepository.CreateAsync(Experience);

            return RedirectToAction("AlumniInfo");
        }

        [Authorize(Roles = "Employer")]

        public async Task<IActionResult> ApplicantInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _db.Entry(user)
                  .Collection(u => u.Experiences)
                  .LoadAsync();
                var major = await _db.Majors.FirstOrDefaultAsync(m => m.Students.Any(s => s.Id == userId));
                if (major != null)
                {
                    user.Major = major; // 
                }
                return View(user);

            }
            else
            {
                return NotFound("User not found.");
            }
        }

    }

}



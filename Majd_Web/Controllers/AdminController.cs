using Majd_Web.Data;
using Majd_Web.Models;
using Majd_Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Majd_Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDBContext _db;
        private IPostRepository _postRepository;

        public IActionResult AdminHomepage()
        {
            return View();
        }

        public AdminController(UserManager<ApplicationUser> userManager, AppDBContext db, IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _db = db;

        }

        public async Task<IActionResult> PendingRegistrations()
        {
            //get all users in employer role
            var employers = await _userManager.GetUsersInRoleAsync("Employer");
            //get unapproved request
            if (employers != null)
            {
                employers = employers.Where(u => u.IsAproved == false).ToList();
            }
            return View(employers);
        }

        public async Task<IActionResult> ApproveEmployer(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsAproved = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("PendingRegistrations");
            }
            else
            {
                return NotFound("User not found.");
            }

        }

        public async Task<IActionResult> RejectEmployer(string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            { await _userManager.DeleteAsync(user);
                return RedirectToAction("PendingRegistrations");
            } else {
                return NotFound("User not found.");
            }
        }


        public async Task<IActionResult> UpsertNews()
        {

            var adminUserId = (await _userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault()?.Id;
            ViewBag.news = _db.Posts.Where(p => p.UserId == adminUserId).ToList();


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(string id)
        {
            await _postRepository.DeleteAsync(id);

            return RedirectToAction("UpsertNews");
        }


        [HttpPost]
        public async Task<IActionResult> UpsertNewsAsync(Post model)

        {

            try
            {
                if (!ModelState.IsValid)
                {
                    var erorrs = ModelState.Values.SelectMany(v => v.Errors);

                }



                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {

                    return BadRequest("User not found");
                }

                var news = new Post
                {
                    PostContent = model.PostContent,
                    UserId = user.Id
                };

                _db.Posts.Add(news);
                _db.SaveChanges();


            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while saving the news : " + ex.InnerException;

                return View("Error", errorMessage);
            }


            return RedirectToAction("UpsertNews");
        }

    }
}

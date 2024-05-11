using Majd_Web.Data;
using Majd_Web.Models;
using Majd_Web.Repository;
using Majd_Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Majd_Web.Controllers
{
    [Authorize]
    public class AlumniController : Controller
    {
        IWebHostEnvironment hostEnvironment;
        private readonly AppDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AlumniController> _logger;
        private readonly IPostRepository _postRepository;


        public AlumniController(IWebHostEnvironment hostEnvironment, AppDBContext db, UserManager<ApplicationUser> userManager, IPostRepository postRepository)
        {
            this.hostEnvironment = hostEnvironment;
            _db = db;
            _userManager = userManager;
            _postRepository = postRepository;

        }

        public async Task<IActionResult> AlumniHomepage()
        {


            var posts = _db.Posts.Include(u => u.User).ToList();
            var Model = new AlumniPostViewModel
            {
                Posts = posts,
                Post = new Post()

            };
            var adminUserId = (await _userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault()?.Id;
            ViewBag.news = _db.Posts.Where(p => p.UserId == adminUserId).ToList();

            return View(Model);
        }

        //like post
        [HttpPost]
        public IActionResult Like(string id)
        {
            var post = _db.Posts.Find(id);
            if (post != null)
            {
                post.LikeCount++;
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }


        //SHARE POST
        [Authorize(Roles = "Alumni")]
        [HttpPost]
        public async Task<IActionResult> AlumniHomepage(Post post, IFormFile? imageFile, IFormFile? videoFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var erorrs = ModelState.Values.SelectMany(v => v.Errors);

                }

                var user = await _userManager.GetUserAsync(User);

                // Check if the User exist

                if (user == null)
                {

                    return BadRequest("User not found");
                }
                else
                {

                    post.UserId = user.Id;

                }


                string uploadFolder;
                string fileName;

                // Check if an image file was uploaded
                if (imageFile != null)
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    uploadFolder = Path.Combine(hostEnvironment.WebRootPath, @"PostImages");
                    post.ImageUrl = "/PostImages/" + fileName;

                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                }

                // Check if a video file was uploaded
                if (videoFile != null)
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
                    uploadFolder = Path.Combine(hostEnvironment.WebRootPath, @"PostVideos");
                    post.VideoUrl = "/PostVideos/" + fileName;

                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                    {
                        await videoFile.CopyToAsync(fileStream);
                    }
                }



                await _postRepository.CreateAsync(post);

            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while saving the alumni Post: " + ex.InnerException;

                return View("Error", errorMessage);
            }


            return RedirectToAction(nameof(AlumniHomepage));
        }

        [HttpPost]

        //DeletePost
        public async Task<IActionResult> DeletePost(string id)
        {
            await _postRepository.DeleteAsync(id);


            return RedirectToAction(nameof(AlumniHomepage));


        }

        private class HttpStatusCodeResult : ActionResult
        {
            private HttpStatusCode badRequest;

            public HttpStatusCodeResult(HttpStatusCode badRequest)
            {
                this.badRequest = badRequest;
            }
        }

        public async Task<IActionResult> AlumniInfo()
        {
            var adminUserId = (await _userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault()?.Id;
            ViewBag.news = _db.Posts.Where(p => p.UserId == adminUserId).ToList();
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return View(user);
            }
            return BadRequest(" user not found");
        }
        public async Task<IActionResult> AlumniPageAsync(string id)
        {
            var alumni = await _userManager.FindByIdAsync(id);
            var posts = _db.Posts.Where(u => u.UserId == id).ToList();
            var viewModel = new AlumniPageViewModel
            {
                User = alumni,
                Posts = posts,
                Post = new Post()

            };
            var adminUserId = (await _userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault()?.Id;
            ViewBag.news = _db.Posts.Where(p => p.UserId == adminUserId).ToList();
            return View(viewModel);

        }

        //Get Alumni/AlumniList

        public async Task<IActionResult> AlumniList()
        {
            var alumni = await _userManager.GetUsersInRoleAsync("Alumni");
            ViewBag.Major = _db.Majors.ToList();


            return View(alumni);
        }


        [HttpPost]
        //Post Alumni/AlumniL/Search term , Majoe
        public async Task<IActionResult> AlumniList(string? firstName, string? lastName, string? selectedMajor)
        {
            //Get all users
            var users = await _userManager.GetUsersInRoleAsync("Alumni");

            if (!string.IsNullOrEmpty(selectedMajor))
            {
                users = users.Where(u => u.MajorId == selectedMajor).ToList();
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                users = users.Where(e => e.FirstName.Contains(firstName)).ToList();


            }
            if (!string.IsNullOrEmpty(lastName))
            {
                users = users.Where(e => e.LastName.Contains(lastName)).ToList();


            }

            return View(users);
        }


    }
}














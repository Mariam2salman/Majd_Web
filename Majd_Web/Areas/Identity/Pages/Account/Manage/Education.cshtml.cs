using Majd_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Majd_Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace Majd_Web.Areas.Identity.Pages.Account.Manage
{
   // [Authorize(Roles = "Alumni")]
    public class EducationModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly  AppDBContext _db;

        public EducationModel(
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager, AppDBContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;   
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public List<Major> MajorList { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        ///     
        public class InputModel
        {
            public string AcademicDegree { get; set; } = string.Empty;
            public int GradYear { get; set; }
            [Range(1.0, 5.0)]
            public double GPA { get; set; }
            [MaxLength(600)]
            public string EductionDescription { get; set; } = string.Empty;
            public string? MajorId { get; set; }
        }


        private async Task LoadAsync(ApplicationUser user)
        {

            Input = new InputModel
            {
                AcademicDegree = user.AcademicDegree,
                GradYear = user.GradYear,
                GPA = user.GPA,
                EductionDescription = user.EductionDescription,
                MajorId = user.MajorId,
                


            };
        }




        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            MajorList = _db.Majors.ToList();


            await LoadAsync(user );
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.AcademicDegree != user.AcademicDegree)
            {
                user.AcademicDegree = Input.AcademicDegree;
                await _userManager.UpdateAsync(user);

            }
            if (Input.GradYear != user.GradYear)
            {
                user.GradYear = Input.GradYear;
                await _userManager.UpdateAsync(user);

            }
            if (Input.GPA != user.GPA)
            {
                user.GPA = Input.GPA;
                await _userManager.UpdateAsync(user);

            }
            if (Input.EductionDescription != user.EductionDescription)
            {
                user.EductionDescription = Input.EductionDescription;
               await _userManager.UpdateAsync(user);

            }
            if (Input.MajorId != user.MajorId)
            {
                user.MajorId = Input.MajorId;
                await _userManager.UpdateAsync(user);

            }





            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your Education has been add";
            return RedirectToPage();
        }
    }
}
    


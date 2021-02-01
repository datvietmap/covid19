using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Covid19App.Data;
using Covid19App.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Covid19App.Controllers
{
    [Authorize (Roles = "SysAdmins,Admins")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, 
            UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager)
        {
            _mapper = mapper;
            _context = context;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }

        // GET: UserModels
        public async Task<IActionResult> Index()
        {
            var listUser = new List<UserModel>();

            var users = await _UserManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var viewUser = _mapper.Map<UserModel>(user);
                var roles = _UserManager.GetRolesAsync(user).Result;
                if (roles.Count > 0)
                    viewUser.RoleName = roles.First();
                listUser.Add(viewUser);
            }

            return View(listUser);
        }

        // GET: UserModels/Create
        public async Task<IActionResult> Create()
        {
            var roles = await _RoleManager.Roles.Select(a => a.Name).ToListAsync();

            ViewBag.Roles = roles;

            return View();
        }

        // POST: UserModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var findUser = _UserManager.FindByEmailAsync(model.Email).Result;
                if (findUser == null)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var temp = _UserManager.CreateAsync(user, model.Password).Result;
                    if (temp.Succeeded) 
                    {
                        findUser = _UserManager.FindByNameAsync(user.UserName).Result;
                        //add role here  
                        await _UserManager.AddToRoleAsync(findUser, model.Role);
                    }
                }
                else
                {
                    TempData["Error"] = "Người dùng đã tồn tại, vui lòng thử một tên khác!";

                    return View();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: UserModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var roles = await _RoleManager.Roles.Select(a => a.Name).ToListAsync();

            var user = _UserManager.FindByIdAsync(id).Result;

            var userModel = _mapper.Map<UserCreateModel>(user);

            ViewBag.Roles = roles;

            var userRole = _UserManager.GetRolesAsync(user).Result;
            if (userRole.Count > 0)
                userModel.Role = userRole.FirstOrDefault();

            return View(userModel);
        }

        // POST: UserModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,  UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var findUser = _UserManager.FindByIdAsync(id).Result;
                if (findUser != null)
                {
                    //add role here
                    //find exist roles
                    var existRoles = _UserManager.GetRolesAsync(findUser).Result;
                    
                    if(!existRoles.Contains(model.Role))
                    {
                        //remove exist role
                        if(existRoles.Count > 0) await _UserManager.RemoveFromRolesAsync(findUser, existRoles);

                        await _UserManager.AddToRoleAsync(findUser, model.Role);
                    }
                }
                else
                {
                    TempData["Error"] = "Người dùng không tồn tại!";

                    return View();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //[HttpGet]
        //public Task<IActionResult> ResetPassword(string id)
        //{

        //}

        // GET: UserModels/Delete/5
        public IActionResult Delete(string id)
        {
            
            var user =  _UserManager.FindByNameAsync(id).Result;
            if(user != null)
            {
                var viewUser = _mapper.Map<UserModel>(user);
                return View(viewUser);
            }

            return View();
        }
        [HttpPost]
        public IActionResult Delete(UserModel model)
        {
            var user = _UserManager.FindByIdAsync(model.Id).Result;
            if(user != null)
            {
                var result = _UserManager.DeleteAsync(user).Result;
            }
            return RedirectToAction("Index");

        }

    }
}

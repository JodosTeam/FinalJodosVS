namespace JodosServer.Controllers
{
    using JodosServer.Repositories;
    using JodosServer.Models;
    using Microsoft.AspNet.Identity;
    using Models;
    using System.Threading.Tasks;
    using System.Web.Http;
    using MongoDB.Driver;
    using JodosServer.Entities;
    using JodosServer.DTOs;

    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly AuthRepository authRepository = null;

        public AccountController(AuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authRepository.RegisterUser(userModel);

            var errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok(new UserDTO(userModel));
        }


        [AutherizeAttributeCustom(Roles = "Admin,SysAdmin")]
        [Route("removeUser")]
        public IHttpActionResult RemoveUser([FromBody]string UserName)
        {
            WriteConcernResult results = this.authRepository.RemoveUser(UserName);
            if (results.DocumentsAffected > 0)
                return Ok();
            else
                return BadRequest("המשתמש לא קיים");
            
        }

        [AutherizeAttributeCustom(Roles = "Admin,SysAdmin")]
        [Route("getAllUsers")]
        public IHttpActionResult GetAllUsers()
        {

            return Ok(authRepository.getAllUsersAndRoles());
        }


        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zembil.Services;
using Zembil.Views;

namespace Zembil.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserCred userCred)
        {
            var token = await _accountService.Authenticate(userCred.Username, userCred.Password);

            if (token == null)
                return NotFound();

            var authDto = new AuthDto
            {
                token = token
            };
            return Ok(authDto);
        }

        // [HttpPost]
        // public ActionResult Upload([FromForm] File)
        // {
        //     using (DBModel db = new DBModel())
        //     {
        //         if (!jsonFile.FileName.EndsWith(".json"))
        //         {
        //             ViewBag.Error = "Invalid file type(Only JSON file allowed)";
        //         }
        //         else
        //         {
        //             jsonFile.SaveAs(Server.MapPath("~/FileUpload/" + Path.GetFileName(jsonFile.FileName)));
        //             StreamReader streamReader = new StreamReader(Server.MapPath("~/FileUpload/" + Path.GetFileName(jsonFile.FileName)));
        //             string data = streamReader.ReadToEnd();
        //             List<Product> products = JsonConvert.DeserializeObject<List<Product>>(data);

        //             products.ForEach(p =>
        //             {
        //                 Product product = new Product()
        //                 {
        //                     Name = p.Name,
        //                     Price = p.Price,
        //                     Quantity = p.Quantity
        //                 };
        //                 db.Products.Add(product);
        //                 db.SaveChanges();
        //             });
        //             ViewBag.Success = "File uploaded Successfully..";
        //         }
        //     }
        //     return View("Index");
        // }

    }
}
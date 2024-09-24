using DBOperationsWithEF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationsWithEF.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;

        public LanguageController(AppDBContext appDBContext) 
        {
            _appDBContext = appDBContext;
        }

        [HttpGet("")]

        //Async()
         public async Task<IActionResult> GetAllLanguage()
         {
             //Linq methods :
             //var result = await _appDBContext.Languages.ToListAsync();
             var result = await (from languages in _appDBContext.Languages
                                 select languages).ToListAsync();
             return Ok(result);
         }

        //Sync()
        /* public IActionResult GetAllLanguage()
         {

             //var result = _appDBContext.Languages.ToList();
            var result = (from languages in _appDBContext.Languages
                          select languages).ToList();
             return Ok(result);
         }*/


        [HttpGet("{id}")]

        //Async()
        public async Task<IActionResult> GetLanguageByIdAsync([FromRoute] int id)
        {
            
            var result = await _appDBContext.Languages.FindAsync(id);
           
            return Ok(result);
        }

    }
}

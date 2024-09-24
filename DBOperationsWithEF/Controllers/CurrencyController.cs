using DBOperationsWithEF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationsWithEF.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDBContext _appDBContext; // naming convesion : _appDBContext

        public CurrencyController(AppDBContext appDBContext) 
        {
            _appDBContext = appDBContext;
        }

        //Feching all data
        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrency() 
        {
            // Sync methods
            // var result =  _appDBContext.Currancies.ToList();
            // var result = (from currencies in _appDBContext.Currancies
            // select currencies).ToList();

            // Async methods -- refer this method
            // var result = await _appDBContext.Currancies.ToListAsync();
             var result = await (from currencies in _appDBContext.Currancies
             select currencies).ToListAsync(); 
            return Ok(result);
        }

        // Feching sigle data using primary key
        [HttpGet("{id:int}")] // {id} - dynamic id
        public async Task<IActionResult> GetCurrencyByIdAsync([FromRoute] int id)
        {
            var result = await _appDBContext.Currancies.FindAsync(id);
            return Ok(result);
        }

        // Feching single data using name
       /* [HttpGet("{name}")] 
        public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name)
        {
           // var result = await _appDBContext.Currancies.Where(x=> x.Title == name).FirstAsync();
            var result = await _appDBContext.Currancies.FirstAsync(x => x.Title == name);
            return Ok(result);
        }*/

        // Feching data using multiple parameters
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByMultipleParaAsync([FromRoute] string name,[FromRoute] string? description)
        {
            // var result = await _appDBContext.Currancies.Where(x=> x.Title == name).FirstAsync();
            /*var result = await _appDBContext.Currancies
                .FirstOrDefaultAsync(x =>
                x.Title == name 
                && (string.IsNullOrEmpty(description) || x.Description == description));
            return Ok(result);*/

            var result = await _appDBContext.Currancies
                .Where(x =>
                x.Title == name
                && (string.IsNullOrEmpty(description) || x.Description == description)).ToListAsync();
            return Ok(result);
        }

        [HttpPost("all")] 
        public async Task<IActionResult> GetCurrenciesByIdAsync([FromBody] List<int> ids)
        {
            //var ids = new List<int> { 1 };
            var result = await _appDBContext.Currancies
                .Where(x=> ids.Contains(x.Id))
                .Select(x=> new Currency()
                { 
                  Id = x.Id,
                  Title = x.Title,
                })
                .ToListAsync();
            return Ok(result);
        }


    }
}

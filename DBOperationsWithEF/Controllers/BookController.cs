using DBOperationsWithEF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBOperationsWithEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(AppDBContext appDBContext) : ControllerBase
    {
        //Insert single record
        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] Book model)
        {

           /* var author = new Author()
            {
                Name = "Author 1",
                Email = "test@gmail.com"
            };
            model.Author = author;*/
            appDBContext.Books.Add(model);
            await appDBContext.SaveChangesAsync();

            return Ok(model);
        }

        // Insert multiple records 
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> model)
        {

            appDBContext.Books.AddRange(model);
            await appDBContext.SaveChangesAsync();

            return Ok(model);
        }

        // Update single Record
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookId,[FromBody] Book model)
        {

            var book = appDBContext.Books.FirstOrDefault(x => x.Id == bookId);
            if (book == null) 
            { 
               return NotFound();
            }

            book.Title = model.Title;
            book.Description = model.Description;  
            book.NoOfPages = model.NoOfPages;

            await appDBContext.SaveChangesAsync();

            return Ok(model);
        }

        // Update Method - Not refere this there are drawbacks. 
        [HttpPut("")]
        public async Task<IActionResult> UpdateBookWithSingleQuery([FromBody] Book model)
        {

           
           // appDBContext.Books.Update(model);
            appDBContext.Entry(model).State= Microsoft.EntityFrameworkCore.EntityState.Modified;
            await appDBContext.SaveChangesAsync();

            return Ok(model);
        }

        // Update multiple Record
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBookInBulk()
        {
           await appDBContext.Books
             .Where(x=> x.NoOfPages == 100) //update data using condition
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Description,p=> p.Title + " This is book description 2") 
           .SetProperty(p=> p.Title,p=> p.Title + " updated 2 ") // current value updated with new text as also keep old text
           //.SetProperty(p=> p.NoOfPages, 100)
           );
            return Ok();
        }

        // Delete Single Record
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookById([FromRoute] int bookId)
        {
            var book = new Book { Id = bookId};
            appDBContext.Entry(book).State = EntityState.Deleted;
            await appDBContext.SaveChangesAsync();

            /* var book = await appDBContext.Books.FindAsync(bookId);

             if (book == null)
             { 
                return NotFound();
             }
             appDBContext.Books.Remove(book);
             await appDBContext.SaveChangesAsync();*/
            return Ok();
        }

        // Delete Multiplt Record
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBookInBulkAsync()
        {
            /*var book = new Book { Id = bookId };
            appDBContext.Entry(book).State = EntityState.Deleted;
            await appDBContext.SaveChangesAsync();*/

            /* var book = await appDBContext.Books.Where(x => x.Id < 5).ToListAsync();
             appDBContext.Books.RemoveRange(book);
             await appDBContext.SaveChangesAsync();*/

            var book = await appDBContext.Books.Where(x => x.Id < 8).ExecuteDeleteAsync();
            return Ok();
        }
    }
}

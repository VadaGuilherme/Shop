using System.Xml.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get() =>
            new List<Category>();

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id) =>
            new Category();

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Post([FromBody]Category category)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(category);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> Put(int id, [FromBody] Category category) 
        {
            if(id != category.Id)
                return NotFound(new { message = "Categoria não encontrada" } );

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(category);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> Delete() =>
            Ok();
    }
}
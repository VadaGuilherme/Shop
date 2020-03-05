using System;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Category>>> Get(
            [FromServices]DataContext context)
        {
            var categories = await context.Categories
                .AsNoTracking()
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(
            int id,
            [FromServices]DataContext context)
        {
            var categories = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(categories);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Post(
            [FromBody]Category category,
            [FromServices]DataContext context)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
            
                return Ok(category);
            }
            catch
            {
                return BadRequest(new { messsage = "Não foi possivel criar a categoria" + "CategoryTitle" + category.Title });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Put(
            int id, 
            [FromBody] Category category,
            [FromServices]DataContext context) 
        {
            try
            {
                context.Entry<Category>(category).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado."});
            }
            catch(Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar a categoria. " + "CategoryId" + category.Id });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Delete(
            int id,
            [FromServices]DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category == null)
                return NotFound(new {message = "Categoria não encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();                
                return Ok(category);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria." + "CategoryId" + category.Id });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [ApiController]
  [Route("v1/categories")]

  public class CategoryController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<ActionResult<List<Category>>> Get(
      [FromServices] DataContext context)
    {
      var categories = await context.Categorias.AsNoTracking().ToListAsync();
      return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(
      int id,
      [FromServices] DataContext context)
    {
      var category = await context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      return category;
    }


    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Post(
      [FromServices] DataContext context,
      [FromBody] Category model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      try
      {
        context.Categorias.Add(model);
        await context.SaveChangesAsync();
        return model;
      }
      catch
      {
        return BadRequest(new { message = "Não foi possível criar a categoria!" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Category>>> Put(
      int id,
      [FromServices] DataContext context,
      [FromBody] Category model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      try
      {
        context.Entry<Category>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Categoria já foi atualizada!" });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível atualizar a categoria!" });
      }


    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Category>>> Delete(
      int id,
      [FromServices] DataContext context
    )
    {
      var category = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
      if (category == null)
        return NotFound(new { message = "Categoria não encontrada!" });

      try
      {
        context.Categorias.Remove(category);
        await context.SaveChangesAsync();
        return Ok(new { message = "Categoria removida com sucesso!" });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível remover a categoria!" });
      }
    }

  }
}

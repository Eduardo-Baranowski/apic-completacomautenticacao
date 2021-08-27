using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [ApiController]
  [Route("categories")]

  public class CategoryController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get(
      [FromServices] DataContext context)
    {
      var categories = await context.Categorias.AsNoTracking().ToListAsync();
      return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(
      int id,
      [FromServices] DataContext context)
    {
      var category = await context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      return category;
    }


    [HttpPost]
    [Route("")]
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

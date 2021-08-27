using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{

  [Route("products")]
  public class ProductController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Get(
      [FromServices] DataContext context)
    {
      var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
      return Ok(products);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> GetById(
      int id,
      [FromServices] DataContext context)
    {
      var product = await context.Products.AsNoTracking().Include(x => x.Id == id).FirstOrDefaultAsync(x => x.Id == id);
      return product;
    }


    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Product>> Post(
      [FromServices] DataContext context,
      [FromBody] Product model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      try
      {
        context.Products.Add(model);
        await context.SaveChangesAsync();
        return model;
      }
      catch
      {
        return BadRequest(new { message = "Não foi possível criar a produto!" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Product>>> Put(
      int id,
      [FromServices] DataContext context,
      [FromBody] Product model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      try
      {
        context.Entry<Product>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Produto já foi atualizada!" });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível atualizar a produto!" });
      }


    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Product>>> Delete(
      int id,
      [FromServices] DataContext context
    )
    {
      var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
      if (product == null)
        return NotFound(new { message = "Produto não encontrada!" });

      try
      {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return Ok(new { message = "Produto removida com sucesso!" });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível remover a produto!" });
      }
    }

  }
}

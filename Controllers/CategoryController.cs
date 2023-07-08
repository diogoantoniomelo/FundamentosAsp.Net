using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase

    {
        [HttpGet("v1/categories")]
        public IActionResult GetAsync(
            [FromServices] IMemoryCache cache,
            [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                });

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("05X04 - Falha interna no servidor"));
            }
        }

        private List<Category> GetCategories(BlogDataContext conext)
        {
            return conext.Categories.ToList();
        }


        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext conext)
        {
            try
            {
                var category = await conext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("2002 -Conteúdo não encontrado"));
                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("0002 - Falha interna no Servidor"));
            }
        }

        [HttpPost("v1/categories/")]
        public async Task<IActionResult> PostAsync(
           [FromBody] EditorCategoryViewModel model,
           [FromServices] BlogDataContext conext)
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            try
            {
                var category = new Category
                {
                    Id = 0,
                    Posts = null,
                    Name = model.Name,
                    Slug = model.Slug.ToLower(),
                };
                await conext.Categories.AddAsync(category);
                await conext.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("1003 - Não foi possível concluir a operação"));
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<Category>("0003 - Falha interna no Servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
           [FromRoute] int id,
           [FromBody] EditorCategoryViewModel model,
           [FromServices] BlogDataContext conext)
        {
            var category = await conext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new ResultViewModel<Category>("2004 -Conteúdo não encontrado"));
            try
            {
                category.Name = model.Name;
                category.Slug = model.Slug;

                conext.Categories.Update(category);
                await conext.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("1004 - Não foi possível concluir a operação"));
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<Category>("2005 - Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
          [FromRoute] int id,
          [FromServices] BlogDataContext conext)
        {
            var category = await conext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new ResultViewModel<Category>("2004 -Conteúdo não encontrado"));
            try
            {
                conext.Categories.Remove(category);
                await conext.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("1005 - Não foi possível concluir a operação"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("0006 - Falha interna no servidor"));
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E4_Meok_Khem_WebApi.Models;

namespace E4_Meok_Khem_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemModelsController : ControllerBase
    {
        private readonly ItemContext _context;

        public ItemModelsController(ItemContext context)
        {
            _context = context;
        }

        // GET: api/ItemModels
        [HttpGet]
       // public async Task<ActionResult<IEnumerable<ItemModel>>> GetTodoItems()
        public async Task<ActionResult<IEnumerable<ItemModelsDTO>>> GetTodoItems()
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            //return await _context.TodoItems.ToListAsync();
            return await _context.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();

        }

        // GET: api/ItemModels/5
        [HttpGet("{id}")]
       // public async Task<ActionResult<ItemModel>> GetItemModel(long id)
        public async Task<ActionResult<ItemModelsDTO>> GetItemModel(long id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var itemModel = await _context.TodoItems.FindAsync(id);

            if (itemModel == null)
            {
                return NotFound();
            }

            return ItemToDTO( itemModel);
        }

        // PUT: api/ItemModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutItemModel(long id, ItemModel itemModel)
        public async Task<IActionResult> PutItemModel(long id, ItemModelsDTO itemDTO)
        {
            if (id != itemDTO.Id)
            {
                return BadRequest();
            }
            var item = await _context.TodoItems.FindAsync(id);

            
            if(item == null)
            {
                return NotFound(itemDTO.Id);
            }

            item.Name= itemDTO.Name;
            item.Status= itemDTO.Status;
            //_context.Entry(itemModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItemModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemModelsDTO>> PostItemModel(ItemModelsDTO itemModel)
        {
          if (_context.TodoItems == null)
          {
              return Problem("Entity set 'ItemContext.TodoItems'  is null.");
          }

            //  _context.TodoItems.Add(itemModel);
            var Item = new ItemModel { Name = itemModel.Name, Status = itemModel.Status };
            _context.TodoItems.Add(Item);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetItemModel", new { id = itemModel.Id }, itemModel);
            return CreatedAtAction(nameof(GetItemModel), new { id = itemModel.Id},ItemToDTO(Item));
        }

        // DELETE: api/ItemModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemModel(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var itemModel = await _context.TodoItems.FindAsync(id);
            if (itemModel == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(itemModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemModelExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private static ItemModelsDTO ItemToDTO(ItemModel Item) =>
       new ItemModelsDTO
       {
           Id = Item.Id,
           Name = Item.Name,
           Status = Item.Status
       };
    }
}

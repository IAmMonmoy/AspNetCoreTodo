using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }
        public async Task<IActionResult> Index()
        {
            var todoItems = await _todoItemService.GetIncompleteItemsAsync();
            var model = new TodoViewModel()
            {
                Items = todoItems
            };
            return View(model);
        }

        public async Task<IActionResult> AddItem(NewTodoItem newItem)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sucessfull = await _todoItemService.AddItemAsync(newItem);

            if(!sucessfull)
            {
                return BadRequest(new {error = "Could not add item"} );
            }

            return Ok();
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if(id == Guid.Empty) return BadRequest();

            var sucessfull = await _todoItemService.MarkDoneAsync(id);

            if(!sucessfull) return BadRequest();

            return Ok();
        }
    }
}
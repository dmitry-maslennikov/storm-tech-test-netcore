using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.EntityModelMappers.TodoLists;
using Todo.Helpers;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoItemController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Create(int todoListId)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            var fields = TodoItemCreateFieldsFactory.Create(todoList, User.Id());
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemCreateFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            await CreateToDoItem(fields);

            return RedirectToListDetail(fields.TodoListId);
        }

        [HttpGet]
        public IActionResult Edit(int todoItemId)
        {
            var todoItem = dbContext.SingleTodoItem(todoItemId);
            var fields = TodoItemEditFieldsFactory.Create(todoItem);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoItemEditFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var todoItem = dbContext.SingleTodoItem(fields.TodoItemId);

            TodoItemEditFieldsFactory.Update(fields, todoItem);

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();

            return RedirectToListDetail(todoItem.TodoListId);
        }

        [HttpPost]
        public async Task<IActionResult> ApiCreate(TodoItemCreateFields fields, bool orderByRank, bool hideDoneItems)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // In a real-world scenario, we should check if the logged-in user is eligible to create to-do items in the to-do list (the user might not have access to the list), but we'll omit that check in the test task for brevity.
            // For now, just create the item.
            await CreateToDoItem(fields);

            // Then render the partial view and return as string
            var todoList = dbContext.SingleTodoList(fields.TodoListId);
            var viewmodel = TodoListDetailViewmodelFactory.Create(todoList, orderByRank, User.Id(), hideDoneItems);

            var viewAsString = await RazorHelper.RenderViewAsync<TodoListDetailViewmodel>(this, "_DetailsToDo", viewmodel, true);

            return Ok(viewAsString);
        }

        private RedirectToActionResult RedirectToListDetail(int fieldsTodoListId)
        {
            return RedirectToAction("Detail", "TodoList", new {todoListId = fieldsTodoListId});
        }

        private async Task CreateToDoItem(TodoItemCreateFields fields)
        {
            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance, fields.Rank);

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
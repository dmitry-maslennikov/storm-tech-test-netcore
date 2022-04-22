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

            await CreateTodoItem(fields);

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

            TodoItem todoItem = await UpdateTodoItem(fields);
            return RedirectToListDetail(todoItem.TodoListId);
        }

        [HttpPost]
        public async Task<IActionResult> ApiCreate(TodoItemCreateFields fields, bool orderByRank, bool hideDoneItems)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // In a real-world scenario, we should check if the logged-in user is eligible to create to-do items in the to-do list (the user might not have access to the list), but we'll omit that check in the test task for brevity.            
            await CreateTodoItem(fields); // For now, just create the item.            
            string viewAsString = await RenderTodoListDetailsPartial(fields.TodoListId, orderByRank, hideDoneItems); // Then render the partial view and return as string
            return Ok(viewAsString);
        }

        [HttpPost]
        public async Task<IActionResult> ApiUpdateRank(TodoItemEditFields fields, bool orderByRank, bool hideDoneItems)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // In a real-world scenario, we should check if the logged-in user is eligible to edit to-do items in the to-do list (the user might not have access to the list), but we'll omit that check in the test task for brevity.            
            TodoItem todoItem = await UpdateTodoItem(fields, true); // For now, just update the item.
            string viewAsString = await RenderTodoListDetailsPartial(todoItem.TodoListId, orderByRank, hideDoneItems);  // Then render the partial view and return as string
            return Ok(viewAsString);
        }

        [HttpGet]
        public async Task<IActionResult> ApiReorder(int todoListId, bool orderByRank, bool hideDoneItems)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // In a real-world scenario, we should check if the logged-in user is eligible to read to-do items in the to-do list (the user might not have access to the list), but we'll omit that check in the test task for brevity.            
            string viewAsString = await RenderTodoListDetailsPartial(todoListId, orderByRank, hideDoneItems);  // Render the partial view and return as string
            return Ok(viewAsString);
        }

        private RedirectToActionResult RedirectToListDetail(int fieldsTodoListId)
        {
            return RedirectToAction("Detail", "TodoList", new {todoListId = fieldsTodoListId});
        }

        private async Task CreateTodoItem(TodoItemCreateFields fields)
        {
            var item = new TodoItem(fields.TodoListId, fields.ResponsiblePartyId, fields.Title, fields.Importance, fields.Rank);

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }

        private async Task<string> RenderTodoListDetailsPartial(int todoListId, bool orderByRank, bool hideDoneItems)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            var viewmodel = TodoListDetailViewmodelFactory.Create(todoList, orderByRank, User.Id(), hideDoneItems);
            var viewAsString = await RazorHelper.RenderViewAsync<TodoListDetailViewmodel>(this, "_DetailsToDo", viewmodel, true);
            return viewAsString;
        }

        private async Task<TodoItem> UpdateTodoItem(TodoItemEditFields fields, bool updateRankOnly = false)
        {
            var todoItem = dbContext.SingleTodoItem(fields.TodoItemId);

            if (updateRankOnly)
                TodoItemEditFieldsFactory.Update(fields.Rank, todoItem);
            else
                TodoItemEditFieldsFactory.Update(fields, todoItem);

            dbContext.Update(todoItem);
            await dbContext.SaveChangesAsync();
            return todoItem;
        }
    }
}
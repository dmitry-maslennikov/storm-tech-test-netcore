using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Services
{
    public static class ApplicationDbContextConvenience
    {
        public static IQueryable<TodoList> RelevantTodoLists(this ApplicationDbContext dbContext, string userId)
        {
            var query1 = dbContext.TodoLists.Where(tl => tl.Owner.Id == userId);

            var query2 = from l in dbContext.TodoLists
                         join i in dbContext.TodoItems on l.TodoListId equals i.TodoListId
                         where i.ResponsiblePartyId == userId && l.Owner.Id != userId
                         select l;

            return query1.Union(query2)
                         .Include(tl => tl.Owner)
                         .Include(tl => tl.Items);
                
        }

        public static TodoList SingleTodoList(this ApplicationDbContext dbContext, int todoListId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .ThenInclude(ti => ti.ResponsibleParty)
                .Single(tl => tl.TodoListId == todoListId);
        }

        public static TodoItem SingleTodoItem(this ApplicationDbContext dbContext, int todoItemId)
        {
            return dbContext.TodoItems.Include(ti => ti.TodoList).Single(ti => ti.TodoItemId == todoItemId);
        }
    }
}
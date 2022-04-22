using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool orderByRank, string defaultResponsibleUserId, bool hideDoneItems = false)
        {
            var createFields = new TodoItemCreateFields(todoList.TodoListId, todoList.Title, defaultResponsibleUserId);
            var items = todoList.Items.Select(TodoItemSummaryViewmodelFactory.Create).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, orderByRank, createFields, hideDoneItems);
        }
    }
}
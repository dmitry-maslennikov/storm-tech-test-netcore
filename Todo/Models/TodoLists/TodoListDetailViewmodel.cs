using System.Collections.Generic;
using System.Linq;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public bool OrderByRank { get; set; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool orderByRank)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            OrderByRank = orderByRank;
        }

        public IEnumerable<TodoItemSummaryViewmodel> GetItemsSorted()
        {
            var orderedQuery = (OrderByRank) ? Items.OrderByDescending(i => i.Rank).ThenBy(i => (int)i.Importance) 
                                             : Items.OrderBy(i => (int)i.Importance).ThenByDescending(i => i.Rank);
            return orderedQuery.ToList();
        }

        public bool HasDoneItems()
        {
            return Items.Any(i => i.IsDone);
        }
    }
}
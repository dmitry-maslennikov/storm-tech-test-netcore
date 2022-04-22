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
        public TodoItemCreateFields CreateFields { get; set; }
        public bool HideDoneItems { get; set; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool orderByRank, TodoItemCreateFields createFields, bool hideDoneItems)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            OrderByRank = orderByRank;
            CreateFields = createFields;
            HideDoneItems = hideDoneItems;
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

        public IEnumerable<string> GetDistinctGravatarHashes(string[] excludeList = null)
        {
            var query = Items.Select(i => i.ResponsibleParty.GravatarHash).Distinct();
            if (excludeList != null)
                query = query.Except(excludeList);
            return query.ToList();
        }
    }
}
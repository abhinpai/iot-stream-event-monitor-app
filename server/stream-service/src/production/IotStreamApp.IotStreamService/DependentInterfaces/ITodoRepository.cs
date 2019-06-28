// ************************************************************************
// *****      COPYRIGHT 2014 - 2018 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace Honeywell.IotStreamApp.IotStreamService.DependentInterfaces
{
    using System;
    using System.Collections.Generic;

    public interface ITodoRepository
    {
        TodoItem GetItem(Guid id);

        IEnumerable<TodoItem> GetAllItems();

        bool AddItem(TodoItem item);

        bool RemoveItem(Guid id, out TodoItem removedItem);
    }
}
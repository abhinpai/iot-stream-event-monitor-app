// ************************************************************************
// *****      COPYRIGHT 2014 - 2018 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace Honeywell.IotStreamApp.IotStreamService
{
    using System;

    public interface ITodoService
    {
        GetResults GetAll();

        AddResult Add(string title, string description);

        UpdateResult Update(Guid id, string title, string description, bool? isFlagged);

        DeleteResult Delete(Guid id);

        GetResult GetById(Guid id);
    }
}
using System;
using System.Linq;

namespace TimeXv2.Model.Data
{
    public interface IDataService
    {
        Guid? AddAction(Action value);
        bool DeleteAction(Guid uid);
        Action GetActionByUid(Guid uid);
        IQueryable<Action> QueryableActions();
        bool UpdateAction(Action value);
    }
}

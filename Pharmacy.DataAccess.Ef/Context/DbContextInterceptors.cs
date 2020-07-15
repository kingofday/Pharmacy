using Elk.Core;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Pharmacy.DataAccess.Ef
{
    public class DbContextInterceptors : DbCommandInterceptor
    {
        //public override InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
        //{
        //    result.Result.ApplyCorrectPersianCharacters();
        //    result.Result.ApplyPersianToEnglishNumeric();

        //    return base.CommandCreating(eventData, result);
        //}

        //public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        //{
        //    result.ApplyCorrectPersianCharacters();
        //    result.ApplyPersianToEnglishNumeric();

        //    return base.CommandCreated(eventData, result);
        //}

        //public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        //{
        //    command.ApplyCorrectPersianCharacters();
        //    command.ApplyPersianToEnglishNumeric();

        //    return base.NonQueryExecuted(command, eventData, result);
        //}

        //public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        //{
        //    command.ApplyCorrectPersianCharacters();
        //    command.ApplyPersianToEnglishNumeric();

        //    return base.ReaderExecuted(command, eventData, result);
        //}

        //public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        //{
        //    command.ApplyCorrectPersianCharacters();
        //    command.ApplyPersianToEnglishNumeric();

        //    return base.ReaderExecuting(command, eventData, result);
        //}
    }
}

using Candidate.Core.Services.Interfaces;
using Candidate.Data.Context;
using Candidate.Data.Models;
using Candidate.Data.Repositories;
using IdentityServer4.Models;
using System.Reflection;
using Candidate.Core.Presentations.Base;
using Candidate.Core.Resources;
using Candidate.Core.Widgets.Log;
using Candidate.Core.Widgets.Method;
using Candidate.OvertimePolicies.Factories;
using Candidate.OvertimePolicies.Interfaces;

namespace Candidate.Core.Services;

public class PersonService:Repository<Person>, IPersonService
{
    private readonly IOvertimeCalculatorFactory _factory;
    private readonly ILogWidget _log;
    public PersonService(DatabaseContext context, IOvertimeCalculatorFactory factory, ILogWidget log) : base(context)
    {
        _factory = factory;
        _log = log;
    }
    public  MessageViewModel Add(Person entity,string overTimeCalculator)
    {
        MessageViewModel result;
        List<ErrorViewModel> errors = new List<ErrorViewModel>();
        try
        {
            try
            {

                entity.TotalSalary = CalculateTotalSalary(entity, overTimeCalculator);

                Create(entity);
                Save();
                result = new MessageViewModel()
                {
                    ID = entity.ID,
                    Status = Statuses.Success,
                    Title = Titles.Save,
                    Message = string.Format(Messages.SaveSuccessedWithID, entity.ID),
                    Errors = errors,
                    Value = ""
                };
                return result;
            }
            catch (Exception ex)
            {
                
                errors.Add(new ErrorViewModel()
                {
                    ErrorCode = "100",
                    ErrorMessage = Errors.ExceptionInRepository
                });
                errors.Add(new ErrorViewModel()
                {
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMessage = _log.GetExceptionMessage(ex)
                });
                result = new MessageViewModel()
                {
                    ID = -1,
                    Status = Statuses.Error,
                    Title = Titles.Error,
                    Message = Messages.SaveFailed,
                    Errors = errors,
                    Value = ""
                };
                return result;
            }
        }
        catch (Exception ex)
        {
            _log.ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName(), 1);

            result = new MessageViewModel()
            {
                ID = -1,
                Status = Statuses.Error,
                Title = Titles.Exception,
                Message = Messages.UnknownException,
                Value = _log.GetExceptionMessage(ex)
            };

            return result;
        }
    }

    public MessageViewModel Edit(Person entity, string overTimeCalculator)
    {
        MessageViewModel result;
        List<ErrorViewModel> errors = new List<ErrorViewModel>();
        try
        {
            var exist = Exist(entity.ID);
            if (exist)
            {
                try
                {
                   entity.TotalSalary= CalculateTotalSalary(entity, overTimeCalculator);

                    Update(entity);
                    Save();
                    result = new MessageViewModel()
                    {
                        ID = entity.ID,
                        Status = Statuses.Success,
                        Title = Titles.Update,
                        Message = Messages.UpdateSuccessed,
                        Errors = null,
                        Value = ""
                    };
                    return result;
                }
                catch (Exception ex)
                {
                    _log.ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName());
                    errors.Add(new ErrorViewModel()
                    {
                        ErrorCode = "100",
                        ErrorMessage = Errors.ExceptionInRepository
                    });
                    errors.Add(new ErrorViewModel()
                    {
                        ErrorCode = ex.HResult.ToString(),
                        ErrorMessage = _log.GetExceptionMessage(ex)
                    });
                    result = new MessageViewModel()
                    {
                        ID = -1,
                        Status = Statuses.Error,
                        Title = Titles.Error,
                        Message = Messages.UpdateFailed,
                        Errors = errors,
                        Value = ""
                    };
                    return result;
                }
            }
            else
            {
                errors.Add(new ErrorViewModel()
                {
                    ErrorCode = "101",
                    ErrorMessage = Errors.RecordNotFoundError
                });
                result = new MessageViewModel()
                {
                    ID = -1,
                    Status = Statuses.Error,
                    Title = Titles.Error,
                    Message = Messages.UpdateFailed,
                    Errors = errors,
                    Value = ""
                };
                return result;
            }
        }
        catch (Exception ex)
        {
            _log.ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName(), 1);
            errors.Add(new ErrorViewModel()
            {
                ErrorCode = "100",
                ErrorMessage = Errors.ExceptionInRepository
            });
            errors.Add(new ErrorViewModel()
            {
                ErrorCode = ex.HResult.ToString(),
                ErrorMessage = _log.GetExceptionMessage(ex)
            });
            result = new MessageViewModel()
            {
                ID = -1,
                Status = Statuses.Error,
                Title = Titles.Exception,
                Message = Messages.UnknownException,
                Errors = errors,
                Value = ""
            };
            return result;
        }
    }

    private double CalculateTotalSalary(Person entity, string overTimeCalculator)
    {
        IOvertimeCalculator calculator = _factory.CreateOvertimeCalculator(overTimeCalculator);
        var overTime = calculator.CalculateOvertime(entity.BasicSalary, entity.Allowance, entity.hoursWorked);

        var totalSalary = entity.BasicSalary + entity.Allowance + entity.Transportation + overTime;
        var tax = totalSalary * 10 / 100;
        return totalSalary - tax;
    }

    public  MessageViewModel Remove(int id, bool hardDelete = false)
    {
        MessageViewModel result;
        List<ErrorViewModel> errors = new List<ErrorViewModel>();
        try
        {
            var exist = Exist(id);
            if (exist)
            {
                try
                {
                    Delete(id, hardDelete);
                    Save();
                    result = new MessageViewModel()
                    {
                        ID = id,
                        Status = Statuses.Success,
                        Title = Titles.Remove,
                        Message = Messages.RemoveSuccessed,
                        Value = ""
                    };
                    return result;
                }
                catch (Exception ex)
                {
                    errors.Add(new ErrorViewModel()
                    {
                        ErrorCode = "100",
                        ErrorMessage = Errors.ExceptionInRepository
                    });
                    errors.Add(new ErrorViewModel()
                    {
                        ErrorCode = ex.HResult.ToString(),
                        ErrorMessage = _log.GetExceptionMessage(ex)
                    });
                    result = new MessageViewModel()
                    {
                        ID = id,
                        Status = Statuses.Error,
                        Title = Titles.Error,
                        Message = Messages.RemoveFailed,
                        Errors = errors,
                        Value = ""
                    };
                    return result;
                }
            }
            else
            {
                errors.Add(new ErrorViewModel()
                {
                    ErrorCode = "101",
                    ErrorMessage = Errors.RecordNotFoundError
                });
                result = new MessageViewModel()
                {
                    ID = -1,
                    Status = Statuses.Error,
                    Title = Titles.Error,
                    Message = Messages.RemoveFailed,
                    Errors = errors,
                    Value = ""
                };
                return result;
            }
        }
        catch (Exception ex)
        {
            _log.ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName(), 1);

            errors.Add(new ErrorViewModel()
            {
                ErrorCode = "100",
                ErrorMessage = Errors.ExceptionInRepository
            });
            errors.Add(new ErrorViewModel()
            {
                ErrorCode = ex.HResult.ToString(),
                ErrorMessage = _log.GetExceptionMessage(ex)
            });
            result = new MessageViewModel()
            {
                ID = -1,
                Status = Statuses.Error,
                Title = Titles.Exception,
                Message = Messages.UnknownException,
                Errors = errors,
                Value = ""
            };

            return result;
        }
    }

    public Person Get(int personCode, string date)
    {
        throw new NotImplementedException();
    }

    public List<Person> GetRange(int personCode, int startDate, int endDate)
    {
        throw new NotImplementedException();
    }
}
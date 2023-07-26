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
using System.Linq.Expressions;
using Candidate.Core.Widgets.Dapper;
using Candidate.Core.Widgets.Calendar;
using Dapper;
using System;
using AutoMapper;
using Candidate.Core.Presentations.Persons;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Formats.Asn1;
using System.Xml.Serialization;
using Candidate.Core.Widgets.Convertor;
using CsvHelper;

namespace Candidate.Core.Services;

public class PersonService : Repository<Person>, IPersonService
{
    private readonly IOvertimeCalculatorFactory _factory;
    private readonly ILogWidget _log;
    private readonly IDapperWidget _dapper;
    private readonly IMapper _mapper;
    public PersonService(DatabaseContext context, IOvertimeCalculatorFactory factory, ILogWidget log, IDapperWidget dapper, IMapper mapper) : base(context)
    {
        _factory = factory;
        _log = log;
        _dapper = dapper;
        _mapper = mapper;
    }
    public MessageViewModel Add(Person entity, string overTimeCalculator)
    {
        MessageViewModel result;
        var errors = new List<ErrorViewModel>();
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
        var errors = new List<ErrorViewModel>();
        try
        {
            var exist = Exist(entity.ID);
            if (exist)
            {
                try
                {
                    entity.TotalSalary = CalculateTotalSalary(entity, overTimeCalculator);

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
        var calculator = _factory.CreateOvertimeCalculator(overTimeCalculator);
        var overTime = calculator.CalculateOvertime(entity.BasicSalary, entity.Allowance, entity.hoursWorked);

        var totalSalary = entity.BasicSalary + entity.Allowance + entity.Transportation + overTime;
        var tax = totalSalary * 10 / 100;
        return totalSalary - tax;
    }

    public MessageViewModel Remove(int id, bool hardDelete = false)
    {
        MessageViewModel result;
        var errors = new List<ErrorViewModel>();
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

    public ResultViewModel<PersonViewModel> Get(int personCode, string date)
    {
        var result = new ResultViewModel<PersonViewModel>();
        try
        {
            var query = "Select * from People where personCode=@personCode and Date=@date";
            var parameters = new DynamicParameters();
            parameters.Add("@personCode", personCode);
            parameters.Add("@date", date);

            var person = _dapper.RunQuery<Person>(query, parameters, "CandidateDB").FirstOrDefault();

            result.Result = _mapper.Map<PersonViewModel>(person);



            result.Message = result.TotalCount > 0
                ? new MessageViewModel { Status = Statuses.Success }
                : new MessageViewModel { Status = Statuses.Warning, Message = Messages.NotFoundAnyRecords };
            return result;
        }
        catch (Exception ex)
        {
            _log.ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName());
            result.Message = new MessageViewModel { Status = Statuses.Error, Message = _log.GetExceptionMessage(ex) };
            return result;
        }

    }

    public ResultViewModel<PersonViewModel> GetRange(int personCode, int startDate, int endDate)
    {
        var result = new ResultViewModel<PersonViewModel>();
        try
        {
            var query = "Select * from People where personCode=@personCode And Date between @fromDate and @toDate";
            var parameters = new DynamicParameters();
            parameters.Add("@personCode", personCode);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            var person = _dapper.RunQuery<Person>(query, parameters, "CandidateDB");

            result.List = _mapper.Map<List<PersonViewModel>>(person);



            result.Message = result.TotalCount > 0
                ? new MessageViewModel { Status = Statuses.Success }
                : new MessageViewModel { Status = Statuses.Warning, Message = Messages.NotFoundAnyRecords };
            return result;
        }
        catch (Exception ex)
        {
            _log.ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName());
            result.Message = new MessageViewModel { Status = Statuses.Error, Message = _log.GetExceptionMessage(ex) };
            return result;
        }
    }

    public ResultViewModel<Person> ParseData(string datatype, InputViewModel data)
    {
        ResultViewModel<Person> result = new ResultViewModel<Person>();
        var errors = new List<ErrorViewModel>();
        try
        {
            switch (datatype.ToLower())
            {

                case "json":
                    // Parse JSON data
                    try
                    {
                        result.Result = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(data.Data);
                        // انجام عملیات با داده‌های jsonData
                        result.Message = new MessageViewModel()
                        {
                            ID = 0,
                            Status = Statuses.Success,
                            Title = Titles.Parse,
                            Message = Messages.ParsedSuccesses,
                            Errors = errors,
                            Value = ""
                        };
                        return result;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = ex.HResult.ToString(),
                            ErrorMessage = _log.GetExceptionMessage(ex)
                        });
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = "102",
                            ErrorMessage = Messages.InvalidJSON
                        });
                        result.Message = new MessageViewModel()
                        {
                            ID = -1,
                            Status = Statuses.Error,
                            Title = Titles.Exception,
                            Message = Messages.ParseFaild,
                            Errors = errors,
                            Value = ""
                        };
                        return result;
                    }

                case "xml":
                    // Parse XML data
                    try
                    {
                        var serializer = new XmlSerializer(typeof(Person));
                        using (var reader = new StringReader(data.Data))
                        {
                            result.Result = (Person)serializer.Deserialize(reader);
                            // انجام عملیات با داده‌های xmlData
                            result.Message = new MessageViewModel()
                            {
                                ID = 0,
                                Status = Statuses.Success,
                                Title = Titles.Parse,
                                Message = Messages.ParsedSuccesses,
                                Errors = errors,
                                Value = ""
                            };
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = ex.HResult.ToString(),
                            ErrorMessage = _log.GetExceptionMessage(ex)
                        });
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = "103",
                            ErrorMessage = Messages.InvalidXML
                        });
                        result.Message = new MessageViewModel()
                        {
                            ID = -1,
                            Status = Statuses.Error,
                            Title = Titles.Exception,
                            Message = Messages.ParseFaild,
                            Errors = errors,
                            Value = ""
                        };
                        return result;
                    }

                case "csv":
                    // Parse CSV data
                    try
                    {
                        using var reader = new StringReader(data.Data);
                        using var csv = new CsvReader(reader,
                            new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo
                                .InvariantCulture));
                        result.Result = csv.GetRecords<Person>().FirstOrDefault();
                        // انجام عملیات با داده‌های csvData
                        result.Message = new MessageViewModel()
                        {
                            ID = 0,
                            Status = Statuses.Success,
                            Title = Titles.Parse,
                            Message = Messages.ParsedSuccesses,
                            Errors = errors,
                            Value = ""
                        };
                        return result;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = ex.HResult.ToString(),
                            ErrorMessage = _log.GetExceptionMessage(ex)
                        });
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = "104",
                            ErrorMessage = Messages.InvalidCsv
                        });
                        result.Message = new MessageViewModel()
                        {
                            ID = -1,
                            Status = Statuses.Error,
                            Title = Titles.Exception,
                            Message = Messages.ParseFaild,
                            Errors = errors,
                            Value = ""
                        };
                        return result;
                    }
                case "custom":
                    // Parse custom data
                    try
                    {
                        var splitData = data.Data.Split('/');
                        Person person = new Person
                        {
                            FirstName = splitData[0],
                            LastName = splitData[1],
                            BasicSalary = splitData[2].ToLong(),
                            Allowance = splitData[3].ToLong(),
                            Transportation = splitData[4].ToLong(),
                            Date = splitData[5],
                            hoursWorked = splitData[6].ToInt()

                        };
                        result.Result = person;
                        // انجام عملیات با داده‌های csvData
                        result.Message = new MessageViewModel()
                        {
                            ID = 0,
                            Status = Statuses.Success,
                            Title = Titles.Parse,
                            Message = Messages.ParsedSuccesses,
                            Errors = errors,
                            Value = ""
                        };
                        return result;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = ex.HResult.ToString(),
                            ErrorMessage = _log.GetExceptionMessage(ex)
                        });
                        errors.Add(new ErrorViewModel()
                        {
                            ErrorCode = "104",
                            ErrorMessage = Messages.InvalidCustom
                        });
                        result.Message = new MessageViewModel()
                        {
                            ID = -1,
                            Status = Statuses.Error,
                            Title = Titles.Exception,
                            Message = Messages.ParseFaild,
                            Errors = errors,
                            Value = ""
                        };
                        return result;
                    }


                default:

                    errors.Add(new ErrorViewModel()
                    {
                        ErrorCode = "106",
                        ErrorMessage = Messages.InvalidDataType
                    });
                    result.Message = new MessageViewModel()
                    {
                        ID = -1,
                        Status = Statuses.Error,
                        Title = Titles.Exception,
                        Message = Messages.ParseFaild,
                        Errors = errors,
                        Value = ""
                    };
                    return result;
            }
        }
        catch (Exception ex)
        {
            _log.ExceptionLog(ex, MethodBase.GetCurrentMethod()?.GetSourceName());
            result.Message = new MessageViewModel { Status = Statuses.Error, Message = _log.GetExceptionMessage(ex) };
            return result;
        }
    }
}
using AutoMapper;
using Project.Domain.Aggregates.User;
using Project.Infrastructure.Utils.Exceptions;
using Project.Web.Responses.Shared;
using Project.Web.ViewModels;
using System;
using System.Collections.Generic;

namespace Project.Utils
{
    public class MappingProfile : Profile
    {
        private static Error MapExceptionToError(Exception e)
        {
            var errorDictionary = new Dictionary<Type, Error>()
            {
                { 
                    typeof(ExpiredAccessTokenException), new Error()
                    {
                        Code = $"{Error.Auth}-001",
                        Description = "Your session has expired, please re-authenticate to continue",
                    }
                },
                {
                    typeof(ArgumentException), new Error()
                    {
                        Code = $"{Error.Request}-001",
                        Description = "Bad request, please review the parameters"
                    }
                }
            };
            var exceptionType = e.GetType();
            if (!errorDictionary.ContainsKey(exceptionType))
            {
                return new Error()
                {
                    Code = $"{Error.System}-001",
                    Description = $"Unknown error (exception type: {exceptionType})"
                };
            }
            return errorDictionary[exceptionType];
        }
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>();
        }

    }
}

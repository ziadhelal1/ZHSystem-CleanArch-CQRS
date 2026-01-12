using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Extensions
{
    public static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> ApplyIgnoreList<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map)
        {
            return map
                .ForMember("Id", opt => opt.Ignore())
                .ForMember("PasswordHash", opt => opt.Ignore())
                .ForMember("UserRoles", opt => opt.Ignore())
                .ForMember("isActive", opt => opt.Ignore())
                .ForMember("EmailVerified", opt => opt.Ignore())
                .ForMember("EmailVerificationTokenHash", opt => opt.Ignore())
                .ForMember("EmailVerificationExpires", opt => opt.Ignore())
                .ForMember("PasswordResetTokenHash", opt => opt.Ignore())
                .ForMember("PasswordResetExpires", opt => opt.Ignore())
                .ForMember("LastSecurityEmailSentAt", opt => opt.Ignore())
                .IgnoreSoftDeleteProperties();
        }

        public static IMappingExpression<TSource, TDestination> IgnoreSoftDeleteProperties<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map)
        {
            return map
                .ForMember("IsDeleted", opt => opt.Ignore())
                .ForMember("DeletedAt", opt => opt.Ignore());
        }
    }
}

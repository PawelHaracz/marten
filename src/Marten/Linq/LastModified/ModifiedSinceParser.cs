using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Baseline;
using Marten.Linq.Parsing;
using Marten.Schema;

namespace Marten.Linq.LastModified
{
    public class ModifiedSinceParser : IMethodCallParser
    {
        private static readonly MethodInfo _method =
            typeof(LastModifiedExtensions).GetMethod(nameof(LastModifiedExtensions.ModifiedSince));

        public bool Matches(MethodCallExpression expression)
        {
            return Equals(expression.Method, _method);
        }

        public IWhereFragment Parse(IQueryableDocument mapping, ISerializer serializer, MethodCallExpression expression)
        {
            var time = expression.Arguments.Last().Value().As<DateTimeOffset>();

            return new WhereFragment($"d.{DocumentMapping.LastModifiedColumn} > ?", time);
        }
    }
}
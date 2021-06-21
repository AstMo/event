using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PartyMaker.Common.Filter
{
    public static class FilterBuilder
    {
        private static readonly Regex DateFilterExpression = new Regex(@"^(?<from>(?<years>\d{4})([-:\.])?(?<months>\d{2})([-:\.])?(?<days>\d{2})(?i)T(?<hours>\d{2})([-:\.])?(?<minutes>\d{2})([-:\.])?((?<seconds>\d{2})(\.[0-9]+)?)((?<fromUtc>(?i)Z)|((?<fromZoneOffsetSign>\-|\+)(?<fromZoneOffsetHours>\d{2})([-:\.])?(?<fromZoneOffsetMinutes>\d{2}))))?[\s]*-[\s]*(?<to>(?<years>\d{4})([-:\.])?(?<months>\d{2})([-:\.])?(?<days>\d{2})(?i)T(?<hours>\d{2})([-:\.])?(?<minutes>\d{2})([-:\.])?((?<seconds>\d{2})(\.[0-9]+)?)((?<toUtc>(?i)Z)|((?<toZoneOffsetSign>\-|\+)(?<toZoneOffsetHours>\d{2})([-:\.])?(?<toZoneOffsetMinutes>\d{2}))))?$");

        public static Expression<Func<T, bool>> Build<T>(KeyValuePair<string, string>[] filters, IList<KeyValuePair<string, IList<string>>> additionalOrArguments = null)
        {
            Expression<Func<T, bool>> filterExpression = null;
            var targetTypeProps = typeof(T).GetProperties();
            var filterExpressions = new List<Expression>();
            var orFilterExpressions = new List<Expression>();
            var exprParam = Expression.Parameter(typeof(T), "x");
            foreach (var filter in filters)
            {
                var prop = targetTypeProps.FirstOrDefault(x => string.Equals(x.Name, filter.Key, StringComparison.InvariantCultureIgnoreCase));
                if (prop != null && filter.Value != null)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        filterExpressions.Add(GetStringFilterExpression(exprParam, prop, filter.Value));
                    }
                    else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        filterExpressions.Add(GetDateRangeFilterExpression(exprParam, prop, filter.Value));
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        filterExpressions.Add(GetEnumFilter(exprParam, prop, filter.Value));
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        filterExpressions.Add(GetBoolFilter(exprParam, prop, filter.Value));
                    }
                    else if (prop.PropertyType == typeof(Guid) && Guid.TryParse(filter.Value, out Guid testGuid))
                    {
                        filterExpressions.Add(GetGuidFilterExpression(exprParam, prop, filter.Value));
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        filterExpressions.Add(GetIntFilter(exprParam, prop, filter.Value));
                    }
                }
            }

            Expression finalFilter = null;
            foreach (var filter in filterExpressions)
            {
                finalFilter = finalFilter == null ? filter : Expression.AndAlso(finalFilter, filter);
            }

            if (additionalOrArguments != null)
            {

                foreach (var filterFields in additionalOrArguments)
                {
                    Expression orExpression = null;
                    orFilterExpressions.Clear();
                    var prop = targetTypeProps.FirstOrDefault(x => string.Equals(x.Name, filterFields.Key, StringComparison.InvariantCultureIgnoreCase));
                    foreach (var filterValue in filterFields.Value)
                    {
                        if (prop != null && filterValue != null)
                        {
                            if (prop.PropertyType == typeof(string))
                            {
                                orFilterExpressions.Add(GetStringFilterExpression(exprParam, prop, filterValue));
                            }
                            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                            {
                                orFilterExpressions.Add(GetDateRangeFilterExpression(exprParam, prop, filterValue));
                            }
                            else if (prop.PropertyType.IsEnum)
                            {
                                orFilterExpressions.Add(GetEnumFilter(exprParam, prop, filterValue));
                            }
                            else if (prop.PropertyType == typeof(bool))
                            {
                                orFilterExpressions.Add(GetBoolFilter(exprParam, prop, filterValue));
                            }
                            else if (prop.PropertyType == typeof(Guid) && Guid.TryParse(filterValue, out Guid testGuid))
                            {
                                orFilterExpressions.Add(GetGuidFilterExpression(exprParam, prop, filterValue));
                            }
                            else if (prop.PropertyType == typeof(int))
                            {
                                orFilterExpressions.Add(GetIntFilter(exprParam, prop, filterValue));
                            }
                        }
                    }

                    foreach (var filter in orFilterExpressions)
                    {
                        orExpression = orExpression == null ? filter : Expression.OrElse(orExpression, filter);
                    }

                    finalFilter = finalFilter == null ? orExpression : Expression.AndAlso(orExpression, finalFilter);
                }
            }

            if (finalFilter != null)
            {
                filterExpression = Expression.Lambda<Func<T, bool>>(finalFilter, exprParam);
            }

            return filterExpression;
        }

        private static Expression GetStringFilterExpression(ParameterExpression exprParam, PropertyInfo property, string filterValue)
        {
            // string.Contains
            var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

            // x.InboundDelivery
            var propertyGetter = Expression.Property(exprParam, property);

            ConstantExpression targetValueExpression;
            // context.InboundDeliveryFilter
            if (filterValue == string.Empty)
            {
                var nullValueExpression = Expression.Constant(null);
                //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
                return Expression.Equal(propertyGetter, nullValueExpression);
            }
            else
            {
                targetValueExpression = Expression.Constant($"%{filterValue}%");
                //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
                return Expression.Call(propertyGetter, containsMethodInfo, targetValueExpression);
            }
        }

        private static Expression GetGuidFilterExpression(ParameterExpression exprParam, PropertyInfo property, string filterValue)
        {
            // x.InboundDelivery
            var propertyGetter = Expression.Property(exprParam, property);

            ConstantExpression targetValueExpression;
            // context.InboundDeliveryFilter
            if (filterValue == string.Empty)
            {
                var nullValueExpression = Expression.Constant(null);
                //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
                return Expression.Equal(propertyGetter, nullValueExpression);
            }
            else
            {
                targetValueExpression = Expression.Constant(Guid.Parse(filterValue));
                //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
                return Expression.Equal(propertyGetter, targetValueExpression);
            }
        }

        private static Expression GetEnumFilter(ParameterExpression exprParam, PropertyInfo property, string filterValue)
        {
            // x.InboundDelivery
            var propertyGetter = Expression.Property(exprParam, property);

            ConstantExpression targetValueExpression;
            // context.InboundDeliveryFilter
            targetValueExpression = Expression.Constant(Enum.ToObject(property.PropertyType, int.Parse(filterValue)));
            //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
            return Expression.Equal(propertyGetter, targetValueExpression);
        }

        private static Expression GetIntFilter(ParameterExpression exprParam, PropertyInfo property, string filterValue)
        {
            // x.InboundDelivery
            var propertyGetter = Expression.Property(exprParam, property);
            ConstantExpression targetValueExpression;
            if (filterValue == string.Empty)
            {
                var nullValueExpression = Expression.Constant(0);
                //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
                return Expression.Equal(propertyGetter, nullValueExpression);
            }
            else
            {
                // context.InboundDeliveryFilter
                targetValueExpression = Expression.Constant(int.Parse(filterValue));
            }
            //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
            return Expression.Equal(propertyGetter, targetValueExpression);
        }

        private static Expression GetBoolFilter(ParameterExpression exprParam, PropertyInfo property, string filterValue)
        {
            // x.InboundDelivery
            var propertyGetter = Expression.Property(exprParam, property);

            ConstantExpression targetValueExpression;
            // context.InboundDeliveryFilter
            targetValueExpression = Expression.Constant(bool.Parse(filterValue));
            //x.InboundDelivery.Contains(context.InboundDeliveryFilter)
            return Expression.Equal(propertyGetter, targetValueExpression);
        }

        private static Expression GetDateRangeFilterExpression(ParameterExpression exprParam, PropertyInfo property, string filterValue)
        {
            var parametrs = ParseFilterExpression(filterValue);
            var from = parametrs.Item1;
            var to = parametrs.Item2;

            Expression fromCompareExpression = null, toCompareExpression = null;
            var propertyGetter = Expression.Property(exprParam, property);

            if (from.HasValue)
            {
                ConstantExpression fromValueExpression;
                // from const
                if (property.PropertyType == typeof(DateTime))
                {
                    fromValueExpression = Expression.Constant(from.Value, typeof(DateTime));
                }
                else
                {
                    fromValueExpression = Expression.Constant(from.Value, typeof(DateTime?));
                }
                // from < delivery.prop
                fromCompareExpression = Expression.GreaterThanOrEqual(propertyGetter, fromValueExpression);
            }

            if (to.HasValue)
            {
                ConstantExpression toValueExpression;
                // to const

                if (property.PropertyType == typeof(DateTime))
                {
                    toValueExpression = Expression.Constant(to.Value, typeof(DateTime));
                }
                else
                {
                    toValueExpression = Expression.Constant(to.Value, typeof(DateTime?));
                }

                // to > delivery.prop
                toCompareExpression = Expression.LessThan(propertyGetter, toValueExpression);
            }

            // from < delivery.prop && to > delivery.prop
            if (fromCompareExpression != null && toCompareExpression != null)
            {
                return Expression.AndAlso(fromCompareExpression, toCompareExpression);
            }

            // from < delivery.prop
            if (fromCompareExpression != null)
            {
                return fromCompareExpression;
            }

            // to > delivery.prop
            return toCompareExpression;
        }

        private static Tuple<DateTime?, DateTime?> ParseFilterExpression(string filterExpression)
        {
            DateTime? from = null, to = null;
            var m = DateFilterExpression.Match(filterExpression);
            if (!m.Success)
            {
                throw new FormatException($"Incorrect filter expression for {filterExpression}");
            }

            var toCaptureIndex = 0;

            if (m.Groups["from"].Success)
            {
                toCaptureIndex = 1;
                from = new DateTime(
                    int.Parse(m.Groups["years"].Captures[0].Value),
                    int.Parse(m.Groups["months"].Captures[0].Value),
                    int.Parse(m.Groups["days"].Captures[0].Value),
                    int.Parse(m.Groups["hours"].Captures[0].Value),
                    int.Parse(m.Groups["minutes"].Captures[0].Value),
                    int.Parse(m.Groups["seconds"].Captures[0].Value),
                    int.Parse(m.Groups["seconds"].Captures[0].Value),
                    DateTimeKind.Utc);
            }

            if (m.Groups["to"].Success)
            {
                to = new DateTime(
                    int.Parse(m.Groups["years"].Captures[toCaptureIndex].Value),
                    int.Parse(m.Groups["months"].Captures[toCaptureIndex].Value),
                    int.Parse(m.Groups["days"].Captures[toCaptureIndex].Value),
                    int.Parse(m.Groups["hours"].Captures[toCaptureIndex].Value),
                    int.Parse(m.Groups["minutes"].Captures[toCaptureIndex].Value),
                    int.Parse(m.Groups["seconds"].Captures[toCaptureIndex].Value),
                    DateTimeKind.Utc);
            }

            return new Tuple<DateTime?, DateTime?>(from, to);
        }
    }
}

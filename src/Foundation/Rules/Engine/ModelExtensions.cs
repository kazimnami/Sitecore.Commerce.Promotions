using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Plugin.Rules;
using Sitecore.Framework.Rules;
using Sitecore.Framework.Rules.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Foundation.Rules.Engine
{
    public static class ModelExtensions
    {
        public static ICondition ConvertToCondition(this ConditionModel model, IEntityMetadata metaData, IEntityRegistry registry, IServiceProvider services)
        {
            if (((IEnumerable<object>)metaData.Type.GetCustomAttributes(typeof(ObsoleteAttribute), false)).Any())
            {
                return null;
            }

            var instance = ActivatorUtilities.CreateInstance(services, metaData.Type) as ICondition;
            if (instance == null)
            {
                return null;
            }

            var properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (((IEnumerable<PropertyInfo>)properties).Any(p => ModelExtensions.IsBinaryOperator(p.PropertyType)))
            {
                var propertyInfo = ((IEnumerable<PropertyInfo>)properties).FirstOrDefault(p => ModelExtensions.IsBinaryOperator(p.PropertyType));
                var operatorModelProperty = model.Properties.FirstOrDefault(x => x.IsOperator);
                if (operatorModelProperty != null)
                {
                    var entityMetadata = registry.GetOperators().FirstOrDefault(m => m.Type.FullName.Equals(operatorModelProperty.Value, StringComparison.OrdinalIgnoreCase));
                    var instance2 = ActivatorUtilities.CreateInstance(services, entityMetadata?.Type);
                    if ((object)propertyInfo != null)
                    {
                        propertyInfo.SetValue(instance, instance2);
                    }
                }
            }
            foreach (var property in model.Properties)
            {
                if (!property.IsOperator)
                {
                    var instanceProperty = instance.GetType().GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public);
                    var type = (instanceProperty.PropertyType.IsGenericType
                        && (typeof(IRuleValue<>).IsAssignableFrom(instanceProperty.PropertyType.GetGenericTypeDefinition())))
                            ? ((IEnumerable<Type>)instanceProperty.PropertyType.GetGenericArguments()).FirstOrDefault()
                            : instanceProperty.PropertyType;

                    if (type != null)
                    {
                        switch (type.FullName)
                        {
                            case "System.Decimal":
                                Decimal.TryParse(property.Value, out Decimal result1);
                                var literalRuleValue1 = new LiteralRuleValue<Decimal>()
                                {
                                    Value = result1
                                };
                                instanceProperty.SetValue(instance, literalRuleValue1, null);
                                break;
                            case "System.Int32":
                                int.TryParse(property.Value, out int result2);
                                var literalRuleValue2 = new LiteralRuleValue<int>()
                                {
                                    Value = result2
                                };
                                instanceProperty.SetValue(instance, literalRuleValue2, null);
                                break;
                            case "System.DateTimeOffset":
                                DateTimeOffset.TryParse(property.Value, out DateTimeOffset result3);
                                var literalRuleValue3 = new LiteralRuleValue<DateTimeOffset>()
                                {
                                    Value = result3
                                };
                                instanceProperty.SetValue(instance, literalRuleValue3, null);
                                break;
                            case "System.DateTime":
                                DateTime.TryParse(property.Value, out DateTime result4);
                                var literalRuleValue4 = new LiteralRuleValue<DateTime>()
                                {
                                    Value = result4
                                };
                                instanceProperty.SetValue(instance, literalRuleValue4, null);
                                break;
                            case "System.Boolean":
                                bool.TryParse(property.Value, out bool result5);
                                var literalRuleValue5 = new LiteralRuleValue<bool>()
                                {
                                    Value = result5
                                };
                                instanceProperty.SetValue(instance, literalRuleValue5, null);
                                break;
                            default:
                                var literalRuleValue6 = new LiteralRuleValue<string>()
                                {
                                    Value = property.Value
                                };
                                instanceProperty.SetValue(instance, literalRuleValue6, null);
                                break;
                        }
                    }
                }
            }

            return instance;
        }

        public static bool SetPropertyValue(this ConditionModel model, string propertyName, string propertyValue)
        {
            if (string.IsNullOrEmpty(propertyValue)|| string.IsNullOrEmpty(propertyName) || (model.Properties == null || !model.Properties.Any()))
            {
                return true;
            }

            var propertyModel = model.Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            if (propertyModel == null)
            {
                return true;
            }

            if (propertyName.Equals("TargetItemId", StringComparison.OrdinalIgnoreCase))
            {
                var strArray = propertyValue.Split('|');
                if (strArray.Length == 2)
                {
                    propertyValue += "|";
                }
                else if (strArray.Length > 3 || strArray.Length <= 1)
                {
                    return true;
                }
            }

            var hasInvalidProperty = false;
            switch (propertyModel.DisplayType)
            {
                case "System.Decimal":
                    hasInvalidProperty = !Decimal.TryParse(propertyValue, out Decimal result1);
                    break;
                case "System.Int32":
                    hasInvalidProperty = !int.TryParse(propertyValue, out int result2);
                    break;
                case "System.DateTimeOffset":
                    hasInvalidProperty = !DateTimeOffset.TryParse(propertyValue, out DateTimeOffset result3);
                    break;
                case "System.DateTime":
                    hasInvalidProperty = !DateTime.TryParse(propertyValue, out DateTime result4);
                    break;
                case "System.Boolean":
                    hasInvalidProperty = !bool.TryParse(propertyValue, out bool result5);
                    break;
            }

            if (hasInvalidProperty)
            {
                return true;
            }

            propertyModel.Value = propertyValue;

            return false;
        }
        
        public static IAction ConvertToAction(this ActionModel model, IEntityMetadata metaData, IEntityRegistry registry, IServiceProvider services)
        {
            if (((IEnumerable<object>)metaData.Type.GetCustomAttributes(typeof(ObsoleteAttribute), false)).Any())
            {
                return null;
            }

            var instance = ActivatorUtilities.CreateInstance(services, metaData.Type) as IAction;
            if (instance == null)
            {
                return null;
            }

            var properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (((IEnumerable<PropertyInfo>)properties).Any(p => ModelExtensions.IsBinaryOperator(p.PropertyType)))
            {
                var propertyInfo = ((IEnumerable<PropertyInfo>)properties).FirstOrDefault(p => ModelExtensions.IsBinaryOperator(p.PropertyType));
                var operatorModelProperty = model.Properties.FirstOrDefault(x => x.IsOperator);
                if (operatorModelProperty != null)
                {
                    var entityMetadata = registry.GetOperators().FirstOrDefault(m => m.Type.FullName.Equals(operatorModelProperty.Value, StringComparison.OrdinalIgnoreCase));
                    var instance2 = ActivatorUtilities.CreateInstance(services, entityMetadata?.Type);
                    if ((object)propertyInfo != null)
                    {
                        propertyInfo.SetValue(instance, instance2);
                    }
                }
            }
            foreach (var property in model.Properties)
            {
                if (!property.IsOperator)
                {
                    var instanceProperty = instance.GetType().GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public);
                    var type = (instanceProperty.PropertyType.IsGenericType
                        && (typeof(IRuleValue<>).IsAssignableFrom(instanceProperty.PropertyType.GetGenericTypeDefinition())))
                            ? ((IEnumerable<Type>)instanceProperty.PropertyType.GetGenericArguments()).FirstOrDefault()
                            : instanceProperty.PropertyType;

                    if (type != null)
                    {
                        switch (type.FullName)
                        {
                            case "System.Decimal":
                                Decimal.TryParse(property.Value, out Decimal result1);
                                var literalRuleValue1 = new LiteralRuleValue<Decimal>()
                                {
                                    Value = result1
                                };
                                instanceProperty.SetValue(instance, literalRuleValue1, null);
                                break;
                            case "System.Int32":
                                int.TryParse(property.Value, out int result2);
                                var literalRuleValue2 = new LiteralRuleValue<int>()
                                {
                                    Value = result2
                                };
                                instanceProperty.SetValue(instance, literalRuleValue2, null);
                                break;
                            case "System.DateTimeOffset":
                                DateTimeOffset.TryParse(property.Value, out DateTimeOffset result3);
                                var literalRuleValue3 = new LiteralRuleValue<DateTimeOffset>()
                                {
                                    Value = result3
                                };
                                instanceProperty.SetValue(instance, literalRuleValue3, null);
                                break;
                            case "System.DateTime":
                                DateTime.TryParse(property.Value, out DateTime result4);
                                var literalRuleValue4 = new LiteralRuleValue<DateTime>()
                                {
                                    Value = result4
                                };
                                instanceProperty.SetValue(instance, literalRuleValue4, null);
                                break;
                            case "System.Boolean":
                                bool.TryParse(property.Value, out bool result5);
                                var literalRuleValue5 = new LiteralRuleValue<bool>()
                                {
                                    Value = result5
                                };
                                instanceProperty.SetValue(instance, literalRuleValue5, null);
                                break;
                            default:
                                var literalRuleValue6 = new LiteralRuleValue<string>()
                                {
                                    Value = property.Value
                                };
                                instanceProperty.SetValue(instance, literalRuleValue6, null);
                                break;
                        }
                    }
                }
            }

            return instance;
        }

        public static bool SetPropertyValue(this ActionModel model, string propertyName, string propertyValue)
        {
            if (string.IsNullOrEmpty(propertyValue) || string.IsNullOrEmpty(propertyName) || (model.Properties == null || !model.Properties.Any()))
            {
                return true;
            }

            var propertyModel = model.Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            if (propertyModel == null)
            {
                return true;
            }

            //if (propertyName.Equals("TargetItemId", StringComparison.OrdinalIgnoreCase))
            //{
            //    var strArray = propertyValue.Split('|');
            //    if (strArray.Length == 2)
            //    {
            //        propertyValue += "|";
            //    }
            //    else if (strArray.Length > 3 || strArray.Length <= 1)
            //    {
            //        return true;
            //    }
            //}

            var hasInvalidProperty = false;
            switch (propertyModel.DisplayType)
            {
                case "System.Decimal":
                    hasInvalidProperty = !Decimal.TryParse(propertyValue, out Decimal result1);
                    break;
                case "System.Int32":
                    hasInvalidProperty = !int.TryParse(propertyValue, out int result2);
                    break;
                case "System.DateTimeOffset":
                    hasInvalidProperty = !DateTimeOffset.TryParse(propertyValue, out DateTimeOffset result3);
                    break;
                case "System.DateTime":
                    hasInvalidProperty = !DateTime.TryParse(propertyValue, out DateTime result4);
                    break;
                case "System.Boolean":
                    hasInvalidProperty = !bool.TryParse(propertyValue, out bool result5);
                    break;
            }

            if (hasInvalidProperty)
            {
                return true;
            }

            propertyModel.Value = propertyValue;

            return false;
        }

        private static bool IsBinaryOperator(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericTypeDefinition() == typeof(IBinaryOperator<,>);
            }

            return false;
        }
    }
}

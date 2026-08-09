//using Adani.Solution.MathParser;
using GMCore.Helper;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace Adani.Solution.DTO.Common
{
    public static class Utility
    {
        public static bool IsMigrationFileUpdate = Convert.ToBoolean((ConfigurationManager.AppSettings["IsMigrationFileUpdate"]));
        public static bool IsSeederUpdate = Convert.ToBoolean((ConfigurationManager.AppSettings["IsSeederUpdate"]));

        public static string MessageLanguage => "EN";
        public static string PermanentJourneyPlanNumberPrefix = "PCP-";
        public static string MonthlyTourPlanNumberPrefix = "MTP-";

        public static string DtoEncrypt(object successDto)
        {
            var dtoJson = JsonHelper.ConvertObjectToJson(successDto);
            return EncryptDecryptHelper.Encrypt(dtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        }

        /// <summary>
        /// Method to convert enum to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> EnumToList<T>()
        {
            var enumType = typeof(T);
            var enumValArray = Enum.GetValues(enumType);
            var enumValList = new List<T>(enumValArray.Length);
            foreach (int value in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, Convert.ToString(value)));
            }
            return enumValList;
        }

        /// <summary>
        /// Method to get enum description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Trim space for first,middle and last
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimAndReduce(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            return ConvertWhiteSpacesToSingleSpaces(str).Trim();
        }

        public static string ConvertWhiteSpacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// Enum index based return values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumFromString<T>(params long[] value) where T : IConvertible
        {
            List<string> status = new List<string>();
            if (typeof(T).IsEnum)
            {
                foreach (var id in value)
                {
                    if (!string.IsNullOrEmpty(Enum.GetName(typeof(T), id)))
                    {
                        status.Add(Enum.GetName(typeof(T), id));
                    }
                }
                return string.Join(",", status);
            }
            return string.Empty;
        }

        /// <summary>
        /// Return Enum Index from value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetEnumIndexFromString<T>(string value) where T : IConvertible
        {
            if (typeof(T).IsEnum)
            {
                return (int)Enum.Parse(typeof(T), value);
            }
            return 0;
        }

        public static string GetStringFromStatus(bool param)
        {
            return param ? "True" : "False";
        }

        /// <summary>
        /// KendoGrid multiple column filter data convert to base type(IFilterDescriptor) 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IList<IFilterDescriptor> ToFilterDescriptor(this IList<IFilterDescriptor> filters)
        {
            IList<IFilterDescriptor> result = new List<IFilterDescriptor>();
            if (filters.Any())
            {
                foreach (var filter in filters)
                {
                    var descriptor = filter as FilterDescriptor;
                    if (descriptor != null)
                    {
                        result.Add(descriptor);
                    }
                    else
                    {
                        var compositeFilterDescriptor = filter as CompositeFilterDescriptor;
                        if (compositeFilterDescriptor != null)
                        {
                            result.AddRange(compositeFilterDescriptor.FilterDescriptors.ToFilterDescriptor());
                        }
                    }
                }
            }
            return result;
        }

        public static int CalculateErrorMessageCount(string errorMessage)
        {
            if (errorMessage.Contains("|"))
                return errorMessage.EndsWith("|") ? errorMessage.Split('|').Count() - 1 : errorMessage.Split('|').Count();
            else
                return string.IsNullOrEmpty(errorMessage) ? 0 : 1;
        }

        /// <summary>
        /// Collection data exists will return true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            if (data != null && data.Any())
                return true;
            return false;
        }

        /// <summary>
        /// Collection data not exists will return true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNotAny<T>(this IEnumerable<T> data)
        {
            if (data == null || !data.Any())
                return true;
            return false;
        }

        //public static decimal StringToDouble(string formula, decimal baseSkuPrice, string replaceWord)
        //{
        //    if (!string.IsNullOrWhiteSpace(formula) && baseSkuPrice > 0)
        //    {
        //        MathsParser mathParser = new MathsParser();
        //        var inputFormula = formula.Replace(replaceWord, baseSkuPrice.ToString());
        //        return Convert.ToDecimal(string.Format("{0:0.00}", mathParser.Parse(inputFormula)));
        //    }
        //    return 0;
        //}

        public static string ConvertToTime(this DateTime dateTime)
        {
            if (dateTime != null)
            {
                return string.Format("{0:hh:mm tt}", dateTime);
            }
            return string.Empty;
        }

        /// <summary>
        /// Percentile Calculator
        /// </summary>
        /// <param name="percentile"></param>
        /// <param name="amountList"></param>
        /// <returns></returns>
        public static decimal PercentileCalculator(decimal percentile, List<decimal> inputList)
        {
            decimal amount = 0;
            if (inputList.IsAny())
            {
                //Arrange the data in the ascending order
                inputList = inputList.OrderBy(o => o).ToList();

                //Compute the position of p'th percentile
                int n = inputList.Count;
                decimal i = ((percentile / 100) * n);
                i = Math.Round(i);
                var index = i > 0 ? (int)i - 1 : (int)i;

                amount = inputList.ElementAtOrDefault(index);
            }
            return amount;
        }

        public static dynamic ConvertUTCToIndiaTime(DateTime? dateTime)
        {
            DateTime? convertedDate = null;
            if (dateTime != null)
            {
                convertedDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(dateTime), DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            }
            return convertedDate;
        }

        public static int GetRecordIndexByPercentile(decimal percentile, List<decimal> inputList)
        {
            int index = 0;
            if (inputList.IsAny())
            {
                //Arrange the data in the ascending order
                inputList = inputList.OrderBy(o => o).ToList();

                //Compute the position of p'th percentile
                int n = inputList.Count;
                decimal i = ((percentile / 100) * n);
                i = Math.Round(i, 0, MidpointRounding.AwayFromZero);
                index = (int)i;
            }
            return index;
        }

        public static decimal PercentageCalculation(decimal percentage, decimal amount)
        {
            decimal percentageAmount = ((percentage / 100) * amount);
            decimal afterPercentageResult = amount + percentageAmount;
            return decimal.Truncate(afterPercentageResult);
        }

        public static decimal DecimalFormatTwo(decimal value)
        {
            try
            {
                return Convert.ToDecimal(string.Format("{0:0.00}", value));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static decimal DecimalFormatThree(decimal value)
        {
            try
            {
                return Convert.ToDecimal(string.Format("{0:0.000}", value));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Calculate one case discount
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="bidQuantityCase"></param>
        /// <returns></returns>
        public static decimal CalculateOneCase(decimal amount, decimal bidQuantityCase)
        {
            var result = bidQuantityCase > 0 ? (amount / bidQuantityCase) : 0;
            return result;
        }

        public static decimal CalculateFRC1(decimal primaryFreight, decimal secondaryFreight, decimal depotCost, decimal detentionCost, long incoTerms, decimal plantSecondaryFreight, decimal OilTransferCostForPlant)
        {
            if (incoTerms == (long)DTO.Enums.IncoTerms.ExDepot)
            {
                return primaryFreight + depotCost + detentionCost + OilTransferCostForPlant;
            }
            else if (incoTerms == (long)DTO.Enums.IncoTerms.ForPlant)
            {
                return plantSecondaryFreight + OilTransferCostForPlant;
            }
            else if (incoTerms == (long)DTO.Enums.IncoTerms.ForDepot)
            {
                return primaryFreight + secondaryFreight + depotCost + detentionCost + OilTransferCostForPlant;
            }
            else if (incoTerms == (long)DTO.Enums.IncoTerms.ExRake)
            {
                return primaryFreight + OilTransferCostForPlant;
            }
            else if (incoTerms == (long)DTO.Enums.IncoTerms.ForRake)
            {
                return primaryFreight + secondaryFreight + depotCost + detentionCost + OilTransferCostForPlant;
            }
            else if (incoTerms == (long)DTO.Enums.IncoTerms.ExPlant)
            {
                return OilTransferCostForPlant;
            }
            else
            {
                return 0;
            }
        }

        public static decimal ExcludeGst(int qty, decimal gstPercentage, decimal includeAmount)
        {
            try
            {
                var gstAmount = qty + gstPercentage / 100;
                var excludeGst = includeAmount / gstAmount;
                return excludeGst; // Convert.ToDecimal(string.Format("{0:0.00}", excludeGst)); //Utility.DecimalFormatTwo(excludeGst)
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static decimal IncludeGst(int qty, decimal gstPercentage, decimal includeAmount)
        {
            try
            {
                var gstAmount = qty + gstPercentage / 100;
                var includeGst = includeAmount * gstAmount;
                return includeGst; // Convert.ToDecimal(string.Format("{0:0.00}", includeGst));  //Utility.DecimalFormatTwo(includeGst)
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static decimal WeightedAverage(List<WeightedAverageDto> inputs)
        {
            decimal weightage = 0;
            try
            {
                if (inputs.IsAny())
                {
                    inputs.ForEach(f =>
                    {
                        f.SumOfWeightAndPrice = (f.Weight * f.Price);
                    });

                    var sumOfWeightAmount = inputs.Select(s => s.SumOfWeightAndPrice).DefaultIfEmpty(0).Sum();
                    var sumOfWeights = inputs.Select(s => s.Weight).DefaultIfEmpty(0).Sum();
                    weightage = sumOfWeightAmount / sumOfWeights;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return weightage;
        }

        public static decimal GetGstAmount(int qty, decimal gstPercentage)
        {
            try
            {
                var gstAmount = qty + gstPercentage / 100;
                return gstAmount;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

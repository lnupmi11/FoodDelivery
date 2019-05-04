﻿using System;
using System.ComponentModel;
using System.Linq;

namespace FoodDelivery.BLL
{
    public static class EnumConverter
    {
        public static int GetEnumByDescription<T>(string description, T element) where T : IConvertible
        {
            if (element is Enum)
            {
                Type type = element.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    var memInfo = type.GetMember(type.GetEnumName(val));
                    var descriptionAttribute = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (description == descriptionAttribute.Description)
                    {
                        return val;
                    }
                }
            }
            return 0;
        }
    }
}



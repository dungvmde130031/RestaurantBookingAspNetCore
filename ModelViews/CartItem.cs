using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAspNetCore.Models;

namespace AppAspNetCore.ModelViews
{
    public class CartItem
    {
        public Meal meal { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount;
    }
}
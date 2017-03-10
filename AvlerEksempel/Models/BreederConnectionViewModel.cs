using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvlerEksempel.Models
{
    public class BreederConnectionViewModel
    {
        public List<Product> Products { get; set; }
        public List<ProductBreeder> ProductBreeders { get; set; }
    }
}
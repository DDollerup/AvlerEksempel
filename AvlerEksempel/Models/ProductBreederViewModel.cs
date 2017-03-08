using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvlerEksempel.Models
{
    public class ProductBreederViewModel
    {
        public Product Product { get; set; }
        public List<Breeder> Breeders { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace peresvet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sklad
    {
        public int sklad_id { get; set; }
        public int product_id { get; set; }
        public Nullable<int> kolichestvo { get; set; }
    
        public virtual Products Products { get; set; }
    }
}

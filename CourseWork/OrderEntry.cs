//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourseWork
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderEntry
    {
        public int Id { get; set; }
        public string RegNumer { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<int> PersonId { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Meter Meter { get; set; }
        public virtual Status Status { get; set; }
        public virtual Person Person { get; set; }
    }
}

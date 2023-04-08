//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectronicJournal.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Message()
        {
            this.Attachments = new HashSet<Attachment>();
        }
    
        public int ID { get; set; }
        public int ReceiverID { get; set; }
        public int SenderID { get; set; }
        public string Text { get; set; }
        public System.DateTime SendDatetime { get; set; }
        public Nullable<System.DateTime> ReadDatetime { get; set; }
    
        public virtual User Receiver { get; set; }
        public virtual User Sender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MobileWebApi.Models
{
    [DataContract]
    public class UserRegistration
    {
        [DataMember]
        [Required]
        public string Name { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [StringLength(3,ErrorMessage ="fdgdf")]
        public String Email { get; set; }
    }
}
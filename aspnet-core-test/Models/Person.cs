using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_test.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Display(Name = "コード")]
        [Required(ErrorMessage ="{0}は必須です。")]
        [StringLength(2, ErrorMessage ="{0}は{1}桁以内で入力してください。")]
        public string Code { get; set; }

        [Display(Name = "名前")]
        [StringLength(10, ErrorMessage = "{0}は{1}文字以内で入力してください。")]
        public string Name { get; set; }
    }
}

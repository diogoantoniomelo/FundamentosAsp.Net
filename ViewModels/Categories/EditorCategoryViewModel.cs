﻿using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve ter entre 3 a 40 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O Slug é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve ter entre 3 a 40 c caracteres")]
        public string Slug { get; set; }
    }
}

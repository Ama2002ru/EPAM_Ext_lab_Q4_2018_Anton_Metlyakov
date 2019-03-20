namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using DAL;

    /// <summary>
    /// Класс описывает модель данных варианта ответа на вопрос
    /// </summary>
    public class VariantModel
    {
        public int Quiz_Id { get; set; }

        public int Question_Id { get; set; }

        public int Variant_Id { get; set; }

        public string Variant_Text { get; set; }

        public static implicit operator VariantModel(Variant v)
        {
            if (v == null) return null;
            var vm = new VariantModel();
            vm.Quiz_Id = v.Quiz_Id;
            vm.Question_Id = v.Question_Id;
            vm.Variant_Id = v.Variant_Id;
            vm.Variant_Text = v.Text;
            return vm;
        }
    }
}
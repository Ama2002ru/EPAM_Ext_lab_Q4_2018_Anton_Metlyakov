namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Quiz.Models;
    using static Quiz.Resources.QuizResources;

    /// <summary>
    /// Контроллер для редактирования вариантов ответов на вопросы квиза
    /// </summary>
    [Authorize(Roles = "Instructor")]
    [QuizExceptionHandler]
    public class VariantController : Controller
    {
        private readonly IQuizRepository quizRepository;

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="_repository"></param>
        public VariantController()
        {
        }

        public VariantController(IQuizRepository repo)
        {
            quizRepository = repo;
        }

        /// <summary>
        /// покажу список ответов на вопрос
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <param name="question_id"></param>
        /// <returns></returns>
        public ActionResult Index(int quiz_id, int question_id)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(quiz_id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Question q = null;
            try
            {
                q = quiz.Questions.First(x => x.Question_Id == question_id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuestionRequest;
                return View("Error");
            }

            List<VariantModel> variants = new List<VariantModel>();
            for (int i = 0; i < (q.Options != null ? q.Options.Length : 0); i++)
                variants.Add(q.Options[i]);
            return View(variants);
        }

        /// <summary>
        /// Открою вью создания ответа
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <param name="question_id"></param>
        /// <returns></returns>
        public ActionResult Create(int quiz_id, int question_id)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(quiz_id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Variant v = new Variant();
            v.Quiz_Id = quiz_id;
            v.Question_Id = question_id;
            v.Variant_Id = -1;
            v.Text = string.Empty;
            VariantModel vm = v;
            return PartialView(vm);
        }

        /// <summary>
        /// Сохранение в БД варианта
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(VariantModel v)
        {
            var createdVariant = new Variant(quizRepository);
            createdVariant.Text = v.Variant_Text;
            createdVariant.Quiz_Id = v.Quiz_Id;
            createdVariant.Question_Id = v.Question_Id;
            createdVariant.Variant_Id = -1;
            if (!createdVariant.Save())
            {
                ViewBag.Error = S_ErrorSaveVariants;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "Variant",
                action = "Index",
                quiz_id = createdVariant.Quiz_Id.ToString(),
                question_id = createdVariant.Question_Id.ToString()
            });
        }

        /// <summary>
        /// Просмотр данных по варианту
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(int quiz_id, int question_id, int variant_id)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(quiz_id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Question q = null;
            try
            {
                q = quiz.Questions.First(x => x.Question_Id == question_id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuestionRequest;
                return View("Error");
            }

            Variant v = null;
            try
            {
                v = q.Options.ToList().First(x => x.Variant_Id == variant_id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidVariantRequest;
                return View("Error");
            }

            VariantModel qv = v;
            return PartialView(qv);
        }

        /// <summary>
        /// Редактирование варианта ответа
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <param name="question_id"></param>
        /// <param name="variant_id"></param>
        /// <returns></returns>
        public ActionResult Edit(int quiz_id, int question_id, int variant_id)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(quiz_id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Question q = null;
            try
            {
                q = quiz.Questions.First(x => x.Question_Id == question_id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuestionRequest;
                return View("Error");
            }

            Variant v = null;
            try
            {
                v = q.Options.ToList().First(x => x.Variant_Id == variant_id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidVariantRequest;
                return View("Error");
            }

            VariantModel vm = v;
            return PartialView(vm);
        }

        /// <summary>
        /// Сохранение в БД полей варианта
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(VariantModel v)
        {
            var editedVariant = new Variant(quizRepository);
            editedVariant.Text = v.Variant_Text;
            editedVariant.Quiz_Id = v.Quiz_Id;
            editedVariant.Question_Id = v.Question_Id;
            editedVariant.Variant_Id = v.Variant_Id;
            if (!editedVariant.Save())
            {
                ViewBag.Error = S_ErrorSaveVariants;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "Variant",
                action = "Index",
                quiz_id = editedVariant.Quiz_Id.ToString(),
                question_id = editedVariant.Question_Id.ToString()
            });
        }

        /// <summary>
        /// Просим подтверждения на удаление варианта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         public ActionResult Delete(int quiz_id, int question_id, int variant_id)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(quiz_id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Question q = null;
            try
            {
                q = quiz.Questions.First(x => x.Question_Id == question_id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuestionRequest;
                return View("Error");
            }

            Variant v = null;
            try
            {
                v = q.Options.ToList().First(x => x.Variant_Id == variant_id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidVariantRequest;
                return View("Error");
            }

            VariantModel variantToDelete = v;
            return PartialView(variantToDelete);
        }

        /// <summary>
        /// Удаляем вопрос по ИД квиза и вопроса
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(VariantModel vm)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(vm.Quiz_Id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Question q = null;
            try
            {
                q = quiz.Questions.First(x => x.Question_Id == vm.Question_Id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuestionRequest;
                return View("Error");
            }

            Variant deletingVariant = new Variant(quizRepository);
            deletingVariant.Quiz_Id = vm.Quiz_Id;
            deletingVariant.Question_Id = vm.Question_Id;
            deletingVariant.Variant_Id = vm.Variant_Id;

            if (!deletingVariant.Delete())
            {
                ViewBag.Error = S_ErrorDeleteVariant;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "Variant",
                action = "Index",
                quiz_id = deletingVariant.Quiz_Id.ToString(),
                question_id = deletingVariant.Question_Id.ToString()
            });
        }
    }
}
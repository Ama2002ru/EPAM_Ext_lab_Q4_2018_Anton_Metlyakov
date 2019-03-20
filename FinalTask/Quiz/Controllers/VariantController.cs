namespace Quiz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DAL;
    using Quiz.Models;

    /// <summary>
    /// Контроллер для редактирования вариантов ответов на вопросы квиза
    /// </summary>
    [Authorize(Roles = "Instructor")]
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
        [Authorize(Roles = "Instructor")]
        public ActionResult Index(int quiz_id, int question_id)
        {
            try
            {
                Quiz quiz;
                if ((quiz = this.quizRepository.Get(quiz_id)) == null)
                {
                    ViewBag.Error = "Invalid quiz request!";
                    return PartialView();
                }

                Question q = null;
                try
                {
                    q = quiz.Questions.First(x => x.Question_Id == question_id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid questopn request!";
                    return PartialView();
                }

                List<VariantModel> variants = new List<VariantModel>();
                for (int i = 0; i < (q.Options != null ? q.Options.Length : 0); i++)
                    variants.Add(q.Options[i]);
                return View(variants);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error get variants !";
                return PartialView();
            }
        }

        /// <summary>
        /// Открою вью создания ответа
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <param name="question_id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Create(int quiz_id, int question_id)
        {
            try
            {
                Quiz quiz;
                if ((quiz = this.quizRepository.Get(quiz_id)) == null)
                {
                    ViewBag.Error = "Invalid quiz request!";
                    return PartialView();
                }

                Variant v = new Variant();
                v.Quiz_Id = quiz_id;
                v.Question_Id = question_id;
                v.Variant_Id = -1;
                v.Text = string.Empty;
                VariantModel vm = v;
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error create variant !";
                return PartialView();
            }
        }

        /// <summary>
        /// Сохранение в БД варианта
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public ActionResult Create(VariantModel v)
        {
            try
            {
                var createdVariant = new Variant(quizRepository);
                createdVariant.Text = v.Variant_Text;
                createdVariant.Quiz_Id = v.Quiz_Id;
                createdVariant.Question_Id = v.Question_Id;
                createdVariant.Variant_Id = -1;
                if (!createdVariant.Save())
                {
                    ViewBag.Error = "Error save variant!";
                    return PartialView();
                }

                return RedirectToRoute(new
                {
                    controller = "Variant",
                    action = "Index",
                    quiz_id = createdVariant.Quiz_Id.ToString(),
                    question_id = createdVariant.Question_Id.ToString()
                });
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error create variant !";
                return PartialView();
            }
        }

        /// <summary>
        /// Просмотр данных по варианту
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Details(int quiz_id, int question_id, int variant_id)
        {
            try
            {
                Quiz quiz;
                if ((quiz = this.quizRepository.Get(quiz_id)) == null)
                {
                    ViewBag.Error = "Invalid quiz request!";
                    return PartialView();
                }

                Question q = null;
                try
                {
                    q = quiz.Questions.First(x => x.Question_Id == question_id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid question request!";
                    return PartialView();
                }

                Variant v = null;
                try
                {
                    v = q.Options.ToList().First(x => x.Variant_Id == variant_id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid variant request!";
                    return PartialView();
                }

                VariantModel qv = v;
                return PartialView(qv);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error view details !";
                return PartialView();
            }
        }

        /// <summary>
        /// Редактирование варианта ответа
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <param name="question_id"></param>
        /// <param name="variant_id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Edit(int quiz_id, int question_id, int variant_id)
        {
            try
            {
                Quiz quiz;
                if ((quiz = this.quizRepository.Get(quiz_id)) == null)
                {
                    ViewBag.Error = "Invalid quiz request!";
                    return PartialView();
                }

                Question q = null;
                try
                {
                    q = quiz.Questions.First(x => x.Question_Id == question_id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid question request!";
                    return PartialView();
                }

                Variant v = null;
                try
                {
                    v = q.Options.ToList().First(x => x.Variant_Id == variant_id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid variant request!";
                    return PartialView();
                }

                VariantModel vm = v;
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error edit variant !";
                return PartialView();
            }
        }

        /// <summary>
        /// Сохранение в БД полей варианта
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public ActionResult Edit(VariantModel v)
        {
            try
            {
                var editedVariant = new Variant(quizRepository);
                editedVariant.Text = v.Variant_Text;
                editedVariant.Quiz_Id = v.Quiz_Id;
                editedVariant.Question_Id = v.Question_Id;
                editedVariant.Variant_Id = v.Variant_Id;
                if (!editedVariant.Save())
                {
                    ViewBag.Error = "Error save variant!";
                    return PartialView();
                }

                return RedirectToRoute(new
                {
                    controller = "Variant",
                    action = "Index",
                    quiz_id = editedVariant.Quiz_Id.ToString(),
                    question_id = editedVariant.Question_Id.ToString()
                });
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error edit variant!";
                return PartialView();
            }
        }

        /// <summary>
        /// Просим подтверждения на удаление варианта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(int quiz_id, int question_id, int variant_id)
        {
            try
            {
                Quiz quiz;
                if ((quiz = this.quizRepository.Get(quiz_id)) == null)
                {
                    ViewBag.Error = "Invalid quiz request!";
                    return PartialView();
                }

                Question q = null;
                try
                {
                    q = quiz.Questions.First(x => x.Question_Id == question_id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid question request!";
                    return PartialView();
                }

                Variant v = null;
                try
                {
                    v = q.Options.ToList().First(x => x.Variant_Id == variant_id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid variant request!";
                    return PartialView();
                }

                VariantModel variantToDelete = v;
                return PartialView(variantToDelete);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error delete variant !";
                return PartialView();
            }
        }

        /// <summary>
        /// Удаляем вопрос по ИД квиза и вопроса
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(VariantModel vm)
        {
            try
            {
                Quiz quiz;
                if ((quiz = this.quizRepository.Get(vm.Quiz_Id)) == null)
                {
                    ViewBag.Error = "Invalid quiz request!";
                    return PartialView();
                }

                Question q = null;
                try
                {
                    q = quiz.Questions.First(x => x.Question_Id == vm.Question_Id);
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "Invalid question request!";
                    return PartialView();
                }

                Variant deletingVariant = new Variant(quizRepository);
                deletingVariant.Quiz_Id = vm.Quiz_Id;
                deletingVariant.Question_Id = vm.Question_Id;
                deletingVariant.Variant_Id = vm.Variant_Id;

                if (!deletingVariant.Delete())
                {
                    ViewBag.Error = "Error delete variant!";
                    return PartialView();
                }

                return RedirectToRoute(new
                {
                    controller = "Variant",
                    action = "Index",
                    quiz_id = deletingVariant.Quiz_Id.ToString(),
                    question_id = deletingVariant.Question_Id.ToString()
                });
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0} {1}\n", ex.Message, ex.Source));
                ViewBag.Error = "Error delete variant !";
                return PartialView();
            }
        }
    }
}
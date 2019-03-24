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

    [Authorize(Roles = "Instructor")]
    [QuizExceptionHandler]
    public class QuestionController : Controller
    {
        /// <summary>
        /// Dependency injections fields
        /// </summary>
        private readonly IQuizRepository quizRepository;
        private readonly IPersonRepository personRepository;

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="_repository"></param>
        public QuestionController()
        {
        }

        public QuestionController(IQuizRepository repo, IPersonRepository pers)
        {
            quizRepository = repo;
            personRepository = pers;
        }

        /// <summary>
        /// просмотр списка вопросов к квизу
        /// </summary>
        /// <param name="quiz_id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Index(int quiz_id)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(quiz_id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            var questions = new List<QuestionModel>(0);
            foreach (var q in quiz.Questions)
                questions.Add(q);
            return PartialView(questions);
        }

        /// <summary>
        /// Создание вопроса
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Create(int quiz_id)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(quiz_id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Question q = new Question();
            q.Quiz_Id = quiz_id;
            q.Info = string.Empty;
            q.Text = string.Empty;
            q.CorrectOptionFlag = 0;
            q.Options = new Variant[0];
            QuestionModel qm = q;
            qm.Options = q.Options;
            return View(qm);
        }

        /// <summary>
        /// Сохранение в БД полей вопроса
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public ActionResult Create(QuestionModel q)
        {
            Question createdQuestion = new Question(quizRepository);
            createdQuestion.Info = q.Info;
            createdQuestion.Text = q.Text;
            createdQuestion.Quiz_Id = q.Quiz_Id;
            createdQuestion.Question_Id = -1;
            createdQuestion.CorrectOptionFlag = q.CorrectOptionFlag;
            if (!createdQuestion.Save())
            {
                ViewBag.Error = S_ErrorSaveQuestion;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "Question",
                action = "Index",
                id = string.Empty
            });
       }

        /// <summary>
        /// Просим подтверждения на удаление вопроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(int quiz_id, int id)
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
                q = quiz.Questions.First(x => x.Question_Id == id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            QuestionModel questionToDelete = q;
            questionToDelete.Options = q.Options;
            int len = q.Options != null ? q.Options.Length : 0;
            for (int i = 0; i < len; i++)
            {
                if ((q.CorrectOptionFlag >> i & 1) == 1)
                    questionToDelete.Options[i].Value = 1;
                else
                    questionToDelete.Options[i].Value = 0;
            }

            return PartialView(questionToDelete);
        }

        /// <summary>
        /// Удаляем вопрос по ИД квиза и вопроса
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public ActionResult Delete(QuestionModel question)
        {
            Quiz quiz;
            if ((quiz = this.quizRepository.Get(question.Quiz_Id)) == null)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            Question q = new Question(quizRepository);
            q.Quiz_Id = question.Quiz_Id;
            q.Question_Id = question.Question_Id;

            if (!q.Delete())
            {
                ViewBag.Error = S_ErrorDeleteQuestion;
                return View("Error"); 
            }

            return RedirectToRoute(new
            {
                controller = "Question",
                action = "Index",
                quiz_id = question.Quiz_Id.ToString()
            });
        }

        /// <summary>
        /// Просмотр данных по квизу
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Details(int quiz_id, int id)
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
                q = quiz.Questions.First(x => x.Question_Id == id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            QuestionModel qm = q;
            qm.Options = q.Options;
            for (int i = 0; i < (q.Options != null ? q.Options.Length : 0); i++)
            {
                if ((q.CorrectOptionFlag >> i & 1) == 1)
                    qm.Options[i].Value = 1;
                else
                    qm.Options[i].Value = 0;
            }

            return PartialView(qm);
        }

        /// <summary>
        /// Редактирование некоторых полей вопроса
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Instructor")]
        public ActionResult Edit(int quiz_id, int id)
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
                q = quiz.Questions.First(x => x.Question_Id == id);
            }
            catch (InvalidOperationException)
            {
                ViewBag.Error = S_InvalidQuizRequest;
                return View("Error");
            }

            QuestionModel qm = q;
            qm.Options = q.Options;
            int len = q.Options != null ? q.Options.Length : 0;
            for (int i = 0; i < len; i++)
            {
                if ((q.CorrectOptionFlag >> i & 1) == 1)
                    qm.Options[i].Value = 1;
                else
                    qm.Options[i].Value = 0;
            }

            return View(qm);
        }

        /// <summary>
        /// Сохранение в БД полей вопроса
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public ActionResult Edit(FormCollection collection)
        {
            QuestionModel q = new QuestionModel();
            int result = 0;
            int.TryParse(collection["Question_Id"], out result);
            q.Question_Id = result;
            int.TryParse(collection["Quiz_Id"], out result);
            q.Quiz_Id = result;
            q.Info = collection["Info"];
            q.Text = collection["Text"];
            q.CorrectOptionFlag = 0;
            for (int i = 0; i < 32; i++)
            {
                var res = collection["variant" + i.ToString()];
                if (res == "on")
                    q.CorrectOptionFlag |= 1 << i;
            }

            Question editedQuestion = new Question(quizRepository);
            editedQuestion.Info = q.Info;
            editedQuestion.Text = q.Text;
            editedQuestion.Quiz_Id = q.Quiz_Id;
            editedQuestion.Question_Id = q.Question_Id;
            editedQuestion.CorrectOptionFlag = q.CorrectOptionFlag;
            if (!editedQuestion.Save())
            {
                ViewBag.Error = S_ErrorSaveQuestion;
                return View("Error");
            }

            return RedirectToRoute(new
            {
                controller = "Question",
                action = "Index",
                quiz_id = editedQuestion.Quiz_Id.ToString(),
            });
        }
    }
}
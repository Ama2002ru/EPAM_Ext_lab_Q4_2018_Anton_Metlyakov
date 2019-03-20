namespace Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /* route - еще поиск по строке ... */
            routes.MapRoute(
                name: "user-search",
                url: "users/{name}",
                defaults: new { controller = "user", action = "index", name = UrlParameter.Optional });

            routes.MapRoute(
                name: "logon",
                url: "logon",
                defaults: new { controller = "logon", action = "Logon" });

            routes.MapRoute(
                name: "logoff",
                url: "logoff",
                defaults: new { controller = "logon", action = "Logoff" });

            routes.MapRoute(
                name: "registeruser",
                url: "registeruser",
                defaults: new { controller = "logon", action = "Register" });

            routes.MapRoute(
                name: "user-create",
                url: "create-user",
                defaults: new { controller = "User", action = "Create" });

            routes.MapRoute(
                name: "user",
                url: "user/{id}",
                defaults: new { controller = "User", action = "Details", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "user-edit",
                url: "user/{id}/edit",
                defaults: new { controller = "User", action = "Edit", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "user-delete",
                url: "user/{id}/delete",
                defaults: new { controller = "User", action = "Delete", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "users",
                url: "user/index",
                defaults: new { controller = "user", action = "Index" });

            routes.MapRoute(
                name: "Quizes",
                url: "Quizes",
                defaults: new { controller = "Quiz", action = "Index" });

            routes.MapRoute(
                 name: "quiz-create",
                 url: "create-quiz",
                 defaults: new { controller = "quiz", action = "Create" });

            routes.MapRoute(
                name: "quiz-edit",
                url: "quiz/{id}/edit",
                defaults: new { controller = "Quiz", action = "Edit", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "quiz-delete",
                url: "quiz/{id}/delete",
                defaults: new { controller = "Quiz", action = "Delete", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "quiz",
                url: "quiz/{id}",
                defaults: new { controller = "Quiz", action = "Details", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "Questions",
                url: "quiz/{quiz_id}/questions",
                defaults: new { controller = "Question", action = "Index", quiz_id = "id" },
                constraints: new { quiz_id = @"\d+" });

            routes.MapRoute(
                 name: "question-create",
                 url: "{quiz_id}/create-question",
                 defaults: new { controller = "Question", action = "Create", quiz_id = "quiz_id" },
                 constraints: new { quiz_id = @"\d+" });

            routes.MapRoute(
                name: "question-edit",
                url: "{quiz_id}/question/{id}/edit",
                defaults: new { controller = "Question", action = "Edit", quiz_id = "id", id = "id" },
                constraints: new { quiz_id = @"\d+", id = @"\d+" });

            routes.MapRoute(
                name: "question-delete",
                url: "{quiz_id}/question/{id}/delete",
                defaults: new { controller = "Question", action = "Delete", quiz_id = "id", id = "id" },
                constraints: new { quiz_id = @"\d+", id = @"\d+" });

            routes.MapRoute(
                name: "question",
                url: "quiz/{quiz_id}/question/{id}",
                defaults: new { controller = "Question", action = "Details", quiz_id = "id", id = "id" },
                constraints: new { quiz_id = @"\d+", id = @"\d+" });

            routes.MapRoute(
                name: "variants",
                url: "{quiz_id}/question/{question_id}/variants",
                defaults: new { controller = "Variant", action = "Index", quiz_id = "id", question_id = "id" },
                constraints: new { quiz_id = @"\d+", question_id = @"\d+" });

            routes.MapRoute(
                 name: "variant-create",
                 url: "{quiz_id}/question/{question_id}/create-variant",
                 defaults: new { controller = "Variant", action = "Create", quiz_id = "quiz_id", question_id = "question_id" },
                 constraints: new { quiz_id = @"\d+", question_id = @"\d+" });

            routes.MapRoute(
                name: "variant-edit",
                url: "{quiz_id}/question/{question_id}/edit/{variant_id}",
                defaults: new { controller = "Question", action = "Edit", quiz_id = "quiz_id", question_id = "question_id", variant_id = "variant_id" },
                constraints: new { quiz_id = @"\d+", question_id = @"\d+", variant_id = @"\d+" });

            routes.MapRoute(
                name: "variant-delete",
                url: "{quiz_id}/question/{question_id}/delete/{variant_id}",
                defaults: new { controller = "Question", action = "Delete", quiz_id = "quiz_id", question_id = "question_id", variant_id = "variant_id" },
                constraints: new { quiz_id = @"\d+", question_id = @"\d+", variant_id = @"\d+" });

            routes.MapRoute(
                name: "variant",
                url: "quiz/{quiz_id}/question/{question_id}/variant/{variant_id}",
                defaults: new { controller = "Question", action = "Details", quiz_id = "quiz_id", question_id = "question_id", variant_id = "variant_id" },
                constraints: new { quiz_id = @"\d+", question_id = @"\d+", variant_id = @"\d+" });

            routes.MapRoute(
                name: "myquizes",
                url: "myquizes/{user_id}",
                defaults: new { controller = "Myquizes", action = "Index", user_id = "user_id" },
                constraints: new { user_id = @"\d+" });

            routes.MapRoute(
                name: "myquizesview",
                url: "myquizes/{quizresult_id}/view",
                defaults: new { controller = "Myquizes", action = "Details", quiz_id = "quizresult_id" },
                constraints: new { quizresult_id = @"\d+" });

            routes.MapRoute(
                name: "startquiz",
                url: "myquizes/{quizresult_id}/startquiz",
                defaults: new { controller = "Myquizes", action = "StartQuiz", quizresult_id = "quizresult_id" },
                constraints: new { quizresult_id = @"\d+" });

            routes.MapRoute(
                name: "getnextquestion",
                url: "myquizes/{quizresult_id}/getnextquestion",
                defaults: new { controller = "Myquizes", action = "GetNextQuestion", quizresult_id = "quizresult_id" },
                constraints: new { quizresult_id = @"\d+" });

            routes.MapRoute(
                name: "finishquiz",
                url: "myquizes/{quizresult_id}/finishquiz",
                defaults: new { controller = "Myquizes", action = "FinishQuiz", quizresult_id = "quizresult_id" },
                constraints: new { quizresult_id = @"\d+" });

            routes.MapRoute(
                name: "assignquiz",
                url: "myquizes/assignquiz/{user_id}",
                defaults: new { controller = "Myquizes", action = "AssignQuiz", user_id = "user_id" },
                constraints: new { user_id = @"\d+" });

            routes.MapRoute(
                name: "statistic",
                url: "statistic",
                defaults: new { controller = "Statistic", action = "Index" });

            routes.MapRoute(
                name: "allquizes",
                url: "statistic/allquizes",
                defaults: new { controller = "Statistic", action = "AllQuizes" });

            routes.MapRoute(
                name: "allusers",
                url: "statistic/allusers",
                defaults: new { controller = "Statistic", action = "AllUsers" });

            routes.MapRoute(
                name: "byquiz",
                url: "statistic/byquiz/{quiz_id}",
                defaults: new { controller = "Statistic", action = "ByQuiz", user_id = "quiz_id" },
                constraints: new { quiz_id = @"\d+" });

            routes.MapRoute(
                name: "byuser",
                url: "statistic/byuser/{user_id}",
                defaults: new { controller = "Statistic", action = "ByUser", user_id = "user_id" },
                constraints: new { user_id = @"\d+" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional });
        }
    }
}

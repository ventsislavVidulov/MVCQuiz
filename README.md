# ASP.NET Core MVC Quiz App

### A simple app in which multiple users can solve quizzes with multiple-choice answers.

### To use the app, download the **repository** and run it in Visual Studio.

### Features
1. You may add your own quizzes as **JSON files** in the **/Data** folder with the following signature:
```json
{
  "title": "C# Programming Quiz",
  "questions": [
    {
      "id": 0,
      "title": "What is the purpose of the 'using' statement in C#?",
      "answer": "Automatic resource disposal",
      "options": [
        "Import namespaces",
        "Automatic resource disposal",
        "Define variables",
        "Create objects"
      ]
    },
  ]
}
```
2. After submitting, a **review** of your performance **is generated**. This includes the **count of correct answers**, **overall performance feedback**, and a breakdown of **answered/correct questions**.
3. You may take the **Mega Quiz**, which is a combination of all **quizzes**.
4. All **quiz** questions and answers (except for the **Mega Quiz**) are randomized on every attempt.
5. **A timer** (**written in JavaScript** and **restarting on refresh**) with **30 seconds** per question starts when the quiz is attempted and **submits** the quiz after the time is up.



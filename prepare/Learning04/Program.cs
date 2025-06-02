using System;

class Program
{
    static void Main(string[] args)
    {
        MathAssignments mathAssignment = new MathAssignments("Samuel Bennett", "Multiplication", "7.3", "8-19");
        Console.WriteLine(mathAssignment.GetSummary());
        Console.WriteLine(mathAssignment.GetHomeworkList());

        WritingAssignments writingAssignment = new WritingAssignments("Mary Waters", "European History", "The Causes of World War II");
        Console.WriteLine(writingAssignment.GetSummary());
        Console.WriteLine(writingAssignment.GetHomeworkList());
    }

    public class Assignment
    {
        private string _studentName;
        private string _topic;

        public Assignment(string studentName, string topic)
        {
            _studentName = studentName;
            _topic = topic;
        }

        public string GetSummary()
        {
            return $"{_studentName} - {_topic}";
        }

        public string GetStudentName()
        {
            return _studentName;
        }
    }

    public class MathAssignments : Assignment
    {
        private string _textbookSection;
        private string _problems;

        public MathAssignments(string studentName, string topic, string textbookSection, string problems) : base(studentName, topic)
        {
            _textbookSection = textbookSection;
            _problems = problems;
        }

        public string GetHomeworkList()
        {
            return $"Section {_textbookSection} Problems {_problems}";
        }
    }

    public class WritingAssignments : Assignment
    {
        private string _title;

        public WritingAssignments(string studentName, string topic, string title) : base(studentName, topic)
        {
            _title = title;
        }

        public string GetHomeworkList()
        {
            return $"{_title} by {GetStudentName()}";
        }
    }
}
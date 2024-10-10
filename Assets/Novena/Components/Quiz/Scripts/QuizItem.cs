namespace Scripts.Quiz {
  public class QuizItem {
    //Properties + Getters & Setters
    string _answer1;
    public string Answer1
    {
      get { return _answer1; }
      set { _answer1 = value; }
    }

    string _answer2;
    public string Answer2
    {
      get { return _answer2; }
      set { _answer2 = value; }
    }

    string _answer3;
    public string Answer3
    {
      get { return _answer3; }
      set { _answer3 = value; }
    }

    string _answer4;
    public string Answer4
    {
      get { return _answer4; }
      set { _answer4 = value; }
    }

    string _question;
    public string Question
    {
      get { return _question; }
      set { _question = value; }
    }

    string _imagePath;
    public string ImagePath
    {
      get { return _imagePath; }
      set { _imagePath = value; }
    }

    int _rightAnswerIndicator;
    public int RightAnswerIndicator
    {
      get { return _rightAnswerIndicator; }
      set { _rightAnswerIndicator = value; }
    }

    // Constructor
    public QuizItem(string question, string imagepath, int rightAnswerIndicator, string answer1 = "", string answer2  = "", string answer3 = "", string answer4 = "")
    {
      _answer1 = answer1;
      _answer2 = answer2;
      _answer3 = answer3;
      _answer4 = answer4;
      _question = question;
      _imagePath = imagepath;
      _rightAnswerIndicator = rightAnswerIndicator;
    }
  }
}
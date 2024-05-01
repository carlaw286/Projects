import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const Quiz = ({ id, userID }) => {
  const [quizzes, setQuizzes] = useState([]);
  const [userScore, setUserScore] = useState(0); // Added state for user's score
  const [submitted, setSubmitted] = useState(false);
  const currentCourseID = id;
  console.log("Course ID from cView = " + currentCourseID);
  const navigate = useNavigate();

  const handleButtonClick = () => {
    console.log(currentCourseID + (" ") + userID);
    navigate(`/certificate?userID=${userID}&courseID=${currentCourseID}`);
  };

  useEffect(() => {
    const fetchQuizzes = async () => {
      try {
        const response = await axios.get('http://localhost:3002/quiz');
        setQuizzes(response.data);
      } catch (error) {
        console.error('Error fetching quizzes:', error);
      }
    };

    fetchQuizzes();
  }, []);

  const filteredQuizzes = quizzes.filter((quiz) => quiz.courseID === currentCourseID);
  const [userAnswers, setUserAnswers] = useState({});

  const handleRadioChange = (questionId, choice) => {
    setUserAnswers((prevAnswers) => ({
      ...prevAnswers,
      [questionId]: choice,
    }));
  };

  const handleSubmit = () => {
    setSubmitted(true);
    const score = calculateScore();
    setUserScore(score); // Set the user's score in state
  };

  const calculateScore = () => {
    let score = 0;
    filteredQuizzes.forEach((quiz) => {
      quiz.questions.forEach((question) => {
        const userAnswer = userAnswers[question._id];
        if (userAnswer === question.correctAnswer) {
          score += 1;
        }
      });
    });
    return score;
  };

  const handleRetake = () => {
    setSubmitted(false);
    setUserScore(0); // Reset the user's score when retaking the quiz
  };

  return (
    <div className="courseQuiz">
      <div className="QuizTitle">
        <h2>Quiz</h2>
      </div>
      <ul>
        {filteredQuizzes.map((quiz) => (
          <li key={quiz._id}>
            <ul className="choices-list">
              {quiz.questions.map((question) => (
                <li key={question._id}>
                  <p>{question.questionText}</p>
                  <ul>
                    {question.choices.map((choice, index) => (
                      <li key={index}>
                        <input
                          type="radio"
                          id={`${question._id}_${index}`}
                          name={question._id}
                          value={choice}
                          onChange={() => handleRadioChange(question._id, choice)}
                          disabled={submitted}
                        />
                        <label htmlFor={`${question._id}_${index}`}>{choice}</label>
                      </li>
                    ))}
                  </ul>
                </li>
              ))}
            </ul>
          </li>
        ))}
      </ul>
      {submitted && (
        <p>
          Your Score: {userScore} / {filteredQuizzes.reduce((total, quiz) => total + quiz.questions.length, 0)}
        </p>
      )}
      <button
        className="Quiz_submit-button"
        type="button"
        onClick={handleSubmit}
        disabled={submitted}
      >
        Submit Quiz
      </button>
      <button
        className="Quiz_retake-button"
        type="button"
        onClick={handleRetake}
        disabled={!submitted}
      >
        Retake Quiz
      </button>
      {/* Render the "Get Certificate" button only if the user's score is 75% or higher */}
      {submitted && userScore >= (filteredQuizzes.reduce((total, quiz) => total + quiz.questions.length, 0) * 0.75) && (
        <button className="getCert-button" onClick={handleButtonClick} disabled={!submitted}>
          Get Certificate
        </button>
      )}
    </div>
  );
};

export default Quiz;

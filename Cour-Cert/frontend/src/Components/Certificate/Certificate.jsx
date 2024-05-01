import React, { useEffect, useRef, useState } from "react";
import html2canvas from "html2canvas";
import jsPDF from "jspdf";
import "./Certificate1.css";
import { useLocation } from "react-router-dom";
import axios from "axios";

export function Certificate() {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const certificateRef = useRef(null);
  const userId = queryParams.get('userID');
  const courseID = queryParams.get('courseID');
  const [studentUser, setStudentUser] = useState({});
  const [courseData, setCourseData] = useState({});
  const [teacherUser, setTeacherUser] = useState({});
  
  const currentDate = new Date().toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
  
  useEffect(()=> {
    //Student Data
    axios.get('http://localhost:3002/getCertificate', {
      params: {
        id : userId,
      }
    })
    .then(result =>{
      setStudentUser(result.data);
    })
    .catch(err => console.error(err));

    //Course Data
    axios.get('http://localhost:3002/getCourseCertificate', {
      params: {
        id : courseID,
      }
    })
    .then(result =>{
      setCourseData(result.data);
    })
    .catch(err => console.error(err));
  }, [])

  useEffect(() => {
    if (courseData.user_id) {
      //Teacher Data
      axios.get('http://localhost:3002/getCourseTeacher', {
        params: {
          id: courseData.user_id,
        }
      })
      .then(result => {
        setTeacherUser(result.data);
      })
      .catch(err => console.error(err));
    }
  }, [courseData.user_id]);
  

  const handleDownload = async () => {
    const certificateElement = certificateRef.current;

    if (certificateElement) {
      // Temporarily hide the download button
      const downloadButton =
        certificateElement.querySelector(".download-button");
      if (downloadButton) {
        downloadButton.style.display = "none";
      }

      // Capture the content of the certificate
      const desiredSizePercentage = 100;
      const scaleFactor = desiredSizePercentage / 100;
      const canvas = await html2canvas(certificateElement, {
        scale: scaleFactor,
      });

      // Restore the download button visibility
      if (downloadButton) {
        downloadButton.style.display = "block";
      }

      // Create a PDF document
      const pdf = new jsPDF("landscape", "mm", "a4");
      pdf.addImage(canvas.toDataURL("image/png"), "PNG", 0, 0, 297, 210);

      // Download the PDF
      pdf.save("certificate.pdf");
    }
  };

  return (
    <div ref={certificateRef}>
      <div className="App">
        <p className="byline"></p>
        <div className="content">
          <p>This certificate is presented to</p>
          <h1>{studentUser.firstName} {studentUser.lastName}</h1>
          <p>
            for successfully demonstrating exceptional knowledge and proficiency
            by passing<p></p> the course: {courseData.course_title}. Issued on {currentDate} ,<p></p>{" "}
            this accomplishment signifies {studentUser.gender === "Male" ? "his" : "her"} commitment to excellence,
            dedication<p></p> to continuous learning, and mastery of the course
            content.{" "}
          </p>
          <h2>{teacherUser.firstName} {teacherUser.lastName}</h2>
          <button className="download-button" onClick={handleDownload}>
            Download
          </button>
        </div>
      </div>
    </div>
  );
}



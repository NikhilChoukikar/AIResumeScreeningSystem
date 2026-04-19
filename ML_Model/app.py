from flask import Flask, request, jsonify
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import PyPDF2
import re

app = Flask(__name__)

# ✅ STEP 1: ADD SKILLS LIST HERE
SKILLS = [
    "python", "java", "c#", ".net", "asp.net", "sql", "mysql",
    "html", "css", "javascript", "react", "angular",
    "docker", "kubernetes", "aws", "azure", "git"
]

def extract_skills(text):
    text = text.lower()
    found_skills = []

    for skill in SKILLS:
        if skill in text:
            found_skills.append(skill)

    return list(set(found_skills))


# ✅ Extract text from PDF
def extract_text_from_pdf(file):
    reader = PyPDF2.PdfReader(file)
    text = ""
    for page in reader.pages:
        text += page.extract_text() or ""
    return text

def clean_text(text):
    text = text.lower()

    # 🔥 Fix broken characters (n i k h i l → nikhil)
    text = re.sub(r'(?<=\b\w)\s(?=\w\b)', '', text)

    # Remove extra spaces
    text = re.sub(r'\s+', ' ', text)

    return text.strip()
# ✅ Calculate similarity
def calculate_similarity(resume_text, job_desc):
    documents = [resume_text, job_desc]

    tfidf = TfidfVectorizer(stop_words='english')
    tfidf_matrix = tfidf.fit_transform(documents)

    score = cosine_similarity(tfidf_matrix[0:1], tfidf_matrix[1:2])
    return float(score[0][0])

@app.route('/match-pdf', methods=['POST'])
def match_pdf():
    file = request.files['resume']
    job_desc = request.form['job_desc']

    resume_text = extract_text_from_pdf(file)
    resume_text = clean_text(resume_text)
    job_desc = clean_text(job_desc)

    score = calculate_similarity(resume_text, job_desc)

    # 🔥 NEW FEATURE
    resume_skills = extract_skills(resume_text)
    job_skills = extract_skills(job_desc)

    matched_skills = list(set(resume_skills) & set(job_skills))
    missing_skills = list(set(job_skills) - set(resume_skills))

    return jsonify({
        "score": score,
        "extracted_text": resume_text[:500],
        "matched_skills": matched_skills,
        "missing_skills": missing_skills
    })


if __name__ == "__main__":
    app.run(debug=True, port=5000)
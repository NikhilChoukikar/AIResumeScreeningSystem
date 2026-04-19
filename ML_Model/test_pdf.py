import requests

url = "http://127.0.0.1:5000/match-pdf"

# Path to your PDF file (change this)
file_path = r"C:\Users\Nikhil\Desktop\New Resume\CV Nikhil Choukikar.pdf"
# Open file
with open(file_path, "rb") as f:
    files = {
        "resume": (file_path, f, "application/pdf")
    }

    data = {
        "job_desc": "Looking for Python and SQL developer"
    }

    response = requests.post(url, files=files, data=data)

    print(response.json())
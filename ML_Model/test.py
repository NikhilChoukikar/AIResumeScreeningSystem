import requests

url = "http://127.0.0.1:5000/match"

data = {
    "resume": "I know Python and SQL developer. Looking for a job as a Python and SQL developer",
    "job_desc": "Looking for Python and SQL developer"
}

response = requests.post(url, json=data)

print(response.json())
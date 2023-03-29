# syntax=docker/dockerfile:1
FROM unityci/editor:ubuntu-2021.3.8f1-base-1.0.1

# Setup backend
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1
WORKDIR /code
COPY requirements.txt /code/
RUN apt update && apt install -y postgresql postgresql-contrib python3-pip libpq-dev python3-dev && pip3 install -r requirements.txt
COPY . /code/
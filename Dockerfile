# syntax=docker/dockerfile:1
FROM unityci/editor:ubuntu-2021.3.8f1-base-1.0.1

# Install dependencies
RUN apt update && apt install -y python-pip

# Setup backend
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1
WORKDIR /code
COPY requirements.txt /code/
RUN pip install -r requirements.txt
COPY . /code/
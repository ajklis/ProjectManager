@echo off

docker-compose down
docker-compose up -d --build

start firefox "http://localhost:8080/"
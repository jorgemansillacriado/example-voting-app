# example-voting-app
Desarrollo de la app "to do" incluida en el tutorial de Docker estudiando e implementando el S-SDLC

# Cómo ejecutar la aplicación

Este proyecto contiene una aplicación web para simular votaciones en un sistema de votación. La aplicación utiliza Docker para ejecutar la base de datos y el servidor web. Aquí están los pasos para ejecutar la aplicación localmente:

---

## 🚀 Requisitos

- Docker Desktop instalado y corriendo en tu máquina (Windows, macOS o Linux)
- Git instalado para clonar el repositorio

---

## Pasos para ejecutar la app localmente

### 1. Clonar este repositorio

[bash]
git clone --branch grupo7-practica2 https://github.com/jorgemansillacriado/example-voting-app.git 
cd example-voting-app/app
### 2. Levantar la app con Docker
Puedes usar dos comandos distintos:

[bash]
docker compose up --build
///
o alternativamente
///
docker compose -f 'compose.yml' up -d --build

### 3. Acceder con tu navegador
Abre tu navegador y visita http://localhost:80

### 4. Detener la app
Para ello puedes acceder directamente a Docker Desktop y terminar la app
o puedes usar el siguiente comando:

[bash]
docker compose down
///
Si has usado el primer comando en el apartado 2 es posible que sea necesario
Crtl^C para poder escribir el comando
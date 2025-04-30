## Repositorios de la actividad anterior: 

Álvaro Pascual Cordero: https://github.com/Daikuroo/G7_T1_todo-app 

Jorge Mansilla Criado: https://github.com/jorgemansillacriado/practica1-todo-app.git 

 

Para la realización de la segunda tarea se ha escogido una aplicación con un poco más de desarrollo, cuyo repositorio se puede encontrar aquí: https://github.com/jorgemansillacriado/example-voting-app.git 

Es una aplicación simple de votación que tiene las siguientes pequeñas aplicaciones: 

vote, result, worker, redis, db 

 

## Resumen rápido de medidas que ya se identifican: 

### Vote app (frontend): 

No se observa validación de inputs del usuario (debería haber más controles). 

### Result app (backend): 

No hay autenticación. 

### Worker app (procesamiento): 

Solo conecta internamente entre Redis y DB, pero sin cifrado de datos. 

### Infraestructura (Docker): 

Uso de imágenes públicas sin hardening. 

No hay escaneos automáticos de vulnerabilidades. 

Conclusión: falta integrar bastantes medidas de seguridad en cada fase del S-SDLC. 

# 1. Análisis de las Medidas de Seguridad en el S-SDLC de la Aplicación de Votación 

Análisis S-SDLC del proyecto example-voting-app. S-SDLC divide el desarrollo en varias etapas, considerando la seguridad desde el principio. Aplicado a la app: 

Shape  

### Análisis de Requisitos

| Qué se ve en el proyecto| Medidas de Seguridad identificadas| Riesgos|
|------------|------------------|-----------------------------------|
| No se documentan requisitos de seguridad específicos.| No se identifican requisitos explícitos como autenticación, autorización, validaciones de entrada.| Falta de definición de requerimientos de seguridad y exposición de servicios sensibles en redes abiertas.|


### Diseño del Sistema 

| Qué se ve en el proyecto| Medidas de Seguridad identificadas| Riesgos|
|------------|------------------|-----------------------------------|
| Arquitectura basada en microservicios (vote, result, worker, db, redis).| La separación de responsabilidades ayuda a reducir el impacto de vulnerabilidades (por ejemplo, un error en vote no afecta a result). Sin embargo, no se ve diseño explícito para seguridad (por ejemplo, no hay diseño para autenticación o control de acceso).| La falta de políticas de acceso y protocolos de seguridad como TLS/SSL exponen a la aplicación a ataques “Man in the Middle” e inyección de datos entre capas.|

### Implementación

| Qué se ve en el proyecto| Medidas de Seguridad identificadas| Riesgos|
|------------|------------------|-----------------------------------|
| Código en Node.js, Python y .NET. No se observan buenas prácticas estrictas de seguridad (por ejemplo: input validation, sanitización de datos).| No hay validación de entradas robusta.<br>No hay autenticación ni control de usuarios.<br>No hay protección contra inyección de código.<br>No se usan HTTPS o tokens de sesión.<br>Dockerfiles bastante básicos sin refuerzo de seguridad. |Aunque la sencillez del código reduce la superficie de exposición, las medidas de seguridad no implementadas pueden comprometer el sistema vía inputs maliciosos o la confidencialidad mediante exfiltración de los datos.|

### Pruebas de Seguridad 

| Qué se ve en el proyecto| Medidas de Seguridad identificadas| Riesgos|
|------------|------------------|-----------------------------------|
| No hay tests de seguridad en el repo (sólo pruebas funcionales básicas si acaso). | Faltan pruebas de seguridad automatizadas: análisis estático (SAST), escaneo de dependencias vulnerables, tests de penetración.  |La falta de pruebas puede resultar en fallos al detectar vulnerabilidades antes del despliegue. |

### Despliegue 

| Qué se ve en el proyecto| Medidas de Seguridad identificadas| Riesgos|
|------------|------------------|-----------------------------------|
| Uso de docker-compose.yml para levantar todos los servicios.  | Se usa restart: always, lo que mejora disponibilidad, pero no hay configuración segura de la red entre servicios.<br>No se aplica secrets management (las contraseñas están hardcodeadas). <br>No se usan HTTPS o tokens de sesión<br>No hay escaneo de imágenes de contenedores.  |Se expone la aplicación a vulnerabilidades conocidas en imágenes base. |

### Mantenimiento

| Qué se ve en el proyecto| Medidas de Seguridad identificadas| Riesgos|
|------------|------------------|-----------------------------------|
| Al ser un ejemplo, no presenta procesos de actualización o parcheo.  | Faltan políticas de actualización de dependencias, monitoreo de vulnerabilidades o alertas de seguridad.  |La falta de medidas en esta fase inhabilita la capacidad de detectar actividades maliciosas.  |


## Problemas más importantes de seguridad detectados: 

- Contraseñas visibles en docker-compose.yml. 

- Falta de validación de datos de entrada (puede dar lugar a inyección). 

- No hay autenticación o autorización en los servicios web. 

- No se usa HTTPS (todo funciona en HTTP en localhost). 

- No hay separación fuerte de redes Docker (no se aíslan servicios críticos). 

- No se usan mecanismos de protección tipo helmet, cors en Node.js. 

 

## 2. Unificación del proyecto y propuesta de DevSecOps + S-SDLC 

A continuación se presenta la propuesta de proyecto tipo para aplicar el S-SDLC incorporándo DevSecOps
Ahora, ¿cómo unificar las apps en una y aplicar DevSecOps en este tipo de proyecto? 

### Unificación: 

Una única API en Node.js: 

/vote para registrar votos. 

/results para obtener el estado actual de la votación. 

Un solo frontend React. 

Un único servicio de base de datos (PostgreSQL). 

Redis opcional como cola de mensajes para procesar votos si quieres hacerlo escalable. 

 

### Aplicación del ciclo DevSecOps 

El ciclo DevSecOps puede desarrollarse como sigue:

Planificación ➔ Diseño ➔ Desarrollo ➔ Build ➔ Test ➔ Despliegue (con Seguridad integrada)

Cada uno de los pasos incluye lo siguiente:

1. **Planificación**   

    - Definir requisitos de seguridad: input validation, autenticación mínima. 

2. **Diseño Seguro** 

    - Arquitectura microservicios → controlar acceso a servicios. 

    - Gestión segura de secretos. 

3. **Desarrollo Seguro** 

    - Linter + ESLint reglas de seguridad. 

    - Validaciones de entrada y sanitización. 

4. **Build Seguro** 

    - Dockerfiles minimalistas y escaneados (`docker scan`, `trivy`). 

5. **Testing** 

    - Pruebas unitarias + escaneo de dependencias (`npm audit`, `snyk`). 

    - Escaneo SAST automático en pipelines (ej: SonarQube). 

6. **Despliegue seguro** 

    - Uso de HTTPS. 

    - Variables de entorno en vez de hardcodeo. 

    - Monitoreo de logs + alertas de anomalías. 

 

--- 

 

# Pequeño Diagrama Visual S-SDLC + DevSecOps 

 

 

                                  +------------------+ 

                                  |   Planificación  | 
 
                                  +------------------+ 

                                           ↓ 

                                  +------------------+ 

                                  |   Diseño Seguro  | 

                                  +------------------+ 

                                           ↓ 

                                +---------------------+ 

                                | Codificación Segura | <---------------------------+ 

                                +---------------------+                             | 

                                           ↓                                        | 

                            +-----------------------------+                         | 

                            | Construcción y Build Seguro |                         | 

                            +-----------------------------+                         | 

                                           ↓                                        | 

                          +-----------------------------------+                     | 

                          | Pruebas de Seguridad (SAST, DAST) |                     | 

                          +-----------------------------------+                     | 

                                           ↓                                        | 

                                  +-------------------+                             | 

                                  | Despliegue Seguro |                             | 

                                  +-------------------+                             | 

                                           ↓                                        | 

                                 +----------------------+                           | 

                                 | Monitoreo y Feedback | --------------------------+ 

                                 +----------------------+ 




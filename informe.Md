# 🛡️ Informe de Evaluación de Seguridad - Example Voting App

## 📘 Descripción General

Este informe documenta las modificaciones realizadas sobre la aplicación [example-voting-app](https://github.com/jorgemansillacriado/example-voting-app/tree/grupo7-practica2) y el análisis de seguridad aplicado utilizando herramientas gratuitas y técnicas de evaluación automatizadas.

---

##  1. Técnicas y herramientas de evaluación utilizadas

| Técnica / Herramienta | Tipo | Descripción |
|-----------------------|------|-------------|
| **Dependabot**        | Análisis de dependencias | Detecta automáticamente librerías con vulnerabilidades conocidas. |
| **CodeQL**            | Análisis estático | Detecta vulnerabilidades a nivel de código fuente. |
| **Bandit**            | Análisis estático (Python) | Identifica patrones peligrosos en código Python. |
| **Trivy**             | Escaneo de imágenes Docker | Busca CVEs en imágenes y dependencias del sistema. |
| **OWASP ZAP**         | Análisis dinámico (DAST) | Prueba la aplicación web en ejecución para detectar XSS, CSRF, etc. |

---

##  2. Modificaciones realizadas para forzar detección de fallos

| Servicio | Archivo | Modificación | Justificación |
|----------|---------|--------------|---------------|
| `vote` | `app.py` | Se añadió endpoint `/insecure` con uso de `eval()` | Forzar detección de uso inseguro de entrada del usuario. |
| `worker` | `requirements.txt` | Downgrade a `Flask==0.10` | Introducir dependencia vulnerable conocida. |
| `result` | `ResultController.cs` | Consulta SQL con concatenación sin parámetro | Simular vulnerabilidad de inyección SQL. |
| `frontend` | `package.json` | Se añadió `lodash@3.0.0` | Versiones antiguas con vulnerabilidades conocidas. |
| `docker-compose.yml` | Imagen `redis:3.0` | Uso de imagen de Docker con CVEs. |

---

##  3. Workflows automatizados en GitHub Actions

Se añadieron los siguientes workflows bajo `.github/workflows/`:

- `codeql-analysis.yml`: Escanea código Python, JavaScript y C# usando CodeQL.
- `bandit.yml`: Ejecuta Bandit sobre servicios Python (`vote`, `worker`).
- `trivy.yml`: Analiza el sistema de archivos con Trivy para encontrar CVEs.

---

##  4. Resultados obtenidos

###  Dependabot

| Servicio | Dependencia | Alerta |
|----------|-------------|--------|
| `frontend` | `lodash@3.0.0` | CVE-2019-10744, CVE-2021-23337 |
| `worker` | `Flask==0.10` | Múltiples CVEs (inyección, denegación de servicio) |

###  CodeQL

| Lenguaje | Hallazgo | Severidad |
|----------|----------|-----------|
| Python | Uso de `eval()` en `vote/app.py` | Alta |
| C# | SQL concatenado sin parámetros en `result` | Alta |

###  Bandit

| Archivo | Línea | Problema detectado |
|--------|-------|---------------------|
| `vote/app.py` | Uso de `eval()` | [B307] Use of `eval` detected |
| `worker/worker.py` | No crítico | - |

###  Trivy

| Imagen | Vulnerabilidades encontradas | Severidad |
|--------|------------------------------|-----------|
| `redis:3.0` | CVE-2015-4335, CVE-2018-12326 | Alta |
| Imágenes Python | CVEs en `fl

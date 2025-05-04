string id = Request.Query["id"];
string query = "SELECT * FROM Results WHERE ID = " + id; // ⚠️ Inyección SQL



// questo script controlla se l'utente è autenticato
/// Returns: True o False

async function checkAuth() {
    try {
        const response = await fetch("http://localhost:5150/api/auth/isAuthenticated", {
            method: "GET",
            credentials: "include" // per i cookie
        });

        // Se non autorizzato, return false 
        if (response.status === 401) {
            return false;
        }

        // Controlla se la risposta HTTP è OK 
        if (!response.ok) {
            console.error("Errore HTTP:", response.status);
            return false;
        }

        const result = await response.json();
        
        return result;

    } catch (error) {
        console.error("Errore nella richiesta:", error);
        return false;
    }
}
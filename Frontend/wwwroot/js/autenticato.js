

// questo script controlla se l'utente è autenticato
/// Returns: True o False

async function checkAuth() {
    try {
        const response = await fetch("http://localhost:5150/api/auth/isAuthenticated", {
            method: "GET",
            credentials: "include"
        });

        if (response.status === 401) {
            return false;
        }

        if (!response.ok) {
            console.error("Errore HTTP:", response.status);
            return false;
        }

        return true;

    } catch (error) {
        console.error("Errore nella richiesta:", error);
        return false;
    }
}
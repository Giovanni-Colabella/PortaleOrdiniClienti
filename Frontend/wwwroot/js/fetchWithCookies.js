
// Questa funzione ha come obiettivo quello di essere un comune fetch js 
// ma che includa i cookie di default 
export async function fetchWithCookies(url, options = {}) {
    
    const baseOptions = {
        credentials: 'include'
    };

    const completeOptions = { ...baseOptions, ...options};

    return await fetch(url, completeOptions);
}